using Data.Context.Entities;
using Data.Enumerations;

namespace Application.DTOs
{
    public class TemplateDTO
    {
        public int Id { get; set; }
        public DecorationMethod DecorationMethod { get; set; }
        public string Name { get; set; }
        public List<Design> Designs { get; set; } = new List<Design>();
    }
}
