using Data.Context;
using Data.Context.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class DesignRepository : Repository<Design>, IDesignRepository
    {
        public DesignRepository(ApplicationDbContext context) : base (context) { }

        public async Task<Design> FindById(int id)
        {
            return await FindByCondition(x => x.Id == id).Include(d => d.TemplateDesigns).ThenInclude(td => td.Template).FirstOrDefaultAsync();
        }
    }
}
