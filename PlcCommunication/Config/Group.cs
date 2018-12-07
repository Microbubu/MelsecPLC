using System.Collections.Generic;
using System.Xml.Serialization;

namespace PlcCommunication.Config
{
    [XmlRoot]
    public class Group
    {
        [XmlAttribute]
        public string GroupName { get; set; }

        public List<Tag> Tags { get; set; }
    }
}
