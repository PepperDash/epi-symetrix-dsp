using Crestron.SimplSharp;
using Crestron.SimplSharpPro.CrestronThread;
using Crestron.SimplSharpPro.DeviceSupport;
using PepperDash.Core;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.Core.Bridges;
using SymetrixComposerEpi.JoinMaps;
using SymetrixComposerEpi.Utils;
using Feedback = PepperDash.Essentials.Core.Feedback;

namespace SymetrixComposerEpi
{
    public class SymetrixComposerFader : EssentialsBridgeableDevice, IBasicVolumeWithFeedback, IHasFeedback
    {
        public const int DefaultFaderMinimum = -72;
        public const int DefaultFaderMaximum = 12;
        public const int DefaultIncrement = 2;
        public const int SpeakerType = 0;
        public const int MicrophoneType = 1;

        public readonly int UserMinumum;
        public readonly int UserMaximum;
        public readonly int FaderMinumum;
        public readonly int FaderMaximum;
        public readonly int Increment;
        public readonly int VolumeControllerId;
        public readonly int MuteControllerId;
        public readonly IBasicCommunication Coms;
        public readonly bool IsMic;
        public readonly bool UnmuteOnVolumeChange;
        public readonly StringFeedback NameFeedback;
        public readonly IntFeedback ControlTypeFeedback;

        private bool _isMuted;
        private ushort _volume;

        public bool IsMuted
        {
            get { return _isMuted; }
            set
            {
                _isMuted = value;
                MuteFeedback.FireUpdate();
            }
        }

        public ushort Volume
        {
            get { return _volume; }
            set
            {
                var scaledUserMinimum = FaderUtils.ScaleToUshortRange(UserMinumum, FaderMinumum, FaderMaximum);
                var scaledUserMaximum = FaderUtils.ScaleToUshortRange(UserMaximum, FaderMinumum, FaderMaximum);
                _volume = FaderUtils.ScaleFromUshortRange(value, scaledUserMinimum, scaledUserMaximum);
                VolumeLevelFeedback.FireUpdate();
            }
        }

        // CC <CONTROLLER NUMBER> <DEC/INC> <AMOUNT><CR>
        public SymetrixComposerFader(string key, FaderConfig config, IBasicCommunication coms)
            : base(key, config.Label)
        {
            Key = key;
            Debug.Console(1, this, "Building...");
            Name = config.Label;
            VolumeControllerId = config.LevelControlId;
            MuteControllerId = config.MuteControlId;
            IsMic = config.IsMic;
            UnmuteOnVolumeChange = config.UnmuteOnVolChange;
            UserMinumum = config.UserMinimum ?? DefaultFaderMinimum;
            UserMaximum = config.UserMaximim ?? DefaultFaderMaximum;
            FaderMinumum = config.FaderMinimum ?? DefaultFaderMinimum;
            FaderMaximum = config.FaderMaximum ?? DefaultFaderMaximum;
            Increment = config.Increment ?? DefaultIncrement;
            Coms = coms;
            MuteFeedback = new BoolFeedback(Key + "-Mute", () => IsMuted);
            VolumeLevelFeedback = new IntFeedback(Key + "-Volume", () => Volume);
            NameFeedback = new StringFeedback(() => Name);
            ControlTypeFeedback = new IntFeedback(() => config.IsMic ? MicrophoneType : SpeakerType);

            Debug.Console(1, this, "Adding myself to the Device Manager");
            DeviceManager.AddDevice(this);
        }

        public override bool CustomActivate()
        {
            Feedbacks = new FeedbackCollection<Feedback>
            {
                VolumeLevelFeedback,
                MuteFeedback,
                NameFeedback,
                ControlTypeFeedback
            };

            Feedbacks.RegisterForFeedbackText(this);
            Feedbacks.ForEach(fb => fb.FireUpdate());
            return base.CustomActivate();
        }

        private bool _volumeUpRamping;

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


        public void SetVolume(ushort level)
        {
            if (VolumeControllerId == 0)
                return;

            var s = FaderUtils.GetVolumeCommand(
                VolumeControllerId,
                level,
                UserMaximum,
                UserMinumum,
                FaderMinumum,
                FaderMaximum);

            Coms.SendText(s);
        }

        public void MuteToggle()
        {
            if (MuteControllerId == 0)
                return;

            if (IsMuted)
            {
                MuteOn();
            }
            else
            {
                MuteOff();
            }
        }

        public void MuteOn()
        {
            if (MuteControllerId == 0)
                return;

            var s = FaderUtils.GetStateCommand(MuteControllerId, true);
            Coms.SendText(s);
        }

        public void MuteOff()
        {
            if (MuteControllerId == 0)
                return;

            var s = FaderUtils.GetStateCommand(MuteControllerId, false);
            Coms.SendText(s);
        }

        public IntFeedback VolumeLevelFeedback { get; private set; }
        public BoolFeedback MuteFeedback { get; private set; }

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
        }

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
        }

        public FeedbackCollection<Feedback> Feedbacks { get; private set; }
    }
}