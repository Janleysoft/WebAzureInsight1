using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAuthorUser1.Models
{
    public class InsightEventData
    {
        [Key]
        public int Id { get; set; }
        public DateTime MyTimeStamp {   get; set;}
        public string url { get; set; }
        public string appName { get; set; }
        public int processingDuration { get; set; }
    }
}