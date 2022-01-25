using System;
using System.Collections.Generic;
using System.Linq;
using PepperDashPluginSymetrixComposer.Enums;

namespace PepperDashPluginSymetrixComposer.Utils
{
    public static class DialerUtils
    {
        public static bool HasControllerId(this SymetrixComposerDialer dialer, int controllerId)
        {
            return dialer.IsRingingId == controllerId || dialer.IsConnectedId == controllerId ||
                   dialer.IsOnHoldId == controllerId || dialer.IsBusyId == controllerId ||
                   dialer.IsDialingId == controllerId;
        }

        public static string GetNumberToDialCommand(string numberToDial, int unit, int controllerId, int card)
        {
            const string dialCommandTemplate = "SSYSS {0}.{1}.0.{2}.0={3}\r";
            return string.Format(dialCommandTemplate, unit, controllerId, card, numberToDial);
        }

        public static IEnumerable<EKeypadKeys> ParseStringToKeypad(string digit)
        {
            return digit
                .Where(
                    d =>
                        d == '0' || d == '1' || d == '2' || d == '3' ||
                        d == '4' || d == '5' || d == '6' || d == '7' ||
                        d == '8' || d == '9' || d == '*' || d == '#')
                .Select(
                    d =>
                    {
                        if (d == '0')
                            return EKeypadKeys.Num0;

                        if (d == '1')
                            return EKeypadKeys.Num1;

                        if (d == '2')
                            return EKeypadKeys.Num2;

                        if (d == '3')
                            return EKeypadKeys.Num3;

                        if (d == '4')
                            return EKeypadKeys.Num4;

                        if (d == '5')
                            return EKeypadKeys.Num5;

                        if (d == '6')
                            return EKeypadKeys.Num6;

                        if (d == '7')
                            return EKeypadKeys.Num7;

                        if (d == '8')
                            return EKeypadKeys.Num8;

                        if (d == '9')
                            return EKeypadKeys.Num9;

                        if (d == '*')
                            return EKeypadKeys.Pound;

                        if (d == '#')
                            return EKeypadKeys.Star;

                        return EKeypadKeys.Clear;
                    });
        }

        public static string ParseKeypadToString(EKeypadKeys key)
        {
            switch (key)
            {
                case EKeypadKeys.Num1:
                    return "1";
                case EKeypadKeys.Num2:
                    return "2";
                case EKeypadKeys.Num3:
                    return "3";
                case EKeypadKeys.Num4:
                    return "4";
                case EKeypadKeys.Num5:
                    return "5";
                case EKeypadKeys.Num6:
                    return "6";
                case EKeypadKeys.Num7:
                    return "7";
                case EKeypadKeys.Num8:
                    return "8";
                case EKeypadKeys.Num9:
                    return "9";
                case EKeypadKeys.Num0:
                    return "00";
                case EKeypadKeys.Star:
                    return "*";
                case EKeypadKeys.Pound:
                    return "#";
                default:
                    return string.Empty;
            }
        }

        public static Action<SymetrixComposerDialer> ParseAndUpdateDialerState(string response, int controllerId)
        {
            return dialer =>
            {
                if (controllerId == dialer.IsBusyId)
                    dialer.IsBusy = ParsingUtils.ParseState(response);
                if (controllerId == dialer.IsConnectedId)
                    dialer.IsConnected = ParsingUtils.ParseState(response);
                if (controllerId == dialer.IsDialingId)
                    dialer.IsDialing = ParsingUtils.ParseState(response);
                if (controllerId == dialer.IsOnHoldId)
                    dialer.IsOnHold = ParsingUtils.ParseState(response);
                if (controllerId == dialer.DndId)
                    dialer.IsInDnd = ParsingUtils.ParseState(response);
            };
        }
    }
}