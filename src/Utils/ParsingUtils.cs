using PepperDash.Core;

namespace PepperDash.Essentials.Plugin.SymetrixComposer.Utils
{
    public static class ParsingUtils
    {
        /// <summary>
        /// Parses controller ID from response, ex. '#00007=12321\r'
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static ushort ParseControllerId(string response)
        {
            const ushort defaultResult = 0;
            if (string.IsNullOrEmpty(response) || !response.StartsWith("#"))
                return defaultResult;

            var stringToParse = CleanResponse(response);
            var id = stringToParse.Split('=')[0];
            Debug.LogVerbose("[Symetrix ParsingUtils] ParseControllerId: response-'{0}' id-'{1}'", response, id);

            ushort result;
            return ushort.TryParse(id, out result) ? result : defaultResult;
        }

        /// <summary>
        /// Parses volume response
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static ushort ParseVolume(string response)
        {
            const ushort defaultResult = 0;
            if (string.IsNullOrEmpty(response) || !response.StartsWith("#"))
                return defaultResult;

            var segments = response.Split('=');
            if (segments.Length < 2)
                return defaultResult;

            var result = segments[1];
            Debug.LogVerbose("[Symetrix ParsingUtils] ParseVolume: response-'{0}' result-'{1}'", response, result);

            ushort volume;
            return ushort.TryParse(result, out volume) ? volume : defaultResult;
        }

        /// <summary>
        /// Parses mute state response
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static bool ParseState(string response)
        {
            const bool defaultResult = false;
            if (string.IsNullOrEmpty(response) || !response.StartsWith("#"))
                return defaultResult;

            var segments = response.Split('=');
            if (segments.Length < 2)
                return defaultResult;

            ushort muteResult;
            if (!ushort.TryParse(segments[1], out muteResult))
                return defaultResult;

            Debug.LogVerbose("[Symetrix ParsingUtils] ParseState: response-'{0}' muteResult-'{1}'", response, muteResult);
            return muteResult == ushort.MaxValue;
        }

        /// <summary>
        /// Cleans the response by removing the prefixed char '#' 
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string CleanResponse(string response)
        {
            var stringToParse = response.Trim().Replace("#", "");
            return stringToParse;
        }

    }
}
