namespace Contracts.DTOs.SecPage;

public class SecPageOrderItemDto
{
    public int Id { get; set; }
    public int PageOrder { get; set; }
}

/// <summary>
/// Apply many page order changes in one transaction (avoids unique index violations on rapid PUTs).
/// </summary>
public class ReorderSecPagesDto
{
    public List<SecPageOrderItemDto> Items { get; set; } = new();
}
