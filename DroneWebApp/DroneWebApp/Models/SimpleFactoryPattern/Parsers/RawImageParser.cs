using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.SimpleFactoryPattern.Parsers
{
	public class RawImageParser : IParser
	{
		public bool Parse(string path, int flightId, DroneDBEntities db)
		{
			//DroneFlight droneFlight = db.DroneFlights.Find(flightId);
			//todo: moet er hier nog iets gebeuren met de droneFlight?? 

			// Set culture for double conversion 
			CultureInfo customCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
			customCulture.NumberFormat.NumberDecimalSeparator = ".";
			System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

			//culture for date parsing 
			CultureInfo provider = CultureInfo.InvariantCulture;
			string format = "yyyy:dd:MM HH:mm:ss"; //2019:09:12 15:49:47

			//reading metadata
			var directories = ImageMetadataReader.ReadMetadata(path);

			byte[] rawData; //ingelezen image in raw bytes
			using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
			{
				rawData = new byte[fs.Length];
				fs.Read(rawData, 0, System.Convert.ToInt32(fs.Length));
			}
			//todo: try catch??


			//make RawImage object and set its attributes
			RawImage rawImage = new RawImage
			{
				FileName = directories[10].Tags[0].Description,
				Image = rawData,  
				FlightId = flightId,

				FileSize = Double.Parse(directories[10].Tags[1].Description.Split(' ')[0]),    
				FileTypeExtension = directories[9].Tags[3].Description,
				Orientation = directories[1].Tags[3].Description,

				XResolution = Int32.Parse(directories[1].Tags[4].Description.Split(' ')[0]),
				YResolution = Int32.Parse(directories[1].Tags[5].Description.Split(' ')[0]),
				ResolutionUnit = directories[1].Tags[6].Description,
				CreateDate = DateTime.ParseExact(directories[1].Tags[8].Description, format, provider),

				Make = directories[1].Tags[1].Description,

				//drone (aircraft) 
				SpeedX = Convert.ToDouble(directories[3].Tags[2].Description, customCulture),
				SpeedY = Convert.ToDouble(directories[3].Tags[3].Description, customCulture),
				SpeedZ = Convert.ToDouble(directories[3].Tags[4].Description, customCulture),
				Pitch = Convert.ToDouble(directories[3].Tags[5].Description, customCulture),
				Yaw = Convert.ToDouble(directories[3].Tags[6].Description, customCulture),
				Roll = Convert.ToDouble(directories[3].Tags[7].Description, customCulture),

				CameraPitch = Convert.ToDouble(directories[3].Tags[8].Description, customCulture),
				CameraYaw = Convert.ToDouble(directories[3].Tags[9].Description, customCulture),
				CameraRoll = Convert.ToDouble(directories[3].Tags[10].Description, customCulture),

				ImageWidth = Int32.Parse(directories[0].Tags[3].Description.Split(' ')[0]),
				ImageHeight = Int32.Parse(directories[0].Tags[2].Description.Split(' ')[0]),

				GpsVersionId = directories[5].Tags[0].Description,
				GpsLatitudeRef = directories[5].Tags[1].Description,
				GpsLongitudeRef = directories[5].Tags[3].Description,
				GpsAltitudeRef = directories[5].Tags[5].Description,

				XpComment = directories[1].Tags[10].Description,

				AbsoluteAltitude = null,
				RelativeAltitude = null,

				GpsAltitude = directories[5].Tags[6].Description,
				GpsLatitude = directories[5].Tags[2].Description,
				GpsLongitude = directories[5].Tags[4].Description,
				GpsPosition = null,

				PreviewImage = null, 
				MegaPixels = null,

				ThumbnailImage = null, 

				Fov = null,
				RawHeader = null
			};

			//Add rawImage Object to DB and save changes
			db.RawImages.Add(rawImage);
			db.SaveChanges();


			#region console prints 
			//Debug.WriteLine(directories[0]); //JPEG Directory (8 tags)
			//Debug.WriteLine(directories[1]); //Exif IFD0 Directory (12 tags)
			//Debug.WriteLine(directories[2]); //Exif SubIFD Directory (37 tags)
			//Debug.WriteLine(directories[3]); //DJI Makernote Directory (35 tags)
			//Debug.WriteLine(directories[4]); //Interoperability Directory (2 tags)
			//Debug.WriteLine(directories[5]); //GPS Directory (7 tags)
			//Debug.WriteLine(directories[6]); //Exif Thumbnail Directory (6 tags)
			//Debug.WriteLine(directories[7]); //XMP Directory (1 tag)
			//Debug.WriteLine(directories[8]); //Huffman Directory (1 tag)
			//Debug.WriteLine(directories[9]); //File Type Directory (4 tags)
			//Debug.WriteLine(directories[10]); //File Directory (3 tags)
			//Debug.WriteLine("");

			//Debug.WriteLine("Image Height " + directories[0].Tags[2].Description);
			//Debug.WriteLine("Image Width " + directories[0].Tags[3].Description);

			//Debug.WriteLine("File Name: " + directories[10].Tags[0].Description);
			//Debug.WriteLine("File Size: " + directories[10].Tags[1].Description);

			//Debug.WriteLine("File Type Extension: " + directories[9].Tags[3].Description);

			//Debug.WriteLine("GPS Version ID: " + directories[5].Tags[0].Description);
			//Debug.WriteLine("GPS Latitude Ref: " + directories[5].Tags[1].Description);
			//Debug.WriteLine("GPS Latitude: " + directories[5].Tags[2].Description);
			//Debug.WriteLine("GPS Longitude Ref: " + directories[5].Tags[3].Description);
			//Debug.WriteLine("GPS Longitude: " + directories[5].Tags[4].Description);
			//Debug.WriteLine("GPS Altitude Ref: " + directories[5].Tags[5].Description);
			//Debug.WriteLine("GPS Altitude: " + directories[5].Tags[6].Description);

			//Debug.WriteLine("Make: " + directories[3].Tags[0].Description);

			//Debug.WriteLine("X Speed: " + directories[3].Tags[2].Description);
			//Debug.WriteLine("Y Speed: " + directories[3].Tags[3].Description);
			//Debug.WriteLine("Z Speed: " + directories[3].Tags[4].Description);

			//Debug.WriteLine("Aircraft Pitch: " + directories[3].Tags[5].Description);
			//Debug.WriteLine("Aircraft Yaw: " + directories[3].Tags[6].Description);
			//Debug.WriteLine("Aircraft Roll: " + directories[3].Tags[7].Description);

			//Debug.WriteLine("Camera Pitch: " + directories[3].Tags[8].Description);
			//Debug.WriteLine("Camera Yaw: " + directories[3].Tags[9].Description);
			//Debug.WriteLine("Camera Roll: " + directories[3].Tags[10].Description);


			//foreach (var directory in directories)
			//{
			//	foreach (var tag in directory.Tags)
			//	{
			//		//System.Diagnostics.Debug.WriteLine($"{directory.Name} - {tag.Name} = {tag.Description}");
			//	}
			//}

			#endregion

			return true;
		}
	}
}