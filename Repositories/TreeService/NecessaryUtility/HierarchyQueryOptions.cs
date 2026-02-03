namespace Repositories.TreeService;

public class HierarchyQueryOptions
{
    public string Delimiter { get; set; } = " / ";
    public bool PreventCycles { get; set; } = true;
    public bool TopDown { get; set; } = true;
    public int? StartId { get; set; }
    public bool StartFromRoot { get; set; } = true;
    public int? MaxDepth { get; set; } = null;

    public string? FilterColumn { get; set; }
    public string? FilterValue { get; set; }
    public string? FilterType { get; set; } = "Contains";

    public List<int>? Ids { get; set; } = new List<int>();

    public List<string>? StringValues { get; set; } = new List<string>();
}



