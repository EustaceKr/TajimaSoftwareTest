using Application.Responses;
using Data.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.EntitiesServices.Interfaces
{
    public interface IDesignService : IBaseService<Design>
    {
        Task<ServiceResponse<Design>> GetById(int id);
        Task<BaseServiceResponse> Update(int id, Design entity);
        Task<BaseServiceResponse> Delete(int id);
        Task<bool> IsUsedInTemplate(int id);
    }
}
