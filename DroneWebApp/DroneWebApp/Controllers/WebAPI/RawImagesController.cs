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
					rawImage.FileName,
					rawImage.RawData,   //raw image data (bytes)
					rawImage.FlightId,
					
					rawImage.XResolution,
					rawImage.YResolution,

					rawImage.CreateDate,
					
					rawImage.Make,
					
					rawImage.SpeedX,
					rawImage.SpeedY,
					rawImage.SpeedZ,
					rawImage.Pitch,
					rawImage.Yaw,
					rawImage.Roll,

					rawImage.ImageWidth,
					rawImage.ImageHeight,

					rawImage.GpsAltitude,
					rawImage.GpsLatitude,
					rawImage.GpsLongitude,
					rawImage.GpsPosition

				}).ToList();

			//config to set to json 
			var response = new HttpResponseMessage(HttpStatusCode.OK);
			response.Content = new StringContent(JsonConvert.SerializeObject(rawImages));
			response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			return response;
		}


	}
}