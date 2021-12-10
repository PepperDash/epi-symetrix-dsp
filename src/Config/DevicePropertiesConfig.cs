using System.Collections.Generic;
using PepperDash.Core;
using PepperDash.Essentials.Core;
using SymetrixComposerEpi.Config;

namespace SymetrixComposerEpi
{
    public class DevicePropertiesConfig
    {
        public CommunicationMonitorConfig CommunicationMonitorProperties { get; set; }
        public ControlPropertiesConfig Control { get; set; }
        public Dictionary<string, FaderConfig> LevelControlBlocks { get; set; }
        public Dictionary<string, DialerConfig> DialerControlBlocks { get; set; }
        public Dictionary<string, PresetConfig> Presets { get; set; }
    }
}