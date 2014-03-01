using Nessus.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Nessus.Controllers
{
    /// <summary>
    /// Abstract controller that provides initial authorization through Basic auth and then through Nessus auth
    /// </summary>
    public abstract class BaseController : ApiController
    {
        /// <summary>
        /// Authorization repository used to create, store, and release authorization tokens
        /// </summary>
        protected static readonly IAuthorizationRepository authRepository = new AuthorizationRepository();

        /// <summary>
        /// For a Basic authorization attempt will return a valid Nessus token, otherwise null
        /// </summary>
        /// <returns>Nessus authorization token if valid, otherwise null</returns>
        protected string getBasicAuthorization()
        {
            if (Request.Headers.Authorization == null)
            {
                return null;
            }

            string authToken = Request.Headers.Authorization.Parameter;
            
            if (string.IsNullOrEmpty(authToken))
            {
                return null;
            }

            if (Request.Headers.Authorization.Scheme.ToLower() != "basic")
            {
                return null;
            }

            string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));

            return decodedToken;
        }

        /// <summary>
        /// Gets the Nessus authorization token that should be passed with each api request after login
        /// </summary>
        /// <returns>Nessus authorization token if valid, otherwise null</returns>
        protected string getNessusAuthorization()
        {
            if (Request.Headers.Authorization == null)
            {
                return null;
            }

            string authToken = Request.Headers.Authorization.Parameter;

            if (string.IsNullOrEmpty(authToken))
            {
                return null;

            }

            if (Request.Headers.Authorization.Scheme.ToLower() != "nessus")
            {
                authToken = null;
            }

            return authToken;
        }

        /// <summary>
        /// Authorization function which will check for a valid Nessus token, throws 403 exception if not valid
        /// </summary>
        protected void OnAuthorization()
        {
            // The authorization process can be short-circuited for testing by web.config key DoAuthorization
            string DoAuthorization = ConfigurationManager.AppSettings["DoAuthorization"];
            if (!string.IsNullOrEmpty(DoAuthorization) && DoAuthorization == "false")
            {
                return;
            }

            string token = getNessusAuthorization();
            if (!authRepository.IsValidNessusToken(token))
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
        }
    }
}
