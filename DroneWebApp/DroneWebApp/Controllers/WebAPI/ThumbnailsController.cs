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

namespace DroneWebApp.Controllers.WebAPI
{
    public class ThumbnailsController : ApiController
    {
        private DroneDBEntities db = new DroneDBEntities();

        // GET: api/Thumbnails
        public HttpResponseMessage GetThumbNail(int id, int imageid)
        {
            System.Diagnostics.Debug.WriteLine("aaaaaaaa");

            //find the right image in db 
            DroneFlight droneFlight = db.DroneFlights.Find(id);
            RawImage rawImage = null;
            bool found = false;
            foreach (RawImage img in droneFlight.RawImages)
            {
                if (img.RawImageKey == imageid)
                {
                    rawImage = img;
                    found = true;
                }
            }
            if (!found) return new HttpResponseMessage(HttpStatusCode.NotFound);

            System.Diagnostics.Debug.WriteLine("bbbbbbbbbbbb");

            //compression of image (making the thumbnail)
            System.IO.MemoryStream myMemStream = new System.IO.MemoryStream(rawImage.RawData);
            System.Drawing.Image fullsizeImage = System.Drawing.Image.FromStream(myMemStream);
            System.Diagnostics.Debug.WriteLine("ccccccccccccccccc");

            int resizeFactor = 64; 
            int newWidth = (int) 5472 / resizeFactor;
            int newHeight = (int) 3648 / resizeFactor;
            
            System.Diagnostics.Debug.WriteLine("ddddddddddddddddd");
            
            System.Drawing.Image newImage = fullsizeImage.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);
            System.IO.MemoryStream myResult = new System.IO.MemoryStream();
            newImage.Save(myResult, System.Drawing.Imaging.ImageFormat.Jpeg);  //Or whatever format you want.
            //return myResult.ToArray();  //Returns a new byte array.
            
            System.Diagnostics.Debug.WriteLine("eeeeeeeeeeeeeeee");
            
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(myResult.ToArray());
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
            return result;

        }

        
    }
}