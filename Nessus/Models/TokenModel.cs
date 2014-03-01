using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nessus.Models
{
    /// <summary>
    /// Model for the login response, status can be "OK" or "ERROR"
    /// </summary>
    public class TokenModel
    {
        public string status { get; set;}
        public string token { get; set; }
    }
}