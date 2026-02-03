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
public class SecGroupPageController : BaseController<SecGroupPage, SecGroupPageDto, SecGroupPageCreateDto, SecGroupPageUpdateDto>
{
    private readonly ILoggerManager _logger;
    private readonly IRepositoryWrapper _repositoryWrapper;
    public SecGroupPageController(       
        ILoggerManager logger,
        IRepositoryWrapper repositoryWrapper)
        : base()
    {
        _logger = logger;
        _repositoryWrapper = repositoryWrapper;
        _repository = _repositoryWrapper.SecGroupPages;
    }

    [HttpGet("GetLookupsForCreatePage")]
    public async Task<IActionResult> GetLookupsForCreatePage()
    {
        var response = await _repositoryWrapper.SecPages.GetLookupsForCreatePage();
        return HandleResponse(response);
    }

    [HttpGet("FindPagesBasedOnGroupId/{groupId}")]
    public async Task<IActionResult> FindPagesBasedOnGroupId(int groupId)
    {
        var response = await _repositoryWrapper.SecGroupPages.FindPagesBasedOnGroupId(groupId);
        return HandleResponse(response);
    }

    [HttpPost("AssignControlsToPageInGroup")]
    public async Task<IActionResult> AssignControlsToPageInGroup([FromBody]AssignControlsToPageInGroup assignControlsToPageInGroup)
    {
        var response = await _repositoryWrapper.SecGroupPages.AssignControlsToPageInGroup(assignControlsToPageInGroup);
        return HandleResponse(response);
    }

    [HttpPost("AssignDeletePageFromGroup")]
    public async Task<IActionResult> AssignDeletePageFromGroup([FromBody] AssignDeletePageFromGroup assignDeletePageFromGroup )
    {
        var response = await _repositoryWrapper.SecGroupPages.AssignDeletePageFromGroup(assignDeletePageFromGroup);
        return HandleResponse(response);
    }
    [HttpGet("GetPagesForLookup")]
    public async Task<IActionResult> GetPagesForLookup()
    {
        var response = await _repositoryWrapper.SecPages.GetPagesForLookup();
        return HandleResponse(response);
    }

}
