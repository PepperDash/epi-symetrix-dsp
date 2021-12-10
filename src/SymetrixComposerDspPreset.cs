using System;
using Crestron.SimplSharpPro.DeviceSupport;
using PepperDash.Core;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.Core.Bridges;

namespace SymetrixComposerEpi
{
    public class SymetrixComposerDspPreset : EssentialsDevice, IDspPreset
    {
        // LP <PRESET NUMBER><CR>
        public readonly int PresetNumber;

        public SymetrixComposerDspPreset(string key, PresetConfig config)
            : base(key, config.Label)
        {
            PresetNumber = config.PresetNumber;
            DeviceManager.AddDevice(this);
        }

        public static string GetPresetCommand(int presetNumber)
        {
            return string.Format("LP {0}\r", presetNumber);
        }
    }
}