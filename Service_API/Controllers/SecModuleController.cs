using System.Reflection;
using Contracts.DTOs.SecModule;
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
public class SecModuleController : BaseController<SecModule, SecModuleDto, SecModuleCreateDto, SecModuleUpdateDto>
{
    private readonly ILoggerManager _logger;
    private readonly IRepositoryWrapper _repositoryWrapper;
    public SecModuleController(       
        ILoggerManager logger,
        IRepositoryWrapper repositoryWrapper)
        : base()
    {
        _logger = logger;
        _repositoryWrapper = repositoryWrapper;
        _repository = _repositoryWrapper.SecModules;
    }

    [HttpGet("GetServicesBasedOnModule/{moduleId}")]
    public  async Task<IActionResult> GetServicesBasedOnModule(int moduleId)
    {
        var response = await _repositoryWrapper.SecModules.GetServicesBasedOnModule(moduleId);
        return HandleResponse(response);
    }

    [HttpPost("CreateServicesForModules")]
    public  async Task<IActionResult> CreateServicesForModules([FromBody] AssignServiceForModule assignServiceForModule)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _repositoryWrapper.SecModules.CreateServicesForModules(assignServiceForModule);
        return HandleResponse(response);
    }

    [HttpDelete("DeleteSpecificServiceInModule/{serviceId}")]
    public  async Task<IActionResult> DeleteSpecificServiceInModule(int serviceId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _repositoryWrapper.SecServices.Delete(serviceId);
        return HandleResponse(response);
    }

    [HttpGet("GetModulesWithTakenServices")]
    public async Task<IActionResult> GetModulesWithTakenServices()
    {
        var response = await _repositoryWrapper.SecModules.GetModulesWithTakenServices();
        return HandleResponse(response);
    }
    [HttpPost("AssignTakenToServiceInModule")]
    public async Task<IActionResult> AssignTakenToServiceInModule([FromBody] AssignTakenServiceForModule assignTakenServiceForModule )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _repositoryWrapper.SecModules.AssignTakenToServiceInModule(assignTakenServiceForModule);
        return HandleResponse(response);
    }



}
