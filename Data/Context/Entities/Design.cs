using Data.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Data.Context.Entities
{
    public class Design
    {
        public int Id { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid decoration method.")]
        public DecorationMethod DecorationMethod { get; set; }
        public string Name { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid height.")]
        [Display(Name = "Width in mm")]
        public double Width { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid height.")]
        [Display(Name = "Height in mm")]
        public double Height { get; set; }
        public ICollection<TemplateDesign> TemplateDesigns { get; set; } = new List<TemplateDesign>();
    }
}
