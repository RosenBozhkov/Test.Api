using inacs.v8.nuget.DevAttributes;
using Microsoft.Extensions.Primitives;

namespace Config.Extensions;

[Developer("Vasil Egov", "v.egov@itsoft.bg")]
internal static class StringValuesExtension
{
    internal static bool IsEmpty(this StringValues values)
    {
        return values.Count == 0;
    }
}