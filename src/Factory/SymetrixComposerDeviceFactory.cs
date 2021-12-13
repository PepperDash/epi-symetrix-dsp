using System.Collections.Generic;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.Core.Config;
using SymetrixComposerEpi.Utils;

namespace SymetrixComposerEpi.Factory
{
    public class SymetrixComposerDeviceFactory : EssentialsPluginDeviceFactory<SymetrixComposerDevice>
    {
        public const string MinimumEssentialsVersion = "1.9.6";

        public SymetrixComposerDeviceFactory()
        {
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