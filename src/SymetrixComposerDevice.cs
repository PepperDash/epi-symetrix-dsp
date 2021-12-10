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
using SymetrixComposerEpi.Config;
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
        public readonly IEnumerable<SymetrixComposerLevelControl> Faders;
        public readonly IEnumerable<SymetrixComposerDialer> Dialers;
        public readonly IEnumerable<SymetrixComposerDspPreset> PresetsImpl;

        public static readonly CommunicationMonitorConfig DefaultMonitorConfig = new CommunicationMonitorConfig
        {
            PollInterval = 30000,
            PollString = "V\r",
            TimeToWarning = 60000,
            TimeToError = 120000
        };

        public SymetrixComposerDevice(DeviceConfig config) : base(config)
        {
            Coms = CommFactory.CreateCommForDevice(config);
            var props = config.Properties.ToObject<DevicePropertiesConfig>();
            var monitorConfig = props.CommunicationMonitorProperties ?? DefaultMonitorConfig;
            CommunicationMonitor = new GenericCommunicationMonitor(this, Coms, monitorConfig);
            PortGather = new CommunicationGather(Coms, "\r");
            PortGather.LineReceived += PortGatherOnLineReceived;
            Feedbacks = new FeedbackCollection<Feedback>();
            Faders = ConfigUtils.BuildFaders(Key, props, Coms);
            PresetsImpl = ConfigUtils.BuildPresets(Key, props);
            Dialers = ConfigUtils.BuildDialers(Key, props, Coms);

            IsOnline.OutputChange += (sender, args) =>
            {
                if (args.BoolValue)
                    Coms.SendText("PUR\r");
            };

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

                // TODO System string parsing
            }
            catch (Exception ex)
            {
                Debug.Console(
                    1, this, Debug.ErrorLogLevel.Notice, "Error parsing response:{0} | {1}", response, ex.Message);
            }
        }

        public override void LinkToApi(BasicTriList trilist, uint joinStart, string joinMapKey, EiscApiAdvanced bridge)
        {
            throw new NotImplementedException();
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