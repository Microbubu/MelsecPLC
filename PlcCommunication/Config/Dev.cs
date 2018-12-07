using System.Collections.Generic;
using System.Xml.Serialization;

namespace PlcCommunication.Config
{
    [XmlRoot]
    public class Dev
    {
        [XmlAttribute]
        public string DevName { get; set; }

        public List<Group> Groups { get; set; }
    }
}
