using Data.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class TemplateDesignDTO
    {
        public int TemplateId { get; set; }
        public Template Template { get; set; }
        public int DesignId { get; set; }
        public Design Design { get; set; }
    }
}
