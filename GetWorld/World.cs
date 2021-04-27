using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetWorld
{
    public class World
    {

        public Guid ControlID { get; set; }
        public string Province_State{ get; set; }
        public string Country_Region { get; set; }
        public DateTime ReportedDate { get; set; }
        public string DataType { get; set; }
        public int DataValue { get; set; }
    }
}
