using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DroneWebApp.Models.SimpleFactoryPattern.Parsers
{
    public class XYZParser : IParser
    {
        public void Parse(string path)
        {
            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(" ");

                //temp
                int i = 0;

                // Set culture to ensure decimal point
                CultureInfo customCulture = (CultureInfo) System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                customCulture.NumberFormat.NumberDecimalSeparator = ".";
                System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

                while (!parser.EndOfData)
                {
                    //Process a row and parse string fields to floats
                    IList<string> attributes = parser.ReadFields();
                    List<float> xyzAttributes = new List<float>();
                    foreach (string xyzAttribute in attributes)
                    {
                        xyzAttributes.Add(float.Parse(xyzAttribute, customCulture));
                    }

                    PointCloudXYZ pointCloudXYZ = new PointCloudXYZ();
                    // xyz
                    pointCloudXYZ.X = xyzAttributes[0];
                    pointCloudXYZ.Y = xyzAttributes[1];
                    pointCloudXYZ.Z = xyzAttributes[2];
                    if (xyzAttributes.Count == 6)
                    {
                        // xyz + rgb
                        pointCloudXYZ.Red = (int) xyzAttributes[3];
                        pointCloudXYZ.Green = (int) xyzAttributes[4];
                        pointCloudXYZ.Blue = (int) xyzAttributes[5];
                    }
                    else if (xyzAttributes.Count == 7)
                    {
                        // xyz + i + rgb
                        pointCloudXYZ.Intensity = (int) xyzAttributes[6]; // TODO: (int) weg en float in database
                    }


                    //temp
                    i++;
                    if (i > 5) { break; }
                }
            }





            // Old code to write to View
            /*
            IList<List<float>> xyzCoords;
            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(" ");
                xyzCoords = new List<List<float>>();

                //temp
                int i = 0;

                // Set culture to ensure decimal point
                CultureInfo customCulture = (CultureInfo) System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                customCulture.NumberFormat.NumberDecimalSeparator = ".";
                System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
                while (!parser.EndOfData)
                {
                    //Process row
                    IList<string> attributes = parser.ReadFields();
                    List<float> xyzAttributes = new List<float>();
                    foreach (string xyzAttribute in attributes)
                    {
                        xyzAttributes.Add(float.Parse(xyzAttribute, customCulture));
                    }
                    xyzCoords.Add(xyzAttributes);

                    //temp
                    i++;
                    if (i > 5) { break; }
                }
            }
            return xyzCoords;
            */
        }
    }
}