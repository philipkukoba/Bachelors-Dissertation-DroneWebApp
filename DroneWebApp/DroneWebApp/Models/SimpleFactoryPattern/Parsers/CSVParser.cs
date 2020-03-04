using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.SimpleFactoryPattern.Parsers
{
    public class CSVParser : IParser
    {
        public void Parse(string path, int flightId, DroneDBEntities db)
        {
            DroneFlight droneFlight = db.DroneFlights.Find(flightId);
            GroundControlPoint gcp;

            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                CultureInfo customCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                customCulture.NumberFormat.NumberDecimalSeparator = ".";
                System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

                while (!parser.EndOfData)
                {
                    IList<string> fields_string = parser.ReadFields();
                    IList<double> fields_double = new List<double>();
                    
                    for (int i=1; i<4; i++)
                    {
                        fields_double.Add(double.Parse(fields_string[i]), customCulture);
                    }

                    gcp = new GroundControlPoint
                    {
                        GCPId = fields_string[0],
                        X = fields_double[0],
                        Y = fields_double[1],
                        Z = fields_double[2]
                    };
                    gcp.FlightId = droneFlight.FlightId;

                    db.GroundControlPoints.Add(gcp);
                }

                db.SaveChanges();
            }
        }
    }
}