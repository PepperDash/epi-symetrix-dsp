namespace SymetrixComposerEpi
{
    public class FaderConfig
    {
        public string Label { get; set; }
        public int LevelControlId { get; set; }
        public int MuteControlId { get; set; }
        public bool Disabled { get; set; }
        public bool IsMic { get; set; }
        public bool UnmuteOnVolChange { get; set; }
        public int? UserMinimum { get; set; }
        public int? UserMaximim { get; set; }
        public int? FaderMinimum { get; set; }
        public int? FaderMaximum { get; set; }
        public int? Increment { get; set; }
    }
}