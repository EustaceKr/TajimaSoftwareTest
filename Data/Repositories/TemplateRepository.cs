using Data.Context;
using Data.Context.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class TemplateRepository : Repository<Template>, ITemplateRepository
    {
        public TemplateRepository(ApplicationDbContext context) : base(context) { }

        public override IQueryable<Template> FindAll()
        {
            return base.FindAll().OrderBy(x => x.Id).Include(t => t.TemplateDesigns).ThenInclude(td => td.Design);
        }
        public async Task<Template> FindById(int id)
        {
            return await FindByCondition(t => t.Id == id).Include(t => t.TemplateDesigns).ThenInclude(td => td.Design).FirstOrDefaultAsync();
        }

        public override void Update(Template template)
        {
            var oldTemplateDesigns = _context.TemplateDesigns.Where(x => x.TemplateId == template.Id);
            foreach ( var templateDesign in template.TemplateDesigns )
            {
                if (!oldTemplateDesigns.Contains(templateDesign))
                    _context.TemplateDesigns.Add(templateDesign);
            }
            foreach (var oldTemplateDesign in oldTemplateDesigns)
            {
                if (!template.TemplateDesigns.Contains(oldTemplateDesign))
                {
                    _context.TemplateDesigns.Remove(oldTemplateDesign);
                }
            }

            base.Update(template);
        }
    }
}
