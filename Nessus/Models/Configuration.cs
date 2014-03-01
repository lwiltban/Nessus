using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nessus.Models
{
    /// <summary>
    /// Configuration class to store the configuration of a nessus object
    /// </summary>
    public class Configuration
    {
        public string name { get; set; }
        public string hostname { get; set; }
        public int port { get; set; }
        public string username { get; set; }

        /// <summary>
        /// Copy the value of the incoming Configuration into this Configuration
        /// </summary>
        /// <param name="val">Configuration to copy from</param>
        public void Copy(Configuration val)
        {
            this.name = val.name;
            this.hostname = val.hostname;
            this.port = val.port;
            this.username = val.username;
        }
    }
}