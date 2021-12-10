using System.Collections.Generic;
using System.Linq;
using PepperDash.Core;

namespace SymetrixComposerEpi.Utils
{
    public static class ConfigUtils
    {
        public static IEnumerable<SymetrixComposerLevelControl> BuildFaders(
            string key,
            DevicePropertiesConfig config,
            IBasicCommunication coms)
        {
            if (config.LevelControlBlocks == null)
                return new List<SymetrixComposerLevelControl>();

            return from faderConfig in config.LevelControlBlocks.Where(c => !c.Value.Disabled)
                select new SymetrixComposerLevelControl(key + "-" + faderConfig.Key, faderConfig.Value, coms);
        }

        public static IEnumerable<SymetrixComposerDspPreset> BuildPresets(
            string key,
            DevicePropertiesConfig config)
        {
            if (config.Presets == null)
                return new List<SymetrixComposerDspPreset>();

            return from presetConfig in config.Presets
                   select new SymetrixComposerDspPreset(key + "-" + presetConfig.Key, presetConfig.Value);
        }

        public static IEnumerable<SymetrixComposerDialer> BuildDialers(
           string key,
           DevicePropertiesConfig config,
            IBasicCommunication coms)
        {
            if (config.DialerControlBlocks == null)
                return new List<SymetrixComposerDialer>();

            return from dialerConfig in config.DialerControlBlocks
                   select new SymetrixComposerDialer(key + "-" + dialerConfig.Key, dialerConfig.Value, coms);
        }
    }
}