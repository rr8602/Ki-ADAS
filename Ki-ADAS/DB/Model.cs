namespace Ki_ADAS.DB
{
    public class Model
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
        public double? Wheelbase { get; set; }

        // Front Camera
        public double? FC_Distance { get; set; }
        public double? FC_Height { get; set; }
        public double? FC_InterDistance { get; set; }
        public double? FC_Htu { get; set; }
        public double? FC_Htl { get; set; }
        public double? FC_Ts { get; set; }
        public double? FC_AlignmentAxeOffset { get; set; }
        public double? FC_Vv { get; set; }
        public double? FC_StCt { get; set; }
        public bool FC_IsTest { get; set; }

        // Front Radar
        public double? FR_X { get; set; }
        public double? FR_Y { get; set; }
        public double? FR_Z { get; set; }
        public double? FR_Angle { get; set; }
        public double? FL_X { get; set; }
        public double? FL_Y { get; set; }
        public double? FL_Z { get; set; }
        public double? FL_Angle { get; set; }
        public bool F_IsTest { get; set; }

        // Rear Radar
        public double? RR_X { get; set; }
        public double? RR_Y { get; set; }
        public double? RR_Z { get; set; }
        public double? RR_Angle { get; set; }
        public double? RL_X { get; set; }
        public double? RL_Y { get; set; }
        public double? RL_Z { get; set; }
        public double? RL_Angle { get; set; }
        public bool R_IsTest { get; set; }
    }
}
