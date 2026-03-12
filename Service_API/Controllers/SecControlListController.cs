using Contracts.DTOs.SecControlList;
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
public class SecControlListController : BaseController<SecControlList, SecControlListDto, SecControlListCreateDto, SecControlListUpdateDto>
{
    private readonly ILoggerManager _logger;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IKeycloakService _keycloakService;
    public SecControlListController(       
        ILoggerManager logger,
        IRepositoryWrapper repositoryWrapper,
        IKeycloakService keycloakService)
        : base()
    {
        _logger = logger;
        _repositoryWrapper = repositoryWrapper;
        _keycloakService = keycloakService;
        _repository = _repositoryWrapper.SecControlLists;
    }

    [HttpPost]
    public override async Task<IActionResult> Create([FromBody] SecControlListCreateDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _repository.Create(createDto);

        if (response.IsDone && !string.IsNullOrWhiteSpace(createDto.ControlCode))
        {
            await _keycloakService.CreateRealmRoleAsync(createDto.ControlCode, createDto.ControlDescription);
        }

        return HandleResponse(response);
    }

    [HttpPut]
    public override async Task<IActionResult> Update([FromBody] SecControlListUpdateDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var currentControl = await GetControlByIdAsync(updateDto.Id);
        var response = await _repository.Update(updateDto);

        if (response.IsDone &&
            currentControl != null &&
            !string.IsNullOrWhiteSpace(currentControl.ControlCode) &&
            !string.IsNullOrWhiteSpace(updateDto.ControlCode))
        {
            await _keycloakService.UpdateRealmRoleAsync(
                currentControl.ControlCode,
                updateDto.ControlCode,
                updateDto.ControlDescription
            );
        }

        return HandleResponse(response);
    }

    [HttpDelete("{id}")]
    public override async Task<IActionResult> Delete(int id, [FromQuery] bool softDelete = true)
    {
        var currentControl = await GetControlByIdAsync(id);
        var response = await _repository.Delete(id, softDelete);

        if (response.IsDone &&
            currentControl != null &&
            !string.IsNullOrWhiteSpace(currentControl.ControlCode))
        {
            await _keycloakService.DeleteRealmRoleAsync(currentControl.ControlCode);
        }

        return HandleResponse(response);
    }

    private async Task<SecControlListDto?> GetControlByIdAsync(int id)
    {
        var controlResponse = await _repository.FindById(id);
        if (!controlResponse.IsDone)
        {
            return null;
        }

        if (controlResponse is SingleObjectResponseModel<SecControlListDto> singleResponse)
        {
            return singleResponse.SingleObject;
        }

        return null;
    }

}
