using Contracts.BaseDtos;

namespace Contracts.DTOs.User;

public class UserNameDto:BaseDto
{
    public string UserName { get; set; } = null!;
    public string EmpName { get; set; } = null!;
    public int? UserActivation { get; set; }

    public string? Sign { get; set; }


    public int? EmpSerial { get; set; }
}
