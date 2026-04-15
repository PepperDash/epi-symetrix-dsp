using System;
using PepperDash.Core;

namespace PepperDashPluginSymetrixComposer.Utils
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
            const int defaultResult = 0;
            if (string.IsNullOrEmpty(response) || !response.StartsWith("#"))
                return defaultResult;

            var stringToParse = CleanResponse(response);
            var id = stringToParse.Split('=')[0];
            Debug.LogVerbose("[Symetrix ParsingUtils] ParseControllerId: response-'{response}' id-'{id}'", response, id);
            return Convert.ToUInt16(id);
        }

        /// <summary>
        /// Parses volume response
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static ushort ParseVolume(string response)
        {
            const int defaultResult = 0;
            if (string.IsNullOrEmpty(response) || !response.StartsWith("#"))
                return defaultResult;

            var result = response.Split('=')[1];
            Debug.LogVerbose("[Symetrix ParsingUtils] ParseVolume: response-'{response}' result-'{result}'", response, result);
            return Convert.ToUInt16(result);
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

            var result = response.Split('=')[1];
            var muteResult = Convert.ToUInt16(result);
            Debug.LogVerbose("[Symetrix ParsingUtils] ParseState: response-'{response}' muteResult-'{muteResult}'", response, muteResult);
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
