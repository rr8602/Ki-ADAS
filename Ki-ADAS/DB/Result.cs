using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_ADAS.DB
{
    public class Result
    {
        public string AcceptNo { get; set; }
        public string PJI { get; set; }
        public string Model { get; set; }
        public bool FC_IsOk { get; set; }
        public bool FR_IsOk { get; set; }
        public bool RR_IsOk { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
