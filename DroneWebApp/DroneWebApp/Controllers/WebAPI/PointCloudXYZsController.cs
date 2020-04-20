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
    public class PointCloudXYZsController : ApiController
    {
        private DroneDBEntities db = new DroneDBEntities();

        // GET: api/PointCloudXYZs
        //get all, unimplemented
        //public HttpResponseMessage GetPointCloudXYZs()
        //{
        //    //unimplemented
        //    return response;
        //}

        // GET: api/PointCloudXYZs/5
        //get pointcloud by flight id 
        //[ResponseType(typeof(PointCloudXYZ))]
        public HttpResponseMessage GetPointCloudXYZByFlightID(int id)
        {
            var Flight = db.DroneFlights.Find(id);
            if (Flight == null) return new HttpResponseMessage(HttpStatusCode.NotFound);

            //data projection
            var PointCloudXYZ = Flight.PointCloudXYZs.Select(p => new { p.PointCloudXYZId,  p.X, p.Y, p.Z, p.Red, p.Green, p.Blue, p.Intensity, p.FlightId}).ToList();

            //config to set to json 
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(PointCloudXYZ));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return response;
        }

        //// PUT: api/PointCloudXYZs/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutPointCloudXYZ(int id, PointCloudXYZ pointCloudXYZ)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != pointCloudXYZ.PointCloudXYZId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(pointCloudXYZ).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PointCloudXYZExists(id))
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

        //// POST: api/PointCloudXYZs
        //[ResponseType(typeof(PointCloudXYZ))]
        //public IHttpActionResult PostPointCloudXYZ(PointCloudXYZ pointCloudXYZ)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.PointCloudXYZs.Add(pointCloudXYZ);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = pointCloudXYZ.PointCloudXYZId }, pointCloudXYZ);
        //}

        //// DELETE: api/PointCloudXYZs/5
        //[ResponseType(typeof(PointCloudXYZ))]
        //public IHttpActionResult DeletePointCloudXYZ(int id)
        //{
        //    PointCloudXYZ pointCloudXYZ = db.PointCloudXYZs.Find(id);
        //    if (pointCloudXYZ == null)
        //    {
        //        return NotFound();
        //    }

        //    db.PointCloudXYZs.Remove(pointCloudXYZ);
        //    db.SaveChanges();

        //    return Ok(pointCloudXYZ);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool PointCloudXYZExists(int id)
        //{
        //    return db.PointCloudXYZs.Count(e => e.PointCloudXYZId == id) > 0;
        //}
    }
}