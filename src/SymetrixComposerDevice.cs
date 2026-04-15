using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro.DeviceSupport;
using PepperDash.Core;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.Core.Bridges;
using PepperDash.Essentials.Core.Config;
using PepperDash.Essentials.Core.Devices;
using PepperDashPluginSymetrixComposer.Config;
using PepperDashPluginSymetrixComposer.JoinMaps;
using PepperDashPluginSymetrixComposer.Utils;
using Feedback = PepperDash.Essentials.Core.Feedback;

namespace PepperDashPluginSymetrixComposer
{
    public class SymetrixComposerDevice : ReconfigurableBridgableDevice,
        ICommunicationMonitor,
        IDspPresets,
        IHasFeedback,
        IOnline
    {
        public readonly IBasicCommunication Coms;
        public readonly CommunicationGather PortGather;
        public readonly IEnumerable<SymetrixComposerFader> Faders;
        public readonly IEnumerable<SymetrixComposerDialer> Dialers;
        public readonly IEnumerable<SymetrixComposerDspPreset> PresetsImpl;

        public static readonly CommunicationMonitorConfig DefaultMonitorConfig = new CommunicationMonitorConfig
        {
            PollInterval = 30000,
            PollString = "V\r",
            TimeToWarning = 60000,
            TimeToError = 120000
        };

        public SymetrixComposerDevice(DeviceConfig config,
            IBasicCommunication coms,
            IEnumerable<SymetrixComposerFader> faders,
            IEnumerable<SymetrixComposerDialer> dialers,
            IEnumerable<SymetrixComposerDspPreset> presets)
            : base(config)
        {
            Coms = coms;
            Faders = faders;
            Dialers = dialers;
            PresetsImpl = presets;
            var props = config.Properties.ToObject<DevicePropertiesConfig>();
            var monitorConfig = props.CommunicationMonitorProperties ?? DefaultMonitorConfig;
            CommunicationMonitor = new GenericCommunicationMonitor(this, Coms, monitorConfig);
            PortGather = new CommunicationGather(Coms, "\r");
            PortGather.LineReceived += PortGatherOnLineReceived;
            Feedbacks = new FeedbackCollection<Feedback>();

            IsOnline.OutputChange += (sender, args) =>
            {
                if (args.BoolValue)
                    Coms.SendText("PUR\r");
            };

            Debug.LogVerbose(this, "Device created");

            DeviceManager.AllDevicesActivated += (sender, args) =>
            {
                CrestronInvoke.BeginInvoke(o => CommunicationMonitor.Start());
                CrestronInvoke.BeginInvoke(o => Coms.Connect());
            };
        }

        private void PortGatherOnLineReceived(object sender, GenericCommMethodReceiveTextArgs args)
        {
            var response = args.Text;
            Debug.LogVerbose("PortGatherOnLineReceived: response = {0}", response);

            try
            {
                if (response.StartsWith("#"))
                {
                    var controllerId = ParsingUtils.ParseControllerId(response);
                    Debug.LogVerbose("PortGatherOnLineReceived: controllerId = {0}", controllerId);
                    if (controllerId == 0)
                        return;

                    Faders
                        .Where(f => f.VolumeControllerId == controllerId)
                        .ToList()
                        .ForEach(FaderUtils.UpdateVolumeControllerPosition(response));

                    Faders
                        .Where(m => m.MuteControllerId == controllerId)
                        .ToList()
                        .ForEach(FaderUtils.UpdateMuteControllerPosition(response));

                    // TODO [ ] Verify dialer feedback
                    Dialers
                        .Where(d => d.HasControllerId(controllerId))
                        .ToList()
                        .ForEach(DialerUtils.ParseAndUpdateDialerState(response, controllerId));
                }

                // TODO [ ] Verify dialer feedback
                if (response.StartsWith("GSSYS"))
                {
                    throw new NotImplementedException("[Symetrix DSP] Dialer GSSYS feedback processing not implemented");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex, this, "Exception in PortGatherOnLineReceived: {0}", ex.Message);
            }
        }

        public override void LinkToApi(BasicTriList trilist, uint joinStart, string joinMapKey, EiscApiAdvanced bridge)
        {
            var joinMap = new ApplicationJoinMap(joinStart);
            if (bridge != null)
                bridge.AddJoinMap(Key, joinMap);

            IsOnline.LinkInputSig(trilist.BooleanInput[joinMap.IsOnline.JoinNumber]);

            trilist.SetUShortSigAction(
                joinMap.PresetRecallDiscrete.JoinNumber,
                value => PresetsImpl
                    .Where(x => x.PresetNumber == value)
                    .ToList()
                    .ForEach(RecallPresetInternal));

            trilist.SetStringSigAction(
                joinMap.PresetRecallDiscrete.JoinNumber,
                value => PresetsImpl
                    .Where(x => x.Key.Equals(value, StringComparison.OrdinalIgnoreCase))
                    .ToList()
                    .ForEach(RecallPresetInternal));

            const uint faderJoinStart = 200;
            for (uint x = 0; x < Faders.Count(); ++x)
            {
                var join = faderJoinStart + joinStart + x;
                var fader = Faders.ElementAt((int)x);
                fader.LinkToApplicationApi(trilist, join, joinMapKey, bridge);
            }

            for (uint x = 0; x < PresetsImpl.Count(); ++x)
            {
                var preset = PresetsImpl.ElementAt((int)x);
                trilist.SetSigTrueAction(joinMap.PresetRecall.JoinNumber + x, () => RecallPresetInternal(preset));
                var fb = new StringFeedback(preset.Key + "-Name", () => preset.Name);
                fb.LinkInputSig(trilist.StringInput[joinMap.PresetRecall.JoinNumber + x]);
                fb.FireUpdate();
            }

            const uint dialerJoinStart = 3100;
            for (uint x = 0; x < Dialers.Count(); ++x)
            {
                var dialer = Dialers.ElementAt((int)x);
                var join = dialerJoinStart + joinStart + (x * 50);
                dialer.LinkToApi(trilist, join, joinMapKey, bridge);
            }
        }

        public StatusMonitorBase CommunicationMonitor { get; private set; }

        private void RecallPresetInternal(SymetrixComposerDspPreset preset)
        {
            var command = SymetrixComposerDspPreset.GetPresetCommand(preset.PresetNumber);
            Coms.SendText(command);
        }

        public void RecallPreset(string key)
        {
            var preset = PresetsImpl.FirstOrDefault(p => p.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
            if (preset == null)
                return;

            RecallPresetInternal(preset);
        }

        public Dictionary<string, IKeyName> Presets
        {
            get
            {
                return PresetsImpl.ToDictionary<SymetrixComposerDspPreset, string, IKeyName>(
                    p => p.Key,
                    p => p);
            }
        }

        public FeedbackCollection<Feedback> Feedbacks { get; private set; }
        public BoolFeedback IsOnline { get { return CommunicationMonitor.IsOnlineFeedback; } }
    }
}
