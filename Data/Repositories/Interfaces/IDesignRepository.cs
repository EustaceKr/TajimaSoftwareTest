using Data.Context.Entities;

namespace Data.Repositories.Interfaces
{
    public interface IDesignRepository : IRepository<Design>
    {
        Task<Design> FindById(int id);
    }
}
