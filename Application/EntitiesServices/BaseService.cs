using Application.EntitiesServices.Interfaces;
using Application.Responses;
using Data.Context.Entities;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.EntitiesServices
{
    public abstract class BaseService<T> : IBaseService<T> where T : class
    {
        public readonly IRepository<T> _repository;
        public readonly ILogger<BaseService<T>> _logger;

        public BaseService(IRepository<T> repository, ILogger<BaseService<T>> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public virtual async Task<BaseServiceResponse> Complete()
        {
            try
            {
                bool response =  await _repository.SaveChangesAsync();
                if (response)
                    return new BaseServiceResponse(HttpStatusCode.OK, null);
                else
                    return new BaseServiceResponse(HttpStatusCode.InternalServerError, null);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                return new BaseServiceResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public virtual async Task<BaseServiceResponse> Create(T entity)
        {
            try
            {
                var response =  _repository.Create(entity);
                if (response is not null)
                {
                    var saveResponse = await Complete();
                    if(saveResponse.Response == HttpStatusCode.OK)
                        return new BaseServiceResponse(HttpStatusCode.Created, null);
                    else
                        return new BaseServiceResponse(HttpStatusCode.InternalServerError, saveResponse.Error);
                }
                else
                    return new BaseServiceResponse(HttpStatusCode.BadRequest, null);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, ex.Message);
                return new BaseServiceResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public virtual async Task<BaseServiceResponse> Delete(T entity)
        {
            try
            {
                _repository.Delete(entity);
                var saveResponse = await Complete();
                if(saveResponse.Response == HttpStatusCode.OK)
                    return new BaseServiceResponse(HttpStatusCode.OK, null);
                else
                    return new BaseServiceResponse(HttpStatusCode.InternalServerError, saveResponse.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BaseServiceResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public virtual async Task<ServiceResponse<List<T>>> GetAll()
        {
            try
            {
                var response = await _repository.FindAll().ToListAsync();
                return new ServiceResponse<List<T>>(HttpStatusCode.OK, null, response);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, ex.Message);
                return new ServiceResponse<List<T>>(HttpStatusCode.InternalServerError, ex.Message, null);
            }
        }

        public virtual async Task<BaseServiceResponse> Update(T entity)
        {
            try
            {
                _repository.Update(entity);
                var saveResponse = await Complete();
                if (saveResponse.Response == HttpStatusCode.OK)
                    return new BaseServiceResponse(HttpStatusCode.OK, null);
                else
                    return new BaseServiceResponse(HttpStatusCode.InternalServerError, saveResponse.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BaseServiceResponse(HttpStatusCode.InternalServerError,ex.Message);
            }
        }
    }
}
