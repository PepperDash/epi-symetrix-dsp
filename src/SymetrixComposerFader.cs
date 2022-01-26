using Crestron.SimplSharp;
using Crestron.SimplSharpPro.CrestronThread;
using Crestron.SimplSharpPro.DeviceSupport;
using PepperDash.Core;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.Core.Bridges;
using PepperDashPluginSymetrixComposer.Config;
using PepperDashPluginSymetrixComposer.JoinMaps;
using PepperDashPluginSymetrixComposer.Utils;
using Feedback = PepperDash.Essentials.Core.Feedback;

namespace PepperDashPluginSymetrixComposer
{
    public class SymetrixComposerFader : EssentialsBridgeableDevice, IBasicVolumeWithFeedback, IHasFeedback
    {
        private const int DebugLevel1 = 1;
        private const int DebugLevel2 = 2;

        public const int DefaultFaderMinimum = -72;
        public const int DefaultFaderMaximum = 12;
        public const int DefaultIncrement = 2;
        public const int DefaultPermissions = 0;
        public const int SpeakerType = 0;
        public const int MicrophoneType = 1;
        public const int PermissionsAll = 0;
        public const int PermissionsUserOnly = 1;
        public const int PermissionsTechOnly = 2;

        public readonly int UserMinumum;
        public readonly int UserMaximum;
        public readonly int FaderMinumum;
        public readonly int FaderMaximum;
        public readonly int Increment;
        public readonly int Permissions;
        public readonly int VolumeControllerId;
        public readonly int MuteControllerId;
        public readonly IBasicCommunication Coms;
        public readonly bool IsMic;
        public readonly bool UnmuteOnVolumeChange;
        public readonly StringFeedback NameFeedback;
        public readonly IntFeedback ControlTypeFeedback;
        public readonly IntFeedback PermissionsFeedback;

        private bool _isMuted;
        private ushort _volume;

        /// <summary>
        /// Fader mute property
        /// </summary>
        public bool IsMuted
        {
            get { return _isMuted; }
            set
            {
                _isMuted = value;
                MuteFeedback.FireUpdate();
            }
        }

        /// <summary>
        /// Fader volume level property
        /// </summary>
        public ushort Volume
        {
            get { return _volume; }
            set
            {
                var scaledUserMinimum = FaderUtils.ScaleToUshortRange(UserMinumum, FaderMinumum, FaderMaximum);
                var scaledUserMaximum = FaderUtils.ScaleToUshortRange(UserMaximum, FaderMinumum, FaderMaximum);
                Debug.Console(DebugLevel2, this, "Volume: scaledUserMinimum = '{0}'; scaledUserMaximum = '{1}'", scaledUserMinimum, scaledUserMaximum);

                _volume = FaderUtils.ScaleFromUshortRange(value, scaledUserMinimum, scaledUserMaximum);
                Debug.Console(DebugLevel2, this, "Volume: _volume = '{0}'", _volume);
                VolumeLevelFeedback.FireUpdate();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="key"></param>
        /// <param name="config"></param>
        /// <param name="coms"></param>
        public SymetrixComposerFader(string key, FaderConfig config, IBasicCommunication coms)
            : base(key, config.Label)
        {
            Key = key;
            Debug.Console(DebugLevel1, this, "Building...");
            Name = config.Label;
            VolumeControllerId = config.LevelControlId;
            MuteControllerId = config.MuteControlId;
            IsMic = config.IsMic;
            UnmuteOnVolumeChange = config.UnmuteOnVolChange;
            // if configured, set the fader min/max.  this changes based on the DSP object used
            FaderMinumum = config.FaderMinimum ?? DefaultFaderMinimum;
            FaderMaximum = config.FaderMaximum ?? DefaultFaderMaximum;
            // if not configured, use the fader min/max
            UserMinumum = config.UserMinimum ?? FaderMinumum;
            UserMaximum = config.UserMaximum ?? FaderMaximum;
            Increment = config.Increment ?? DefaultIncrement;
            Permissions = config.Permissions ?? DefaultPermissions;
            Coms = coms;
            MuteFeedback = new BoolFeedback(Key + "-Mute", () => IsMuted);
            VolumeLevelFeedback = new IntFeedback(Key + "-Volume", () => Volume);
            NameFeedback = new StringFeedback(() => Name);
            ControlTypeFeedback = new IntFeedback(() => config.IsMic ? MicrophoneType : SpeakerType);
            PermissionsFeedback = new IntFeedback(() => config.Permissions ?? DefaultPermissions);

            Debug.Console(DebugLevel1, this, "Adding myself to the Device Manager");
            DeviceManager.AddDevice(this);
        }

        /// <summary>
        /// Custom activate
        /// </summary>
        /// <returns></returns>
        public override bool CustomActivate()
        {
            Feedbacks = new FeedbackCollection<Feedback>
            {
                VolumeLevelFeedback,
                MuteFeedback,
                NameFeedback,
                ControlTypeFeedback,
                PermissionsFeedback
            };

            Feedbacks.RegisterForFeedbackText(this);
            Feedbacks.ForEach(fb => fb.FireUpdate());
            return base.CustomActivate();
        }

        private bool _volumeUpRamping;

        /// <summary>
        /// Increments the volume level
        /// </summary>
        /// <param name="pressRelease"></param>
        public void VolumeUp(bool pressRelease)
        {
            if (VolumeControllerId == 0)
                return;

            if (!pressRelease)
            {
                _volumeUpRamping = false;
            }
            else
            {
                RampVolumeUp();
            }
        }

        private void RampVolumeUp()
        {
            _volumeDownRamping = false;
            if (UnmuteOnVolumeChange)
                MuteOff();

            CrestronInvoke.BeginInvoke(
                o =>
                {
                    while (_volumeUpRamping)
                    {
                        var s = FaderUtils.GetVolumeUpCommand(
                            VolumeControllerId,
                            Increment,
                            FaderMinumum,
                            FaderMaximum);

                        Coms.SendText(s);
                        Thread.Sleep(250);
                    }
                });
        }

        private bool _volumeDownRamping;

        /// <summary>
        /// Decrememnts the volume level
        /// </summary>
        /// <param name="pressRelease"></param>
        public void VolumeDown(bool pressRelease)
        {
            if (VolumeControllerId == 0)
                return;

            if (!pressRelease)
            {
                _volumeDownRamping = false;
            }
            else
            {
                RampVolumeDown();
            }
        }

        private void RampVolumeDown()
        {
            _volumeUpRamping = false;
            if (UnmuteOnVolumeChange)
                MuteOff();

            CrestronInvoke.BeginInvoke(
                o =>
                {
                    while (_volumeDownRamping)
                    {
                        var s = FaderUtils.GetVolumeDownCommand(
                            VolumeControllerId,
                            Increment,
                            FaderMinumum,
                            FaderMaximum);

                        Coms.SendText(s);
                        Thread.Sleep(250);
                    }
                });
        }

        /// <summary>
        /// Sets the volume level
        /// </summary>
        /// <param name="level">volume ushort level to set</param>
        public void SetVolume(ushort level)
        {
            if (VolumeControllerId == 0)
                return;

            var s = FaderUtils.GetVolumeCommand(
                VolumeControllerId,
                level,
                UserMinumum,
                UserMaximum,
                FaderMinumum,
                FaderMaximum);

            Coms.SendText(s);
        }

        /// <summary>
        /// Toggles the fader mute state
        /// </summary>
        public void MuteToggle()
        {
            if (MuteControllerId == 0)
                return;

            if (IsMuted)
            {
                MuteOff();
            }
            else
            {
                MuteOn();
            }
        }

        /// <summary>
        /// Sets the fader mute state on
        /// </summary>
        public void MuteOn()
        {
            if (MuteControllerId == 0)
                return;

            var s = FaderUtils.GetStateCommand(MuteControllerId, true);
            Coms.SendText(s);
        }

        /// <summary>
        /// Sets the fader mute state off
        /// </summary>
        public void MuteOff()
        {
            if (MuteControllerId == 0)
                return;

            var s = FaderUtils.GetStateCommand(MuteControllerId, false);
            Coms.SendText(s);
        }

        /// <summary>
        /// Fader volume level feedback
        /// </summary>
        public IntFeedback VolumeLevelFeedback { get; private set; }

        /// <summary>
        /// Fader mute state feedack
        /// </summary>
        public BoolFeedback MuteFeedback { get; private set; }

        /// <summary>
        /// Links the bridge
        /// </summary>
        /// <param name="trilist"></param>
        /// <param name="joinStart"></param>
        /// <param name="joinMapKey"></param>
        /// <param name="bridge"></param>
        public override void LinkToApi(BasicTriList trilist, uint joinStart, string joinMapKey, EiscApiAdvanced bridge)
        {
            var joinMap = new FaderJoinMap(joinStart);
            if (bridge != null)
                bridge.AddJoinMap(Key, joinMap);

            trilist.SetBoolSigAction(joinMap.VolumeUp.JoinNumber, VolumeUp);
            trilist.SetBoolSigAction(joinMap.VolumeDown.JoinNumber, VolumeDown);
            trilist.SetSigTrueAction(joinMap.MuteOn.JoinNumber, MuteOn);
            trilist.SetSigTrueAction(joinMap.MuteOff.JoinNumber, MuteOff);
            trilist.SetSigTrueAction(joinMap.MuteToggle.JoinNumber, MuteToggle);
            trilist.SetUShortSigAction(joinMap.Volume.JoinNumber, SetVolume);

            VolumeLevelFeedback.LinkInputSig(trilist.UShortInput[joinMap.Volume.JoinNumber]);
            MuteFeedback.LinkInputSig(trilist.BooleanInput[joinMap.MuteOn.JoinNumber]);
            NameFeedback.LinkInputSig(trilist.StringInput[joinMap.Name.JoinNumber]);
            ControlTypeFeedback.LinkInputSig(trilist.UShortInput[joinMap.Type.JoinNumber]);
            PermissionsFeedback.LinkInputSig(trilist.UShortInput[joinMap.Permissions.JoinNumber]);
        }

        /// <summary>
        /// Links the bridge to the application API
        /// </summary>
        /// <param name="trilist"></param>
        /// <param name="joinStart"></param>
        /// <param name="joinMapKey"></param>
        /// <param name="bridge"></param>
        public void LinkToApplicationApi(BasicTriList trilist, uint joinStart, string joinMapKey, EiscApiAdvanced bridge)
        {
            var joinMap = new FaderApplicationJoinMap(joinStart);
            if (bridge != null)
                bridge.AddJoinMap(Key, joinMap);

            trilist.SetBoolSigAction(joinMap.VolumeUp.JoinNumber, VolumeUp);
            trilist.SetBoolSigAction(joinMap.VolumeDown.JoinNumber, VolumeDown);
            trilist.SetSigTrueAction(joinMap.MuteOn.JoinNumber, MuteOn);
            trilist.SetSigTrueAction(joinMap.MuteOff.JoinNumber, MuteOff);
            trilist.SetSigTrueAction(joinMap.MuteToggle.JoinNumber, MuteToggle);
            trilist.SetUShortSigAction(joinMap.Volume.JoinNumber, SetVolume);

            VolumeLevelFeedback.LinkInputSig(trilist.UShortInput[joinMap.Volume.JoinNumber]);
            MuteFeedback.LinkInputSig(trilist.BooleanInput[joinMap.MuteOn.JoinNumber]);
            NameFeedback.LinkInputSig(trilist.StringInput[joinMap.Name.JoinNumber]);
            ControlTypeFeedback.LinkInputSig(trilist.UShortInput[joinMap.Type.JoinNumber]);
            PermissionsFeedback.LinkInputSig(trilist.UShortInput[joinMap.Permissions.JoinNumber]);
        }

        /// <summary>
        /// Feedback collection
        /// </summary>
        public FeedbackCollection<Feedback> Feedbacks { get; private set; }
    }
}