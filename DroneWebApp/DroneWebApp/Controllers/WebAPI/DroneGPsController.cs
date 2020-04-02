using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;
using DroneWebApp.Models;
using Newtonsoft.Json;

namespace DroneWebApp.Controllers
{
    public class DroneGPsController : ApiController
    {
        private DroneDBEntities db = new DroneDBEntities();

        /*
        // GET: api/DroneGPs
        public IQueryable<DroneGP> GetDroneGPS()
        {
            return db.DroneGPS;
        }
        */

        // GET: api/DroneGPs/5
        [ResponseType(typeof(DroneGP))]
        public HttpResponseMessage GetDroneGP(int id)
        {
            var Flight = db.DroneFlights.Find(id);
            if (Flight == null) return new HttpResponseMessage(HttpStatusCode.NotFound);

            var droneLogEntries = Flight.DroneLogEntries.ToList();
            List<DroneGP> droneGPs = new List<DroneGP>();

            foreach (DroneLogEntry log in droneLogEntries)
            {
                droneGPs.Add(log.DroneGP);
            }

            //data projection
            var GPs = droneGPs.Select(gp => new { gp.GPSId, gp.Long, gp.Lat, gp.Date, gp.Time, gp.DateTimeStamp, gp.HeightMSL, gp.HDOP, gp.PDOP, gp.SAcc, gp.NumGPS, gp.NumGLNAS, gp.NumSV, gp.VelN, gp.VelE, gp.VelD}).ToList();

            //config to set to json 
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(GPs));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return response;
        }

        /*
        // PUT: api/DroneGPs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDroneGP(int id, DroneGP droneGP)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != droneGP.GPSId)
            {
                return BadRequest();
            }

            db.Entry(droneGP).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DroneGPExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/DroneGPs
        [ResponseType(typeof(DroneGP))]
        public IHttpActionResult PostDroneGP(DroneGP droneGP)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DroneGPS.Add(droneGP);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (DroneGPExists(droneGP.GPSId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = droneGP.GPSId }, droneGP);
        }

        // DELETE: api/DroneGPs/5
        [ResponseType(typeof(DroneGP))]
        public IHttpActionResult DeleteDroneGP(int id)
        {
            DroneGP droneGP = db.DroneGPS.Find(id);
            if (droneGP == null)
            {
                return NotFound();
            }

            db.DroneGPS.Remove(droneGP);
            db.SaveChanges();

            return Ok(droneGP);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DroneGPExists(int id)
        {
            return db.DroneGPS.Count(e => e.GPSId == id) > 0;
        }
        */
    }
}