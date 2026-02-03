using Contracts.interfaces.Models;
using Entities.Models.Tables;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Repositories.Repositories;
using Contracts.DTOs.SecGroupJob;
using MapsterMapper;
using Grpc_Client;
using LoggerService;

namespace Repositories.Models;

public class SecGroupJobRepository : RepositoryBase<SecGroupJob, SecGroupJobDto, SecGroupJobCreateDto, SecGroupJobUpdateDto>, ISecGroupJobRepository
{
    private readonly RepositoryContext _repositoryContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    //private readonly IStructureGrpcClient _structureGrpcClient;





    public SecGroupJobRepository(RepositoryContext repositoryContext, IHttpContextAccessor httpContextAccessor
        , IMapper mapper, ILoggerManager logger)
        : base(logger,repositoryContext, httpContextAccessor, mapper)
    {
        _repositoryContext = repositoryContext;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _logger = logger;
    }


}
