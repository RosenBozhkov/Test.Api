using inacs.v8.nuget.DevAttributes;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

#nullable disable warnings

namespace Common.Helpers;

/// <summary>
/// Xml helper
/// </summary>
[Developer("Daniel Tanev", "daniel.tanev@itsoft.bg")]
public static class XmlHelper
{
    /// <summary>
    /// Deserialize a XML from string
    /// </summary>
    /// <param name="xml"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? Deserialize<T>(string xml) where T : class
    {
        XmlSerializer deserializer = new(typeof(T));
        using TextReader reader = new StringReader(xml);
        return deserializer.Deserialize(reader) as T;
    }

    /// <summary>
    /// Deserialize XML from XDocument
    /// </summary>
    /// <param name="xml"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? Deserialize<T>(XDocument xml) where T : class => Deserialize<T>(xml.ToString());

    /// <summary>
    /// Deserialize xml from XElement
    /// </summary>
    /// <param name="xml"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? Deserialize<T>(XElement xml) where T : class => Deserialize<T>(xml.ToString());

    /// <summary>
    /// Deserialize section from XDocument
    /// </summary>
    /// <param name="xDoc"></param>
    /// <param name="currentSectionName"></param>
    /// <param name="expectedSectionName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? DeserializeXmlSection<T>(XDocument xDoc, string? currentSectionName = null,
        string? expectedSectionName = null) where T : class
    {
        XDocument section;

        if (currentSectionName is null)
        {
            section = new XDocument(xDoc.Root!);
        }
        else
        {
            section = new XDocument(xDoc.Root!.Element(currentSectionName)!);
        }

        if (!section.Descendants().Any())
        {
            return null;
        }

        section.Root!.Name = expectedSectionName ?? typeof(T).Name;
        return Deserialize<T>(section);
    }
}