using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.SimpleFactoryPattern.Parsers
{
    public class TFWParser : IParser
    {
        public void Parse(string path, int flightId, DroneDBEntities db)
        {
            //Get the appropriate DroneFlight that goes with this data
            DroneFlight droneFlight = db.DroneFlights.Find(flightId);
            TFW tfw;

            //Parse
            using (TextFieldParser parser = new TextFieldParser(path))
            {
                //Set culture to ensure decimal point
                CultureInfo customeCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                customeCulture.NumberFormat.NumberDecimalSeparator = ".";
                System.Threading.Thread.CurrentThread.CurrentCulture = customeCulture;

                //Create ORM-object for database mapping
                tfw = new TFW
                {
                    //Process all elements and store in the right variables
                    xScale_X = float.Parse(parser.ReadLine()),
                    xRotationTerm_Y = float.Parse(parser.ReadLine()),
                    yRotationTerm_X = float.Parse(parser.ReadLine()),
                    yNegativeScale_Y = float.Parse(parser.ReadLine()),
                    TranslationTerm_X = float.Parse(parser.ReadLine()),
                    TranslationTerm_Y = float.Parse(parser.ReadLine())
                };

                //Assign data to the appropriate flightId
                tfw.TFWId = droneFlight.FlightId;

                //Add to list of TFWs to be added to the database
                db.TFWs.Add(tfw);

                //Set hasTFW to true
                droneFlight.hasTFW = true;

                //Save changes to the database
                db.SaveChanges();
            }
        }
    }
}