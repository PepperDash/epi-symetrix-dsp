using PepperDash.Essentials.Core;

namespace PepperDashPluginSymetrixComposer.JoinMaps
{
    /// <summary>
    /// Join map for single EISC bridge instance
    /// </summary>
    public class FaderApplicationJoinMap : JoinMapBaseAdvanced
    {
        #region Digital

        [JoinName("MuteToggle")]
        public JoinDataComplete MuteToggle = new JoinDataComplete(
            new JoinData
            {
                JoinNumber = 201,
                JoinSpan = 1
            },
            new JoinMetadata
            {
                Description = "Mute Toggle",
                JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
                JoinType = eJoinType.Digital
            });

        [JoinName("MuteOn")]
        public JoinDataComplete MuteOn = new JoinDataComplete(
            new JoinData
            {
                JoinNumber = 401,
                JoinSpan = 1
            },
            new JoinMetadata
            {
                Description = "Mute On",
                JoinCapabilities = eJoinCapabilities.ToSIMPL,
                JoinType = eJoinType.Digital
            });

        [JoinName("MuteOff")]
        public JoinDataComplete MuteOff = new JoinDataComplete(
            new JoinData
            {
                JoinNumber = 601,
                JoinSpan = 1
            },
            new JoinMetadata
            {
                Description = "Mute Off",
                JoinCapabilities = eJoinCapabilities.ToSIMPL,
                JoinType = eJoinType.Digital
            });

        [JoinName("VolumeUp")]
        public JoinDataComplete VolumeUp = new JoinDataComplete(
            new JoinData
            {
                JoinNumber = 801,
                JoinSpan = 1
            },
            new JoinMetadata
            {
                Description = "Volume Up",
                JoinCapabilities = eJoinCapabilities.FromSIMPL,
                JoinType = eJoinType.Digital
            });

        [JoinName("VolumeDown")]
        public JoinDataComplete VolumeDown = new JoinDataComplete(
            new JoinData
            {
                JoinNumber = 1001,
                JoinSpan = 1
            },
            new JoinMetadata
            {
                Description = "Volume Down",
                JoinCapabilities = eJoinCapabilities.FromSIMPL,
                JoinType = eJoinType.Digital
            });

        #endregion

        #region Analog

        [JoinName("Volume")]
        public JoinDataComplete Volume = new JoinDataComplete(
            new JoinData
            {
                JoinNumber = 1,
                JoinSpan = 1
            },
            new JoinMetadata
            {
                Description = "Volume",
                JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
                JoinType = eJoinType.Analog
            });


        [JoinName("MuteIcon")]
        public JoinDataComplete Type = new JoinDataComplete(
            new JoinData
            {
                // +200 offset applied in SymetrixComposerDevice.LinkToApi
                JoinNumber = 201,
                JoinSpan = 1
            },
            new JoinMetadata
            {
                Description = "Fader mute type, level mute (0) or mic mute (1)",
                JoinCapabilities = eJoinCapabilities.ToSIMPL,
                JoinType = eJoinType.Analog,
                ValidValues = new[] {"0", "1"},
            });

        [JoinName("FaderControls")]
        public JoinDataComplete FaderControls = new JoinDataComplete(
            new JoinData
            {
                // +200 offset applied in SymetrixComposerDevice.LinkToApi
                JoinNumber = 401,
                JoinSpan = 1
            },
            new JoinMetadata
            {
                Description = "Fader controls, level & mute (0), level only (1), mute only (2)",
                JoinCapabilities = eJoinCapabilities.ToSIMPL,
                JoinType = eJoinType.Analog
            });

        [JoinName("Permissions")]
        public JoinDataComplete Permissions = new JoinDataComplete(
            new JoinData
            {
                // +200 offset applied in SymetrixComposerDevice.LinkToApi
                JoinNumber = 601,
                JoinSpan = 1
            },
            new JoinMetadata
            {
                Description = "Fader permissions, user & tech accessible (0), user only (1), tech only (2)",
                JoinCapabilities = eJoinCapabilities.ToSIMPL,
                JoinType = eJoinType.Analog
            });

        #endregion

        #region Serial


        [JoinName("Name")]
        public JoinDataComplete Name = new JoinDataComplete(
            new JoinData
            {
                JoinNumber = 1,
                JoinSpan = 1
            },
            new JoinMetadata
            {
                Description = "Name",
                JoinCapabilities = eJoinCapabilities.ToSIMPL,
                JoinType = eJoinType.Serial
            });

        #endregion

        public FaderApplicationJoinMap(uint joinStart)
            : base(joinStart, typeof(FaderApplicationJoinMap))
        {
        }
    }
}