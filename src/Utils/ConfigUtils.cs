using System.Collections.Generic;
using PepperDash.Core;
using PepperDashPluginSymetrixComposer.Config;

namespace PepperDashPluginSymetrixComposer.Utils
{
    public static class ConfigUtils
    {
        public static IEnumerable<SymetrixComposerFader> BuildFaders(
            string key,
            DevicePropertiesConfig config,
            IBasicCommunication coms)
        {
            var result = new List<SymetrixComposerFader>();
            if (config.LevelControlBlocks == null)
                return result;

            foreach (var faderConfig in config.LevelControlBlocks)
                result.Add(new SymetrixComposerFader(key + "-" + faderConfig.Key, faderConfig.Value, coms));

            return result;
        }

        public static IEnumerable<SymetrixComposerDspPreset> BuildPresets(
            string key,
            DevicePropertiesConfig config)
        {
            var result = new List<SymetrixComposerDspPreset>();
            if (config.Presets == null)
                return result;

            foreach (var presetConfig in config.Presets)
                result.Add(new SymetrixComposerDspPreset(key + "-" + presetConfig.Key, presetConfig.Value));

            return result;
        }

        public static IEnumerable<SymetrixComposerDialer> BuildDialers(
           string key,
           DevicePropertiesConfig config,
            IBasicCommunication coms)
        {
            var result = new List<SymetrixComposerDialer>();
            if (config.DialerControlBlocks == null)
                return result;

            foreach (var dialerConfig in config.DialerControlBlocks)
                result.Add(new SymetrixComposerDialer(key + "-" + dialerConfig.Key, dialerConfig.Value, coms));

            return result;
        }
    }
}