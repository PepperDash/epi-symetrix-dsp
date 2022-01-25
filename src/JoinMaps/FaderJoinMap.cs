using PepperDash.Essentials.Core;

namespace PepperDashPluginSymetrixComposer.JoinMaps
{
    /// <summary>
    /// Join map for individual faders
    /// </summary>
    public class FaderJoinMap : JoinMapBaseAdvanced
    {
        [JoinName("Volume Up")] 
        public JoinDataComplete VolumeUp = new JoinDataComplete(
            new JoinData
            {
                JoinNumber = 1,
                JoinSpan = 1
            },
            new JoinMetadata
            {
                JoinCapabilities = eJoinCapabilities.FromSIMPL,
                JoinType = eJoinType.Digital,
                Description = "Volume Up"
            });

        [JoinName("Volume Down")] 
        public JoinDataComplete VolumeDown = new JoinDataComplete(
            new JoinData
            {
                JoinNumber = 2,
                JoinSpan = 1
            },
            new JoinMetadata
            {
                JoinCapabilities = eJoinCapabilities.FromSIMPL,
                JoinType = eJoinType.Digital,
                Description = "Volume Down"
            });

        [JoinName("Mute On")] 
        public JoinDataComplete MuteOn = new JoinDataComplete(
            new JoinData
            {
                JoinNumber = 3,
                JoinSpan = 1
            },
            new JoinMetadata
            {
                JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
                JoinType = eJoinType.Digital,
                Description = "Mute On"
            });

        [JoinName("Mute Off")] 
        public JoinDataComplete MuteOff = new JoinDataComplete(
            new JoinData
            {
                JoinNumber = 4,
                JoinSpan = 1
            },
            new JoinMetadata
            {
                JoinCapabilities = eJoinCapabilities.FromSIMPL,
                JoinType = eJoinType.Digital,
                Description = "Mute Off"
            });

        [JoinName("Mute Toggle")] 
        public JoinDataComplete MuteToggle = new JoinDataComplete(
            new JoinData
            {
                JoinNumber = 5,
                JoinSpan = 1
            },
            new JoinMetadata
            {
                JoinCapabilities = eJoinCapabilities.FromSIMPL,
                JoinType = eJoinType.Digital,
                Description = "Mute Toggle"
            });

        [JoinName("Volume")] 
        public JoinDataComplete Volume = new JoinDataComplete(
            new JoinData
            {
                JoinNumber = 1,
                JoinSpan = 1
            },
            new JoinMetadata
            {
                JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
                JoinType = eJoinType.Analog,
                Description = "Volume"
            });

        [JoinName("Type")] 
        public JoinDataComplete Type = new JoinDataComplete(
            new JoinData
            {
                JoinNumber = 2,
                JoinSpan = 1
            },
            new JoinMetadata
            {
                JoinCapabilities = eJoinCapabilities.ToSIMPL,
                JoinType = eJoinType.Analog,
                Description = "Fader mute type, level mute (0) or mic mute (1)",
                ValidValues = new[] {"0", "1"},
            });

        [JoinName("Permissions")]
        public JoinDataComplete Permissions = new JoinDataComplete(
            new JoinData()
            {
                JoinNumber = 3,
                JoinSpan = 1
            },
            new JoinMetadata()
            {
                Description = "Fader permissions, user & tech accessible (0), user only (1), tech only (2)",
                JoinCapabilities = eJoinCapabilities.ToSIMPL,
                JoinType = eJoinType.Analog,
                ValidValues = new[] { "0", "1", "2" },
            });

        [JoinName("Name")] 
        public JoinDataComplete Name = new JoinDataComplete(
            new JoinData
            {
                JoinNumber = 1,
                JoinSpan = 1
            },
            new JoinMetadata
            {
                JoinCapabilities = eJoinCapabilities.ToSIMPL,
                JoinType = eJoinType.Serial,
                Description = "Name"
            });

        public FaderJoinMap(uint joinStart) : base(joinStart, typeof (FaderJoinMap))
        {
        }
    }
}