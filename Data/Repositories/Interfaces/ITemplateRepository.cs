using Data.Context.Entities;

namespace Data.Repositories.Interfaces
{
    public interface ITemplateRepository : IRepository<Template>
    {
        Task<Template> FindById(int id);
    }
}
