using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReportDeport.Models
{
   public class TemplateViewModel
    {
        [Key]
        public int Id { get; set; }

        public string name { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        public List<string> topFields { get; set; }
    }


    public class ContactInfo
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public string department { get; set; }
        public string email { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
    }

}
