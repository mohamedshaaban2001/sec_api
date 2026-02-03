
using System;
namespace Repositories.TreeService;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class TableNameAttribute : Attribute
{
    public string Name { get; }
    public TableNameAttribute(string name) => Name = name;
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class ColumnNameAttribute : Attribute
{
    public string Name { get; }
    public ColumnNameAttribute(string name) => Name = name;
}



