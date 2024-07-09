using Data.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Data.Context.Entities
{
    public class Template
    {
        public int Id { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid decoration method.")]
        public DecorationMethod DecorationMethod { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public ICollection<TemplateDesign> TemplateDesigns { get; set; } = new List<TemplateDesign>();
    }
}
