using Contracts.DTOs.SecGroup;
using Contracts.DTOs.SecGroupPage;
using Contracts.interfaces.Models;
using Contracts.interfaces.Repository;
using Contracts.Responses;
using Entities.Models.Tables;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Repositories.Models;
using Repositories.Repositories;
using Service_API.BaseControllers;
using Service_API.Services;

namespace Service_API.Controllers;

[ApiController]
[Route("[controller]")]
public class SecGroupController : BaseController<SecGroup, SecGroupDto, SecGroupCreateDto, SecGroupUpdateDto>
{
    private readonly ILoggerManager _logger;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IKeycloakService _keycloakService;
    public SecGroupController(       
        ILoggerManager logger,
        IRepositoryWrapper repositoryWrapper,
        IKeycloakService keycloakService)
        : base()
    {
        _logger = logger;
        _repositoryWrapper = repositoryWrapper;
        _keycloakService = keycloakService;
        _repository = _repositoryWrapper.SecGroups;
    }

    [HttpPost]
    public override async Task<IActionResult> Create([FromBody] SecGroupCreateDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _repository.Create(createDto);

        if (response.IsDone && !string.IsNullOrWhiteSpace(createDto.GroupName))
        {
            await _keycloakService.CreateGroupAsync(createDto.GroupName);
        }

        return HandleResponse(response);
    }

    [HttpPut]
    public override async Task<IActionResult> Update([FromBody] SecGroupUpdateDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var currentGroupName = await GetGroupNameByIdAsync(updateDto.Id);
        var response = await _repository.Update(updateDto);

        if (response.IsDone &&
            !string.IsNullOrWhiteSpace(currentGroupName) &&
            !string.IsNullOrWhiteSpace(updateDto.GroupName))
        {
            await _keycloakService.UpdateGroupAsync(currentGroupName, updateDto.GroupName);
        }

        return HandleResponse(response);
    }

    [HttpDelete("{id}")]
    public override async Task<IActionResult> Delete(int id, [FromQuery] bool softDelete = true)
    {
        var groupName = await GetGroupNameByIdAsync(id);
        var response = await _repository.Delete(id, softDelete);

        if (response.IsDone && !string.IsNullOrWhiteSpace(groupName))
        {
            await _keycloakService.DeleteGroupAsync(groupName);
        }

        return HandleResponse(response);
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

    private async Task<string?> GetGroupNameByIdAsync(int id)
    {
        var groupResponse = await _repository.FindById(id);
        if (!groupResponse.IsDone)
        {
            return null;
        }

        if (groupResponse is SingleObjectResponseModel<SecGroupDto> singleResponse)
        {
            return singleResponse.SingleObject?.GroupName;
        }

        return null;
    }

}
