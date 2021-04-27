using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace covid19_api.Models
{
    [Table("Vaccine")]
    public class Vaccine
    {
        [Key]
        public Guid ControlID { get; set; }
        public DateTime Week_End { get; set; }
        public int PrUid { get; set; }
        public string PrName { get; set; }
        public int numtotal_atleast1dose { get; set; }
        public int numtotal_1dose { get; set; }
        public int numtotal_2doses { get; set; }
        public decimal proptotal_atleast1dose { get; set; }
        public decimal proptotal_1dose { get; set; }
        public decimal proptotal_2doses { get; set; }
    }
}

