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
    
    public partial class question_answers
    {
        public int question_answersId { get; set; }
        public int questionId { get; set; }
        public string answer { get; set; }
        public Nullable<decimal> fraction { get; set; }
        public string feedback { get; set; }
    
        public virtual question question { get; set; }
    }
}
