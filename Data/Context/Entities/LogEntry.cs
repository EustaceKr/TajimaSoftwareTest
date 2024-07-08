using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Context.Entities
{
    public class LogEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [MaxLength(10)]
        public string Level { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}