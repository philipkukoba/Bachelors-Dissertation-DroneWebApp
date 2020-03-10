using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
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
            AbsoluteGeolocationVariance absoluteGeolocationVariance;  

            // Set culture
            CultureInfo customCulture = (CultureInfo) System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            try
            {
                qrp = new QualityReport();
                qrp.QualityReportId = droneFlight.FlightId;
                pdfParser = new PdfParser(IvyDocumentReader.ReadPdf(path));

                //summary
                string Processed = pdfParser.Find("Processed").Right().Text;
                string CameraModelNames = pdfParser.Find("Camera Model Name(s)").Right().Text;
                string AverageGroundSamplingDistance = pdfParser.Find("Average Ground Sampling Distance (GSD)").Right().Text;
                string AreaCovered = pdfParser.Find("Area Covered").Right().Text + pdfParser.Right().Text + pdfParser.Right().Text;
                string TimeForInitialProcessing = pdfParser.Find("Time for Initial Processing (without report)").Right().Text;

                //regex conversions 
                AverageGroundSamplingDistance = Regex.Replace(AverageGroundSamplingDistance, " cm(.*)", ""); //enkel cm waarde behouden
                AreaCovered = Regex.Replace(AreaCovered, " km2(.*)", ""); //enkel km2 waarde behouden
                //TimeForInitialProcessing = Regex.Replace(TimeForInitialProcessing, "m", ""); // 'm' wegdoen
                //TimeForInitialProcessing = Regex.Replace(TimeForInitialProcessing, "s", ""); // 's' wegdoen
                TimeForInitialProcessing = "00:" + TimeForInitialProcessing; // "00:" er bij plakken in het begin voor de juiste timespan format

                qrp.Processed = Convert.ToDateTime(Processed);
                qrp.CameraModelName = CameraModelNames;
                qrp.AverageGSD = Convert.ToDouble(AverageGroundSamplingDistance, customCulture);
                qrp.AreaCovered = Convert.ToDouble(AreaCovered, customCulture);
                //qrp.InitialProcessingTime = TimeSpan.ParseExact(TimeForInitialProcessing, "hh\\:mm\\:ss", CultureInfo.InvariantCulture);

                //quality check
                string Dataset = pdfParser.Find("Dataset").Right().Text;
                string CameraOptimization = pdfParser.Find("Camera Optimization").Right().Text;
                string Georeferencing = pdfParser.Find("Georeferencing").Right().Text;

                qrp.Dataset = Dataset;
                qrp.CameraOptimization = CameraOptimization;
                qrp.Georeferencing = Georeferencing;

                // DroneFlight now has a Quality Report
                droneFlight.hasQR = true;
                //Add to list of QualityReports to be added to the database
                db.QualityReports.Add(qrp);
                // Save changes to the database
                db.SaveChanges();

                #region Regex DataSet (unused)
                //Regex dataset
                //Regex datasetRegex = new Regex(@"(\d+?) out of (\d+?) images calibrated \((?:\d+?)%\), (.*)", RegexOptions.Singleline | RegexOptions.Compiled);
                //Match mDataSet = datasetRegex.Match(Dataset);
                //string DatasetAmountCalibrated = mDataSet.Groups[0].Value;
                //string DatasetAmountTotal = mDataSet.Groups[1].Value;
                //string DatasetStatus = mDataSet.Groups[2].Value;

                //System.Diagnostics.Debug.WriteLine("DatasetAmountCalibrated: " + DatasetAmountCalibrated);
                //System.Diagnostics.Debug.WriteLine("DatasetAmountTotal: " + DatasetAmountTotal);
                //System.Diagnostics.Debug.WriteLine("DatasetStatus: " + DatasetStatus);

                //Regex cameraoptimization
                //Regex georeferencingRegex = new Regex(@"(.*?), (\d*?) GCPs (?:.*?) RMS error = (.*?) m", RegexOptions.Singleline | RegexOptions.Compiled);
                #endregion
                
                //absolute camera position and orientation uncertainties 
                uncertainty = new Uncertainty();

                pdfParser.FilterPage(3); //voor de zekerheid dat hij op pagina 3 zit
                string AbsoluteMeanX = pdfParser.Find("Mean").Right().Text;
                string AbsoluteMeanY = pdfParser.Right().Text;
                string AbsoluteMeanZ = pdfParser.Right().Text;

                string AbsoluteSigmaX = pdfParser.Find("Sigma").Right().Text;
                string AbsoluteSigmaY = pdfParser.Right().Text;
                string AbsoluteSigmaZ = pdfParser.Right().Text;
                pdfParser.FilterClear();

                uncertainty.AMU_x = Convert.ToDouble(AbsoluteMeanX, customCulture);
                uncertainty.AMU_y = Convert.ToDouble(AbsoluteMeanY, customCulture);
                uncertainty.AMU_z = Convert.ToDouble(AbsoluteMeanZ, customCulture);
                uncertainty.ASU_x = Convert.ToDouble(AbsoluteSigmaX, customCulture);
                uncertainty.ASU_y = Convert.ToDouble(AbsoluteSigmaY, customCulture);
                uncertainty.ASU_z = Convert.ToDouble(AbsoluteSigmaZ, customCulture);

                //absolute camera position and orientation uncertainties 
                pdfParser.FilterPage(6);
                string RelativeMeanX = pdfParser.Find("Mean").Right().Text;
                string RelativeMeanY = pdfParser.Right().Text;
                string RelativeMeanZ = pdfParser.Right().Text;

                string RelativeSigmaX = pdfParser.Find("Sigma").Right().Text;
                string RelativeSigmaY = pdfParser.Right().Text;
                string RelativeSigmaZ = pdfParser.Right().Text;

                uncertainty.RMU_x = Convert.ToDouble(RelativeMeanX, customCulture);
                uncertainty.RMU_y = Convert.ToDouble(RelativeMeanY, customCulture);
                uncertainty.RMU_z = Convert.ToDouble(RelativeMeanZ, customCulture);
                uncertainty.RSU_x = Convert.ToDouble(RelativeSigmaX, customCulture);
                uncertainty.RSU_y = Convert.ToDouble(RelativeSigmaY, customCulture);
                uncertainty.RSU_z = Convert.ToDouble(RelativeSigmaZ, customCulture);

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

                //define gcpError object for ORM mapping
                gcpError = new GCPError
                {
                    GCPMeanError_x = Convert.ToDouble(MeanErrorX, customCulture),
                    GCPMeanError_y = Convert.ToDouble(MeanErrorY, customCulture),
                    GCPMeanError_z = Convert.ToDouble(MeanErrorZ, customCulture),

                    GCPSigma_x = Convert.ToDouble(SigmaErrorX, customCulture),
                    GCPSigma_y = Convert.ToDouble(SigmaErrorY, customCulture),
                    GCPSigma_z = Convert.ToDouble(SigmaErrorZ, customCulture),

                    GCPRMS_x = Convert.ToDouble(RMSErrorX, customCulture),
                    GCPRMS_y = Convert.ToDouble(RMSErrorY, customCulture),
                    GCPRMS_z = Convert.ToDouble(RMSErrorZ, customCulture)
                };

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

                //define absoluteGeolocationVariance object for ORM mapping
                absoluteGeolocationVariance = new AbsoluteGeolocationVariance
                {
                    AGVMeanError_x = Convert.ToDouble(MeanGeolocationErrorX, customCulture),
                    AGVMeanError_y = Convert.ToDouble(MeanGeolocationErrorY, customCulture),
                    AGVMeanError_z = Convert.ToDouble(MeanGeolocationErrorZ, customCulture),

                    AGVSigma_x = Convert.ToDouble(SigmaGeolocationErrorX, customCulture),
                    AGVSigma_y = Convert.ToDouble(SigmaGeolocationErrorY, customCulture),
                    AGVSigma_z = Convert.ToDouble(SigmaGeolocationErrorZ, customCulture),

                    AGVRMS_x = Convert.ToDouble(RMSGeolocationErrorX, customCulture),
                    AGVRMS_y = Convert.ToDouble(RMSGeolocationErrorY, customCulture),
                    AGVRMS_z = Convert.ToDouble(RMSGeolocationErrorZ, customCulture)
                };

                //system information
                string Hardware = pdfParser.FilterPage(7).FilterWindow(133, 431, 539, 465).ExtractText();
                pdfParser.FilterClear();
                string OperatingSystem = pdfParser.Find("Operating System").Right().Text;

                //regex for hardware string
                Regex rx = new Regex(@"CPU: (.*?)RAM: (.*?)GPU: (.*)", RegexOptions.Compiled | RegexOptions.Singleline);
                Match m = rx.Match(Hardware);
                string CPU = m.Groups[1].Value;
                CPU = Regex.Replace(CPU, "CPU: ", ""); 
                string RAM = m.Groups[2].Value;
                RAM = Regex.Replace(RAM, "GB", "");
                string GPU = m.Groups[3].Value;
                GPU.Trim();

                qrp.CPU = CPU;
                qrp.RAM = Int32.Parse(RAM); //GB
                qrp.GPU = GPU; 
                qrp.OS = OperatingSystem;

                //coordinate systems 
                string ImageCoordinateSystem = pdfParser.Find("Image Coordinate System").Right().Text;
                string GroundControlPointCoordinateSystem = pdfParser.Find("Ground Control Point (GCP) Coordinate System").Right().Text;
                string OutputCoordinateSystem = pdfParser.Find("Output Coordinate System").Right().Text;

                qrp.ImageCoordinateSystem = ImageCoordinateSystem;
                qrp.GCPCoordinateSystem = GroundControlPointCoordinateSystem;
                qrp.OutputCoordinateSystem = OutputCoordinateSystem;

                //results
                string NumberOfGeneratedTiles = pdfParser.Find("Number of Generated Tiles").Right().Text;
                string NumberOf3DDensifiedPoints = pdfParser.Find("Number of 3D Densified Points").Right().Text;
                string AverageDensity = pdfParser.FindPattern("Average Density*").Right().Right().Right().Text;

                qrp.GeneratedTiles = Convert.ToInt32(NumberOfGeneratedTiles);
                qrp.DensifiedPoints3D = Convert.ToInt32(NumberOf3DDensifiedPoints);
                qrp.AverageDensity = float.Parse(AverageDensity);

                // Map all ids 1-to-1
                uncertainty.UncertaintyId = qrp.QualityReportId; //droneFlight.FlightId;
                gcpError.GCPErrorId = qrp.QualityReportId; //droneFlight.FlightId;
                absoluteGeolocationVariance.AbsoluteGeolocationVarianceId = qrp.QualityReportId;


                //Add to list of GCPErrors to be added to the database
                db.GCPErrors.Add(gcpError);

                //Add to list of Uncertainties to be added to the database
                db.Uncertainties.Add(uncertainty);

                //Add to list of AbsoluteGeolocationVariances to be added to the database
                db.AbsoluteGeolocationVariances.Add(absoluteGeolocationVariance);

                // Save changes to the database
                db.SaveChanges();


                #region Debugging with Console 
                System.Diagnostics.Debug.WriteLine("AverageGroundSamplingDistance: " + AverageGroundSamplingDistance);
                System.Diagnostics.Debug.WriteLine("AreaCovered: " + AreaCovered);
                System.Diagnostics.Debug.WriteLine("TimeForInitialProcessing: " + TimeForInitialProcessing);

                System.Diagnostics.Debug.WriteLine("Processed: " + Processed);
                System.Diagnostics.Debug.WriteLine("CameraModelNames: " + CameraModelNames);
                System.Diagnostics.Debug.WriteLine("AverageGroundSamplingDistance: " + AverageGroundSamplingDistance);
                System.Diagnostics.Debug.WriteLine("AreaCovered: " + AreaCovered);
                System.Diagnostics.Debug.WriteLine("TimeForInitialProcessing: " + TimeForInitialProcessing);

                System.Diagnostics.Debug.WriteLine("Dataset: " + Dataset);
                System.Diagnostics.Debug.WriteLine("CameraOptimization: " + CameraOptimization);
                System.Diagnostics.Debug.WriteLine("Georeferencing: " + Georeferencing);

                System.Diagnostics.Debug.WriteLine("0.008 ?: " + AbsoluteMeanX);
                System.Diagnostics.Debug.WriteLine("0.008 ?: " + AbsoluteMeanY);
                System.Diagnostics.Debug.WriteLine("0.014 ?: " + AbsoluteMeanZ);
                System.Diagnostics.Debug.WriteLine("0.001 ?: " + AbsoluteSigmaX);
                System.Diagnostics.Debug.WriteLine("0.001 ?: " + AbsoluteSigmaY);
                System.Diagnostics.Debug.WriteLine("0.001 ?: " + AbsoluteSigmaZ);


                System.Diagnostics.Debug.WriteLine("RelativeMeanX 0.007: " + RelativeMeanX);
                System.Diagnostics.Debug.WriteLine("RelativeMeanY 0.006: " + RelativeMeanY);
                System.Diagnostics.Debug.WriteLine("RelativeMeanZ 0.008: " + RelativeMeanZ);

                System.Diagnostics.Debug.WriteLine("RelativeSigmaX 0.002: " + RelativeSigmaX);
                System.Diagnostics.Debug.WriteLine("RelativeSigmaY 0.002: " + RelativeSigmaY);
                System.Diagnostics.Debug.WriteLine("RelativeSigmaZ 0.004: " + RelativeSigmaZ);

                System.Diagnostics.Debug.WriteLine("MeanErrorX -0.000051: " + MeanErrorX);
                System.Diagnostics.Debug.WriteLine("MeanErrorY -0.000158: " + MeanErrorY);
                System.Diagnostics.Debug.WriteLine("MeanErrorZ -0.000181: " + MeanErrorZ);

                System.Diagnostics.Debug.WriteLine("SigmaErrorX 0.007058: " + SigmaErrorX);
                System.Diagnostics.Debug.WriteLine("SigmaErrorY 0.009429: " + SigmaErrorY);
                System.Diagnostics.Debug.WriteLine("SigmaErrorZ 0.005903: " + SigmaErrorZ);

                System.Diagnostics.Debug.WriteLine("RMSErrorX 0.007058: " + RMSErrorX);
                System.Diagnostics.Debug.WriteLine("RMSErrorY 0.009430: " + RMSErrorY);
                System.Diagnostics.Debug.WriteLine("RMSErrorZ 0.005906: " + RMSErrorZ);

                System.Diagnostics.Debug.WriteLine("MeanGeolocationErrorX -0.715677: " + MeanGeolocationErrorX);
                System.Diagnostics.Debug.WriteLine("MeanGeolocationErrorY -0.235702: " + MeanGeolocationErrorY);
                System.Diagnostics.Debug.WriteLine("MeanGeolocationErrorZ -122.511322: " + MeanGeolocationErrorZ);

                System.Diagnostics.Debug.WriteLine("SigmaGeolocationErrorX 1.235047: " + SigmaGeolocationErrorX);
                System.Diagnostics.Debug.WriteLine("SigmaGeolocationErrorY 1.445375: " + SigmaGeolocationErrorY);
                System.Diagnostics.Debug.WriteLine("SigmaGeolocationErrorZ 0.837803: " + SigmaGeolocationErrorZ);

                System.Diagnostics.Debug.WriteLine("RMSGeolocationErrorX 1.427423: " + RMSGeolocationErrorX);
                System.Diagnostics.Debug.WriteLine("RMSGeolocationErrorY 1.464467: " + RMSGeolocationErrorY);
                System.Diagnostics.Debug.WriteLine("RMSGeolocationErrorZ 122.514187: " + RMSGeolocationErrorZ);
                System.Diagnostics.Debug.WriteLine("CPU: *" + CPU + "*");
                System.Diagnostics.Debug.WriteLine("RAM: *" + RAM + "*");
                System.Diagnostics.Debug.WriteLine("GPU: *" + GPU + "*");
                System.Diagnostics.Debug.WriteLine(GPU.Length);


                System.Diagnostics.Debug.WriteLine("hardware: " + Hardware);
                System.Diagnostics.Debug.WriteLine("OperatingSystem: " + OperatingSystem);


                System.Diagnostics.Debug.WriteLine("ImageCoordinateSystem: " + ImageCoordinateSystem);
                System.Diagnostics.Debug.WriteLine("GroundControlPointCoordinateSystem: " + GroundControlPointCoordinateSystem);
                System.Diagnostics.Debug.WriteLine("OutputCoordinateSystem: " + OutputCoordinateSystem);


                System.Diagnostics.Debug.WriteLine("NumberOfGeneratedTiles: " + NumberOfGeneratedTiles);
                System.Diagnostics.Debug.WriteLine("NumberOf3DDensifiedPoints: " + NumberOf3DDensifiedPoints);
                System.Diagnostics.Debug.WriteLine("AverageDensity: " + AverageDensity);
                #endregion 
                
            }
            catch (DbEntityValidationException e)
            {
                //important debugging code, prints out readable Database exceptions
                foreach (var eve in e.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                //throw;
            }
            catch (Exception ex) {
                //for all other exceptions
                System.Diagnostics.Debug.WriteLine("Exception caught in PDFParser: " + ex);
            }
        }
    }
}