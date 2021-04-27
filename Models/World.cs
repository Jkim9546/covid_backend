using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace covid19_api.Models
{
    [Table("World")]
    public class World
    {
        [Key]
        public Guid ControlID { get; set; }
        public string Province_State { get; set; }
        public string Country_Region { get; set; }
        public DateTime ReportedDate { get; set; }
        public string DataType { get; set; }
        public int DataValue { get; set; }
    }
}