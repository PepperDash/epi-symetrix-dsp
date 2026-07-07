using System.Collections.Generic;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.Core.Config;
using PepperDash.Essentials.Plugin.SymetrixComposer.Config;
using PepperDash.Essentials.Plugin.SymetrixComposer.Utils;

namespace PepperDash.Essentials.Plugin
{
    public class SymetrixComposerDeviceFactory : EssentialsPluginDeviceFactory<SymetrixComposerDevice>
    {
        public SymetrixComposerDeviceFactory()
        {
            MinimumEssentialsFrameworkVersion = "2.12.1";
            TypeNames = new List<string> { "SymetrixDsp" };
        }

        public override EssentialsDevice BuildDevice(DeviceConfig dc)
        {
            var key = dc.Key;
            var coms = CommFactory.CreateCommForDevice(dc);
            var props = dc.Properties.ToObject<DevicePropertiesConfig>();

            var faders = ConfigUtils.BuildFaders(key, props, coms);
            var presets = ConfigUtils.BuildPresets(key, props);
            var dialers = ConfigUtils.BuildDialers(key, props, coms);

            return new SymetrixComposerDevice(dc, coms, faders, dialers, presets);
        }
    }
}
