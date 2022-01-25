using PepperDash.Essentials.Core;
using PepperDashPluginSymetrixComposer.Config;

namespace PepperDashPluginSymetrixComposer
{
    public class SymetrixComposerDspPreset : EssentialsDevice, IDspPreset
    {
        /// <summary>
        /// Preset number, ex. 'LP {PRESET_NUMBER}\r'
        /// </summary>
        public readonly int PresetNumber;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="key"></param>
        /// <param name="config"></param>
        public SymetrixComposerDspPreset(string key, PresetConfig config)
            : base(key, config.Label)
        {
            PresetNumber = config.PresetNumber;
            DeviceManager.AddDevice(this);
        }

        /// <summary>
        /// Builds the preset command with the provided present number
        /// </summary>
        /// <param name="presetNumber">integer</param>
        /// <returns>string</returns>
        public static string GetPresetCommand(int presetNumber)
        {
            return string.Format("LP {0}\r", presetNumber);
        }
    }
}