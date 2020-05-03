using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;
using DroneWebApp.Models;
using Newtonsoft.Json;

namespace DroneWebApp.Controllers.WebAPI
{
	public class RawImagesController : ApiController
	{
		private DroneDBEntities db = new DroneDBEntities();

		// GET: api/RawImages
		public HttpResponseMessage GetRawImagesByFlightID(int id)
		{
			var Flight = db.DroneFlights.Find(id);

			if (Flight == null || Flight.RawImages.Count == 0)
				return new HttpResponseMessage(HttpStatusCode.NotFound);

			//data projection
			var rawImages = Flight.RawImages.Select(
				rawImage => new
				{
					FileName = rawImage.FileName,

					ImageID = rawImage.RawImageKey,

					FlightID = rawImage.FlightId,

					rawImage.CreateDate,

					rawImage.SpeedX,
					rawImage.SpeedY,
					rawImage.SpeedZ,
					rawImage.Pitch,
					rawImage.Yaw,
					rawImage.Roll,

					rawImage.GpsAltitude,
					rawImage.GpsLatitude,
					rawImage.GpsLongitude,
					rawImage.GpsPosition,

					rawImage.GPSAltitudeRef,
					rawImage.GPSLatRef,
					rawImage.GPSLongRef
				}).ToList();

			//config to set to json 
			var response = new HttpResponseMessage(HttpStatusCode.OK);
			response.Content = new StringContent(JsonConvert.SerializeObject(rawImages));
			response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			return response;
		}

		
		//get the full image by flightid and by imageid
		public HttpResponseMessage GetImage(int id, int imageid)
		{
			try {
				//using parameters against sql injections
				RawImage rawImage = db.RawImages.SqlQuery(
					"SELECT * FROM RawImages WHERE FlightId = @id AND RawImageKey = @imageid;",
					 new SqlParameter("id", id),
					 new SqlParameter("imageid", imageid)
					).First<RawImage>();

				//config to an image
				HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
				result.Content = new ByteArrayContent(rawImage.RawData);
				result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
				return result;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
				return new HttpResponseMessage(HttpStatusCode.NotFound);
			}
		}
	}
}