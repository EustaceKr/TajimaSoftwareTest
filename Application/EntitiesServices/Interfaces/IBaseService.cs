using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Responses;

namespace Application.EntitiesServices.Interfaces
{
    public interface IBaseService<T>
    {
        Task<ServiceResponse<List<T>>> GetAll();
        Task<BaseServiceResponse> Create(T entity);
        Task<BaseServiceResponse> Update(T entity, params string[] propertiesToIgnore);
        Task<BaseServiceResponse> Delete(T entity);
        Task<BaseServiceResponse> Complete();
    }
}
