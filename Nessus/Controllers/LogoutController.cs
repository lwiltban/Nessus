using Nessus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Nessus.Controllers
{
    /// <summary>
    /// Logout controller that provides the logout api
    /// </summary>
    public class LogoutController : BaseController
    {

        // GET api/logout
        /// <summary>
        /// Logout the user based on the Nessus token
        /// Throws 403 if not authorized
        /// User must be authorized
        /// </summary>
        /// <returns>{"status","OK" or "ERROR"}</returns>
        public LogoutModel Get()
        {
            OnAuthorization();
            string status = "OK";
            if (!authRepository.ReleaseNessusToken(getNessusAuthorization()))
            {
                status = "ERROR";
            }
            return new LogoutModel{
                status = status
            };
        }

    }
}
