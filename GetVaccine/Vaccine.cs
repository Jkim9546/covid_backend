using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetVaccine
{
    public class Vaccine
    {
        public Guid ControlID { get; set; }
        public DateTime Week_End { get; set; }
        public int PrUid { get; set; }

        public string PrName { get; set; }
        public int numtotal_atleast1dose { get; set; }
        public int numtotal_1dose { get; set; }
        public int numtotal_2doses { get; set; }
        public double proptotal_atleast1dose { get; set; }
        public double proptotal_1dose { get; set; }
        public double proptotal_2doses { get; set; }

        public Vaccine()
        {
            numtotal_atleast1dose = 0;
            numtotal_1dose = 0;
            numtotal_2doses = 0;
            proptotal_atleast1dose = 0.0;
            proptotal_1dose = 0.0;
            proptotal_2doses = 0.0;

        }
    }
}