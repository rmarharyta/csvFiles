using System;
using System.ComponentModel.DataAnnotations;

namespace YourProject.Models
{
    public class CsvRecord
    {
        [Key]
        public int RecordId { get; set; } 

        [Required]
        public string FileId { get; set; }   

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public bool Married { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public decimal Salary { get; set; }

        public CsvFile CsvFile { get; set; }
    }
}

