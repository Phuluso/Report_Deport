//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReportDeport.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class template
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public template()
        {
            this.templateColumns = new HashSet<templateColumn>();
        }
    
        public int templateId { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public string name { get; set; }
        public string userId { get; set; }
    
        public virtual template template1 { get; set; }
        public virtual template template2 { get; set; }
        public virtual template template11 { get; set; }
        public virtual template template3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<templateColumn> templateColumns { get; set; }
        public bool isHistory { get; internal set; }
    }
}
