using Contracts.interfaces.Models;
using Entities.Models.Tables;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Repositories.Repositories;
using Contracts.DTOs.User;
using Mapster;
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

    public override async Task<ParentResponseModel> Create(UserCreateDto entityCreate)
    {
        try
        {
            var entity = entityCreate.Adapt<User>();
            if (string.IsNullOrWhiteSpace(entity.Sign))
                entity.Sign = null;

            string userCode = _httpContextAccessor?.HttpContext?.User.FindFirst("EMP_SERIAL")?.Value;
            entity.InsertUserCode = !string.IsNullOrEmpty(userCode) ? userCode : "no create user code detected";
            entity.InsertDate = DateTime.Now;
            entity.IsDeleted = false;
            await RepositoryContext.Set<User>().AddAsync(entity);
            await RepositoryContext.SaveChangesAsync();
            return new SingleObjectResponseModel<UserDto>()
            {
                ErrorCode = ErrorCatalog.noError,
                SingleObject = entity.Adapt<UserDto>(),
                IsDone = true,
                ReturnMessage = "Object Added Successufly"
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(User).Name} ===> Create ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }

    public async Task<ParentResponseModel> GetUsersManagementPageData()
    {
        try
        {
            var users = await (from u in RepositoryContext.Set<User>()
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

            var usedPersonIds = await RepositoryContext.Set<User>()
                .AsNoTracking()
                .Where(u => u.IsDeleted == false && u.EmpSerial != null)
                .Select(u => u.EmpSerial!.Value)
                .Distinct()
                .ToListAsync();

            var usedSet = usedPersonIds.ToHashSet();

            var personsWithoutUser = await RepositoryContext.Persons.AsNoTracking()
                .Where(p => !usedSet.Contains(p.Id))
                .OrderBy(p => p.FullName)
                .Select(p => new PersonWithoutUserDto
                {
                    Id = p.Id,
                    FullName = p.FullName
                })
                .ToListAsync();

            var payload = new UsersManagementPageDto
            {
                Users = users,
                PersonsWithoutUser = personsWithoutUser
            };

            return new SingleObjectResponseModel<UsersManagementPageDto>()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Users and available persons loaded successfully",
                SingleObject = payload
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(User).Name} ===> GetUsersManagementPageData ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
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
            _logger.logErrorWithException(ex, $"{typeof(User).Name} ===> GetUsers ");
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
            _logger.logErrorWithException(ex, $"{typeof(User).Name} ===> ResetPassword ");
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

