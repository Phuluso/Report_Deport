using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportDeport.Models
{
    public class CategoryViewModel
    {
        public int categoryId { get; set; }
        public string name { get; set; }
        public string idnumber { get; set; }
        public string description { get; set; }
        public int coursecount { get; set; }
        public bool visible { get; set; }
        public System.DateTime timemodified { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<question> questions { get; set; }
    }
}