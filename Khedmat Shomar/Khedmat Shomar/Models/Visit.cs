using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Khedmat_Shomar
{
    public class Visit
    {
        [Key]
        public int id { get; set; }
        public string IP { get; set; }
        public string Browser { get; set; }
        public string OS { get; set; }
        public DateTime VisitDate { get; set; } =  DateTime.Now;
    }
}