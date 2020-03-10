﻿using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DroneWebApp.Models.SimpleFactoryPattern.Parsers
{
    public class XYZParser : IParser
    {
        public void Parse(string path, int flightId, DroneDBEntities db)
        {
            // Get the approriate DroneFlight that goes with this data
            DroneFlight droneFlight = db.DroneFlights.Find(flightId);
            PointCloudXYZ pointCloudXYZ;

            // Parse
            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(" ");

                int i = 0;
                int limit = 5000;

                // Set culture to ensure decimal point
                CultureInfo customCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                customCulture.NumberFormat.NumberDecimalSeparator = ".";
                System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

                IList<string> attributes_strings;
                List<double> attributes_doubles;
                // Read data
                while (!parser.EndOfData)
                {
                    try
                    {
                        attributes_strings = parser.ReadFields();
                        attributes_doubles = new List<double>();

                        //Process a row and parse string fields to floats
                        foreach (string xyzAttribute in attributes_strings)
                        {
                            attributes_doubles.Add(double.Parse(xyzAttribute, customCulture));
                        }
                        // Create ORM-object for database mapping
                        pointCloudXYZ = new PointCloudXYZ
                        {
                            // xyz
                            X = attributes_doubles[0],
                            Y = attributes_doubles[1],
                            Z = attributes_doubles[2]
                        };
                        if (attributes_doubles.Count == 6)
                        {
                            // xyz + rgb
                            pointCloudXYZ.Red = (int)attributes_doubles[3];
                            pointCloudXYZ.Green = (int)attributes_doubles[4];
                            pointCloudXYZ.Blue = (int)attributes_doubles[5];
                        }
                        else if (attributes_doubles.Count == 7)
                        {
                            // xyz + i + rgb
                            pointCloudXYZ.Intensity = attributes_doubles[6]; // TODO: (int) weg en float in database
                        }

                        // Assign data the appropriate FlightId
                        pointCloudXYZ.FlightId = droneFlight.FlightId;

                        // Add to list of PointCloudXYZs that are to be added to the DB
                        db.PointCloudXYZs.Add(pointCloudXYZ);

                        //Set hasXYZ to true
                        droneFlight.hasXYZ = true;
     
                        //if(i == 5) { break; }
                        System.Diagnostics.Debug.WriteLine("Processed Line: " + i);
                        i++;
                        if (i == limit) break; 

                    }
                    catch(Exception ex) { }
                }

                // Commit changes to the DB
                db.SaveChanges();
            }
        }
    }
}