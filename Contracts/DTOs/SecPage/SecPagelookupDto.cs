using Contracts.DTOs.SecGroup;
using Contracts.DTOs.SecService;

namespace Contracts.DTOs.SecPage;

/// <summary>
/// Lookups for page administration. Modules are no longer part of the public model.
/// </summary>
public class SecPagelookupDto
{
    public List<SecGroupSmallDto> Groups { get; set; } = null!;
    public List<SecServiceDto> Services { get; set; } = null!;
}
