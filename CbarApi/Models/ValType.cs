using System.Xml.Serialization;

namespace CbarApi.Models;

[XmlRoot(ElementName = "ValType")]
public class ValType
{
    [XmlElement(ElementName = "Valute")] public List<Valute> Valute { get; set; }
    [XmlAttribute(AttributeName = "Type")] public string Type { get; set; }
}