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
using SymetrixComposerEpi.JoinMaps;
using SymetrixComposerEpi.Utils;
using Feedback = PepperDash.Essentials.Core.Feedback;

namespace SymetrixComposerEpi
{
    public class SymetrixComposerDevice : ReconfigurableBridgableDevice,
        ICommunicationMonitor,
        IHasDspPresets,
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
            IEnumerable<SymetrixComposerDspPreset> presets) : base(config)
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

            Debug.Console(1, this, "Device created");

            DeviceManager.AllDevicesActivated += (sender, args) => CrestronInvoke.BeginInvoke(o => Coms.Connect());
        }

        private void PortGatherOnLineReceived(object sender, GenericCommMethodReceiveTextArgs args)
        {
            var response = args.Text;
            try
            {
                if (response.StartsWith("#"))
                {
                    var controllerId = ParsingUtils.ParseControllerId(response);
                    if (controllerId == 0)
                        return;

                    var cleanedResponse = ParsingUtils.CleanResponse(response);

                    Faders
                        .Where(f => f.VolumeControllerId == controllerId)
                        .ToList()
                        .ForEach(FaderUtils.UpdateVolumeControllerPosition(cleanedResponse));

                    Faders
                        .Where(f => f.MuteControllerId == controllerId)
                        .ToList()
                        .ForEach(FaderUtils.UpdateMuteControllerPosition(cleanedResponse));

                    Dialers
                        .Where(d => d.HasControllerId(controllerId))
                        .ToList()
                        .ForEach(DialerUtils.ParseAndUpdateDialerState(cleanedResponse, controllerId));
                }

                if (response.StartsWith("GSSYS"))
                {
                }
            }
            catch (Exception ex)
            {
                Debug.Console(
                    1, this, Debug.ErrorLogLevel.Notice, "Error parsing response:{0} | {1}", response, ex.Message);
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
                    .ForEach(RecallPreset));

            trilist.SetStringSigAction(
                joinMap.PresetRecallDiscrete.JoinNumber,
                value => PresetsImpl
                    .Where(x => x.Key.Equals(value, StringComparison.OrdinalIgnoreCase))
                    .ToList()
                    .ForEach(RecallPreset));

            const uint faderJoinStart = 200;
            for (uint x = 0; x < Faders.Count(); ++x)
            {
                var join = faderJoinStart + joinStart + x;
                var fader = Faders.ElementAt((int) x);
                fader.LinkToApplicationApi(trilist, join, joinMapKey, bridge);
            }

            for (uint x = 0; x < PresetsImpl.Count(); ++x)
            {
                var preset = PresetsImpl.ElementAt((int) x);
                trilist.SetSigTrueAction(joinMap.PresetRecall.JoinNumber + x, () => RecallPreset(preset));
                var fb = new StringFeedback(() => preset.Name);
                fb.LinkInputSig(trilist.StringInput[joinMap.PresetRecall.JoinNumber + x]);
                fb.FireUpdate();
            }

            const uint dialerJoinStart = 3100;
            for (uint x = 0; x < Dialers.Count(); ++x)
            {
                var dialer = Dialers.ElementAt((int) x);
                var join = dialerJoinStart + joinStart + (x * 50);
                dialer.LinkToApi(trilist, join, joinMapKey, bridge);
            }
        }

        public StatusMonitorBase CommunicationMonitor { get; private set; }

        public void RecallPreset(IDspPreset preset)
        {
            var symetrixDspPreset = preset as SymetrixComposerDspPreset;
            if (symetrixDspPreset == null)
                return;

            var command = SymetrixComposerDspPreset.GetPresetCommand(symetrixDspPreset.PresetNumber);
            Coms.SendText(command);
        }

        public List<IDspPreset> Presets { get { return PresetsImpl.OfType<IDspPreset>().ToList(); } }
        public FeedbackCollection<Feedback> Feedbacks { get; private set; }
        public BoolFeedback IsOnline { get { return CommunicationMonitor.IsOnlineFeedback; } }
    }
}