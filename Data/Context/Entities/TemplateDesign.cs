using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context.Entities
{
    public class TemplateDesign
    {
        public int TemplateId { get; set; }
        public Template Template { get; set; }
        public int DesignId { get; set; }
        public Design Design { get; set; }
    }
}
