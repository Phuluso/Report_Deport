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
    
    public partial class TestPerson
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TestPerson()
        {
            this.TestSalePersons = new HashSet<TestSalePerson>();
        }
    
        public int TestPersonId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TestSalePerson> TestSalePersons { get; set; }
    }
}
