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
    public class DroneFlightsAPIController : ApiController
    {
        private DroneDBEntities db = new DroneDBEntities();

        //// GET: api/DroneFlightsAPI
        //public IQueryable<DroneFlight> GetDroneFlights()
        //{
        //    return db.DroneFlights;
        //}


        // GET: api/DroneFlightsAPI/5
        [ResponseType(typeof(DroneFlight))]
        public HttpResponseMessage GetDroneFlight(int id)
        {
            var Flight = db.DroneFlights.Find(id);
            if (Flight == null) return new HttpResponseMessage(HttpStatusCode.NotFound);

            //data projection
            var flightProjected = (new { Flight.FlightId, Flight.DroneId, Flight.Location, Flight.Date, Flight.hasTFW, Flight.hasGCPs, Flight.hasCTRLs, Flight.hasDepInfo, Flight.hasDestInfo, Flight.hasQR, Flight.hasXYZ, Flight.hasDroneLog, Flight.PilotId, Flight.TypeOfActivity, Flight.Other, Flight.Simulator, Flight.Instructor, Flight.Remarks });

            //config to set to json 
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(flightProjected));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return response;
        }

        //// PUT: api/DroneFlightsAPI/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutDroneFlight(int id, DroneFlight droneFlight)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != droneFlight.FlightId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(droneFlight).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!DroneFlightExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/DroneFlightsAPI
        //[ResponseType(typeof(DroneFlight))]
        //public IHttpActionResult PostDroneFlight(DroneFlight droneFlight)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.DroneFlights.Add(droneFlight);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = droneFlight.FlightId }, droneFlight);
        //}

        //// DELETE: api/DroneFlightsAPI/5
        //[ResponseType(typeof(DroneFlight))]
        //public IHttpActionResult DeleteDroneFlight(int id)
        //{
        //    DroneFlight droneFlight = db.DroneFlights.Find(id);
        //    if (droneFlight == null)
        //    {
        //        return NotFound();
        //    }

        //    db.DroneFlights.Remove(droneFlight);
        //    db.SaveChanges();

        //    return Ok(droneFlight);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool DroneFlightExists(int id)
        //{
        //    return db.DroneFlights.Count(e => e.FlightId == id) > 0;
        //}
    }
}