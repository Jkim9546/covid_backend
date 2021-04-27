using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCanada
{
    public class Canada
    {
        public Guid ControlID { get; set; }
        public int PrUid { get; set; }
        public string PrName { get; set; }
        public DateTime Date { get; set; }
        public int NumConfirmed { get; set; }
        public int NumProbable { get; set; }
        public int NumDeaths { get; set; }
        public int NumTotal { get; set; }
        public int NumToday { get; set; }
        public int Activated { get; set; }
        public Nullable<int> NumTested { get; set; }
        public Nullable<int> NumRecover { get; set; }

        public Canada(){
            NumTested = 0;
            NumRecover = 0;
        }
    }


}
