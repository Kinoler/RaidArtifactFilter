using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RaidArtifactsFilter
{
    public class Configuration
    {
        private static string ConfigurationPath = "Configuration.json";

        public static Configuration Instance { get; }

        static Configuration()
        {
            if (!File.Exists(ConfigurationPath))
            {
                throw new Exception($"Configuration file isn't found at {Path.Combine(Directory.GetCurrentDirectory(), ConfigurationPath)}");
            }
            var configurationStr = File.ReadAllText(ConfigurationPath);
            Instance = JsonConvert.DeserializeObject<Configuration>(configurationStr);
        }

        public Dictionary<string, string> ArgumentStatTransfer { get; set; }
        public Dictionary<string, string> ArgumentArtifactTypeTransfer { get; set; }
        public Dictionary<string, string> ArtifactFractionTransfer { get; set; }
        public Dictionary<string, string> ArgumentArtifactSetKindTransfer { get; set; }
        public Dictionary<string, int> ArtifactRarityTransfer { get; set; }
        public Dictionary<string, int> ArtifactTypeTransfer { get; set; }
        public Dictionary<string, int> ArtifactStatTransfer { get; set; }

        public Dictionary<string, Color> ArtifactRarityColorsTransfer = new()
        {
            { "Common", Color.FromArgb(0xFF, 0xFF, 0xFF) },
            { "Uncommon", Color.FromArgb(0x0, 0x80, 0x0) },
            { "Rare", Color.FromArgb(0x1E, 0x90, 0xFF) },
            { "Epic", Color.FromArgb(0x99, 0x32, 0xCC) },
            { "Legendary", Color.FromArgb(0xFF, 0xB0, 0x00) },
        };
    }
}
