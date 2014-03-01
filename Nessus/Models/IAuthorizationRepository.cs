using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nessus.Models
{
    /// <summary>
    /// Interface to provide a simple authorization scheme
    /// </summary>
    public interface IAuthorizationRepository
    {
        /// <summary>
        /// Get a Nessus token created from the Basic authorization token
        /// </summary>
        /// <param name="basicToken">token from the Basic authorization</param>
        /// <returns>Nessus token if valid basic token, null otherwise</returns>
        string GetNessusToken(string basicToken);

        /// <summary>
        /// Check if passed in token is a valid Nessus token
        /// </summary>
        /// <param name="token">Passed in Nessus token</param>
        /// <returns>True if valid Nessus token, false otherwise</returns>
        bool IsValidNessusToken(string token);

        /// <summary>
        /// Invalidates the passed in token
        /// </summary>
        /// <param name="token">Passed in Nessus token</param>
        /// <returns>True if removed, false otherwise</returns>
        bool ReleaseNessusToken(string token);
    }
}
