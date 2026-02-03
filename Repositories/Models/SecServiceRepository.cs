using Contracts.interfaces.Models;
using Entities.Models.Tables;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Repositories.Repositories;
using Contracts.DTOs.SecService;
using MapsterMapper;
using Grpc_Client;
using LoggerService;
using Contracts.enums;
using Contracts.Responses;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Models;

public class SecServiceRepository : RepositoryBase<SecService, SecServiceDto, SecServiceCreateDto, SecServiceUpdateDto>, ISecServiceRepository
{
    private readonly RepositoryContext _repositoryContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    //private readonly IStructureGrpcClient _structureGrpcClient;





    public SecServiceRepository(RepositoryContext repositoryContext, IHttpContextAccessor httpContextAccessor
        , IMapper mapper, ILoggerManager logger)
        : base(logger, repositoryContext, httpContextAccessor, mapper)
    {
        _repositoryContext = repositoryContext;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _logger = logger;
    }
    public override async Task<ParentResponseModel> Delete(int id, bool softDelete = true)
    {
        try
        {
            var entity = await RepositoryContext.SecServices.Include(e => e.SecPages).FirstOrDefaultAsync(t => t.Id == id && t.IsDeleted == false);
            if (entity != null)
            {
                if ( entity.SecPages.Count > 0)
                {
                    return new ParentResponseModel()
                    {
                        ErrorCode = ErrorCatalog.VioleteConstrains,
                        IsDone = false,
                        ReturnMessage = "object has child object , Please Delete Pages Assigned To Service"
                    };
                }
                if (softDelete)
                {
                    string userCode = _httpContextAccessor.HttpContext?.User.FindFirst("EMP_SERIAL")?.Value;
                    entity.DeleteUserCode = !string.IsNullOrEmpty(userCode) ? userCode : "no delete user code detected";
                    entity.DeleteDate = DateTime.Now;
                    entity.IsDeleted = true;
                    RepositoryContext.SecServices.Update(entity);
                }
                else
                {
                    RepositoryContext.SecServices.Remove(entity);
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
            _logger.logErrorWithException(ex, $"{typeof(SecService).Name} ===> Delete ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }


}
