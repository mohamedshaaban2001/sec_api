using Contracts.interfaces.Models;
using Entities.Models.Tables;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Repositories.Repositories;
using Contracts.DTOs.Signature;
using MapsterMapper;
using LoggerService;
using Contracts.enums;
using Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Repositories.Models;

public class SignatureRepository : RepositoryBase<Signature, SignatureDto, SignatureCreateDto, SignatureUpdateDto>, ISignatureRepository
{

    private readonly RepositoryContext _repositoryContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    //private readonly IStructureGrpcClient _structureGrpcClient;





    public SignatureRepository(RepositoryContext repositoryContext, IHttpContextAccessor httpContextAccessor
        , IMapper mapper, ILoggerManager logger)
        : base(logger, repositoryContext, httpContextAccessor, mapper)
    {
        _repositoryContext = repositoryContext;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _logger = logger;
    }

    public override async Task<ParentResponseModel> FindAll()
    {
        try
        {
            var signatures = await RepositoryContext.Signatures.AsNoTracking().ToListAsync();

            var employeeIdsInSignatures = signatures
                        .Select(s => s.EmployeeId) 
                        .Distinct()
                        .ToList();

            var employees = employeeIdsInSignatures.Any()
                ? await RepositoryContext.Persons.AsNoTracking()
                    .Where(p => employeeIdsInSignatures.Contains(p.Id))
                    .Select(p => new { p.Id, p.FullName })
                    .ToDictionaryAsync(p => p.Id, p => p.FullName)
                : new Dictionary<int, string>();

            // Step 4: Map data into DTOs
            var signatureDtos = signatures
                .Select(sig => new SignatureDto
                {
                    Id = sig.Id,
                    EmployeeId = sig.EmployeeId,
                    EmployeeName = employees.GetValueOrDefault(sig.EmployeeId, "Unknown"),
                    SignatureColor = sig.SignatureColor,
                    JobName = "Unknown", // user requested to skip linking job/dept
                    DepartmentName = "Unknown" 
                })
                .ToList();
            return new ListOfObjectsResponseModel<SignatureDto>()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Objects Loaded Successufly",
                Objects = signatureDtos
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SignatureDto).Name} ===> FindAll ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }


}
