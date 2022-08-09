using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using inacs.v8.nuget.DevAttributes;

namespace Config.Extensions;

[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
[Developer("Vasil Egov", "v.egov@itsoft.bg")]
internal static class FiltersHelper
{
    private const string Dashes = "--";
    private const string MultipartFormData = "multipart/form-data";

    internal static async Task<string> GetRequestBodyAsync(HttpContext context)
    {
        context.Request.Body.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 8192, true);
            
        string body = await reader.ReadToEndAsync();
        string contentType = context.Request.ContentType!;
        if (string.IsNullOrEmpty(contentType) || !contentType.StartsWith(MultipartFormData))
        {
            return body;
        }

        var boundary = contentType
            .Split("; boundary=")[1];
        body = RemoveBlobFiles(boundary, body);

        context.Request.Body.Position = 0;

        return body;
    }

    /// <summary>
    /// Removes the blob binary information from the body, as to not generate any further logs
    /// </summary>
    /// <param name="boundary"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    private static string RemoveBlobFiles(string boundary, string body)
    {
        // Checks if the file is a json. If so, keeps the body, if not, omits it.
        string boundaryValue = Dashes + boundary.Replace("-", string.Empty);
        string endValue = boundaryValue + Dashes;

        StringBuilder updatedBody = new();
        StringReader reader = new(body);

        string? line = reader.ReadLine();
        bool isJson = true;
        while (line is not null)
        {
            if (line.EndsWith(endValue, StringComparison.Ordinal))
            {
                updatedBody.Append(line);
                break;
            }

            if (line.EndsWith(boundaryValue, StringComparison.Ordinal))
            {
                updatedBody.Append(line).AppendLine();
                updatedBody.Append(reader.ReadLine()).AppendLine();
                line = reader.ReadLine();
                isJson = line?.Contains(MediaTypeNames.Application.Json) ?? true;
                if (!isJson)
                {
                    updatedBody.Append(line).AppendLine();
                }
            }

            if (isJson)
            {
                updatedBody.Append(line).AppendLine();
            }

            line = reader.ReadLine();
        }

        return updatedBody.ToString();
    }

    internal static IDictionary<string, object> GetRequestHeaders(HttpContext context)
    {
        if (context.Request.Headers is null)
        {
            return new Dictionary<string, object>();
        }

        var headers = new Dictionary<string, object>();
        foreach (string header in context.Request.Headers.Keys)
        {
            context.Request.Headers.TryGetValue(header, out var values);
            headers.Add(header, values);
        }

        return headers;
    }

    internal static IDictionary<string, object> GetResponseHeaders(HttpContext context)
    {
        if (context.Response.Headers is null)
        {
            return new Dictionary<string, object>();
        }

        var headers = new Dictionary<string, object>();
        foreach (string header in context.Response.Headers.Keys)
        {
            context.Response.Headers.TryGetValue(header, out var values);
            headers.Add(header, values);
        }

        return headers;
    }

    internal static string GetHeadersAsJsonString(Dictionary<string, object>? headers)
    {
        if (headers is null || headers.Count == 0)
        {
            return string.Empty;
        }

        JObject headersObject = new();
        foreach (var header in headers)
        {
            headersObject.Add(new JProperty(header.Key, header.Value.ToString()));
        }

        return JsonConvert.SerializeObject(headersObject);
    }
}