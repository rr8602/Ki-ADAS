using System.Text;

namespace Zebra420T
{
    public interface IZebraPrintData
    {
        string GeneratePrintString();
    }

    public struct ADASString : IZebraPrintData
    {
        public string Identification;
        public string Description;
        public string Results;
        public string BankNumber;
        public string Data;
        public string Time;
        public string Duration;

        public string GeneratePrintString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("-- INFO DO TESTE --");
            sb.AppendLine($"Identificaçao     :       {Identification}");
            sb.AppendLine($"Descriçao         :       {Description}");
            sb.AppendLine($"Resultados        :       {Results}");
            sb.AppendLine($"Nº Banco         :       {BankNumber}");
            sb.AppendLine($"Data              :       {Data}");
            sb.AppendLine($"Hora              :       {Time}");
            sb.AppendLine($"Duraçao           :       {Duration}");

            return sb.ToString();
        }
    }

    public struct AlignmentString : IZebraPrintData
    {
        public string ResultParallelism;
        public string ParaDE;
        public string ApertoDE;
        public string ParaDD;
        public string ApertoDD;
        public string ParaTrasTotal;
        public string CamberDE;
        public string CamberDD;
        public string CamberTE;
        public string CamberTD;
        public string AlighmentVolante;
        public string ResultFarois;
        public string LeftFarois;
        public string RightFarois;
        public string ResultPEV;

        public string GeneratePrintString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("-------------------------------------------------------");
            sb.AppendLine($"Paralelismo          :     {ResultParallelism}");
            sb.AppendLine($"Para DE              :     {ParaDE}");
            sb.AppendLine($"Aperto DE            :     {ApertoDE}");
            sb.AppendLine($"Para DD              :     {ParaDD}");
            sb.AppendLine($"Aperto DD            :     {ApertoDD}");
            sb.AppendLine($"Para Tras Total      :     {ParaTrasTotal}");
            sb.AppendLine($"Camber DE            :     {CamberDE}");
            sb.AppendLine($"Camber DD            :     {CamberDD}");
            sb.AppendLine($"Camber TE            :     {CamberTE}");
            sb.AppendLine($"Camber TD            :     {CamberTD}");
            sb.AppendLine($"Alinhamento Volante  :     {AlighmentVolante}");
            sb.AppendLine($"Resultado Faróis     :     {ResultFarois}");
            sb.AppendLine($"Esquerdo Faróis      :     {LeftFarois}");
            sb.AppendLine($"Direito Faróis       :     {RightFarois}");
            sb.AppendLine($"Resultado PEV        :     {ResultPEV}");

            return sb.ToString();
        }
    }

    public struct RollerString : IZebraPrintData
    {
        public string RollResult;
        public string RollValue;
        public string PitchValue;
        public string YawValue;

        public string GeneratePrintString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("-- ROLL & PITCH --");
            sb.AppendLine($"Result               :     {RollResult}");
            sb.AppendLine($"Roll                 :     {RollValue}");
            sb.AppendLine($"Pitch                :     {PitchValue}");
            sb.AppendLine($"Yaw                  :     {YawValue}");

            return sb.ToString();
        }
    }
}
