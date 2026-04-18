using System.ComponentModel.DataAnnotations;
using Contracts.BaseDtos;

namespace Contracts.DTOs.Privilege;

public class PrivilegeUpdateDto : BaseUpdateDto
{
    [Required]
    public string Code { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;
}
