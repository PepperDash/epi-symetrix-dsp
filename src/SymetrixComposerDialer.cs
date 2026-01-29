using System.Linq;
using System.Text;
using Crestron.SimplSharpPro.DeviceSupport;
using PepperDash.Core;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.Core.Bridges;
using PepperDash.Essentials.Core.DeviceTypeInterfaces;
using PepperDashPluginSymetrixComposer.Config;
using PepperDashPluginSymetrixComposer.Enums;
using PepperDashPluginSymetrixComposer.JoinMaps;
using PepperDashPluginSymetrixComposer.Utils;

namespace PepperDashPluginSymetrixComposer
{
    public class SymetrixComposerDialer : EssentialsBridgeableDevice, IBasicVolumeWithFeedback, IHasPhoneDialing, IOnline
    {
        private const int DebugLevel1 = 1;
        private const int DebugLevel2 = 2;

        /// <summary>
        /// Caller ID
        /// </summary>
        public string CallerId
        {
            set
            {
                _callerId = value;
                CallerIdNumberFeedback.FireUpdate();
            }
        }

        /// <summary>
        /// Incoming call
        /// </summary>
        public bool IsRinging
        {
            set
            {
                _isRinging = value;
                IncomingCallFeedback.FireUpdate();
                DialStringFeedback.FireUpdate();
            }
        }

        /// <summary>
        /// Call is connected
        /// </summary>
        public bool IsConnected
        {
            set
            {
                _isConnected = value;
                PhoneOffHookFeedback.FireUpdate();
                DialStringFeedback.FireUpdate();
            }
        }

        /// <summary>
        /// Call is on hold
        /// </summary>
        public bool IsOnHold
        {
            set
            {
                _isOnHold = value;
                PhoneOffHookFeedback.FireUpdate();
                DialStringFeedback.FireUpdate();
            }
        }

        /// <summary>
        /// Is busy
        /// </summary>
        public bool IsBusy
        {
            set
            {
                _isBusy = value;
                PhoneOffHookFeedback.FireUpdate();
                DialStringFeedback.FireUpdate();
            }
        }

        /// <summary>
        /// Dialing 
        /// </summary>
        public bool IsDialing
        {
            set
            {
                _isDialing = value;
                PhoneOffHookFeedback.FireUpdate();
                DialStringFeedback.FireUpdate();
            }
        }

        /// <summary>
        /// Do not disturb
        /// </summary>
        public bool IsInDnd
        {
            set
            {
                _isIsDnd = value;
                PhoneOffHookFeedback.FireUpdate();
                DialStringFeedback.FireUpdate();
            }
        }

        /// <summary>
        /// Off hook feedback
        /// </summary>
        public BoolFeedback PhoneOffHookFeedback { get; private set; }

        /// <summary>
        /// Caller ID name feedback
        /// </summary>
        public StringFeedback CallerIdNameFeedback { get; private set; }

        /// <summary>
        /// Caller ID number feedback
        /// </summary>
        public StringFeedback CallerIdNumberFeedback { get; private set; }

        /// <summary>
        /// Communication object
        /// </summary>
        public readonly IBasicCommunication Coms;

        public readonly bool ClearDialstringWhenConnected;

        public readonly int UnitNumber;
        public readonly int CardSlot;
        public readonly int LineNumber;

        public readonly int IsRingingId;
        public readonly int IsConnectedId;
        public readonly int IsOnHoldId;
        public readonly int IsBusyId;
        public readonly int IsDialingId;
        public readonly int DialStringId;
        public readonly int ConnectAndDisconnectId;
        public readonly int RejectId;
        public readonly int DndId;
        public readonly int RedialId;
        public readonly int KeypadBackspaceId;
        public readonly int KeypadClearId;
        public readonly int Keypad1Id;
        public readonly int Keypad2Id;
        public readonly int Keypad3Id;
        public readonly int Keypad4Id;
        public readonly int Keypad5Id;
        public readonly int Keypad6Id;
        public readonly int Keypad7Id;
        public readonly int Keypad8Id;
        public readonly int Keypad9Id;
        public readonly int Keypad0Id;
        public readonly int KeypadPoundId;
        public readonly int KeypadStarId;

        public readonly BoolFeedback IncomingCallFeedback;
        public readonly BoolFeedback DoNotDisturbFeedback;
        public readonly StringFeedback DialStringFeedback;

        private StringBuilder _numberToDial = new StringBuilder();
        private bool _isRinging;
        private bool _isConnected;
        private bool _isOnHold;
        private bool _isBusy;
        private bool _isDialing;
        private bool _isIsDnd;
        private string _callerId;

        private readonly SymetrixComposerFader _atcRx;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="key"></param>
        /// <param name="config"></param>
        /// <param name="coms"></param>
        public SymetrixComposerDialer(string key, DialerConfig config, IBasicCommunication coms) : base(key)
        {
            Debug.Console(DebugLevel1, this, "Building...");
            Coms = coms;
            UnitNumber = config.UnitNumber;
            CardSlot = config.CardSlot;
            LineNumber = config.LineNumber;
            IsRingingId = config.IsRingingId;
            IsConnectedId = config.IsConnectedId;
            IsOnHoldId = config.IsOnHoldId;
            IsBusyId = config.IsBusyId;
            IsDialingId = config.IsDialingId;
            ConnectAndDisconnectId = config.ConnectAndDisconnectId;
            RejectId = config.RejectId;
            DndId = config.DndId;
            RedialId = config.RedialId;
            KeypadBackspaceId = config.KeypadBackspaceId;
            KeypadClearId = config.KeypadClearId;
            Keypad1Id = config.Keypad1Id;
            Keypad2Id = config.Keypad2Id;
            Keypad3Id = config.Keypad3Id;
            Keypad4Id = config.Keypad4Id;
            Keypad5Id = config.Keypad5Id;
            Keypad6Id = config.Keypad6Id;
            Keypad7Id = config.Keypad7Id;
            Keypad8Id = config.Keypad8Id;
            Keypad9Id = config.Keypad9Id;
            Keypad0Id = config.Keypad0Id;
            KeypadPoundId = config.KeypadPoundId;
            KeypadStarId = config.KeypadStarId;
            ClearDialstringWhenConnected = config.ClearDialstringWhenConnected;
            DialStringId = config.DialStringId;

            _atcRx = new SymetrixComposerFader(Key + "-AtcRx", new FaderConfig
            {
                Label = Key + " Rx",
                LevelControlId = config.RxVolumeId,
                MuteControlId = config.RxMuteId,
                UserMaximum = config.RxUserMaximum,
                UserMinimum = config.RxUserMinimum,
            }, Coms);

            IncomingCallFeedback =
                new BoolFeedback(Key + "-IncomingCall", () => _isRinging);
            PhoneOffHookFeedback =
                new BoolFeedback(Key + "-OffHook", () => _isConnected || _isDialing || _isOnHold || _isBusy);
            DoNotDisturbFeedback =
                new BoolFeedback(Key + "-DND", () => _isIsDnd);
            DialStringFeedback =
                new StringFeedback(Key + "-DialString", () => _numberToDial.ToString());
            CallerIdNumberFeedback =
                new StringFeedback(Key + "-CallerId", () => _callerId);
            CallerIdNameFeedback = 
                new StringFeedback(() => string.Empty);

            Debug.Console(DebugLevel1, this, "Adding myself to the Device Manager");
            DeviceManager.AddDevice(this);

            IncomingCallFeedback.OutputChange += (sender, args) =>
            {
                _numberToDial = new StringBuilder();
                DialStringFeedback.FireUpdate();
            };

            PhoneOffHookFeedback.OutputChange += (sender, args) =>
            {
                if (!args.BoolValue)
                    _numberToDial = new StringBuilder();

                if (args.BoolValue && ClearDialstringWhenConnected)
                    _numberToDial = new StringBuilder();

                DialStringFeedback.FireUpdate();
            };
        }

        public void SetVolume(ushort level)
        {
            ((IBasicVolumeWithFeedback) _atcRx).SetVolume(level);
        }

        public void MuteOn()
        {
            ((IBasicVolumeWithFeedback) _atcRx).MuteOn();
        }

        public void MuteOff()
        {
            ((IBasicVolumeWithFeedback) _atcRx).MuteOff();
        }

        public IntFeedback VolumeLevelFeedback
        {
            get { return _atcRx.VolumeLevelFeedback; } }

        public BoolFeedback MuteFeedback
        {
            get { return _atcRx.MuteFeedback; } }

        public void DoNotDisturbToggle()
        {
            if (_isIsDnd)
            {
                DoNotDisturbOff();
            }
            else
            {
                DoNotDisturbOn();
            }

        }

        public void DoNotDisturbOn()
        {
            var command = FaderUtils.GetStateCommand(DndId, true);
            Coms.SendText(command);
        }

        public void DoNotDisturbOff()
        {
            var command = FaderUtils.GetStateCommand(DndId, false);
            Coms.SendText(command);
        }

        public void DialPhoneCall(string number)
        {
            if (string.IsNullOrEmpty(number))
                return;

            var command = DialerUtils.GetNumberToDialCommand(number, UnitNumber, DialStringId, CardSlot);
            Coms.SendText(command);
        }

        public void AcceptIncomingCall()
        {
            if (!_isRinging)
                return;

            var command = FaderUtils.GetStateCommand(ConnectAndDisconnectId, true);
            Coms.SendText(command);
        }

        public void RejectIncomingCall()
        {
            if (!_isRinging)
                return;

            var command = FaderUtils.GetStatePulseCommand(RejectId);
            Coms.SendText(command);
        }

        public void EndPhoneCall()
        {
            var command = string.Empty;
            if (_isRinging)
                command = FaderUtils.GetStatePulseCommand(RejectId);

            if (PhoneOffHookFeedback.BoolValue)
                command = FaderUtils.GetStatePulseCommand(ConnectAndDisconnectId);

            if (string.IsNullOrEmpty(command))
                return;

            Coms.SendText(command);
        }

        public void SendDtmfToPhone(string digit)
        {
            if(!PhoneOffHookFeedback.BoolValue)
                return;

            DialerUtils
                .ParseStringToKeypad(digit)
                .ToList()
                .ForEach(SendDtmfToPhone);
        }

        public void SetDialString(string dialString)
        {
            _numberToDial = new StringBuilder(dialString);
            DialStringFeedback.FireUpdate();
        }

        public void HandleKeypadPress(EKeypadKeys digit)
        {
            switch (digit)
            {
                case EKeypadKeys.Clear:
                    _numberToDial = new StringBuilder();
                    break;
                case EKeypadKeys.Backspace:
                    _numberToDial.Remove(_numberToDial.Length - 1, 1);
                    break;
                default:
                    var stringToAppend = DialerUtils.ParseKeypadToString(digit);
                    _numberToDial.Append(stringToAppend);
                    break;
            }

            DialStringFeedback.FireUpdate();

            if (!PhoneOffHookFeedback.BoolValue)
                SendDtmfToPhone(digit);
        }

        public void SendDtmfToPhone(EKeypadKeys digit)
        {
            if (!PhoneOffHookFeedback.BoolValue)
                return;

            var command = string.Empty;
            switch (digit)
            {
                case EKeypadKeys.Num1:
                    command = FaderUtils.GetStatePulseCommand(Keypad1Id);
                    break;
                case EKeypadKeys.Num2:
                    command = FaderUtils.GetStatePulseCommand(Keypad2Id);
                    break;
                case EKeypadKeys.Num3:
                    command = FaderUtils.GetStatePulseCommand(Keypad3Id);
                    break;
                case EKeypadKeys.Num4:
                    command = FaderUtils.GetStatePulseCommand(Keypad4Id);
                    break;
                case EKeypadKeys.Num5:
                    command = FaderUtils.GetStatePulseCommand(Keypad5Id);
                    break;
                case EKeypadKeys.Num6:
                    command = FaderUtils.GetStatePulseCommand(Keypad6Id);
                    break;
                case EKeypadKeys.Num7:
                    command = FaderUtils.GetStatePulseCommand(Keypad7Id);
                    break;
                case EKeypadKeys.Num8:
                    command = FaderUtils.GetStatePulseCommand(Keypad8Id);
                    break;
                case EKeypadKeys.Num9:
                    command = FaderUtils.GetStatePulseCommand(Keypad9Id);
                    break;
                case EKeypadKeys.Num0:
                    command = FaderUtils.GetStatePulseCommand(Keypad0Id);
                    break;
                case EKeypadKeys.Star:
                    command = FaderUtils.GetStatePulseCommand(KeypadStarId);
                    break;
                case EKeypadKeys.Pound:
                    command = FaderUtils.GetStatePulseCommand(KeypadPoundId);
                    break;
            }

            if (string.IsNullOrEmpty(command))
                return;

            Coms.SendText(command);
        }

        public override void LinkToApi(BasicTriList trilist, uint joinStart, string joinMapKey, EiscApiAdvanced bridge)
        {
            var joinMap = new DialerJoinMap(joinStart);
            if (bridge != null)
                bridge.AddJoinMap(Key, joinMap);

            trilist.SetSigTrueAction((joinMap.Keypad0.JoinNumber), () => HandleKeypadPress(EKeypadKeys.Num0));
            trilist.SetSigTrueAction((joinMap.Keypad1.JoinNumber), () => HandleKeypadPress(EKeypadKeys.Num1));
            trilist.SetSigTrueAction((joinMap.Keypad2.JoinNumber), () => HandleKeypadPress(EKeypadKeys.Num2));
            trilist.SetSigTrueAction((joinMap.Keypad3.JoinNumber), () => HandleKeypadPress(EKeypadKeys.Num3));
            trilist.SetSigTrueAction((joinMap.Keypad4.JoinNumber), () => HandleKeypadPress(EKeypadKeys.Num4));
            trilist.SetSigTrueAction((joinMap.Keypad5.JoinNumber), () => HandleKeypadPress(EKeypadKeys.Num5));
            trilist.SetSigTrueAction((joinMap.Keypad6.JoinNumber), () => HandleKeypadPress(EKeypadKeys.Num6));
            trilist.SetSigTrueAction((joinMap.Keypad7.JoinNumber), () => HandleKeypadPress(EKeypadKeys.Num7));
            trilist.SetSigTrueAction((joinMap.Keypad8.JoinNumber), () => HandleKeypadPress(EKeypadKeys.Num8));
            trilist.SetSigTrueAction((joinMap.Keypad9.JoinNumber), () => HandleKeypadPress(EKeypadKeys.Num9));
            trilist.SetSigTrueAction((joinMap.KeypadStar.JoinNumber), () => HandleKeypadPress(EKeypadKeys.Star));
            trilist.SetSigTrueAction((joinMap.KeypadPound.JoinNumber), () => HandleKeypadPress(EKeypadKeys.Pound));
            trilist.SetSigTrueAction((joinMap.KeypadClear.JoinNumber), () => HandleKeypadPress(EKeypadKeys.Clear));
            trilist.SetSigTrueAction((joinMap.KeypadBackspace.JoinNumber), () => HandleKeypadPress(EKeypadKeys.Backspace));
            trilist.SetSigTrueAction(joinMap.Dial.JoinNumber, () => DialPhoneCall(_numberToDial.ToString()));
            trilist.SetStringSigAction(joinMap.DialString.JoinNumber, SetDialString);
            trilist.SetSigTrueAction(joinMap.DoNotDisturbToggle.JoinNumber, DoNotDisturbToggle);
            trilist.SetSigTrueAction(joinMap.DoNotDisturbOn.JoinNumber, DoNotDisturbOn);
            trilist.SetSigTrueAction(joinMap.DoNotDisturbOff.JoinNumber, DoNotDisturbOff);
            // trilist.SetSigTrueAction(joinMap.AutoAnswerToggle + dialerLineOffset, () => dialer.Value.AutoAnswerToggle());
            // trilist.SetSigTrueAction(joinMap.AutoAnswerOn + dialerLineOffset, () => dialer.Value.AutoAnswerOn());
            // trilist.SetSigTrueAction(joinMap.AutoAnswerOff + dialerLineOffset, () => dialer.Value.AutoAnswerOff());
            trilist.SetSigTrueAction(joinMap.End.JoinNumber, EndPhoneCall);
            trilist.SetSigTrueAction(joinMap.IncomingCallAccept.JoinNumber, AcceptIncomingCall);
            trilist.SetSigTrueAction(joinMap.IncomingCallReject.JoinNumber, RejectIncomingCall);

            // from Plugin > to SiMPL
            DoNotDisturbFeedback.LinkInputSig(trilist.BooleanInput[joinMap.DoNotDisturbToggle.JoinNumber]);
            DoNotDisturbFeedback.LinkInputSig(trilist.BooleanInput[joinMap.DoNotDisturbOn.JoinNumber]);
            DoNotDisturbFeedback.LinkComplementInputSig(trilist.BooleanInput[joinMap.DoNotDisturbOff.JoinNumber]);

            // from Plugin > to SiMPL
            CallerIdNumberFeedback.LinkInputSig(trilist.StringInput[joinMap.CallerIdNumberFeedback.JoinNumber]);

            // from Plugin > to SiMPL
            PhoneOffHookFeedback.LinkInputSig(trilist.BooleanInput[joinMap.OffHook.JoinNumber]);
            PhoneOffHookFeedback.LinkComplementInputSig(trilist.BooleanInput[joinMap.OnHook.JoinNumber]);
            DialStringFeedback.LinkInputSig(trilist.StringInput[joinMap.DialString.JoinNumber]);

            // from Plugin > to SiMPL
            IncomingCallFeedback.LinkInputSig(trilist.BooleanInput[joinMap.IncomingCall.JoinNumber]);
        }

        public void VolumeUp(bool pressRelease)
        {
            ((IBasicVolumeControls) _atcRx).VolumeUp(pressRelease);
        }

        public void VolumeDown(bool pressRelease)
        {
            ((IBasicVolumeControls) _atcRx).VolumeDown(pressRelease);
        }

        public void MuteToggle()
        {
            ((IBasicVolumeControls) _atcRx).MuteToggle();
        }


        public static class ResourceId
        {
            public const int SpeedDialNumber = 1000;
            public const int SpeedDialName = 1001;
            public const int DialedNumber = 1002;
            public const int CallId = 1003;
            public const int NumberToDial = 1004;
            public const int CallStatus = 1005;
            public const int CallTimer = 1005;
        }

        public BoolFeedback IsOnline { get; private set; }
    }

    // SSYSS <unit>.<resource>.<enum>.<card>.<channel>=[<value>]<CR>
    /* 
        <unit> is the unit enumerator after the dash shown in Composer above each unit, e.g. “Edge-1” means <unit> = 1.
        <enum> is zero based, 0-19 for speed dials 1-20. 
        <card> is 0-3 for plug-in slots A-D.
        <channel> is zero based and 0 where not applicable. 
    */
}