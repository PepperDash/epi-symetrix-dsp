﻿using Newtonsoft.Json;

namespace PepperDashPluginSymetrixComposer.Config
{
    public class FaderConfig
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("levelControlId")]
        public int LevelControlId { get; set; }
        
        [JsonProperty("muteControlId")]
        public int MuteControlId { get; set; }

        [JsonProperty("disabled")]
        public bool Disabled { get; set; }

        [JsonProperty("isMic")]
        public bool IsMic { get; set; }
        
        [JsonProperty("unmuteOnVolChange")]
        public bool UnmuteOnVolChange { get; set; }

        [JsonProperty("userMinimum")]
        public int? UserMinimum { get; set; }

        [JsonProperty("userMaximum")]
        public int? UserMaximum { get; set; }

        [JsonProperty("faderMinimum")]
        public int? FaderMinimum { get; set; }

        [JsonProperty("faderMaximum")]
        public int? FaderMaximum { get; set; }

        [JsonProperty("increment")]
        public int? Increment { get; set; }

        /// <summary>
        /// Fader has level controls
        /// </summary>
        [JsonProperty("hasLevel")]
        public bool HasLevel { get; set; }

        /// <summary>
        /// Fader has mute controls
        /// </summary>
        [JsonProperty("hasMute")]
        public bool HasMute { get; set; }

        /// <summary>
        /// Sets the fader permissions, 0d = User & Tech, 1d = User only, 2d = Tech only
        /// </summary>
        [JsonProperty("permissions")]
        public int? Permissions { get; set; }
    }
}