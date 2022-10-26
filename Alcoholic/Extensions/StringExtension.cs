using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Alcoholic.Extensions
{
    public static class StringExtension
    {
        public static string ToQueryString(this object obj)
        {
            return string.Join("&", obj.GetType().GetProperties()
                .Where(x => x.GetValue(obj, null) != null)
                .Select(x => $"{x.Name}={x.GetValue(obj)}"));
        }
    }
}
