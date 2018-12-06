using System.Collections.Generic;

namespace PlcCommunication.Config
{
    public class Group
    {
        public string GroupName { get; set; }

        public List<Tag> Tags { get; set; }
    }
}
