using Data.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Context.Entities
{
    public class Template : BaseEntity
    {
        public DecorationMethod DecorationMethod { get; set; }
        public string Name { get; set; }
        public ICollection<TemplateDesign> TemplateDesigns { get; set; } = new List<TemplateDesign>();
    }
}
