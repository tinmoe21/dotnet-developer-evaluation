using System.Xml.Serialization;

namespace WebApplication1.Models
{
    [XmlRoot(ElementName = "Data")]
    public class CompanyXMLModel
    {
        [XmlElement(ElementName = "id")]
        public int Id { get; set; }

        [XmlElement(ElementName = "name")]
        public string? Name { get; set; }

        [XmlElement(ElementName = "description")]
        public string? Description { get; set; }
    }
}
