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
    
    public partial class quiz_grades
    {
        public int quiz_gradesId { get; set; }
        public int quizId { get; set; }
        public int userId { get; set; }
        public Nullable<decimal> grade { get; set; }
    
        public virtual quiz quiz { get; set; }
        public virtual user user { get; set; }
    }
}