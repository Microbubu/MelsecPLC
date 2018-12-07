using PlcCommunication.Common;
using System.Xml.Serialization;

namespace PlcCommunication.Config 
{
    [XmlRoot]
    public class Tag : NotifyPropertyChange
    {
        private string _tagName;
        private string _deviceAddress;

        [XmlAttribute]
        public string TagName
        {
            get => _tagName;
            set
            {
                _tagName = value;
                Notify(nameof(TagName));
            }
        }

        [XmlAttribute]
        public string DeviceAddress
        {
            get => _deviceAddress;
            set
            {
                _deviceAddress = value;
                Notify(nameof(DeviceAddress));
            }
        }

        [XmlAttribute]
        public bool Enable { get; set; }
    }
}
