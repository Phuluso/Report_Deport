//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReportDeport
{
    using System;
    using System.Collections.Generic;
    
    public partial class contactForm
    {
        public int contactFormId { get; set; }
        public string name { get; set; }
        public string emailAddress { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public string position { get; set; }
        public string department { get; set; }
        public string message { get; set; }
        public string subject { get; set; }
        public string aspUserId { get; set; }
    
        public virtual contactForm contactForm1 { get; set; }
        public virtual contactForm contactForm2 { get; set; }
    }
}
