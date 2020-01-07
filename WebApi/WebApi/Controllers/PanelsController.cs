using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using WebApi.Models;
using System.Web.Http.Cors;

namespace WebApi.Controllers
{
    [EnableCors(origins: "http://localhost:58088", headers: "*", methods: "*", SupportsCredentials = true)]
    [RoutePrefix("api/panels")]
    public class PanelsController : ApiController
    {

        /// <summary>
        /// Returns an array containing the names of all stored panels.
        /// GET: api/panels
        /// </summary>
        /// <returns>An array containing the names of all stored panels.</returns>
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetPanels()
        {
            List<String> panels = new List<String>();
            foreach(string key in HttpContext.Current.Session.Keys)
            {
                System.Diagnostics.Debug.WriteLine("Panel: " + key);
                panels.Add(key);
            }
            return Json(panels.ToArray());
        }

        /// <summary>
        /// Returns the panel with the specified name, or HTTP response "404 Not Found" if no such panel exists.
        /// GET: api/panels/name
        /// </summary>
        /// <param name="name">The name of the panel to return.</param>
        /// <returns>The panel with the specified name, or HTTP response "404 Not Found" if no such panel exists.</returns>
        [Route("{name}")]
        [HttpGet]
        public IHttpActionResult GetPanel(string name)
        {
            // Return the panel with the corresponding name if it exist.
            if (HttpContext.Current.Session[name] != null)
            {
                System.Diagnostics.Debug.WriteLine("Returned panel: " + name);
                return Json(HttpContext.Current.Session[name]);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No panel named: " + name);
                return NotFound();
            }
        }

        /// <summary>
        /// Stores a panel in the current session, provided that no panel with the same name already exists.
        /// POST: api/panels
        /// </summary>
        /// <param name="value"></param>
        /// <returns>
        /// HTTP response "200 OK" if the panel is stored successfully.
        /// HTTP response "409 Conflict" if a panel with the same name already exists.
        /// HTTP response "400 Bad Request" if the received data does not conform to the Panel model.
        /// </returns>
        [Route("")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]Panel value)
        {
            if(ModelState.IsValid)
            {
                if (HttpContext.Current.Session[value.Name] == null)
                {
                    HttpContext.Current.Session[value.Name] = value;
                    System.Diagnostics.Debug.WriteLine("Created: " + value.Name);
                    return Ok(value.Name);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Panel already exists: " + value.Name);
                    return Conflict();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Replaces an already existing panel.
        /// PUT: api/panels/name
        /// </summary>
        /// <param name="name">The name of the panel.</param>
        /// <param name="value">The panel.</param>
        /// <returns>
        /// HTTP response "200 OK" if the panel is successfully replaced.
        /// HTTP response "404 Not Found" if no panel with the specified name exists.
        /// </returns>
        [Route("{name}")]
        [HttpPut]
        public IHttpActionResult Put(string name, [FromBody]Panel value)
        {
            if(HttpContext.Current.Session[name] != null)
            {
                HttpContext.Current.Session[value.Name] = value;
                System.Diagnostics.Debug.WriteLine("Replaced: " + value.Name);
                return Ok(value.Name);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Deletes a panel from the current session.
        /// DELETE: api/panels/name
        /// </summary>
        /// <param name="name">The name of the panel to delete.</param>
        /// <returns>
        /// HTTP response "200 OK" if the panel is successfully deleted.
        /// HTTP response "404 Not Found" if no panel with the specified name exists.
        /// </returns>
        [Route("{name}")]
        [HttpDelete]
        public IHttpActionResult Delete(string name)
        {
            if(HttpContext.Current.Session[name] == null)
            {
                return NotFound();
            } else
            {
                HttpContext.Current.Session.Remove(name);
                System.Diagnostics.Debug.WriteLine("Deleted: " + name);
                return Ok();
            }
        }




    }
}
