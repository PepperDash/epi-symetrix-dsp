using Newtonsoft.Json;

namespace PepperDashPluginSymetrixComposer.Config
{
    public class PresetConfig
    {
        [JsonProperty("label")]
        public string Label { get; set; }
        
        [JsonProperty("presetNumber")]
        public int PresetNumber { get; set; }
    }
}