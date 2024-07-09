using Application.DTOs;
using Application.EntitiesServices.Interfaces;
using Application.Mapping;
using Application.Responses;
using Data.Context.Entities;
using Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.EntitiesServices
{
    public class TemplateService : BaseService<Template>, ITemplateService
    {
        IDesignService _designService;
        public TemplateService(ITemplateRepository repository, ILogger<TemplateService> logger, IDesignService designService) : base(repository, logger)
        {
            _designService = designService;
        }

        public async Task<BaseServiceResponse> Delete(int id)
        {
            try
            {
                var response = await GetById(id);
                if (response.Response == HttpStatusCode.OK && response.Data is not null)
                {
                    var template = DataMapping.MapTemplate(response.Data);
                    return await base.Delete(template);
                }
                else
                    return new BaseServiceResponse(response.Response, response.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BaseServiceResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<ServiceResponse<TemplateDTO>> GetById(int id)
        {
            try
            {
                var response = await ((ITemplateRepository)_repository).FindById(id);
                if (response is not null)
                {
                    var dto = DataMapping.MapTemplateDTO(response);
                    return new ServiceResponse<TemplateDTO>(HttpStatusCode.OK, null, dto);
                }
                else
                    return new ServiceResponse<TemplateDTO>(HttpStatusCode.NotFound, null, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ServiceResponse<TemplateDTO>(HttpStatusCode.InternalServerError, ex.Message, null);
            }
            throw new NotImplementedException();
        }

        public async Task<BaseServiceResponse> Create(TemplateDTO dto, List<int> designIds)
        {
            try
            {
                if (designIds != null && designIds.Count > 0)
                {
                    foreach (var designId in designIds)
                    {
                        var templateDesign = new TemplateDesign
                        {
                            TemplateId = dto.Id,
                            DesignId = designId
                        };
                        dto.TemplateDesigns.Add(templateDesign);
                    }
                }
                var template = DataMapping.MapTemplate(dto);
                template.CreatedDate = DateTime.UtcNow;
                return await base.Create(template);
            }
            catch(Exception ex)
            {
                _logger?.LogError(ex, ex.Message);
                return new BaseServiceResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<BaseServiceResponse> Update(int id, TemplateDTO dto, List<int> selectedDesignIds)
        {
            try
            {
                var response = await GetById(id);
                if (response.Response == HttpStatusCode.OK && response.Data is not null)
                {
                    var existingTemplate = response.Data;
                    var existingDesignIds = existingTemplate.TemplateDesigns.Select(td => td.DesignId).ToList();

                    // Find designs to remove
                    var designsToRemove = existingTemplate.TemplateDesigns
                        .Where(td => !selectedDesignIds.Contains(td.DesignId))
                        .ToList();

                    // Remove designs
                    foreach (var designToRemove in designsToRemove)
                    {
                        existingTemplate.TemplateDesigns.Remove(designToRemove);
                    }

                    // Find designs to add
                    var designsToAdd = selectedDesignIds
                        .Where(sd => !existingDesignIds.Contains(sd))
                        .Select(sd => new TemplateDesign { TemplateId = dto.Id, DesignId = sd })
                        .ToList();

                    // Add new designs
                    foreach (var designToAdd in designsToAdd)
                    {
                        existingTemplate.TemplateDesigns.Add(designToAdd);
                    }

                    // Update template properties
                    existingTemplate.DecorationMethod = dto.DecorationMethod;
                    existingTemplate.Name = dto.Name;

                    var entity = DataMapping.MapTemplate(existingTemplate);
                    entity.UpdatedDate = DateTime.UtcNow;

                    var updateResponse = await base.Update(entity);
                    if (updateResponse.Response == HttpStatusCode.OK)
                        return new BaseServiceResponse(HttpStatusCode.OK, null);
                    else
                        return new BaseServiceResponse(response.Response, response.Error);
                }
                else
                {
                    return new BaseServiceResponse(response.Response, response.Error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BaseServiceResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<ServiceResponse<List<DesignDTO>>> GetRemainingDesigns(int id)
        {
            var remainingDesigns = new List<DesignDTO>();
            var response = await _designService.GetAll();

            if (response.Response != HttpStatusCode.OK)
                return new ServiceResponse<List<DesignDTO>>(HttpStatusCode.InternalServerError, "Error while fetching Data", null);

            var responseById = await GetById(id);
            if (responseById.Response == HttpStatusCode.OK || responseById.Data is not null)
            {
                foreach (var dto in response.Data!)
                {
                    if (!responseById.Data!.TemplateDesigns.Any(x => x.DesignId == dto.Id))
                        remainingDesigns.Add(dto);
                }
                return new ServiceResponse<List<DesignDTO>>(HttpStatusCode.OK, null, remainingDesigns);
            }
            else
                return new ServiceResponse<List<DesignDTO>>(HttpStatusCode.InternalServerError, "Error while fetching Data", null);
        }

        public async new Task<ServiceResponse<List<TemplateDTO>>> GetAll()
        {
            var response = await base.GetAll();
            if (response.Response == HttpStatusCode.OK)
            {
                if (response.Data is not null)
                {
                    var dtos = new List<TemplateDTO>();
                    foreach (var template in response.Data)
                    {
                        dtos.Add(DataMapping.MapTemplateDTO(template));
                    }
                    return new ServiceResponse<List<TemplateDTO>>(HttpStatusCode.OK, null, dtos);
                }
                else
                    return new ServiceResponse<List<TemplateDTO>>(HttpStatusCode.OK, null, null);
            }
            else
                return new ServiceResponse<List<TemplateDTO>>(response.Response, response.Error, null);
        }
    }
}
