﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportDeport.Models
{
    public class CourseViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
       

        public int courseId { get; set; }
        public string categoryId { get; set; }
        public string fullname { get; set; }
        public string shortname { get; set; }
        public string idnumber { get; set; }
        public System.DateTime startdate { get; set; }
        public bool visible { get; set; }
        public System.DateTime timecreated { get; set; }
        public System.DateTime timemodified { get; set; }
        public bool enablecompletion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<course_completions> course_completions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<enrol> enrols { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<quiz> quizs { get; set; }
    }
}