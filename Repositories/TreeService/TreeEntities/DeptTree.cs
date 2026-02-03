namespace Repositories.TreeService.TreeEntities;


[TableName("depts")]
public class DeptTree : BaseTree
{
    [ColumnName("dept_n")]
    public string Name { get; set; }

}