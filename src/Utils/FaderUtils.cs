using System;
using PepperDash.Core;

namespace PepperDashPluginSymetrixComposer.Utils
{
    public static class FaderUtils
    {
        private const int DebugLevel1 = 1;
        private const int DebugLevel2 = 2;

        /// <summary>
        /// Controller set command format
        /// ex. 'CS {CONTROLLER_NUMBER} {CONTROLLER_POSITION}\r'
        /// </summary>
        public const string CommandSetFormat = "CS {0} {1}\r";

        /// <summary>
        /// Controller change increment command format
        /// ex. 'CC {CONTROLLER_NUMBER} {INC/DEC} {AMOUNT}\r'
        /// </summary>
        public const string VolumeIncrementFormat = "CC {0} INC {1}\r";

        /// <summary>
        /// Controller change decrement command format
        /// ex. 'CC {CONTROLLER_NUMBER} {INC/DEC} {AMOUNT}\r'
        /// </summary>
        public const string VolumeDecrementFormat = "CC {0} DEC {1}\r";

        public static int ScaleToRange(
            int initial,
            int sourceMin,
            int sourceMax,
            int destMin,
            int destMax)
        {
            var destSpan = destMax - destMin;
            var sourceSpan = sourceMax - sourceMin;
            return (destMin + (destSpan * (initial - sourceMin)) / (sourceSpan));
        }

        public static ushort ScaleToUshortRange(
            int initial,
            int sourceMin,
            int sourceMax)
        {
            return (ushort)ScaleToRange(initial, sourceMin, sourceMax, ushort.MinValue, ushort.MaxValue);
            //return (ushort)CrestronEnvironment.ScaleWithLimits(initial, sourceMax, sourceMin, ushort.MaxValue, ushort.MinValue);
        }

        public static ushort ScaleFromUshortRange(
            int initial,
            int destMin,
            int destMax)
        {
            return (ushort)ScaleToRange(initial, ushort.MinValue, ushort.MaxValue, destMin, destMax);
            //return (ushort)CrestronEnvironment.ScaleWithLimits(initial, ushort.MaxValue, ushort.MinValue, destMax, destMin);
        }

        public static string GetStateCommand(int controllerId, bool state)
        {
            return string.Format(CommandSetFormat, controllerId, state ? ushort.MaxValue : ushort.MinValue);
        }

        public static string GetStatePulseCommand(int controllerId)
        {
            var stateOnCommand = GetStateCommand(controllerId, true);
            var stateOffCommand = GetStateCommand(controllerId, false);

            return string.Concat(stateOnCommand, stateOffCommand);
        }

        public static ushort ScalePositionFromUserLimits(
            ushort controllerPosition,
            int userMinimum,
            int userMaximum,
            int faderMin,
            int faderMax)
        {
            var scaledUserMinimum = ScaleToUshortRange(userMinimum, faderMin, faderMax);
            var scaledUserMaximum = ScaleToUshortRange(userMaximum, faderMin, faderMax);
            return ScaleToUshortRange(
                controllerPosition,
                scaledUserMinimum,
                scaledUserMaximum);

            //var scaledUserMinimum = CrestronEnvironment.ScaleWithLimits(userMinimum, faderMax, faderMin,
            //    ushort.MaxValue, ushort.MinValue);
            //var scaledUserMaximum = CrestronEnvironment.ScaleWithLimits(userMaximum, faderMax, faderMin,
            //    ushort.MaxValue, ushort.MinValue);

            //return (ushort)CrestronEnvironment.ScaleWithLimits(controllerPosition, ushort.MaxValue, ushort.MinValue,
            //            scaledUserMaximum, scaledUserMinimum);
        }

        public static string GetVolumeCommand(
            int controllerId,
            ushort controllerPosition,
            int userMinimum,
            int userMaximum,
            int faderMin,
            int faderMax)
        {
            var scaledControllerPosition = ScalePositionFromUserLimits(
                controllerPosition,
                userMinimum,
                userMaximum,
                faderMin,
                faderMax);

            //var scaledControllerPosition = CrestronEnvironment.ScaleWithLimits(controllerPosition, userMaximum,
            //    userMaximum, faderMax, faderMin);

            return string.Format(CommandSetFormat, controllerId, scaledControllerPosition);
        }

        public static string GetVolumeUpCommand(
            int controllerId,
            int increment,
            int faderMin,
            int faderMax)
        {
            var scaledIncrement = CalculateIncrementPercentage(increment, faderMin, faderMax);
            return string.Format(VolumeIncrementFormat, controllerId, scaledIncrement);
        }

        public static string GetVolumeDownCommand(
            int controllerId,
            int increment,
            int faderMin,
            int faderMax)
        {
            var scaledIncrement = CalculateIncrementPercentage(increment, faderMin, faderMax);
            return string.Format(VolumeDecrementFormat, controllerId, scaledIncrement);
        }

        public static double CalculateIncrementPercentage(int increment, int faderMin, int faderMax)
        {
            var incrementPercentage = 100 * ((double)increment / (faderMax - faderMin));
            var scaledIncrement = Math.Round(incrementPercentage / 100 * ushort.MaxValue);
            return scaledIncrement;
        }

        public static Action<SymetrixComposerFader> UpdateVolumeControllerPosition(string response)
        {
            Debug.Console(DebugLevel2, "[Symetrix FaderUtils] UpdateVolumeControllerPosition: response = {0}", response);
            return f => f.Volume = ParsingUtils.ParseVolume(response);
        }

        public static Action<SymetrixComposerFader> UpdateMuteControllerPosition(string response)
        {
            Debug.Console(DebugLevel2, "[Symetrix FaderUtils] UpdateMuteControllerPosition: response = {0}", response);
            return f => f.IsMuted = ParsingUtils.ParseState(response);
        }
    }
}