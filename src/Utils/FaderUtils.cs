using System;
using SymetrixComposerEpi.Config;

namespace SymetrixComposerEpi.Utils
{
    public static class FaderUtils
    {
        // CS <CONTROLLER NUMBER> <CONTROLLER POSITION><CR>
        // CC <CONTROLLER NUMBER> <DEC/INC> <AMOUNT><CR>

        public const string CommandSetFormat = "CS {0} {1}\r";
        public const string VolumeIncrementFormat = "CS {0} INC {1}\r";
        public const string VolumeDecrementFormat = "CS {0} DEC {1}\r";

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
            return (ushort) ScaleToRange(initial, sourceMin, sourceMax, ushort.MinValue, ushort.MaxValue);
        }

        public static ushort ScaleFromUshortRange(
            int initial,
            int destMin,
            int destMax)
        {
            return (ushort) ScaleToRange(initial, ushort.MinValue, ushort.MaxValue, destMin, destMax);
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
            var incrementPercentage = 100 * ((double) increment / (faderMax - faderMin));
            var scaledIncrement = Math.Round(incrementPercentage / 100 * ushort.MaxValue);
            return scaledIncrement;
        }

        public static Action<SymetrixComposerLevelControl> UpdateVolumeControllerPosition(string response)
        {
            return f => f.Volume = ParsingUtils.ParseVolume(response);
        }

        public static Action<SymetrixComposerLevelControl> UpdateMuteControllerPosition(string response)
        {
            return f => f.IsMuted = ParsingUtils.ParseState(response);
        }
    }
}