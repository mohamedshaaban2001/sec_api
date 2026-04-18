using Contracts.DTOs.Privilege;
using Contracts.DTOs.SecControlList;
using Contracts.enums;
using Contracts.interfaces.Repository;
using Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Repositories.Repositories;
using Service_API.Services;

namespace Service_API.Controllers;

/// <summary>
/// Privilege (action) management. Persists to SEC_CONTROLS_LIST; each privilege belongs to one page.
/// </summary>
[ApiController]
[Route("[controller]")]
public class PrivilegesController : ControllerBase
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IKeycloakService _keycloakService;

    public PrivilegesController(
        IRepositoryWrapper repositoryWrapper,
        IKeycloakService keycloakService)
    {
        _repositoryWrapper = repositoryWrapper;
        _keycloakService = keycloakService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? pageId)
    {
        var response = pageId is > 0
            ? await _repositoryWrapper.SecControlLists.FindAllForPage(pageId.Value)
            : await _repositoryWrapper.SecControlLists.FindAll();

        if (!response.IsDone)
            return HandleRepositoryResponse(response);

        if (response is not ListOfObjectsResponseModel<SecControlListDto> listResp || listResp.Objects == null)
            return BadRequest(response);

        return Ok(new ListOfObjectsResponseModel<PrivilegeDto>
        {
            ErrorCode = listResp.ErrorCode,
            IsDone = listResp.IsDone,
            ReturnMessage = listResp.ReturnMessage,
            Objects = listResp.Objects.Select(Map).ToList()
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _repositoryWrapper.SecControlLists.FindById(id);
        if (!response.IsDone)
            return HandleRepositoryResponse(response);

        if (response is not SingleObjectResponseModel<SecControlListDto> single || single.SingleObject == null)
            return BadRequest(response);

        return Ok(new SingleObjectResponseModel<PrivilegeDto>
        {
            ErrorCode = single.ErrorCode,
            IsDone = single.IsDone,
            ReturnMessage = single.ReturnMessage,
            SingleObject = Map(single.SingleObject)
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PrivilegeCreateDto body)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createDto = new SecControlListCreateDto
        {
            PageId = body.PageId,
            ControlCode = body.Code,
            ControlDescription = body.Description
        };

        var response = await _repositoryWrapper.SecControlLists.Create(createDto);

        if (response.IsDone && !string.IsNullOrWhiteSpace(body.Code))
            await _keycloakService.CreateRealmRoleAsync(body.Code, body.Description);

        if (!response.IsDone)
            return HandleRepositoryResponse(response);

        if (response is SingleObjectResponseModel<SecControlListDto> single && single.SingleObject != null)
        {
            return Ok(new SingleObjectResponseModel<PrivilegeDto>
            {
                ErrorCode = single.ErrorCode,
                IsDone = single.IsDone,
                ReturnMessage = single.ReturnMessage,
                SingleObject = Map(single.SingleObject)
            });
        }

        return HandleRepositoryResponse(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] PrivilegeUpdateDto body)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var current = await GetControlByIdAsync(body.Id);
        var updateDto = new SecControlListUpdateDto
        {
            Id = body.Id,
            ControlCode = body.Code,
            ControlDescription = body.Description
        };

        var response = await _repositoryWrapper.SecControlLists.Update(updateDto);

        if (response.IsDone &&
            current != null &&
            !string.IsNullOrWhiteSpace(current.ControlCode) &&
            !string.IsNullOrWhiteSpace(body.Code))
        {
            await _keycloakService.UpdateRealmRoleAsync(
                current.ControlCode,
                body.Code,
                body.Description);
        }

        return HandleRepositoryResponse(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, [FromQuery] bool softDelete = true)
    {
        var current = await GetControlByIdAsync(id);
        var response = await _repositoryWrapper.SecControlLists.Delete(id, softDelete);

        if (response.IsDone &&
            current != null &&
            !string.IsNullOrWhiteSpace(current.ControlCode))
        {
            await _keycloakService.DeleteRealmRoleAsync(current.ControlCode);
        }

        return HandleRepositoryResponse(response);
    }

    private static PrivilegeDto Map(SecControlListDto x) => new()
    {
        Id = x.Id,
        PageId = x.PageId,
        Code = x.ControlCode,
        Description = x.ControlDescription
    };

    private async Task<SecControlListDto?> GetControlByIdAsync(int id)
    {
        var controlResponse = await _repositoryWrapper.SecControlLists.FindById(id);
        if (!controlResponse.IsDone)
            return null;

        if (controlResponse is SingleObjectResponseModel<SecControlListDto> singleResponse)
            return singleResponse.SingleObject;

        return null;
    }

    private IActionResult HandleRepositoryResponse(ParentResponseModel response)
    {
        if (response.IsDone)
            return Ok(response);

        return response.ErrorCode switch
        {
            ErrorCatalog.ObjectNotFound => NotFound(response),
            ErrorCatalog.DataBaseFauiler => StatusCode(500, response),
            ErrorCatalog.missingValues => BadRequest(response),
            ErrorCatalog.ConnectionLost => StatusCode(503, response),
            _ => BadRequest(response)
        };
    }
}
