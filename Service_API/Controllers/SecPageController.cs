using Contracts.DTOs.SecModule;
using Contracts.DTOs.SecPage;
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
public class SecPageController : BaseController<SecPage, SecPageDto, SecPageCreateDto, SecPageUpdateDto>
{
    private readonly ILoggerManager _logger;
    private readonly IRepositoryWrapper _repositoryWrapper;
    public SecPageController(       
        ILoggerManager logger,
        IRepositoryWrapper repositoryWrapper)
        : base()
    {
        _logger = logger;
        _repositoryWrapper = repositoryWrapper;
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
        var response = await _repositoryWrapper.SecControlLists.Delete(controlId);
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
        return HandleResponse(response);
    }

}
