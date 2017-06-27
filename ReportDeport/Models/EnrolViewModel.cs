using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportDeport.Models
{
    public class EnrolViewModel
    {
        public int enrolId { get; set; }
        public string enrol1 { get; set; }
        public int courseId { get; set; }
        public System.DateTime enrolstartdate { get; set; }
        public Nullable<System.DateTime> enrolenddate { get; set; }

        public virtual course course { get; set; }
    }
}