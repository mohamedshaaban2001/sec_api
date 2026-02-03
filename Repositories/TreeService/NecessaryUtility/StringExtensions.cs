using System.Text.RegularExpressions;

namespace Repositories.TreeService;

public static class StringExtensions
{
    public static string ToSnakeCase(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        return Regex.Replace(str, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
    public static string ToPascalCase(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        str = Regex.Replace(str, @"(?:^|_)([a-z])", match => match.Groups[1].Value.ToUpper());

        return char.ToUpper(str[0]) + str.Substring(1);
    }
}