using Contracts.interfaces.Models;
using Entities.Models.Tables;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Repositories.Repositories;
using Contracts.DTOs.SecGroupEmployee;
using MapsterMapper;
using LoggerService;
using Contracts.Responses;
using Contracts.enums;
using Contracts.DTOs.SecGroupPage;
using Microsoft.EntityFrameworkCore;
using Entities.Models.Views;

namespace Repositories.Models;

public class SecGroupEmployeeRepository : RepositoryBase<SecGroupEmployee, SecGroupEmployeeDto, SecGroupEmployeeCreateDto, SecGroupEmployeeUpdateDto>, ISecGroupEmployeeRepository
{
    private readonly RepositoryContext _repositoryContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;






    public SecGroupEmployeeRepository(RepositoryContext repositoryContext, IHttpContextAccessor httpContextAccessor
        , IMapper mapper, ILoggerManager logger)
        : base(logger, repositoryContext, httpContextAccessor, mapper)
    {
        _repositoryContext = repositoryContext;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<ParentResponseModel> GetGroupsWithEmployeesAndJobsFromGrpc()
    {
        try
        {
            var employees = await RepositoryContext.Persons.AsNoTracking()
                .Select(p => new
                {
                    p.Id,
                    EmployeeName = p.FullName,
                    JobName = string.Empty,
                    DepartmentName = string.Empty,
                    JoinDate = string.Empty
                }).ToListAsync();

            var jobs = await RepositoryContext.Jobs.AsNoTracking().ToListAsync();

            var Object = new EmployeesAndJobs();
            Object.Employees = employees;
            Object.Jobs = jobs;

            return new SingleObjectResponseModel<EmployeesAndJobs>()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Objects Loaded Successufly From Views",
                SingleObject = Object
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecGroup).Name} ===> GetGroupsWithEmployeesAndJobsBasedOnModule ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }
}
