using Contracts.DTOs.SecService;
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
public class SecServiceController : BaseController<SecService, SecServiceDto, SecServiceCreateDto, SecServiceUpdateDto>
{
    private readonly ILoggerManager _logger;
    private readonly IRepositoryWrapper _repositoryWrapper;
    public SecServiceController(       
        ILoggerManager logger,
        IRepositoryWrapper repositoryWrapper)
        : base()
    {
        _logger = logger;
        _repositoryWrapper = repositoryWrapper;
        _repository = _repositoryWrapper.SecServices;
    }



}
