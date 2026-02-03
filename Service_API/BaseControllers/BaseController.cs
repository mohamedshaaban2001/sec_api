using Contracts.BaseDtos;
using Contracts.enums;
using Contracts.interfaces.Repository;
using Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
namespace Service_API.BaseControllers;




[ApiController]
[Route("api/[controller]")]
public abstract class BaseController<T, TDto, TCreateDto, TUpdateDto> : ControllerBase
        where T : Entities.Models.BaseTables.BaseTable
        where TDto : BaseDto
        where TCreateDto : BaseCreateDto
        where TUpdateDto : BaseUpdateDto
{
    protected IRepositoryBase<T, TDto, TCreateDto, TUpdateDto> _repository;
    [HttpGet("{id}")]

    public virtual async Task<IActionResult> GetById(int id)
    {
        var response = await _repository.FindById(id);
        return HandleResponse(response);
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetAll()
    {
        var response = await _repository.FindAll();
        return HandleResponse(response);
    }

    //[HttpGet("Paged")]
    //public virtual async Task<IActionResult> GetAllPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    //{
    //    var response = await _repository.FindAllPaged(page, pageSize);
    //    return HandleResponse(response);
    //}

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] TCreateDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _repository.Create(createDto);
        return HandleResponse(response);
    }

    [HttpPut]
    public virtual async Task<IActionResult> Update([FromBody] TUpdateDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _repository.Update(updateDto);
        return HandleResponse(response);
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(int id, [FromQuery] bool softDelete = true)
    {
        var response = await _repository.Delete(id, softDelete);
        return HandleResponse(response);
    }

    protected IActionResult HandleResponse(ParentResponseModel response)
    {
        if (response.IsDone)
        {
            return Ok(response);
        }

        return response.ErrorCode switch
        {
            ErrorCatalog.ObjectNotFound => NotFound(response), // 404 Not Found
            ErrorCatalog.DataBaseFauiler => StatusCode(500, response), // 500 Internal Server Error
            ErrorCatalog.missingValues => BadRequest(response), // 400 Bad Request
            ErrorCatalog.ConnectionLost => StatusCode(503, response), // 503 Service Unavailable
            _ => BadRequest(response) // Default to 400 Bad Request for unhandled cases
        };
    }
}
