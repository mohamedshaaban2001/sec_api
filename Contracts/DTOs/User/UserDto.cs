using Contracts.BaseDtos;

namespace Contracts.DTOs.User;

public class UserDto:BaseDto
{
    public string UserName { get; set; } = null!;

    public int? UserActivation { get; set; }

    public string? Sign { get; set; }


    public int? EmpSerial { get; set; }
}
