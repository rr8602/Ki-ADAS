namespace Ki_ADAS.DB
{
    public class Model
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
        public double? Wheelbase { get; set; }
        public double? Fr_Distance { get; set; }
        public double? Fr_Height { get; set; }
        public double? Fr_InterDistance { get; set; }
        public double? Fr_Htu { get; set; }
        public double? Fr_Htl { get; set; }
        public double? Fr_Ts { get; set; }
        public double? Fr_AlignmentAxeOffset { get; set; }
        public double? Fr_Vv { get; set; }
        public double? Fr_StCt { get; set; }
        public bool Fr_IsTest { get; set; }
        public double? R_X { get; set; }
        public double? R_Y { get; set; }
        public double? R_Z { get; set; }
        public double? R_Angle { get; set; }
        public bool R_IsTest { get; set; }
        public double? L_X { get; set; }
        public double? L_Y { get; set; }
        public double? L_Z { get; set; }
        public double? L_Angle { get; set; }
        public bool L_IsTest { get; set; }
    }
}
