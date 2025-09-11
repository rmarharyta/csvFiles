using CsvHelper;
using CSVuploadFile.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using YourProject.Models;

namespace CSVuploadFile_Back.Controllers;

public class HomeController(AppDbContext _context, IWebHostEnvironment _env, ILogger<HomeController> logger) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirst("UserId")?.Value; 
        if (string.IsNullOrEmpty(userId))
            return View(new List<CsvFile>());

        var files = await _context.CsvFiles
                                  .Where(f => f.UserId == userId)
                                  .ToListAsync();

        return View(files);
    }

    [HttpGet]
    public IActionResult Upload() => View();

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("", "File don`t chosen or it`s empty");
            return View();
        }

        var uploadPath = Path.Combine(_env.WebRootPath, "Uploads");
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        var filePath = Path.Combine(uploadPath, file.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Read CSV and save
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<CSVstruct>().ToList();

            var userId = User.FindFirst("UserId")?.Value;

            var csvFile = new CsvFile
            {
                FileId = Guid.NewGuid().ToString(),
                UserId = userId,
                FileName = file.FileName
            };

            _context.CsvFiles.Add(csvFile);
            _context.SaveChanges();

            int i = 1;
            foreach (var r in records)
            {
                var row = new CsvRecord
                {
                    RecordId = i++,
                    FileId = csvFile.FileId,
                    Name = r.Name,
                    BirthDate = r.DateOfBirth,
                    Married = r.Married,
                    Phone = r.Phone,
                    Salary = r.Salary
                };
                _context.CsvRecords.Add(row);
            }
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ListFiles()
    {
        var userId = User.FindFirst("UserId")?.Value;
        var files = _context.CsvFiles
            .Where(f => f.UserId == userId)
            .ToList();

        return View(files);
    }

    [HttpGet]
    public IActionResult Details(string fileId)
    {
        var rows = _context.CsvRecords
            .Where(r => r.FileId == fileId)
            .ToList();

        if (rows == null || rows.Count == 0)
            return NotFound();

        return View(rows);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var file = _context.CsvRecords.FirstOrDefault(u=>u.RecordId==id);
        if (file == null)
            return NotFound();

        return View(file);
    }

    [HttpPost]
    public IActionResult Edit(int id, CsvRecord record)
    {
        var file = _context.CsvRecords.FirstOrDefault(u => u.RecordId == id);
        file.Name = record.Name;
        file.BirthDate = record.BirthDate;
        file.Phone = record.Phone;
        file.Salary = record.Salary;
        _context.SaveChanges();
        return RedirectToAction("Details", new { fileId = file.FileId });
    }

    [HttpPost]
    public IActionResult DeleteFile(string id)
    {
        var file = _context.CsvFiles.Find(id);
        if (file == null)
            return NotFound();

        _context.CsvFiles.Remove(file);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var row = _context.CsvRecords.FirstOrDefault(u => u.RecordId == id);
        if (row == null)
            return NotFound();

        var fileId = row.FileId;

        _context.CsvRecords.Remove(row);
        _context.SaveChanges();

        return RedirectToAction("Details", new { fileId });
    }

}
