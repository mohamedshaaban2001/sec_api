using System.Reflection;

namespace Repositories.TreeService.ReflectionHelpers;

public static class ReflectionHelpers
{
    public static string GetTableName<T>()
    {
        var attr = typeof(T).GetCustomAttribute<TableNameAttribute>();
        if (attr == null)
            throw new InvalidOperationException($"No [TableName] attribute defined on {typeof(T).Name}.");

        return attr.Name; 
    }
    public static Dictionary<string, string> GetColumnMappings<T>()
    {
        var dict = new Dictionary<string, string>();

        var props = typeof(T).GetProperties();
        foreach (var prop in props)
        {
            var colAttr = prop.GetCustomAttribute<ColumnNameAttribute>();
            string columnName = colAttr != null
                ? colAttr.Name 
                : prop.Name.ToSnakeCase();

            dict[prop.Name] = columnName;
        }

        return dict;
    }
}
