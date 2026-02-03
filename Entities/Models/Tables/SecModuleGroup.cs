using Entities.Models.BaseTables;

namespace Entities.Models.Tables;

public class SecModuleGroup : BaseTable
{
    public int ModuleCode { get; set; }
    public int GroupCode { get; set; }

    public virtual SecModule? SecModule { get; set; }
    public virtual SecGroup? SecGroup { get; set; }

}
