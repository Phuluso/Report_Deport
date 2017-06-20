using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReportDeport.Models
{
   public class Template
    {
        [Key]
        public int Id { get; set; }

        public string name { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        public List<string> topFields { get; set; }
    }

    public class ContactForm
    {

        [Key]
        public int ContactFormId { get; set; }

        public string name { get; set; }
        //email used for who to contact regarding complaint
        public string emailAddress { get; set; }
        public DateTime date { get; set; }
        public string position { get; set; }
        public string department { get; set; }
        public string message { get; set; }
        public string subject { get; set; }
        //email to track id of signed in user
        public string conpanyEmail { get; set; }

        //may need to add a foreign key tag
    }


}
