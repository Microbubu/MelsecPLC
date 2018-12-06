using PlcCommunication.Common;

namespace PlcCommunication.Config 
{
    public class Tag : NotifyPropertyChange
    {
        private string _tagName;
        public string TagName
        {
            get => _tagName;
            set
            {
                _tagName = value;
                Notify(nameof(TagName));
            }
        }

        private string _deviceAddress;
        public string DeviceAddress
        {
            get => _deviceAddress;
            set
            {
                _deviceAddress = value;
                Notify(nameof(DeviceAddress));
            }
        }

        public bool Enable { get; set; }
    }
}
