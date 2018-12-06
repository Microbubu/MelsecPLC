using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace PlcCommunication.Config
{
    public class Config
    {
        public string ConfigName { get; set; }

        public List<Dev> Devs { get; set; }

        public static Config Deserialize(string file)
        {
            Config config = null;
            var text = File.ReadAllText(file);
            config = JsonConvert.DeserializeObject<Config>(text);
            return config;
        }

        public static string Serialize(Config config)
        {
            return JsonConvert.SerializeObject(config);
        }

        public static void Serialize(Config config, string path)
        {
            var text = JsonConvert.SerializeObject(config);
            File.WriteAllText(path, text);
        }
    }
}
