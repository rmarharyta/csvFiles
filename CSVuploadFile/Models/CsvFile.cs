using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YourProject.Models
{
    public class CsvFile
    {
        [Key]
        public string FileId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string FileName { get; set; }

        public User User { get; set; }
        public ICollection<CsvRecord> Records { get; set; }
    }
}

