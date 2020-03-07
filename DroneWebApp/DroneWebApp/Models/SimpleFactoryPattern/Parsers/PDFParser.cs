using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IvyPdf;

namespace DroneWebApp.Models.SimpleFactoryPattern.Parsers
{
    public class PDFParser : IParser
    {
        public void Parse(string path, int flightId, DroneDBEntities db)
        {
            DroneFlight droneFlight = db.DroneFlights.Find(flightId);
            PdfParser pdfParser;
            QualityReport qrp;
            Uncertainty uncertainty;
            GCPError gcpError;

            try
            {
                qrp = new QualityReport();
                pdfParser = new PdfParser(IvyDocumentReader.ReadPdf(@"C:\Users\p_kuk\Desktop\BP\Harelbeke-191210_report_Hightlighted.pdf"));

                //summary
                string Processed = pdfParser.Find("Processed").Right().Text;
                string CameraModelNames = pdfParser.Find("Camera Model Name(s)").Right().Text;
                string AverageGroundSamplingDistance = pdfParser.Find("Average Ground Sampling Distance (GSD)").Right().Text;
                string AreaCovered = pdfParser.Find("Area Covered").Right().Text + pdfParser.Right().Text + pdfParser.Right().Text;
                string TimeForInitialProcessing = pdfParser.Find("Time for Initial Processing (without report)").Right().Text;

                qrp.Processed = Convert.ToDateTime(Processed);
                qrp.CameraModelName = CameraModelNames;
                qrp.AverageGSD = Convert.ToDouble(AverageGroundSamplingDistance);
                qrp.AreaCovered = Convert.ToDouble(AreaCovered);
                qrp.InitialProcessingTime = TimeSpan.Parse(TimeForInitialProcessing); //?? 

                /*
                Console.WriteLine("Processed: " + Processed);
                Console.WriteLine("CameraModelNames: " + CameraModelNames);
                Console.WriteLine("AverageGroundSamplingDistance: " + AverageGroundSamplingDistance);
                Console.WriteLine("AreaCovered: " + AreaCovered);
                Console.WriteLine("TimeForInitialProcessing: " + TimeForInitialProcessing);
                */

                //quality check
                string Dataset = pdfParser.Find("Dataset").Right().Text;
                string CameraOptimization = pdfParser.Find("Camera Optimization").Right().Text;
                string Georeferencing = pdfParser.Find("Georeferencing").Right().Text;

                //TODO!! dataset,cameraoptimiz, georef

                /*
                Console.WriteLine("Dataset: " + Dataset);
                Console.WriteLine("CameraOptimization: " + CameraOptimization);
                Console.WriteLine("Georeferencing: " + Georeferencing);
                */

                //absolute camera position and orientation uncertainties 
                //todo omzetten naar float? 

                uncertainty = new Uncertainty();

                pdfParser.FilterPage(3); //voor de zekerheid dat hij op pagina 3 zit
                string AbsoluteMeanX = pdfParser.Find("Mean").Right().Text;
                string AbsoluteMeanY = pdfParser.Right().Text;
                string AbsoluteMeanZ = pdfParser.Right().Text;

                string AbsoluteSigmaX = pdfParser.Find("Sigma").Right().Text;
                string AbsoluteSigmaY = pdfParser.Right().Text;
                string AbsoluteSigmaZ = pdfParser.Right().Text;
                pdfParser.FilterClear();

                uncertainty.AMU_x = Convert.ToDouble(AbsoluteMeanX);
                uncertainty.AMU_y = Convert.ToDouble(AbsoluteMeanY);
                uncertainty.AMU_z = Convert.ToDouble(AbsoluteMeanZ);
                uncertainty.ASU_x = Convert.ToDouble(AbsoluteSigmaX);
                uncertainty.ASU_y = Convert.ToDouble(AbsoluteSigmaY);
                uncertainty.ASU_z = Convert.ToDouble(AbsoluteSigmaZ);

                /*
                Console.WriteLine("0.008 ?: " + AbsoluteMeanX);
                Console.WriteLine("0.008 ?: " + AbsoluteMeanY);
                Console.WriteLine("0.014 ?: " + AbsoluteMeanZ);
                Console.WriteLine("0.001 ?: " + AbsoluteSigmaX);
                Console.WriteLine("0.001 ?: " + AbsoluteSigmaY);
                Console.WriteLine("0.001 ?: " + AbsoluteSigmaZ);
                */

                //absolute camera position and orientation uncertainties 
                //todo omzetten naar float? 
                pdfParser.FilterPage(6);
                string RelativeMeanX = pdfParser.Find("Mean").Right().Text;
                string RelativeMeanY = pdfParser.Right().Text;
                string RelativeMeanZ = pdfParser.Right().Text;

                string RelativeSigmaX = pdfParser.Find("Sigma").Right().Text;
                string RelativeSigmaY = pdfParser.Right().Text;
                string RelativeSigmaZ = pdfParser.Right().Text;

                uncertainty.RMU_x = Convert.ToDouble(RelativeMeanX);
                uncertainty.RMU_y = Convert.ToDouble(RelativeMeanY);
                uncertainty.RMU_z = Convert.ToDouble(RelativeMeanZ);
                uncertainty.RSU_x = Convert.ToDouble(RelativeSigmaX);
                uncertainty.RSU_y = Convert.ToDouble(RelativeSigmaY);
                uncertainty.RSU_z = Convert.ToDouble(RelativeSigmaZ);

                /*
                Console.WriteLine("RelativeMeanX 0.007: " + RelativeMeanX);
                Console.WriteLine("RelativeMeanY 0.006: " + RelativeMeanY);
                Console.WriteLine("RelativeMeanZ 0.008: " + RelativeMeanZ);

                Console.WriteLine("RelativeSigmaX 0.002: " + RelativeSigmaX);
                Console.WriteLine("RelativeSigmaY 0.002: " + RelativeSigmaY);
                Console.WriteLine("RelativeSigmaZ 0.004: " + RelativeSigmaZ);
                */

                //Ground control points 
                string MeanErrorX = pdfParser.Find("Mean").Right().Text;
                string MeanErrorY = pdfParser.Right().Text;
                string MeanErrorZ = pdfParser.Right().Text;

                string SigmaErrorX = pdfParser.Find("Sigma").Right().Text;
                string SigmaErrorY = pdfParser.Right().Text;
                string SigmaErrorZ = pdfParser.Right().Text;

                string RMSErrorX = pdfParser.Find("RMS Error").Right().Text;
                string RMSErrorY = pdfParser.Right().Text;
                string RMSErrorZ = pdfParser.Right().Text;

                gcpError = new GCPError
                {
                    GCPMeanError_x = Convert.ToDouble(MeanErrorX),
                    GCPMeanError_y = Convert.ToDouble(MeanErrorY),
                    GCPMeanError_z = Convert.ToDouble(MeanErrorZ),

                    GCPSigma_x = Convert.ToDouble(SigmaErrorX),
                    GCPSigma_y = Convert.ToDouble(SigmaErrorY),
                    GCPSigma_z = Convert.ToDouble(SigmaErrorZ),

                    GCPRMS_x = Convert.ToDouble(RMSErrorX),
                    GCPRMS_y = Convert.ToDouble(RMSErrorY),
                    GCPRMS_z = Convert.ToDouble(RMSErrorZ)
                };

                /*
                Console.WriteLine("MeanErrorX -0.000051: " + MeanErrorX);
                Console.WriteLine("MeanErrorY -0.000158: " + MeanErrorY);
                Console.WriteLine("MeanErrorZ -0.000181: " + MeanErrorZ);

                Console.WriteLine("SigmaErrorX 0.007058: " + SigmaErrorX);
                Console.WriteLine("SigmaErrorY 0.009429: " + SigmaErrorY);
                Console.WriteLine("SigmaErrorZ 0.005903: " + SigmaErrorZ);

                Console.WriteLine("RMSErrorX 0.007058: " + RMSErrorX);
                Console.WriteLine("RMSErrorY 0.009430: " + RMSErrorY);
                Console.WriteLine("RMSErrorZ 0.005906: " + RMSErrorZ);
                */

                //absolute geolocation variance 
                string MeanGeolocationErrorX = pdfParser.Find("Mean").Right().Text;
                string MeanGeolocationErrorY = pdfParser.Right().Text;
                string MeanGeolocationErrorZ = pdfParser.Right().Text;

                string SigmaGeolocationErrorX = pdfParser.Find("Sigma").Right().Text;
                string SigmaGeolocationErrorY = pdfParser.Right().Text;
                string SigmaGeolocationErrorZ = pdfParser.Right().Text;

                string RMSGeolocationErrorX = pdfParser.Find("RMS Error").Right().Text;
                string RMSGeolocationErrorY = pdfParser.Right().Text;
                string RMSGeolocationErrorZ = pdfParser.Right().Text;
                pdfParser.FilterClear(); //eind pagina 6

                Console.WriteLine("MeanGeolocationErrorX -0.715677: " + MeanGeolocationErrorX);
                Console.WriteLine("MeanGeolocationErrorY -0.235702: " + MeanGeolocationErrorY);
                Console.WriteLine("MeanGeolocationErrorZ -122.511322: " + MeanGeolocationErrorZ);

                Console.WriteLine("SigmaGeolocationErrorX 1.235047: " + SigmaGeolocationErrorX);
                Console.WriteLine("SigmaGeolocationErrorY 1.445375: " + SigmaGeolocationErrorY);
                Console.WriteLine("SigmaGeolocationErrorZ 0.837803: " + SigmaGeolocationErrorZ);

                Console.WriteLine("RMSGeolocationErrorX 1.427423: " + RMSGeolocationErrorX);
                Console.WriteLine("RMSGeolocationErrorY 1.464467: " + RMSGeolocationErrorY);
                Console.WriteLine("RMSGeolocationErrorZ 122.514187: " + RMSGeolocationErrorZ);


                //system information
                string Hardware = pdfParser.FilterPage(7).FilterWindow(133, 431, 539, 465).ExtractText();
                pdfParser.FilterClear();
                string OperatingSystem = pdfParser.Find("Operating System").Right().Text;

                //qrp.CPU .. qrp.RAM .. qrp.GPU  => TODO
                qrp.OS = OperatingSystem;

                Console.WriteLine("hardware: " + Hardware);
                Console.WriteLine("OperatingSystem: " + OperatingSystem);

                //coordinate systems 
                string ImageCoordinateSystem = pdfParser.Find("Image Coordinate System").Right().Text;
                string GroundControlPointCoordinateSystem = pdfParser.Find("Ground Control Point (GCP) Coordinate System").Right().Text;
                string OutputCoordinateSystem = pdfParser.Find("Output Coordinate System").Right().Text;

                qrp.ImageCoordinateSystem = ImageCoordinateSystem;
                qrp.GCPCoordinateSystem = GroundControlPointCoordinateSystem;
                qrp.OutputCoordinateSystem = OutputCoordinateSystem;

                /*
                Console.WriteLine("ImageCoordinateSystem: " + ImageCoordinateSystem);
                Console.WriteLine("GroundControlPointCoordinateSystem: " + GroundControlPointCoordinateSystem);
                Console.WriteLine("OutputCoordinateSystem: " + OutputCoordinateSystem);
                */

                //results
                //omzetten naar int/floats? 
                string NumberOfGeneratedTiles = pdfParser.Find("Number of Generated Tiles").Right().Text;
                string NumberOf3DDensifiedPoints = pdfParser.Find("Number of 3D Densified Points").Right().Text;
                string AverageDensity = pdfParser.FindPattern("Average Density*").Right().Right().Right().Text;

                qrp.GeneratedTiles = Convert.ToInt32(NumberOfGeneratedTiles);
                qrp.DensifiedPoints3D = Convert.ToInt32(NumberOf3DDensifiedPoints);
                qrp.AverageDensity = float.Parse(AverageDensity);

                /*
                Console.WriteLine("NumberOfGeneratedTiles: " + NumberOfGeneratedTiles);
                Console.WriteLine("NumberOf3DDensifiedPoints: " + NumberOf3DDensifiedPoints);
                Console.WriteLine("AverageDensity: " + AverageDensity);
                */

                //Assign data the appropriate FlightId
                qrp.QualityReportId = droneFlight.FlightId;
                uncertainty.UncertaintyId = droneFlight.FlightId;
                gcpError.GCPErrorId = droneFlight.FlightId;


                //Add to list of GCPErrors to be added to the database
                db.GCPErrors.Add(gcpError);

                //Add to list of GCPErrors to be added to the database
                db.Uncertainties.Add(uncertainty);

                //Add to list of QualityReports to be added to the database
                db.QualityReports.Add(qrp);

                // Set hasQR to true
                droneFlight.hasQR = true;

                // Save changes to the database
                db.SaveChanges();
            }
            catch (Exception ex) { }
        }
    }
}