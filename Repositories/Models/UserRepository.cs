using Contracts.interfaces.Models;
using Entities.Models.Tables;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Repositories.Repositories;
using Contracts.DTOs.User;
using MapsterMapper;
using LoggerService;
using Contracts.DTOs.SecGroupPage;
using Contracts.enums;
using Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;


namespace Repositories.Models;

public class UserRepository : RepositoryBase<User, UserDto, UserCreateDto, UserUpdateDto>, IUserRepository
{
    private readonly RepositoryContext _repositoryContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;





    public UserRepository(RepositoryContext repositoryContext, IHttpContextAccessor httpContextAccessor
        , IMapper mapper, ILoggerManager logger)
        : base(logger, repositoryContext, httpContextAccessor, mapper)
    {
        _repositoryContext = repositoryContext;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<ParentResponseModel> GetUsers()
    {
        try
        {

            var listOfObjects = await (from u in RepositoryContext.Set<User>()
                                       where u.IsDeleted == false
                                       join p in RepositoryContext.Persons on u.EmpSerial equals p.Id into personJoin
                                       from p in personJoin.DefaultIfEmpty()
                                       select new UserNameDto
                                       {
                                           Id = u.Id,
                                           UserName = u.UserName,
                                           Sign = u.Sign,
                                           UserActivation = u.UserActivation,
                                           EmpSerial = u.EmpSerial,
                                           EmpName = p.FullName ?? string.Empty
                                       }).ToListAsync();

            var Object = new EmployeesAndJobs();
            Object.Employees = listOfObjects;


            return new SingleObjectResponseModel<EmployeesAndJobs>()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Objects loaded successfully from Persons view",
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

    public async Task<ParentResponseModel> ResetPassword( int userId)
    {
        try
        {

            var user = await RepositoryContext.Set<User>().FirstOrDefaultAsync(s => s.IsDeleted == false && s.Id == userId);
            user.UserPassword = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";
            RepositoryContext.SaveChanges();

            return new SingleObjectResponseModel<string>()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Objects Loaded Successufly From Grpc",
                SingleObject = { }
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

    public async Task<ParentResponseModel> ChangePassword(string userCode, string oldPassword, string newPassword)
    {
        try
        {
            var user = await RepositoryContext.Set<User>()
                .FirstOrDefaultAsync(u => u.EmpSerial.ToString() == userCode && u.IsDeleted == false);

            if (user == null)
            {
                return new ParentResponseModel()
                {
                    ErrorCode = ErrorCatalog.DataBaseFauiler,
                    IsDone = false,
                    ReturnMessage = "المستخدم غير موجود",
                };
            }

            if (user.UserPassword != HashPassword(oldPassword))
            {
                return new ParentResponseModel()
                {
                    ErrorCode = ErrorCatalog.DataBaseFauiler,
                    IsDone = false,
                    ReturnMessage = "كلمة المرور القديمة غير صحيحة",
                };
            }

            user.UserPassword = HashPassword(newPassword);
            RepositoryContext.SaveChanges();

            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "تم تغيير كلمة المرور بنجاح",
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(User).Name} ===> ChangePassword ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }

    public string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);

            // حول الهاش لـ HEX string (lowercase عشان يطابق اللي في الداتابيز)
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}

