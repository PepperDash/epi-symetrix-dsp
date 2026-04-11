using System.Collections.Generic;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.Core.Config;
using PepperDashPluginSymetrixComposer.Config;
using PepperDashPluginSymetrixComposer.Utils;

namespace PepperDashPluginSymetrixComposer.Factory
{
    public class SymetrixComposerDeviceFactory : EssentialsPluginDeviceFactory<SymetrixComposerDevice>
    {
#if !SERIES4
        public const string MinimumEssentialsVersion = "1.9.6";
#endif

        public SymetrixComposerDeviceFactory()
        {
#if SERIES4
            MinimumEssentialsFrameworkVersion = "2.0.0";
#endif
            TypeNames = new List<string> {"SymetrixDsp"};
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