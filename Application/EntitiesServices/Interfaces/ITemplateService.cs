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
    public interface ITemplateService : IBaseService<Template>
    {
        Task<ServiceResponse<TemplateDTO>> GetById(int id);
        Task<BaseServiceResponse> Create(TemplateDTO dto, List<int> designIds);
        Task<BaseServiceResponse> Update(int id, TemplateDTO dto, List<int> designs);
        Task<BaseServiceResponse> Delete(int id);
        Task<ServiceResponse<List<DesignDTO>>> GetRemainingDesigns(int id);
        Task<ServiceResponse<List<TemplateDTO>>> GetAll();
    }
}
