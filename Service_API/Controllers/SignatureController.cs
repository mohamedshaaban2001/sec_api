using Contracts.DTOs.Signature;
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
public class SignatureController : BaseController<Signature, SignatureDto, SignatureCreateDto, SignatureUpdateDto>
{
    private readonly ILoggerManager _logger;
    private readonly IRepositoryWrapper _repositoryWrapper;
    public SignatureController(       
        ILoggerManager logger,
        IRepositoryWrapper repositoryWrapper)
        : base()
    {
        _logger = logger;
        _repositoryWrapper = repositoryWrapper;
        _repository = _repositoryWrapper.Signatures;
    }



}
