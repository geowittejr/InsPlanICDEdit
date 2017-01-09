using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsPlanIcdEditApi.Models
{
    public class UserObject
    {
        public UserObject()
        {
        }
        public int id { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string username { get; set; }
        public bool isAuthorized { get; set; }
    }
}