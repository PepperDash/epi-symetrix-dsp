using PepperDash.Essentials.Core;

namespace PepperDashPluginSymetrixComposer.JoinMaps
{
    public class ApplicationJoinMap : JoinMapBaseAdvanced
    {
        [JoinName("IsOnline")]
        public JoinDataComplete IsOnline = new JoinDataComplete(
            new JoinData
            {
                JoinNumber = 1,
                JoinSpan = 1
            },
            new JoinMetadata
            {
                Description = "Is Online",
                JoinCapabilities = eJoinCapabilities.FromSIMPL,
                JoinType = eJoinType.Digital
            });

        [JoinName("PresetRecallDiscrete")]
        public JoinDataComplete PresetRecallDiscrete = new JoinDataComplete(
            new JoinData
            {
                JoinNumber = 100,
                JoinSpan = 1
            },
            new JoinMetadata
            {
                Description = "Preset Recall Discrete",
                JoinCapabilities = eJoinCapabilities.FromSIMPL,
                JoinType = eJoinType.AnalogSerial
            });

        [JoinName("PresetRecall")]
        public JoinDataComplete PresetRecall = new JoinDataComplete(
            new JoinData
            {
                JoinNumber = 101,
                JoinSpan = 100
            },
            new JoinMetadata
            {
                Description = "Preset Recall",
                JoinCapabilities = eJoinCapabilities.FromSIMPL,
                JoinType = eJoinType.Digital
            });

        [JoinName("PresetName")]
        public JoinDataComplete PresetName = new JoinDataComplete(
            new JoinData
            {
                JoinNumber = 101,
                JoinSpan = 100
            },
            new JoinMetadata
            {
                Description = "Preset Name",
                JoinCapabilities = eJoinCapabilities.ToSIMPL,
                JoinType = eJoinType.Serial
            });

        public ApplicationJoinMap(uint joinStart)
            : base(joinStart, typeof(ApplicationJoinMap))
        {
        }
    }
}