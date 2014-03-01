using Nessus.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nessus.Models
{
    /// <summary>
    /// Implementation of the IAuthorizationRepository interface, provides an in-memory implementation for login/logout
    /// </summary>
    public class AuthorizationRepository : IAuthorizationRepository
    {
        /// <summary>
        /// Thread-safe dictionary of all valid Nessus tokens, stored in-memory
        /// </summary>
        static readonly ConcurrentDictionary<string, string> validNessusTokens = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Get a Nessus token created from the Basic authorization token
        /// </summary>
        /// <param name="basicToken">token from the Basic authorization</param>
        /// <returns>Nessus token if valid basic token, null otherwise</returns>
        public string GetNessusToken(string basicToken)
        {
            if (string.IsNullOrEmpty(basicToken))
            {
                return null;
            }

            // Ensure Basic authorization has username:password
            string username = basicToken.Substring(0, basicToken.IndexOf(":"));
            string password = basicToken.Substring(basicToken.IndexOf(":") + 1);

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            // Create a Nessus token from the basic token and a timestamp in case we want to timeout user authorizations
            string plainText = string.Format("{0}:{1}", basicToken, DateTime.Now.ToFileTime());
            string token = SymmetricEncryption.Encrypt(plainText);

            if (!validNessusTokens.TryAdd(token, token))
            {
                return null;    
            }

            return token;
        }

        /// <summary>
        /// Check if passed in token is a valid Nessus token
        /// </summary>
        /// <param name="token">Passed in Nessus token</param>
        /// <returns>True if valid Nessus token, false otherwise</returns>
        public bool IsValidNessusToken(string token)
        {
            string validToken;
            return validNessusTokens.TryGetValue(token, out validToken);
        }

        /// <summary>
        /// Invalidates the passed in token
        /// </summary>
        /// <param name="token">Passed in Nessus token</param>
        /// <returns>True if removed, false otherwise</returns>            
        public bool ReleaseNessusToken(string token)
        {
            string validToken;
            return validNessusTokens.TryRemove(token, out validToken);
        }
    }
}