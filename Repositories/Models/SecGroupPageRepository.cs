using Contracts.interfaces.Models;
using Entities.Models.Tables;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Repositories.Repositories;
using Contracts.DTOs.SecGroupPage;
using MapsterMapper;
using Grpc_Client;
using LoggerService;
using Contracts.DTOs.SecGroup;
using Contracts.enums;
using Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using Contracts.DTOs.SecControlList;
using Contracts.DTOs.SecPage;
using Mapster;

namespace Repositories.Models;

public class SecGroupPageRepository : RepositoryBase<SecGroupPage, SecGroupPageDto, SecGroupPageCreateDto, SecGroupPageUpdateDto>, ISecGroupPageRepository
{
    private readonly RepositoryContext _repositoryContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    //private readonly IStructureGrpcClient _structureGrpcClient;





    public SecGroupPageRepository(RepositoryContext repositoryContext, IHttpContextAccessor httpContextAccessor
        , IMapper mapper, ILoggerManager logger)
        : base(logger, repositoryContext, httpContextAccessor, mapper)
    {
        _repositoryContext = repositoryContext;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<ParentResponseModel> FindPagesBasedOnGroupId(int groupId)
    {
        try
        {
            var listOfObjects = await RepositoryContext.SecGroupPages.AsNoTracking().Where(e=>e.GroupCode==groupId).
                Select(e => new SecGroupPageDto
                {
                    Id = e.Id,
                    GroupId = e.GroupCode,
                    PageId = e.PageCode,
                    PageIcon = e.SecPage.Icon,
                    PageName = e.SecPage.PageName,
                    Controls = RepositoryContext.SecControlLists.Where(c => c.PageId == e.PageCode)
                            .Select(m => new ControlsForSpecificPageAndGroup
                            {
                                ControlId = m.Id,
                                ControlName = m.ControlDescription,
                                IsAssigned = m.SecGroupControls.Any(scp => scp.GroupCode == groupId && scp.PageCode == e.PageCode)
                            }).ToList()
                }).ToListAsync();


            return new ListOfObjectsResponseModel<SecGroupPageDto>()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Objects Loaded Successufly",
                Objects = listOfObjects
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecGroup).Name} ===> FindAll ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }

    public async Task<ParentResponseModel> AssignControlsToPageInGroup(AssignControlsToPageInGroup assignControlsToPageInGroup)
    {
        try
        {
            string userCode = _httpContextAccessor?.HttpContext?.User.FindFirst("EMP_SERIAL")?.Value;
            var oldControls = await RepositoryContext.SecGroupControls.Where(e => e.GroupCode == assignControlsToPageInGroup.GroupId &&
            e.PageCode == assignControlsToPageInGroup.PageId).ToListAsync();
            RepositoryContext.SecGroupControls.RemoveRange(oldControls);
            List<SecGroupControl> controls = new List<SecGroupControl>();
            if (assignControlsToPageInGroup.ControlIds != null && assignControlsToPageInGroup.ControlIds.Count > 0)
            {
                controls = assignControlsToPageInGroup.ControlIds.Select(controlId => new SecGroupControl
                {
                    GroupCode = assignControlsToPageInGroup.GroupId,
                    PageCode = assignControlsToPageInGroup.PageId,
                    ControlId = controlId
                }).ToList();
                await RepositoryContext.SecGroupControls.AddRangeAsync(controls);
            }
            await RepositoryContext.SaveChangesAsync();
            return new SingleObjectResponseModel<SecControlListDto>()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Assign Controls To Page In Group Seccessfully"
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecModule).Name} ===> Create ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }

    public async Task<ParentResponseModel> AssignDeletePageFromGroup(AssignDeletePageFromGroup assignDeletePageFromGroup)
    {
        try
        {
            string userCode = _httpContextAccessor?.HttpContext?.User.FindFirst("EMP_SERIAL")?.Value;


            if ( !assignDeletePageFromGroup.AssignFlag)
            {
                var oldAssignedPage = await RepositoryContext.SecGroupPages.FirstOrDefaultAsync(e => e.GroupCode == assignDeletePageFromGroup.GroupId &&
                e.PageCode == assignDeletePageFromGroup.DeletedPageId);
                var oldAssignedControls = await RepositoryContext.SecGroupControls
                            .Where(e => e.GroupCode == assignDeletePageFromGroup.GroupId && e.PageCode == assignDeletePageFromGroup.DeletedPageId).ToListAsync();
                foreach (var item in oldAssignedControls)
                {
                    item.DeleteUserCode = userCode;
                    item.DeleteDate = DateTime.Now;
                    item.IsDeleted = true;
                }
                oldAssignedPage.DeleteUserCode = userCode;
                oldAssignedPage.DeleteDate = DateTime.Now;
                oldAssignedPage.IsDeleted = true;
            }

            else if (assignDeletePageFromGroup.AssignFlag)
            {
                var newControls =assignDeletePageFromGroup.AssignedPageIds.Select(pageId=> new SecGroupPage
                {
                    GroupCode = assignDeletePageFromGroup.GroupId,
                    PageCode = pageId,
                    InsertDate = DateTime.Now,
                    InsertUserCode = userCode
                });
                await RepositoryContext.SecGroupPages.AddRangeAsync(newControls);
            }
            await RepositoryContext.SaveChangesAsync();
            return new SingleObjectResponseModel<SecControlListDto>()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Assign || Delete Page From Group Seccessfully"
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecModule).Name} ===> AssignDeletePageFromGroup ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }

}
