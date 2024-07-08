using Application.EntitiesServices.Interfaces;
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
        public TemplateService(ITemplateRepository repository, ILogger<TemplateService> logger, IDesignService designService) : base(repository,logger)
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
                    return await base.Delete(response.Data);
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

        public async Task<ServiceResponse<Template>> GetById(int id)
        {
            try
            {
                var response = await ((ITemplateRepository)_repository).FindById(id);
                if (response is not null)
                    return new ServiceResponse<Template>(HttpStatusCode.OK, null, response);
                else
                    return new ServiceResponse<Template>(HttpStatusCode.NotFound, null, null);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new ServiceResponse<Template>(HttpStatusCode.InternalServerError, e.Message, null);
            }
            throw new NotImplementedException();
        }

        public async Task<BaseServiceResponse> Create(Template template, List<int> designIds)
        {
            if (designIds.Count > 0)
            {
                foreach (var designId in designIds)
                {
                    var templateDesign = new TemplateDesign
                    {
                        TemplateId = template.Id,
                        DesignId = designId
                    };
                    template.TemplateDesigns.Add(templateDesign);
                }
            }
            return await base.Create(template);
        }

        public async Task<BaseServiceResponse> Update(int id, Template template, List<int> selectedDesignIds)
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
                        .Select(sd => new TemplateDesign { TemplateId = template.Id, DesignId = sd })
                        .ToList();

                    // Add new designs
                    foreach (var designToAdd in designsToAdd)
                    {
                        existingTemplate.TemplateDesigns.Add(designToAdd);
                    }

                    // Update template properties
                    existingTemplate.DecorationMethod = template.DecorationMethod;
                    existingTemplate.Name = template.Name;

                    var updateResponse = await base.Update(existingTemplate);
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
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new BaseServiceResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public async Task<ServiceResponse<List<Design>>> GetRemainingDesigns(int id)
        {
            var remainingDesigns = new List<Design>();
            var response = await _designService.GetAll();

            if (response.Response != HttpStatusCode.OK)
                return new ServiceResponse<List<Design>>(HttpStatusCode.InternalServerError, "Error while fetching Data", null);

            var responseById = await GetById(id);
            if (responseById.Response == HttpStatusCode.OK || responseById.Data is not null)
            {
                foreach (var design in response.Data!)
                {
                    if (!responseById.Data!.TemplateDesigns.Any(x => x.DesignId == design.Id))
                        remainingDesigns.Add(design);
                }
                return new ServiceResponse<List<Design>>(HttpStatusCode.OK, null, remainingDesigns);
            }
            else
                return new ServiceResponse<List<Design>>(HttpStatusCode.InternalServerError, "Error while fetching Data", null);
        }
    }
}
