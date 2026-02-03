using Contracts.interfaces.Models;
using Entities.Models.Tables;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Repositories.Repositories;
using Contracts.DTOs.SecControlList;
using MapsterMapper;
using Grpc_Client;
using LoggerService;
using Contracts.enums;
using Contracts.Responses;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Models;

public class SecControlListRepository : RepositoryBase<SecControlList, SecControlListDto, SecControlListCreateDto, SecControlListUpdateDto>, ISecControlListRepository
{
    private readonly RepositoryContext _repositoryContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    //private readonly IStructureGrpcClient _structureGrpcClient;





    public SecControlListRepository(RepositoryContext repositoryContext, IHttpContextAccessor httpContextAccessor
        , IMapper mapper, ILoggerManager logger)
        : base(logger,repositoryContext, httpContextAccessor, mapper)
    {
        _repositoryContext = repositoryContext;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _logger = logger;
    }
    public async override Task<ParentResponseModel> Delete(int id, bool softDelete = true)
    {
        try
        {
            var entity = await RepositoryContext.SecControlLists.Include(e => e.SecGroupControls)
                .FirstOrDefaultAsync(t => t.Id == id);
            string userCode = _httpContextAccessor.HttpContext?.User.FindFirst("EMP_SERIAL")?.Value;
            if (entity != null)
            {
                if (entity.SecGroupControls.Count > 0)
                {
                    return new ParentResponseModel()
                    {
                        ErrorCode = ErrorCatalog.VioleteConstrains,
                        IsDone = false,
                        ReturnMessage = "object has child object , Please Delete GroupControls"
                    };
                }
                entity.IsDeleted = true;
                entity.DeleteUserCode = !string.IsNullOrEmpty(userCode) ? userCode : "no delete user code detected";
                entity.DeleteDate = DateTime.Now;
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
            _logger.logErrorWithException(ex, $"{typeof(SecPage).Name} ===> Delete ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }

}
