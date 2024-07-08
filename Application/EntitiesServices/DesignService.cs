using Application.DTOs;
using Application.EntitiesServices.Interfaces;
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

        public async Task<ServiceResponse<Design>> GetById(int id)
        {
            try
            {
                var response = await ((IDesignRepository)_repository).FindById(id);
                if (response is not null)
                    return new ServiceResponse<Design>(HttpStatusCode.OK, null, response);
                else
                    return new ServiceResponse<Design>(HttpStatusCode.NotFound, null, null);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new ServiceResponse<Design>(HttpStatusCode.InternalServerError, e.Message, null);
            }
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

        public async Task<BaseServiceResponse> Update(int id, Design entity)
        {
            try
            {
                var response = await GetById(id);
                if (response.Response == HttpStatusCode.OK && response.Data is not null)
                {
                    return await base.Update(entity);
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
    }
}
