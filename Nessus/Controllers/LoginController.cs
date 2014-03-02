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
    /// Login controller provides the login api
    /// </summary>
    public class LoginController : BaseController
    {

        // GET api/login
        /// <summary>
        /// Login user based on the Basic authentication user:password
        /// </summary>
        /// <returns>TokenModel with status of "OK" and Nessus token if valid, otherwise status of "Error" and empty token</returns>
        public TokenModel Get()
        {
            string token = getBasicAuthorization();
            string validToken = authRepository.GetNessusToken(token);
            TokenModel model = new TokenModel { 
                status = !string.IsNullOrEmpty(validToken) ? "OK" : "Error",
                token = validToken 
            };
            return model;
        }

    }
}
