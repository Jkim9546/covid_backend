using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace covid19_api.Models
{
    [Table("Canada")]
    public class Canada
    {
        [Key]
        public System.Guid ControlID { get; set; }
        public int PrUid { get; set; }
        public string PrName { get; set; }
        public System.DateTime Date { get; set; }
        public int NumConfirmed { get; set; }
        public int NumProbable { get; set; }
        public int NumDeaths { get; set; }
        public int NumTotal { get; set; }
        public int NumToday { get; set; }
        public int Activated { get; set; }
        public Nullable<int> NumTested { get; set; }
        public Nullable<int> NumRecover { get; set; }
    }
}