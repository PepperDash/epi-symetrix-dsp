using System;
using PepperDash.Core;

namespace SymetrixComposerEpi.Utils
{
    public static class ParsingUtils
    {
        // #00007=12321<CR>
        public static ushort ParseControllerId(string response)
        {
            const int defaultResult = 0;
            if (string.IsNullOrEmpty(response) || !response.StartsWith("#"))
                return defaultResult;

            var stringToParse = CleanResponse(response);
            var id = stringToParse.Split('=')[0];
            Debug.Console(1, "SymmetrixDSP - Mapped Response To ControllerId:{0} | {1}", response, id);
            return Convert.ToUInt16(id);
        }

        public static ushort ParseVolume(string response)
        {
            const int defaultResult = 0;
            if (string.IsNullOrEmpty(response) || !response.StartsWith("#"))
                return defaultResult;

            var result = response.Split('=')[1];
            Debug.Console(1, "SymmetrixDSP - Mapped Response To Int:{0} | {1}", response, result);
            return Convert.ToUInt16(result);
        }

        public static string CleanResponse(string response)
        {
            var stringToParse = response.Trim().Replace("#", "");
            return stringToParse;
        }

        public static bool ParseState(string response)
        {
            const bool defaultResult = false;
            if (string.IsNullOrEmpty(response) || !response.StartsWith("#"))
                return defaultResult;

            var result = response.Split('=')[1];
            var muteResult = Convert.ToUInt16(result);
            return muteResult == ushort.MaxValue;
        }
    }
}