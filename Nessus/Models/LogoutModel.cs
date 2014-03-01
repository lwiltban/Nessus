using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nessus.Models
{
    /// <summary>
    /// Model for logout, status should be "OK" or "ERROR"
    /// </summary>
    public class LogoutModel
    {
        public string status { get; set; }
    }
}