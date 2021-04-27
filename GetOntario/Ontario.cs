using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetOntario
{
    public class Ontario
    {
        public Guid ControlID { get; set; }
        public DateTime ReportedDate { get; set; }
        public string PHU_NAME { get; set; }

        public int PHU_NUM { get; set; }
        public int ACTIVE_CASES { get; set; }
        public int RESOLVED_CASES { get; set; }
        public int DEATHS { get; set; }

    }
}
