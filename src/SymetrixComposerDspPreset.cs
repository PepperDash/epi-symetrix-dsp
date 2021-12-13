using PepperDash.Essentials.Core;

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