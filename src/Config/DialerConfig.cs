namespace SymetrixComposerEpi.Config
{
    public class DialerConfig
    {
        public bool ClearDialstringWhenConnected { get; set; }
        public int RxVolumeId { get; set; }
        public int RxMuteId { get; set; }
        public int? RxUserMaximum { get; set; }
        public int? RxUserMinimum { get; set; }
        public int UnitNumber { get; set; }
        public int CardSlot { get; set; }
        public int LineNumber { get; set; }
        public int IsRingingId { get; set; }
        public int IsConnectedId { get; set; }
        public int IsOnHoldId { get; set; }
        public int IsBusyId { get; set; }
        public int IsDialingId { get; set; }
        public int DialStringId { get; set; }
        public int ConnectAndDisconnectId { get; set; }
        public int RejectId { get; set; }
        public int DndId { get; set; }
        public int RedialId { get; set; }
        public int KeypadBackspaceId { get; set; }
        public int KeypadClearId { get; set; }
        public int Keypad1Id { get; set; }
        public int Keypad2Id { get; set; }
        public int Keypad3Id { get; set; }
        public int Keypad4Id { get; set; }
        public int Keypad5Id { get; set; }
        public int Keypad6Id { get; set; }
        public int Keypad7Id { get; set; }
        public int Keypad8Id { get; set; }
        public int Keypad9Id { get; set; }
        public int Keypad0Id { get; set; }
        public int KeypadPoundId { get; set; }
        public int KeypadStarId { get; set; }
    }
}