using Contracts.DTOs.SecModule;
using Contracts.DTOs.SecPage;
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
public class SecPageController : BaseController<SecPage, SecPageDto, SecPageCreateDto, SecPageUpdateDto>
{
    private readonly ILoggerManager _logger;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IKeycloakService _keycloakService;
    public SecPageController(       
        ILoggerManager logger,
        IRepositoryWrapper repositoryWrapper,
        IKeycloakService keycloakService)
        : base()
    {
        _logger = logger;
        _repositoryWrapper = repositoryWrapper;
        _keycloakService = keycloakService;
        _repository = _repositoryWrapper.SecPages;
    }

    [HttpGet("GetLookupsForCreatePage")]
    public  async Task<IActionResult> GetLookupsForCreatePage()
    {
        var response = await _repositoryWrapper.SecPages.GetLookupsForCreatePage();
        return HandleResponse(response);
    }

    [HttpDelete("DeleteSpecificControlForPage/{controlId}")]
    public  async Task<IActionResult> DeleteSpecificControlForPage(int controlId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var controlCode = await GetControlCodeByIdAsync(controlId);
        var response = await _repositoryWrapper.SecControlLists.Delete(controlId);

        if (response.IsDone && !string.IsNullOrWhiteSpace(controlCode))
        {
            await _keycloakService.DeleteRealmRoleAsync(controlCode);
        }

        return HandleResponse(response);
    }

    [HttpPost("CreateControlForPage")]
    public async Task<IActionResult> CreateControlForPage([FromBody] AddControlToPage assignControlToPage)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var response = await _repositoryWrapper.SecPages.CreateControlForPage(assignControlToPage);

        if (response.IsDone && !string.IsNullOrWhiteSpace(assignControlToPage.ControlCode))
        {
            await _keycloakService.CreateRealmRoleAsync(assignControlToPage.ControlCode, assignControlToPage.ControlName);
        }

        return HandleResponse(response);
    }

    private async Task<string?> GetControlCodeByIdAsync(int controlId)
    {
        var controlResponse = await _repositoryWrapper.SecControlLists.FindById(controlId);
        if (!controlResponse.IsDone)
        {
            return null;
        }

        if (controlResponse is SingleObjectResponseModel<Contracts.DTOs.SecControlList.SecControlListDto> singleResponse)
        {
            return singleResponse.SingleObject?.ControlCode;
        }

        return null;
    }

}
