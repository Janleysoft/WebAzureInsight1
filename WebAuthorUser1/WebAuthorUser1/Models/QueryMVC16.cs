using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAuthorUser1.Models
{
    public class QueryMVC16
    {
        [Key]
        public System.DateTime timestamp { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string success { get; set; }
        public Nullable<int> resultCode { get; set; }
        public string operation_Name { get; set; }
        public string cloud_RoleInstance { get; set; }
        public string appName { get; set; }
    }
}