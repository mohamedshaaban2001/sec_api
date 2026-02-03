
using Contracts.BaseDtos;
using Contracts.enums;
using Contracts.interfaces.Repository;
using Contracts.Pagging;
using Contracts.Responses;
using Entities.Models;
using LoggerService;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Repositories.Repositories
{
    public abstract class RepositoryBase<T, TDto, TCreateDto, TUpdateDto> : IRepositoryBase<T, TDto, TCreateDto, TUpdateDto>
        where T : Entities.Models.BaseTables.BaseTable
        where TDto : BaseDto
        where TCreateDto : BaseCreateDto
        where TUpdateDto : BaseUpdateDto

    {
        protected RepositoryContext RepositoryContext { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public RepositoryBase(ILoggerManager logger, RepositoryContext repositoryContext, IHttpContextAccessor httpContextAccessor
            , IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            RepositoryContext = repositoryContext;
            _mapper = mapper;
            _logger = logger;
        }
        public virtual async Task<ParentResponseModel> FindById(int id)
        {
            try
            {
                var _entity = await RepositoryContext.Set<T>().FirstOrDefaultAsync(t => t.Id == id && t.IsDeleted == false);
                if (_entity == null)
                {
                    return new ParentResponseModel()
                    {
                        ErrorCode = ErrorCatalog.ObjectNotFound,
                        IsDone = false,
                        ReturnMessage = "Object not found"
                    };
                }
                else
                {
                    return new SingleObjectResponseModel<TDto>()
                    {
                        ErrorCode = ErrorCatalog.noError,
                        IsDone = true,
                        ReturnMessage = "Object loaded successfully",
                        SingleObject = _entity.Adapt<TDto>()
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.logErrorWithException(ex, $"{typeof(T).Name} ===> FindById ");
                return new ParentResponseModel()
                {
                    ErrorCode = ErrorCatalog.DataBaseFauiler,
                    IsDone = false,
                    ReturnMessage = ex.Message,
                };
            }
        }
        public virtual async Task<ParentResponseModel> FindAll()
        {
            try
            {
                var listOfObjects = await RepositoryContext.Set<T>().Where(s => s.IsDeleted == false).AsNoTracking().ProjectToType<TDto>().ToListAsync();
                return new ListOfObjectsResponseModel<TDto>()
                {
                    ErrorCode = ErrorCatalog.noError,
                    IsDone = true,
                    ReturnMessage = "Objects Loaded Successufly",
                    Objects = listOfObjects
                };
            }
            catch (Exception ex)
            {
                _logger.logErrorWithException(ex, $"{typeof(T).Name} ===> FindAll ");
                return new ParentResponseModel()
                {
                    ErrorCode = ErrorCatalog.DataBaseFauiler,
                    IsDone = false,
                    ReturnMessage = ex.Message,
                };
            }
        }

        public virtual async Task<ParentResponseModel> FindByCondition(Expression<Func<T, bool>> expression)
        {
            try
            {
                return new ListOfObjectsResponseModel<TDto>()
                {
                    ErrorCode = ErrorCatalog.noError,
                    IsDone = true,
                    ReturnMessage = "Objects Loaded Successufly",
                    Objects = await RepositoryContext.Set<T>().Where(s => s.IsDeleted == false).Where(expression).ProjectToType<TDto>().ToListAsync()
                };
            }
            catch (Exception ex)
            {
                _logger.logErrorWithException(ex, $"{typeof(T).Name} ===> FindByCondition ");
                return new ParentResponseModel()
                {
                    ErrorCode = ErrorCatalog.DataBaseFauiler,
                    IsDone = false,
                    ReturnMessage = ex.Message,
                };
            }
        }

        public virtual async Task<ParentResponseModel> FindAllPaged(int page = 1, int pagesize = 10)
        {
            try
            {
                var data = await RepositoryContext.Set<T>().ProjectToType<TDto>().AsNoTracking().ToListAsync();
                return new ListOfPagedObjectsResponseModel<TDto>()
                {
                    ErrorCode = ErrorCatalog.noError,
                    IsDone = true,
                    ReturnMessage = "Objects Loaded Successufly",
                    Objects = data.GetPaged(page, pagesize)
                };
            }
            catch (Exception ex)
            {
                _logger.logErrorWithException(ex, $"{typeof(T).Name} ===> FindAllPaged ");
                return new ParentResponseModel()
                {
                    ErrorCode = ErrorCatalog.DataBaseFauiler,
                    IsDone = false,
                    ReturnMessage = ex.Message,
                };
            }
        }

        public virtual async Task<ParentResponseModel> FindByConditionPaged(Expression<Func<T, bool>> expression, int page = 1, int pagesize = 10)
        {
            try
            {
                var data = await RepositoryContext.Set<T>().Where(s => s.IsDeleted == false).Where(expression).ProjectToType<TDto>().ToListAsync();
                return new ListOfPagedObjectsResponseModel<TDto>()
                {
                    ErrorCode = ErrorCatalog.noError,
                    IsDone = true,
                    ReturnMessage = "Objects Loaded Successufly",
                    Objects = data.GetPaged(page, pagesize)
                };
            }
            catch (Exception ex)
            {
                _logger.logErrorWithException(ex, $"{typeof(T).Name} ===> FindByConditionPaged ");
                return new ParentResponseModel()
                {
                    ErrorCode = ErrorCatalog.DataBaseFauiler,
                    IsDone = false,
                    ReturnMessage = ex.Message,
                };
            }
        }

        public virtual async Task<ParentResponseModel> Create(TCreateDto entityCreate)
        {
            try
            {
                T entity = entityCreate.Adapt<T>();
                string userCode = _httpContextAccessor?.HttpContext?.User.FindFirst("EMP_SERIAL")?.Value;
                entity.InsertUserCode = !string.IsNullOrEmpty(userCode) ? userCode : "no create user code detected";
                entity.InsertDate = DateTime.Now;
                entity.IsDeleted = false;
                await RepositoryContext.Set<T>().AddAsync(entity);
                await RepositoryContext.SaveChangesAsync();
                return new SingleObjectResponseModel<TDto>()
                {
                    ErrorCode = ErrorCatalog.noError,
                    SingleObject = entity.Adapt<TDto>(),
                    IsDone = true,
                    ReturnMessage = "Object Added Successufly"
                };
            }
            catch (Exception ex)
            {
                _logger.logErrorWithException(ex, $"{typeof(T).Name} ===> Create ");
                return new ParentResponseModel()
                {
                    ErrorCode = ErrorCatalog.DataBaseFauiler,
                    IsDone = false,
                    ReturnMessage = ex.Message,
                };
            }
        }

        public virtual async Task<ParentResponseModel> Update(TUpdateDto entityUpdate)
        {
            try
            {
                var newEntity = entityUpdate.Adapt<T>();
                var entity = await RepositoryContext.Set<T>().FirstOrDefaultAsync(t => t.Id == newEntity.Id && t.IsDeleted == false);
                if (entity != null)
                {
                    string userCode = _httpContextAccessor.HttpContext?.User.FindFirst("EMP_SERIAL")?.Value;
                    entity.UpdateUserCode = !string.IsNullOrEmpty(userCode) ? userCode : "no update user code detected";
                    entity.LastUpdate = DateTime.Now;
                    foreach (var property in typeof(T).GetProperties())
                    {
                        if (property.Name == "InsertUserCode" || property.Name == "InsertDate" || property.Name == "UpdateUserCode" || property.Name == "LastUpdate"
                            || property.Name == "IsDeleted" || property.Name == "DeleteUserCode" || property.Name == "DeleteDate" || property.Name == "Id")
                        {
                            continue;
                        }
                        var value = property.GetValue(newEntity);
                        var entityProperty = entity.GetType().GetProperty(property.Name);

                        if (entityProperty != null && entityProperty.CanWrite && value != null)
                        {
                            entityProperty.SetValue(entity, value);
                        }
                    }
                    RepositoryContext.Set<T>().Update(entity);
                    await RepositoryContext.SaveChangesAsync();

                    return new ParentResponseModel()
                    {
                        ErrorCode = ErrorCatalog.noError,
                        IsDone = true,
                        ReturnMessage = "Object updated successufly",
                    };
                }
                else
                {
                    return new ParentResponseModel()
                    {
                        ErrorCode = ErrorCatalog.ObjectNotFound,
                        IsDone = false,
                        ReturnMessage = "Object not found"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.logErrorWithException(ex, $"{typeof(T).Name} ===> Update ");
                return new ParentResponseModel()
                {
                    ErrorCode = ErrorCatalog.DataBaseFauiler,
                    IsDone = false,
                    ReturnMessage = ex.Message,
                };
            }
        }

        public virtual async Task<ParentResponseModel> Delete(int id, bool softDelete = true)
        {
            try
            {
                var entity = await RepositoryContext.Set<T>().FirstOrDefaultAsync(t => t.Id == id && t.IsDeleted == false);
                if (entity != null)
                {
                    if (softDelete)
                    {
                        string userCode = _httpContextAccessor.HttpContext?.User.FindFirst("EMP_SERIAL")?.Value;
                        entity.DeleteUserCode = !string.IsNullOrEmpty(userCode) ? userCode : "no delete user code detected";
                        entity.DeleteDate = DateTime.Now;
                        entity.IsDeleted = true;

                        RepositoryContext.Set<T>().Update(entity);// Remove(entity);

                    }
                    else
                    {
                        RepositoryContext.Set<T>().Remove(entity);
                    }

                    await RepositoryContext.SaveChangesAsync();
                    return new ParentResponseModel()
                    {
                        ErrorCode = ErrorCatalog.noError,
                        IsDone = true,
                        ReturnMessage = "object removed successufly"
                    };
                }
                else
                {
                    return new ParentResponseModel()
                    {
                        ErrorCode = ErrorCatalog.ObjectNotFound,
                        IsDone = false,
                        ReturnMessage = "no object found with that id"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.logErrorWithException(ex, $"{typeof(T).Name} ===> Delete ");
                return new ParentResponseModel()
                {
                    ErrorCode = ErrorCatalog.DataBaseFauiler,
                    IsDone = false,
                    ReturnMessage = ex.Message,
                };
            }
        }


    }
}
