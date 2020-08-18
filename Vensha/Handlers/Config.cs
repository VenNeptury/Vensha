using System.IO;
using Vensha.DataStructures;
using Newtonsoft.Json;

namespace Vensha.Handlers
{
    public class Constants
    {
        public static Config Config { get; set; }

        public static void Initialise() => Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
    }
}