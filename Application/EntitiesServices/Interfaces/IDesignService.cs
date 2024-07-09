using Application.DTOs;
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
        Task<ServiceResponse<DesignDTO>> GetById(int id);
        Task<BaseServiceResponse> Create (DesignDTO dto);
        Task<BaseServiceResponse> Update(int id, DesignDTO dto);
        Task<BaseServiceResponse> Delete(int id);
        Task<ServiceResponse<List<DesignDTO>>> GetAll();
        Task<bool> IsUsedInTemplate(int id);
    }
}
