using Nessus.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Nessus.Controllers
{
    /// <summary>
    /// Helper class to handle the optional paging parameters, max and offset
    /// </summary>
    public class PagingQuery
    {
        // Default the max to 100,000 if the user doesn't pass it in
        private int _max = 100000;
        public int max
        {
            get
            {
                return _max;
            }
            set
            {
                _max = value;
            }
        }
        public int offset { get; set; }
    }

    /// <summary>
    /// Controller to handle all the configuration operations
    /// </summary>
    public class ConfigurationsController : BaseController
    {
        /// <summary>
        /// Configuration repository to provide a configuration interface for the controller
        /// </summary>
        static readonly IConfigurationRepository repository = new ConfigurationRepository();


        // GET api/configuration
        /// <summary>
        /// Gets all the Configurations with an optional paging query
        /// User must be authorized
        /// </summary>
        /// <param name="query">Optional paging query based on ?max=num&offset=num</param>
        /// <returns>All the configurations based on the paging query, or 404 if invalid paging query</returns>
        [HttpGet]
        public Configurations Get([FromUri] PagingQuery query)
        {
            OnAuthorization();
            var configs = repository.GetAll(query.max,query.offset);
            if (configs != null)
            {
                return configs;
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }
       

        // GET api/configuration/host
        /// <summary>
        /// Get a configuration based on host name
        /// User must be authorized
        /// </summary>
        /// <param name="id">host for the Configuration</param>
        /// <returns>Configuration if found, or 404 if not found</returns>
        [HttpGet]
        public Configuration GetConfiguration(string id)
        {
            OnAuthorization();
            var config = repository.Get(id);
            if (config != null)
            {
                return config;
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }


        // Get api/configuration?sort=sortOrder
        /// <summary>
        /// Gets all the Configurations with an optional paging query
        /// User must be authorized
        /// </summary>
        /// <param name="sort">Comma delimited list of sort parameters (name, hostname, port and/or username)</param>
        /// <returns>All the configurations based on the paging query, or 404 if invalid sort query</returns>
        [HttpGet]
        public Configurations GetBySort(string sort)
        {
            OnAuthorization();
            var configs = repository.GetBySort(sort);
            if (configs != null)
            {
                return configs;
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }


        // POST api/configuration
        /// <summary>
        /// Adds a new Configuration from the Configuration JSON passed in the post body
        /// User must be authorized
        /// </summary>
        /// <param name="configuration">Configuration from the JSON in the post body</param>
        /// <returns>statuscode 201 with uri for the new configuration if added, or 404 if unable to add configuration</returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody]Configuration configuration)
        {
            OnAuthorization();
            var value = repository.Add(configuration);

            if (value == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            // Return 201 status code with the new Configuration's location in the Location header
            var response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri(Request.RequestUri, string.Format("configurations/{0}", value.name));        
            return response;
        }


        // PUT api/configuration/host
        /// <summary>
        /// Updates the specified Configuration 
        /// Throws a 404 exception if unable to update Configuration
        /// User must be authorized
        /// </summary>
        /// <param name="configuration">Configuration passed in the body</param>
        [HttpPut]
        public void Put([FromBody]Configuration configuration)
        {
            OnAuthorization();
            var update = repository.Update(configuration);

            if (update == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }


        // DELETE api/configuration/host
        /// <summary>
        /// Deletes the configuration specified by the id (host)
        /// Throws a 404 exception if Configuration not found or any other problem
        /// User must be authorized
        /// </summary>
        /// <param name="id">name of the Configuration to delete</param>
        [HttpDelete]
        public void Delete(string id)
        {
            OnAuthorization();
            if (!repository.Remove(id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}
