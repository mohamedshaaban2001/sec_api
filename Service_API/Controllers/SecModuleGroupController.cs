using Contracts.DTOs.SecModuleGroup;
using Contracts.interfaces.Models;
using Contracts.interfaces.Repository;
using Entities.Models.Tables;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Repositories.Models;
using Repositories.Repositories;
using Service_API.BaseControllers;

namespace Service_API.Controllers;

[ApiController]
[Route("[controller]")]
public class SecModuleGroupController : BaseController<SecModuleGroup, SecModuleGroupDto, SecModuleGroupCreateDto, SecModuleGroupUpdateDto>
{
    private readonly ILoggerManager _logger;
    private readonly IRepositoryWrapper _repositoryWrapper;
    public SecModuleGroupController(       
        ILoggerManager logger,
        IRepositoryWrapper repositoryWrapper)
        : base()
    {
        _logger = logger;
        _repositoryWrapper = repositoryWrapper;
        _repository = _repositoryWrapper.SecModuleGroups;
    }



}
