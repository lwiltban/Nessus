using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nessus.Models
{
    /// <summary>
    /// Interface to provide Configuration operations
    /// </summary>
    interface IConfigurationRepository
    {
        /// <summary>
        /// Retrieve all the configurations based on max number to return and offset to begin from
        /// </summary>
        /// <returns>Configurations object with an array of all Configuration objects in memory if valid max and offset, otherwise null</returns>
        Configurations GetAll(int max, int offset);

        /// <summary>
        /// Find a Configuration based on a name
        /// </summary>
        /// <param name="name">name to search for</param>
        /// <returns>Returns the found Configuration, null otherwise</returns>
        Configuration Get(string name);

        /// <summary>
        /// Sort Configurations based on the sort oder
        /// </summary>
        /// <param name="sort">Comma delimited list of sort parameters (name, hostname, port and/or username)</param>
        /// <returns>All the configurations based on the paging query, or 404 if invalid sort query</returns>
        Configurations GetBySort(string sort);

        /// <summary>
        /// Add a Configuration based on the name of the incoming configuration
        /// </summary>
        /// <param name="value">Configuration to add</param>
        /// <returns>Configuration if added, null otherwise</returns>
        Configuration Add(Configuration value);

        /// <summary>
        /// Update a Configuration based on the name of the incoming configuration
        /// </summary>
        /// <param name="value">Configuration to update</param>
        /// <returns>Configuration if found and updated, null otherwise</returns>
        Configuration Update(Configuration value);

        /// <summary>
        /// Remove a Configuration based on name
        /// </summary>
        /// <param name="name">name to remove</param>
        /// <returns>true if found and removed, false otherwise</returns>
        bool Remove(string name);
    }
}
