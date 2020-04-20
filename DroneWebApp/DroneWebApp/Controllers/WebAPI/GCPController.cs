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
    public class GCPController : ApiController
    {
        private DroneDBEntities db = new DroneDBEntities();

        //// GET: api/GCP
        //public IQueryable<GroundControlPoint> GetGroundControlPoints()
        //{
        //    return db.GroundControlPoints;
        //}

        // GET: api/GCP/5
        //[ResponseType(typeof(GroundControlPoint))]
        public HttpResponseMessage GetGroundControlPointsByFlightID(int id)
        {
            var Flight = db.DroneFlights.Find(id);   //bijhorende vlucht vinden 
            if (Flight == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            var GroundControlPoints = Flight.GroundControlPoints.Select(gcp => new { gcp.GCPId, gcp.GCPName, gcp.X, gcp.Y, gcp.Z, gcp.FlightId }).ToList();

            //config to set to json 
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(GroundControlPoints));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return response;
        }

        //// PUT: api/GCP/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutGroundControlPoint(int id, GroundControlPoint groundControlPoint)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != groundControlPoint.GCPId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(groundControlPoint).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!GroundControlPointExists(id))
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

        //// POST: api/GCP
        //[ResponseType(typeof(GroundControlPoint))]
        //public IHttpActionResult PostGroundControlPoint(GroundControlPoint groundControlPoint)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.GroundControlPoints.Add(groundControlPoint);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = groundControlPoint.GCPId }, groundControlPoint);
        //}

        //// DELETE: api/GCP/5
        //[ResponseType(typeof(GroundControlPoint))]
        //public IHttpActionResult DeleteGroundControlPoint(int id)
        //{
        //    GroundControlPoint groundControlPoint = db.GroundControlPoints.Find(id);
        //    if (groundControlPoint == null)
        //    {
        //        return NotFound();
        //    }

        //    db.GroundControlPoints.Remove(groundControlPoint);
        //    db.SaveChanges();

        //    return Ok(groundControlPoint);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool GroundControlPointExists(int id)
        //{
        //    return db.GroundControlPoints.Count(e => e.GCPId == id) > 0;
        //}
    }
}