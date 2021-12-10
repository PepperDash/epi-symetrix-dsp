using System;
using System.Linq;
using System.Text;
using Crestron.SimplSharpPro.DeviceSupport;
using PepperDash.Core;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.Core.Bridges;
using PepperDash.Essentials.Core.DeviceTypeInterfaces;
using SymetrixComposerEpi.Config;
using SymetrixComposerEpi.Utils;

namespace SymetrixComposerEpi
{
    // Todo: Add IOnline
    public class SymetrixComposerDialer : EssentialsBridgeableDevice, IBasicVolumeWithFeedback, IHasPhoneDialing, IOnline
    {
        public string CallerId
        {
            set
            {
                _callerId = value;
                CallerIdNumberFeedback.FireUpdate();
            }
        }

        public bool IsRinging
        {
            set
            {
                _isRinging = value;
                IncomingCallFeedback.FireUpdate();
                DialStringFeedback.FireUpdate();
            }
        }

        public bool IsConnected
        {
            set
            {
                _isConnected = value;
                PhoneOffHookFeedback.FireUpdate();
                DialStringFeedback.FireUpdate();
            }
        }

        public bool IsOnHold
        {
            set
            {
                _isOnHold = value;
                PhoneOffHookFeedback.FireUpdate();
                DialStringFeedback.FireUpdate();
            }
        }

        public bool IsBusy
        {
            set
            {
                _isBusy = value;
                PhoneOffHookFeedback.FireUpdate();
                DialStringFeedback.FireUpdate();
            }
        }

        public bool IsDialing
        {
            set
            {
                _isDialing = value;
                PhoneOffHookFeedback.FireUpdate();
                DialStringFeedback.FireUpdate();
            }
        }

        public bool IsInDnd
        {
            set
            {
                _isIsDnd = value;
                PhoneOffHookFeedback.FireUpdate();
                DialStringFeedback.FireUpdate();
            }
        }

        public BoolFeedback PhoneOffHookFeedback { get; private set; }
        public StringFeedback CallerIdNameFeedback { get; private set; }
        public StringFeedback CallerIdNumberFeedback { get; private set; }

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

        private readonly SymetrixComposerLevelControl _atcRx;

        public SymetrixComposerDialer(string key, DialerConfig config, IBasicCommunication coms) : base(key)
        {
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

            _atcRx = new SymetrixComposerLevelControl(Key + "-AtcRx", new FaderConfig
            {
                Label = Key + " Rx",
                LevelControlId = config.RxVolumeId,
                MuteControlId = config.RxMuteId,
                UserMaximim = config.RxUserMaximum,
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

        /// <summary>
        /// Toggles the do not disturb state
        /// </summary>
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

        /// <summary>
        /// Sets the do not disturb state on
        /// </summary>
        public void DoNotDisturbOn()
        {
            throw new NotImplementedException();
            // Parent.SendLine(string.Format("csv \"{0}\" 1", Tags.DoNotDisturbTag));
        }

        /// <summary>
        /// Sets the do not disturb state off
        /// </summary>
        public void DoNotDisturbOff()
        {
            throw new NotImplementedException();
            // Parent.SendLine(string.Format("csv \"{0}\" 0", Tags.DoNotDisturbTag));
        }

        public void DialPhoneCall(string number)
        {
            if (string.IsNullOrEmpty(number))
                return;

            var command = DialerUtils.GetNumberToDialCommand(number, UnitNumber, DialStringId, CardSlot);
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
            throw new NotImplementedException();
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