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
using DroneWebApp.Models.PointcloudControlTool;
using Newtonsoft.Json;

namespace DroneWebApp.Controllers.WebAPI
{
    public class PointCloudControlToolController : ApiController
    {
        private DroneDBEntities db = new DroneDBEntities();

        /*
        // GET: api/PointCloudControlTool
        public IQueryable<PointCloudXYZ> GetPointCloudXYZs()
        {
            return db.PointCloudXYZs;
        }
        */

        // GET: api/PointCloudControlTool/5
        //[ResponseType(typeof(PointCloudXYZ))]
        public HttpResponseMessage GetPointCloudXYZ(int id)
        {
            var Flight = db.DroneFlights.Find(id);
            if (Flight == null) return new HttpResponseMessage(HttpStatusCode.NotFound);

            List<PointCloudXYZ> pointCloudXYZs = Flight.PointCloudXYZs.ToList();
            List<CTRLPoint> CTRLPoints = Flight.CTRLPoints.ToList();

            Polygon polygon = new Polygon(pointCloudXYZs);
            PointcloudControlTool tool = new PointcloudControlTool(polygon);

            var list = new List<Tuple<int, string, bool>>().Select(t => new { CTRLId = t.Item1, CTRLName = t.Item2, Inside = t.Item3 }).ToList();

            foreach (CTRLPoint ctrl in CTRLPoints)
            {
                bool inside = tool.PointInside3DPolygonSimplified((double)ctrl.X, (double)ctrl.Y, (double)ctrl.Z);
                list.Add(new { ctrl.CTRLId, ctrl.CTRLName, Inside = inside });
            }

            //config to set to json 
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(list));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return response;
        }

        /*
        // PUT: api/PointCloudControlTool/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPointCloudXYZ(int id, PointCloudXYZ pointCloudXYZ)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pointCloudXYZ.PointCloudXYZId)
            {
                return BadRequest();
            }

            db.Entry(pointCloudXYZ).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PointCloudXYZExists(id))
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

        // POST: api/PointCloudControlTool
        [ResponseType(typeof(PointCloudXYZ))]
        public IHttpActionResult PostPointCloudXYZ(PointCloudXYZ pointCloudXYZ)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PointCloudXYZs.Add(pointCloudXYZ);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = pointCloudXYZ.PointCloudXYZId }, pointCloudXYZ);
        }

        // DELETE: api/PointCloudControlTool/5
        [ResponseType(typeof(PointCloudXYZ))]
        public IHttpActionResult DeletePointCloudXYZ(int id)
        {
            PointCloudXYZ pointCloudXYZ = db.PointCloudXYZs.Find(id);
            if (pointCloudXYZ == null)
            {
                return NotFound();
            }

            db.PointCloudXYZs.Remove(pointCloudXYZ);
            db.SaveChanges();

            return Ok(pointCloudXYZ);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PointCloudXYZExists(int id)
        {
            return db.PointCloudXYZs.Count(e => e.PointCloudXYZId == id) > 0;
        }
        */
    }
}