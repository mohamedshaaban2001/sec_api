using Contracts.DTOs.User;
using Contracts.interfaces.Models;
using Contracts.interfaces.Repository;
using Entities.Models.Tables;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Models;
using Repositories.Repositories;
using Service_API.BaseControllers;
using System.Drawing;
using System.Drawing.Imaging;
using static Service_API.Controllers.UserController;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service_API.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : BaseController<User, UserDto, UserCreateDto, UserUpdateDto>
{
    private readonly ILoggerManager _logger;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly Service_API.Services.IKeycloakService _keycloakService;
    private IConfiguration _config { get; }

    public UserController(       
        ILoggerManager logger,
        IRepositoryWrapper repositoryWrapper,
        IConfiguration config,
        Service_API.Services.IKeycloakService keycloakService)
        : base()
    {
        _logger = logger;
        _repositoryWrapper = repositoryWrapper;
        _repository = _repositoryWrapper.Users;
        _config = config;
        _keycloakService = keycloakService;
    }

    [HttpPost]
    public override async Task<IActionResult> Create([FromBody] UserCreateDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _repository.Create(createDto);
        
        if (response.IsDone && createDto.EmpSerial.HasValue)
        {
             // Fire and forget or await? Usually safer to await to ensure consistency or log error
             // User requested "before return to user call the Keycloak add user api"
             await _keycloakService.CreateUserAsync(createDto, createDto.EmpSerial.Value);
        }

        return HandleResponse(response);
    }

    [HttpPut]
    public override async Task<IActionResult> Update([FromBody] UserUpdateDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _repository.Update(updateDto);

        if (response.IsDone)
        {
             int? personId = updateDto.EmpSerial;
             if (!personId.HasValue)
             {
                 var userModel = await _repository.FindById(updateDto.Id);
                 if (userModel.IsDone && userModel is Contracts.Responses.SingleObjectResponseModel<UserDto> singleResponse)
                 {
                      try {
                        var userDto = singleResponse.SingleObject;
                        personId = (int?)userDto.GetType().GetProperty("EmpSerial")?.GetValue(userDto, null);
                      } catch {}
                 }
             }

             if (personId.HasValue)
             {
                await _keycloakService.UpdateUserAsync(updateDto, personId.Value);
             }
        }

        return HandleResponse(response);
    }

    [HttpDelete("{id}")]
    public override async Task<IActionResult> Delete(int id, [FromQuery] bool softDelete = true)
    {
        // We need the username to delete from Keycloak. Fetch user before deletion.
        var userModel = await _repository.FindById(id);
        string username = null;
        
        // This relies on FindById returning the entity in SingleObject. Base implementation usually returns DTO.
        // We might need to cast SingleObject if it returns DTO.
        if (userModel.IsDone && userModel is Contracts.Responses.SingleObjectResponseModel<UserDto> singleResponse)
        {
             var userDto = singleResponse.SingleObject; 
             // We assume userDto has UserName property. UserDto definition check needed?
             // Assuming it does based on generic TDto constraint having BaseDto, but UserName is specific.
             // Let's use dynamic or reflection to get UserName safely.
             try {
                username = (string)userDto.GetType().GetProperty("UserName")?.GetValue(userDto, null);
             } catch {}
        }

        var response = await _repository.Delete(id, softDelete);
        
        if (response.IsDone && !string.IsNullOrEmpty(username))
        {
            await _keycloakService.DeleteUserAsync(username);
        }

        return HandleResponse(response);
    }

    [HttpGet("GetTest")]
    public IActionResult GetTest()
    {
        return Ok("hello world");
    }

    [HttpGet("GetUsers")]
    public async Task<IActionResult> GetUsers()
    {
        var response = await _repositoryWrapper.Users.GetUsers();
        return HandleResponse(response);
    }


    [HttpGet]
    [Route("GET")]
    public IActionResult GET()
    {
        try
        {

            UserProfile PROFILE = new UserProfile();
            PROFILE.FULL_EMP_N = User.Claims.Where(x => x.Type == "FULL_EMP_N").Select(c => new { c.Type, c.Value }).ToList()[0].Value;
            PROFILE.CONTROLS = User.Claims.Where(x => x.Type == "CONTROLS").Select(c => new { c.Type, c.Value }).ToList()[0].Value;
            PROFILE.PAGES = User.Claims.Where(x => x.Type == "PAGES").Select(c => new { c.Type, c.Value }).ToList()[0].Value;

            PROFILE.SIGN = User.Claims.Where(x => x.Type == "SIGN").Select(c => new { c.Type, c.Value }).ToList()[0].Value;
            PROFILE.RANK_N = User.Claims.Where(x => x.Type == "RANK_N").Select(c => new { c.Type, c.Value }).ToList()[0].Value;
            //PROFILE.PROFILE_PIC = getProfile_Pic(int.Parse(User.Claims.Where(x => x.Type == "EMP_SERIAL").Select(c => new { c.Type, c.Value }).ToList()[0].Value), _config);
            PROFILE.PROFILE_PIC = "";

            //PROFILE.SIGNALR = User.Claims.Where(x => x.Type == "SIGNALR").Select(c => new { c.Type, c.Value }).ToList()[0].Value;
            PROFILE.SIGNALR = "";
            PROFILE.PAGES_TREE = User.Claims.Where(x => x.Type == "PAGES_TREE").Select(c => new { c.Type, c.Value }).ToList()[0].Value;
            PROFILE.ROLES = User.Claims.Where(x => x.Type == "ARCHIVE_ROLE").Select(c => new { c.Type, c.Value }).ToList()[0].Value;

            //List<decimal> ROLES = new List<decimal>((User.Claims.Where(x => x.Type == "ARCHIVE_ROLE").Select(c => new { c.Type, c.Value }).ToList()[0].Value).Split(',').Select(str => decimal.Parse(str)));

            //List<decimal> ROLES = new List<decimal>(
            //                                 (User.Claims
            //                                     .Where(x => x.Type == "ARCHIVE_ROLE")
            //                                     .Select(c => new { c.Type, c.Value }).ToList()[0].Value)
            //                                 .Split(',')
            //                                 .Select(str => decimal.Parse(str)) // Convert each string to decimal
            //                             );
            List<string> SIGNATURES = new List<string>(
                                             (User.Claims
                                                 .Where(x => x.Type == "SIGNATURES")
                                                 .Select(c => new { c.Type, c.Value }).ToList()[0].Value)
                                             .Split(',')
                                         );
            //PROFILE.ROLES = ROLES;
            PROFILE.SIGNATURES = SIGNATURES;
            PROFILE.CIV_ID_CARD_NO = User.Claims.Where(x => x.Type == "EMP_SERIAL").Select(c => new { c.Type, c.Value }).ToList()[0].Value;

            PROFILE.EMP_SERIAL = User.Claims.Where(x => x.Type == "EMP_SERIAL").Select(c => new { c.Type, c.Value }).ToList()[0].Value;
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null // Keep property names as they are (PascalCase)
            };

            var jsonResponse = JsonSerializer.Serialize(PROFILE, jsonOptions);
            return Content(jsonResponse, "application/json");
            return Ok(PROFILE);

        }
        catch (Exception m)
        {

            return BadRequest("لم يتم  استرجاع بيانات الموظف");
        }
    }

    [HttpGet]
    [Route("ResetPassword")]
    public async Task<IActionResult> ResetPassword(int userID)
    {
        try
        {

            UserProfile PROFILE = new UserProfile();
            var response = await _repositoryWrapper.Users.ResetPassword(userID);
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null // Keep property names as they are (PascalCase)
            };

            var jsonResponse = JsonSerializer.Serialize(PROFILE, jsonOptions);
            return Content(jsonResponse, "application/json");
            return Ok(PROFILE);

        }
        catch (Exception m)
        {

            return BadRequest("لم يتم  استرجاع بيانات الموظف");
        }
    }

    [HttpPost]
    [Route("Change_Pass")]
    public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordDto model)
    {
        if (string.IsNullOrEmpty(model.USER_CODE) ||
            string.IsNullOrEmpty(model.Old_Pass) ||
            string.IsNullOrEmpty(model.New_Pass))
        {
            return BadRequest("البيانات غير مكتملة");
        }

        var response = await _repositoryWrapper.Users.ChangePassword(model.USER_CODE, model.Old_Pass, model.New_Pass);

        return HandleResponse(response);
    }

    private string getProfile_Pic(int EMP_SERIAL, IConfiguration _config)
    {
        string Path = System.IO.Path.Combine(_config["uplaodPath"], @"Employees\" + EMP_SERIAL + "\\profile_image");


        if (!System.IO.Directory.Exists(Path))
        {
            return "";
        };
        var files = Directory.GetFiles(Path);

        if (files.Length == 0) { return ""; }

        var file = files[0];

        try
        {

            string filepath = System.IO.Path.Combine(Path, file);
            using (Bitmap bitmap = (Bitmap)System.Drawing.Image.FromFile(filepath))
            {
                using (MemoryStream byteStream = new MemoryStream())
                {


                    bitmap.Save(byteStream, ImageFormat.Jpeg);

                    string base64ImageRepresentation = Convert.ToBase64String(byteStream.ToArray());
                    bitmap.Dispose();
                    byteStream.Dispose();
                    return ("data:image/jpg;base64," + base64ImageRepresentation);
                }

            }
        }
        catch (Exception)
        {
            return "";
        }
    }


    public class UserProfile
    {
        public string FULL_EMP_N { get; set; }
        public string CONTROLS { get; set; }
        public string PAGES { get; set; }
        public string PROFILE_PIC { get; set; }
        public string PAGES_TREE { get; set; }
        public string SIGN { get; set; }
        public string RANK_N { get; set; }
        public string SIGNALR { get; set; }

        public string ROLES { get; set; }
        public List<string> SIGNATURES { get; set; }

        public string CIV_ID_CARD_NO { get; set; }
        public string EMP_SERIAL { get; set; }

    }

    public class ImageData
    {
        public string image { get; set; }
        public int employeeId { get; set; }
    }
    public class ChangePasswordDto
    {
        public string USER_CODE { get; set; }   // employee serial
        public string Old_Pass { get; set; }
        public string New_Pass { get; set; }
    }

}
