using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Text;

namespace PlcCommunication.Config
{
    [XmlRoot]
    public class Config
    {
        [XmlAttribute]
        public string ConfigName { get; set; }

        public List<Dev> Devs { get; set; }

        public static Config DeserializeFromJson(string file)
        {
            Config config = null;
            var text = File.ReadAllText(file);
            config = JsonConvert.DeserializeObject<Config>(text);
            return config;
        }

        public static string SerializeToJson(Config config)
        {
            return JsonConvert.SerializeObject(config);
        }

        public static void SerializeJsonToFile(Config config, string file)
        {
            if (!File.Exists(file)) return;

            var text = JsonConvert.SerializeObject(config);
            File.WriteAllText(file, text);
        }

        public static Config DeserializeFromXml(string file)
        {
            if (!File.Exists(file)) return null;

            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            using (FileStream fs = File.OpenRead(file))
            {
                StreamReader reader = new StreamReader(fs, Encoding.UTF8);
                return (Config)serializer.Deserialize(reader);
            }
        }

        public static string SerializeToXml(Config config)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            using (StringWriter sw = new StringWriter())
            {
                serializer.Serialize(sw, config);
                StringBuilder sb = sw.GetStringBuilder();
                return sb.ToString();
            } 
        }

        public static void SerializeXmlToFile(Config config, string file)
        {
            if (!File.Exists(file)) return;

            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            using (StringWriter sw = new StringWriter())
            {
                serializer.Serialize(sw, config);
                File.WriteAllText(file, sw.ToString(), Encoding.UTF8);
            }
        }
    }
}
