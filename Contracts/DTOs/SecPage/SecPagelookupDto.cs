using Contracts.DTOs.SecGroup;
using Contracts.DTOs.SecModule;
using Contracts.DTOs.SecService;

namespace Contracts.DTOs.SecPage;

public class SecPagelookupDto 
{
    public List<SecModuleSmallDto> Modules { get; set; } 
    public List <SecGroupSmallDto> Groups { get; set; }
    public List <SecServiceDto> Services { get; set; }

}
