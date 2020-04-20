﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;
using DroneWebApp.Models;
using DroneWebApp.Models.PointcloudControlTool;
using Newtonsoft.Json;


// TEST

namespace DroneWebApp.Controllers
{
    public class CTRLPointsController : ApiController
    {
        private DroneDBEntities db = new DroneDBEntities();

        //GET: api/CTRLPoints
        // Hier kan je ook via tags[HttpGet] en[Route("find")] de http type en de route instellen(niet nodig)
        public HttpResponseMessage GetCTRLPoints()
        {
            //nodige data projection 
            var ctrlPoints = db.CTRLPoints.Select(c => new { c.CTRLId, c.CTRLName, c.X, c.Y, c.Z, c.FlightId }).ToList();

            //config to set to json 
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(ctrlPoints));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return response;
        }

        // GET: api/CTRLPoints/5
        //[ResponseType(typeof(CTRLPoint))]   //niet nodig? 
        //[Route("{FlightID}")]
        public HttpResponseMessage GetCTRLPointsByFlightID(int id)    //lampje kwam op als je methode renamet (?) 
        {
            var Flight = db.DroneFlights.Find(id);   //bijhorende vlucht vinden 
            if (Flight == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            //var ctrlPoint = Flight.CTRLPoints.Select(c => new { c.CTRLId, c.CTRLName, c.X, c.Y, c.Z, c.FlightId }).ToList();

            List<PointCloudXYZ> pointCloudXYZs = Flight.PointCloudXYZs.ToList();
            List<CTRLPoint> CTRLPoints = Flight.CTRLPoints.ToList();

            var list = new List<Tuple<int, string, double, double, double, int, string>>().Select(t => new { CTRLId = t.Item1, CTRLName = t.Item2, X = t.Item3, Y = t.Item4, Z = t.Item5, FlightId = t.Item6, Inside = t.Item7 }).ToList();

            if (pointCloudXYZs.Count != 0)
            {
                Polygon polygon = new Polygon(pointCloudXYZs);
                PointcloudControlTool tool = new PointcloudControlTool(polygon);

                foreach (CTRLPoint ctrl in CTRLPoints)
                {
                    bool inside = tool.PointInside3DPolygonSimplified((double)ctrl.X, (double)ctrl.Y, (double)ctrl.Z);
                    string insideString = "false";
                    if (inside)
                    {
                        insideString = "true";
                    }
                    list.Add(new { ctrl.CTRLId, ctrl.CTRLName, X = (double)ctrl.X, Y = (double)ctrl.Y, Z = (double)ctrl.Z, FlightId = (int)ctrl.FlightId, Inside = insideString });
                }
            }
            else
            {
                foreach (CTRLPoint ctrl in CTRLPoints)
                {
                    list.Add(new { ctrl.CTRLId, ctrl.CTRLName, X = (double)ctrl.X, Y = (double)ctrl.Y, Z = (double)ctrl.Z, FlightId = (int)ctrl.FlightId, Inside = "No pointcloud available" });
                }
            }

            //config to set to json 
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(list));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return response;
        }

        //    // PUT: api/CTRLPoints/5
        //    [ResponseType(typeof(void))]
        //    public IHttpActionResult PutCTRLPoint(int id, CTRLPoint cTRLPoint)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        if (id != cTRLPoint.CTRLId)
        //        {
        //            return BadRequest();
        //        }

        //        db.Entry(cTRLPoint).State = EntityState.Modified;

        //        try
        //        {
        //            db.SaveChanges();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CTRLPointExists(id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }

        //        return StatusCode(HttpStatusCode.NoContent);
        //    }

        //    // POST: api/CTRLPoints
        //    [ResponseType(typeof(CTRLPoint))]
        //    public IHttpActionResult PostCTRLPoint(CTRLPoint cTRLPoint)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        db.CTRLPoints.Add(cTRLPoint);
        //        db.SaveChanges();

        //        return CreatedAtRoute("DefaultApi", new { id = cTRLPoint.CTRLId }, cTRLPoint);
        //    }

        //    // DELETE: api/CTRLPoints/5
        //    [ResponseType(typeof(CTRLPoint))]
        //    public IHttpActionResult DeleteCTRLPoint(int id)
        //    {
        //        CTRLPoint cTRLPoint = db.CTRLPoints.Find(id);
        //        if (cTRLPoint == null)
        //        {
        //            return NotFound();
        //        }

        //        db.CTRLPoints.Remove(cTRLPoint);
        //        db.SaveChanges();

        //        return Ok(cTRLPoint);
        //    }

        //    protected override void Dispose(bool disposing)
        //    {
        //        if (disposing)
        //        {
        //            db.Dispose();
        //        }
        //        base.Dispose(disposing);
        //    }

        //    private bool CTRLPointExists(int id)
        //    {
        //        return db.CTRLPoints.Count(e => e.CTRLId == id) > 0;
        //    }

    }
}