using Contracts.DTOs.SecGroup;
using Contracts.DTOs.SecGroupPage;
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
public class SecGroupController : BaseController<SecGroup, SecGroupDto, SecGroupCreateDto, SecGroupUpdateDto>
{
    private readonly ILoggerManager _logger;
    private readonly IRepositoryWrapper _repositoryWrapper;
    public SecGroupController(       
        ILoggerManager logger,
        IRepositoryWrapper repositoryWrapper)
        : base()
    {
        _logger = logger;
        _repositoryWrapper = repositoryWrapper;
        _repository = _repositoryWrapper.SecGroups;
    }

    [HttpGet("AvailableModulesWithGroup/{groupId}")]
    public  async Task<IActionResult> AvailableModulesWithGroup(int groupId)
    {
        var response = await _repositoryWrapper.SecGroups.AvailableModulesWithGroup(groupId);
        return HandleResponse(response);
    }
    [HttpGet("GetModulesForLookups")]
    public async Task<IActionResult> GetModulesForLookups()
    {
        var response = await _repositoryWrapper.SecGroups.GetModulesForLookups();
        return HandleResponse(response);
    }

    [HttpGet("GetGroupsWithEmployeesAndJobsBasedOnModule/{moduleId}")]
    public  async Task<IActionResult> GetGroupsWithEmployeesAndJobsBasedOnModule(int moduleId)
    {
        var response = await _repositoryWrapper.SecGroups.GetGroupsWithEmployeesAndJobsBasedOnModule(moduleId);
        return HandleResponse(response);
    }
    [HttpGet("GetGroupsWithEmployeesAndJobs")]
    public async Task<IActionResult> GetGroupsWithEmployeesAndJobs()
    {
        var response = await _repositoryWrapper.SecGroupEmployees.GetGroupsWithEmployeesAndJobsFromGrpc();
        return HandleResponse(response);
    }

    [HttpPost("AssignJobsEmployeesToGroup")]
    public async Task<IActionResult> AssignJobsEmployeesToGroup([FromBody] AssignJobsEmployeesToGroup assignJobsEmployeesToGroup)
    {
        var response = await _repositoryWrapper.SecGroups.AssignJobsEmployeesToGroup( assignJobsEmployeesToGroup);
        return HandleResponse(response);
    }

    [HttpPost("DeleteEmployeeOrJobFromGroup")]
    public async Task<IActionResult> DeleteEmployeeOrJobFromGroup([FromBody] DeleteJobEmployeeFromGroup deleteJobEmployeeFromGroup )
    {
        var response = await _repositoryWrapper.SecGroups.DeleteEmployeeOrJobFromGroup(deleteJobEmployeeFromGroup);
        return HandleResponse(response);
    }


}
