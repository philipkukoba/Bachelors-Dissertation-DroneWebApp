using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DroneWebApp.Models;

// TEST

namespace DroneWebApp.Controllers
{
    public class CTRLPointsController : ApiController
    {
        private DroneDBEntities db = new DroneDBEntities();

        // GET: api/CTRLPoints
        public IQueryable<CTRLPoint> GetCTRLPoints()
        {
            return db.CTRLPoints;
        }

        // GET: api/CTRLPoints/5
        [ResponseType(typeof(CTRLPoint))]
        public IHttpActionResult GetCTRLPoint(int id)
        {
            CTRLPoint cTRLPoint = db.CTRLPoints.Find(id);
            if (cTRLPoint == null)
            {
                return NotFound();
            }

            return Ok(cTRLPoint);
        }

        // PUT: api/CTRLPoints/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCTRLPoint(int id, CTRLPoint cTRLPoint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cTRLPoint.CTRLId)
            {
                return BadRequest();
            }

            db.Entry(cTRLPoint).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CTRLPointExists(id))
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

        // POST: api/CTRLPoints
        [ResponseType(typeof(CTRLPoint))]
        public IHttpActionResult PostCTRLPoint(CTRLPoint cTRLPoint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CTRLPoints.Add(cTRLPoint);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cTRLPoint.CTRLId }, cTRLPoint);
        }

        // DELETE: api/CTRLPoints/5
        [ResponseType(typeof(CTRLPoint))]
        public IHttpActionResult DeleteCTRLPoint(int id)
        {
            CTRLPoint cTRLPoint = db.CTRLPoints.Find(id);
            if (cTRLPoint == null)
            {
                return NotFound();
            }

            db.CTRLPoints.Remove(cTRLPoint);
            db.SaveChanges();

            return Ok(cTRLPoint);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CTRLPointExists(int id)
        {
            return db.CTRLPoints.Count(e => e.CTRLId == id) > 0;
        }
    }
}