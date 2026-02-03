using Entities.Models.BaseTables;

namespace Entities.Models.Tables;

public class Signature : BaseTable
{
    public string SignatureColor { get; set; }
    public int EmployeeId { get; set; }

}