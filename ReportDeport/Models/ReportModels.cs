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
}
