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
        Task<ServiceResponse<Template>> GetById(int id);
        Task<BaseServiceResponse> Create(Template template, List<int> designIds);
        Task<BaseServiceResponse> Update(int id, Template entity, List<int> designs);
        Task<BaseServiceResponse> Delete(int id);
        Task<ServiceResponse<List<Design>>> GetRemainingDesigns(int id);
    }
}
