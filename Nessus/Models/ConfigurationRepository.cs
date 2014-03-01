using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nessus.Models
{
    /// <summary>
    /// Implementation of the IConfigurationRepository, provides an in-memory implementation for Configuration operations
    /// </summary>
    public class ConfigurationRepository : IConfigurationRepository
    {
        
        /// <summary>
        /// List of valid sort filters currently available
        /// </summary>
        private static List<string> sortFilter = new List<string>{
            "name",
            "hostname",
            "port",
            "username"
        };


        /// <summary>
        /// List of Configuration used to store them in memory
        /// </summary>
        private static List<Configuration> configs = new List<Configuration>();


        /// <summary>
        /// Initialize the configs list to some default parameters for now
        /// </summary>
        void DefaultInit()
        {
            configs.Add(new Configuration
            {
                name = "host1",
                hostname = "nessus-ntp.lab.com",
                port = 1241,
                username = "toto"
            });

            configs.Add(new Configuration
            {
                name = "host2",
                hostname = "nessux-xml.lab.com",
                port = 3384,
                username = "admin"
            });
        }

        
        /// <summary>
        /// Default constructor, calls DefaultInit to get us to a known state
        /// </summary>
        public ConfigurationRepository()
        {
            DefaultInit();
        }


        /// <summary>
        /// Retrieve all the configurations based on max number to return and offset to begin from
        /// </summary>
        /// <returns>Configurations object with an array of all Configuration objects in memory if valid max and offset, otherwise null</returns>
        public Configurations GetAll(int max, int offset)
        {
            // Validate the max and offset based on the configurations we have
            int configurationsCount = configs.Count();

            // If offset is too large, this is fatal and we return null;
            if (offset > configurationsCount)
            {
                return null;
            }

            // If max is too big, it is not fatal, we just set it to the max number of Configurations we have
            if (max  + offset > configurationsCount)
            {
                max = configurationsCount - offset;
            }

            // Get all the valid configs by skipping the offset and then taking the max and select them
            var validConfigs = (from c in configs.Skip(offset).Take(max) select c);

            return new Configurations
            {
                configurations = validConfigs
            };
        }

        
        /// <summary>
        /// Find a Configuration based on a name
        /// </summary>
        /// <param name="name">name to search for</param>
        /// <returns>Returns the found Configuration, null otherwise</returns>
        public Configuration Get(string name)
        {
            var config = configs.Where(c => c.name == name).FirstOrDefault();
            return config;
        }

        
        /// <summary>
        /// Sort Configurations based on the sort oder
        /// </summary>
        /// <param name="sort">Comma delimited list of sort parameters (name, hostname, port and/or username)</param>
        /// <returns>All the configurations based on the paging query, or 404 if invalid sort query</returns>
        public Configurations GetBySort(string sort)
        {
            // Validate the sort order
            if (string.IsNullOrEmpty(sort))
            {
                return null;
            }

            string[] sortParameters = sort.Split(',');

            // Validate that each sort parameter is in the sort filter list
            foreach (string filter in sortParameters)
            {
                var found = sortFilter.Where(s => s == filter).FirstOrDefault();
                if (found == null)
                {
                    return null;
                }                
            }

            // Now do the sort based on the sort parameters, this is some evil Linq 
            IEnumerable<Configuration> query = from c in configs select c;
            IOrderedEnumerable<Configuration> orderedQuery = null;
            for (int i = 0; i < sortParameters.Count(); i++)
            {
                int copyOfI = i;
                // Tailor "object" depending on what GetProperty returns.
                Func<Configuration, object> expression = item =>
                        item.GetType()
                            .GetProperty(sortParameters[copyOfI])
                            .GetValue(item, null);

                    orderedQuery = (i == 0) ? query.OrderBy(expression)
                                            : orderedQuery.ThenBy(expression);
            }

            var sortedConfigs = orderedQuery.ToList();

            return new Configurations
            {
                configurations = sortedConfigs
            };
        }

        
        /// <summary>
        /// Add a configuration based on the name of the incoming configuration
        /// </summary>
        /// <param name="value">Configuration to add</param>
        /// <returns>Configuration if added, null otherwise</returns>
        public Configuration Add(Configuration value)
        {
            // find if we already have the configuration
            var config = configs.Where(c => c.name == value.name).FirstOrDefault();
            if (config == null)
            {
                configs.Add(value);
                return value;
            }

            return null;
        }

        
        /// <summary>
        /// Update a Configuration based on the name of the incoming configuration
        /// </summary>
        /// <param name="value">Configuration to update</param>
        /// <returns>Configuration if found and updated, null otherwise</returns>
        public Configuration Update(Configuration value)
        {
            // find if we have the configuration
            var config = configs.Where(c => c.name == value.name).FirstOrDefault();
            if (config != null)
            {
                config.Copy(value);
                return value;
            }

            return null;
        }

        
        /// <summary>
        /// Remove a Configuration based on name
        /// </summary>
        /// <param name="name">name to remove</param>
        /// <returns>true if found and removed, false otherwise</returns>
        public bool Remove(string name)
        {
            // find if we already have the configuration
            var config = configs.Where(c => c.name == name).FirstOrDefault();
            if (config != null)
            {
                configs.Remove(config);
                return true;
            }

            return false;
        }
    }
}