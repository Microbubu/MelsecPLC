using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Text;

namespace PlcCommunication.Config
{
    public class Config
    {
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
            var text = JsonConvert.SerializeObject(config);
            File.WriteAllText(file, text);
        }

        public static Config DeserializeFromXml(string file)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            Config config = null;
            using (FileStream stream = File.OpenRead(file))
            {
                config = (Config)serializer.Deserialize(stream);
            }
            return config;
        }

        public static string SerializeToXml(Config config)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            StringWriter sw = new StringWriter();
            serializer.Serialize(sw, config);
            StringBuilder sb = sw.GetStringBuilder();
            return sb.ToString();
        }

        public static void SerializeXmlToFile(Config config, string file)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            using(FileStream stream = File.OpenWrite(file))
            {
                serializer.Serialize(stream, config);
            }
        }
    }
}
