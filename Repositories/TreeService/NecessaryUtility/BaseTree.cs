namespace Repositories.TreeService;

public class BaseTree
{
    [ColumnName("id")]          
    public virtual int Id { get; set; }

    [ColumnName("parent_id")]  
    public virtual int? ParentId { get; set; }

    public virtual int? Level { get; set; }
    public virtual string? Path { get; set; }
    public virtual bool? IsLeaf { get; set; }


}
