using PepperDash.Essentials.Core;

namespace SymetrixComposerEpi.JoinMaps
{
    public class DialerJoinMap : JoinMapBaseAdvanced
    {
        [JoinName("IncomingCall")]
        public JoinDataComplete IncomingCall =
            new JoinDataComplete(new JoinData { JoinNumber = 1, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Call Incoming",
                    JoinCapabilities = eJoinCapabilities.ToSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("IncomingCallAccept")]
        public JoinDataComplete IncomingCallAccept =
            new JoinDataComplete(new JoinData { JoinNumber = 37, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Call Incoming",
                    JoinCapabilities = eJoinCapabilities.ToSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("IncomingCallReject")]
        public JoinDataComplete IncomingCallReject =
            new JoinDataComplete(new JoinData { JoinNumber = 38, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Call Incoming",
                    JoinCapabilities = eJoinCapabilities.ToSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("End")]
        public JoinDataComplete End =
            new JoinDataComplete(new JoinData { JoinNumber = 8, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "End Call",
                    JoinCapabilities = eJoinCapabilities.FromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("Keypad0")]
        public JoinDataComplete Keypad0 =
            new JoinDataComplete(new JoinData { JoinNumber = 11, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Keypad Digits 0-9",
                    JoinCapabilities = eJoinCapabilities.FromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("Keypad1")]
        public JoinDataComplete Keypad1 =
            new JoinDataComplete(new JoinData { JoinNumber = 12, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Keypad Digits 0-9",
                    JoinCapabilities = eJoinCapabilities.FromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("Keypad2")]
        public JoinDataComplete Keypad2 =
            new JoinDataComplete(new JoinData { JoinNumber = 13, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Keypad Digits 0-9",
                    JoinCapabilities = eJoinCapabilities.FromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("Keypad3")]
        public JoinDataComplete Keypad3 =
            new JoinDataComplete(new JoinData { JoinNumber = 14, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Keypad Digits 0-9",
                    JoinCapabilities = eJoinCapabilities.FromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("Keypad4")]
        public JoinDataComplete Keypad4 =
            new JoinDataComplete(new JoinData { JoinNumber = 15, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Keypad Digits 0-9",
                    JoinCapabilities = eJoinCapabilities.FromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("Keypad5")]
        public JoinDataComplete Keypad5 =
            new JoinDataComplete(new JoinData { JoinNumber = 16, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Keypad Digits 0-9",
                    JoinCapabilities = eJoinCapabilities.FromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("Keypad6")]
        public JoinDataComplete Keypad6 =
            new JoinDataComplete(new JoinData { JoinNumber = 17, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Keypad Digits 0-9",
                    JoinCapabilities = eJoinCapabilities.FromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("Keypad7")]
        public JoinDataComplete Keypad7 =
            new JoinDataComplete(new JoinData { JoinNumber = 18, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Keypad Digits 0-9",
                    JoinCapabilities = eJoinCapabilities.FromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("Keypad8")]
        public JoinDataComplete Keypad8 =
            new JoinDataComplete(new JoinData { JoinNumber = 19, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Keypad Digits 0-9",
                    JoinCapabilities = eJoinCapabilities.FromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("Keypad9")]
        public JoinDataComplete Keypad9 =
            new JoinDataComplete(new JoinData { JoinNumber = 20, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Keypad Digits 0-9",
                    JoinCapabilities = eJoinCapabilities.FromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("KeypadStar")]
        public JoinDataComplete KeypadStar =
            new JoinDataComplete(new JoinData { JoinNumber = 21, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Keypad *",
                    JoinCapabilities = eJoinCapabilities.FromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("KeypadPound")]
        public JoinDataComplete KeypadPound =
            new JoinDataComplete(new JoinData { JoinNumber = 22, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Keypad #",
                    JoinCapabilities = eJoinCapabilities.FromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("KeypadClear")]
        public JoinDataComplete KeypadClear =
            new JoinDataComplete(new JoinData { JoinNumber = 23, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Keypad Clear",
                    JoinCapabilities = eJoinCapabilities.FromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("KeypadBackspace")]
        public JoinDataComplete KeypadBackspace =
            new JoinDataComplete(new JoinData { JoinNumber = 24, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Keypad Backspace",
                    JoinCapabilities = eJoinCapabilities.FromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("Dial")]
        public JoinDataComplete Dial =
            new JoinDataComplete(new JoinData { JoinNumber = 25, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Keypad Dial and Feedback",
                    JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("AutoAnswerOn")]
        public JoinDataComplete AutoAnswerOn =
            new JoinDataComplete(new JoinData { JoinNumber = 26, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Auto Answer On and Feedback",
                    JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("AutoAnswerOff")]
        public JoinDataComplete AutoAnswerOff =
            new JoinDataComplete(new JoinData { JoinNumber = 27, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Auto Answer Off and Feedback",
                    JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("AutoAnswerToggle")]
        public JoinDataComplete AutoAnswerToggle =
            new JoinDataComplete(new JoinData { JoinNumber = 28, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Auto Answer Toggle and On Feedback",
                    JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("OnHook")]
        public JoinDataComplete OnHook =
            new JoinDataComplete(new JoinData { JoinNumber = 30, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "On Hook Set and Feedback",
                    JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("OffHook")]
        public JoinDataComplete OffHook =
            new JoinDataComplete(new JoinData { JoinNumber = 31, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Off Hook Set and Feedback",
                    JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("DoNotDisturbToggle")]
        public JoinDataComplete DoNotDisturbToggle =
            new JoinDataComplete(new JoinData { JoinNumber = 33, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Do Not Disturb Toggle and Feedback",
                    JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("DoNotDisturbOn")]
        public JoinDataComplete DoNotDisturbOn =
            new JoinDataComplete(new JoinData { JoinNumber = 34, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Do Not Disturb On Set and Feedback",
                    JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
                    JoinType = eJoinType.Digital
                });

        [JoinName("DoNotDisturbOff")]
        public JoinDataComplete DoNotDisturbOff =
            new JoinDataComplete(new JoinData { JoinNumber = 35, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Do Not Disturb Of Set and Feedback",
                    JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
                    JoinType = eJoinType.Digital
                });


        [JoinName("DialString")]
        public JoinDataComplete DialString =
            new JoinDataComplete(new JoinData { JoinNumber = 1, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Dial String Send and Feedback",
                    JoinCapabilities = eJoinCapabilities.ToFromSIMPL,
                    JoinType = eJoinType.Serial
                });


        [JoinName("CallerIdNumberFeedback")]
        public JoinDataComplete CallerIdNumberFeedback =
            new JoinDataComplete(new JoinData { JoinNumber = 5, JoinSpan = 1 },
                new JoinMetadata
                {
                    Description = "Caller ID Number",
                    JoinCapabilities = eJoinCapabilities.ToSIMPL,
                    JoinType = eJoinType.Serial
                });


        public DialerJoinMap(uint joinStart)
            : base(joinStart, typeof(DialerJoinMap))
        {
        }
    }
}