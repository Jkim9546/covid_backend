using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace covid19_api.Models
{
    [Table("Ontario")]
    public class Ontario
    {
        [Key]
        public Guid ControlID { get; set; }

        public DateTime ReportedDate { get; set; }
        public string PHU_NAME { get; set; }
        public int PHU_NUM { get; set; }
        public int ACTIVE_CASES { get; set; }
        public int RESOLVED_CASES { get; set; }
        public int DEATHS { get; set; }

    }
}