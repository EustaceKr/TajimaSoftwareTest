using Data.Context.Entities;
using Data.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class DesignDTO
    {
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid decoration method.")]
        public DecorationMethod DecorationMethod { get; set; }

        [Required(ErrorMessage = "Name is required.")]
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
