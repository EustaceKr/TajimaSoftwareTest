using Application.DTOs;
using Application.EntitiesServices.Interfaces;
using Application.Mapping;
using Application.Responses;
using Azure;
using Data.Context.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.EntitiesServices
{
    public class DesignService : BaseService<Design>, IDesignService
    {
        public DesignService(IDesignRepository repository, ILogger<DesignService> logger) : base(repository, logger)
        {
            
        }

        public async Task<ServiceResponse<DesignDTO>> GetById(int id)
        {
            try
            {
                var response = await ((IDesignRepository)_repository).FindById(id);
                if (response is not null)
                {
                    var dto = DataMapping.MapDesignDTO(response);
                    return new ServiceResponse<DesignDTO>(HttpStatusCode.OK, null, dto);
                }
                else
                    return new ServiceResponse<DesignDTO>(HttpStatusCode.NotFound, null, null);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new ServiceResponse<DesignDTO>(HttpStatusCode.InternalServerError, e.Message, null);
            }
        }

        public async Task<BaseServiceResponse> Create(DesignDTO dto)
        {
            try
            {
                var entity = DataMapping.MapDesign(dto);
                entity.CreatedDate = DateTime.UtcNow;
                var response = await base.Create(entity);
                if (response.Response == HttpStatusCode.Created)
                    return new BaseServiceResponse(HttpStatusCode.Created, null);
                else
                    return new BaseServiceResponse(response.Response, response.Error);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new BaseServiceResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public async Task<BaseServiceResponse> Delete(int id)
        {
            try
            {
                var response = await GetById(id);
                if (response.Response == HttpStatusCode.OK && response.Data is not null)
                    if (!response.Data.TemplateDesigns.Any())
                    {
                        var entity = DataMapping.MapDesign(response.Data);
                        return await base.Delete(entity);
                    }
                    else
                        return new BaseServiceResponse(HttpStatusCode.BadRequest, "Design is used in a template.");
                else
                    return new BaseServiceResponse(response.Response, response.Error);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new BaseServiceResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public async Task<BaseServiceResponse> Update(int id, DesignDTO dto)
        {
            try
            {
                var response = await GetById(id);
                if (response.Response == HttpStatusCode.OK && response.Data is not null)
                {
                    var entity = DataMapping.MapDesign(dto);
                    entity.UpdatedDate = DateTime.UtcNow;
                    return await base.Update(entity, ["CreatedDate"]);
                }
                else
                    return new BaseServiceResponse(response.Response, response.Error);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new BaseServiceResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public async Task<bool> IsUsedInTemplate(int id)
        {
            var response = await GetById(id);
            if(response.Response == HttpStatusCode.OK && response.Data is not null)
            {
                if (response.Data.TemplateDesigns.Any())
                    return true;
                else
                    return false;
            }
            return false;
        }

        public async new Task<ServiceResponse<List<DesignDTO>>> GetAll()
        {
            var response = await base.GetAll();
            if (response.Response == HttpStatusCode.OK)
            {
                if(response.Data is not null)
                {
                    var dtos = new List<DesignDTO>();
                    foreach (var design in response.Data)
                    {
                        dtos.Add(DataMapping.MapDesignDTO(design));
                    }
                    return new ServiceResponse<List<DesignDTO>>(HttpStatusCode.OK, null, dtos);
                }
                else 
                    return new ServiceResponse<List<DesignDTO>>(HttpStatusCode.OK, null, null);
            }
            else
                return new ServiceResponse<List<DesignDTO>>(response.Response, response.Error, null);
        }
    }
}
