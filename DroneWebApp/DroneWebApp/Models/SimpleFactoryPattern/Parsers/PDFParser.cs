using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using IvyPdf;

namespace DroneWebApp.Models.SimpleFactoryPattern.Parsers
{
    public class PDFParser : IParser
    {
        public void Parse(string path, int flightId, DroneDBEntities db)
        {
            PdfParser p = new PdfParser(IvyDocumentReader.ReadPdf(@"C:\Users\p_kuk\Desktop\BP\Harelbeke-191210_report_Hightlighted.pdf"));
            QualityReport qrp = new QualityReport();

            //summary
            string Processed = p.Find("Processed").Right().Text;
            string CameraModelNames = p.Find("Camera Model Name(s)").Right().Text;
            string AverageGroundSamplingDistance = p.Find("Average Ground Sampling Distance (GSD)").Right().Text;
            string AreaCovered = p.Find("Area Covered").Right().Text + p.Right().Text + p.Right().Text;
            string TimeForInitialProcessing = p.Find("Time for Initial Processing (without report)").Right().Text;

            Console.WriteLine("Processed: " + Processed);
            qrp.Processed = Convert.ToDateTime(Processed);

            Console.WriteLine("CameraModelNames: " + CameraModelNames);
            Console.WriteLine("AverageGroundSamplingDistance: " + AverageGroundSamplingDistance);
            Console.WriteLine("AreaCovered: " + AreaCovered);
            Console.WriteLine("TimeForInitialProcessing: " + TimeForInitialProcessing);

            //quality check
            string Dataset = p.Find("Dataset").Right().Text;
            string CameraOptimization = p.Find("Camera Optimization").Right().Text;
            string Georeferencing = p.Find("Georeferencing").Right().Text;

            Console.WriteLine("Dataset: " + Dataset);
            Console.WriteLine("CameraOptimization: " + CameraOptimization);
            Console.WriteLine("Georeferencing: " + Georeferencing);

            //absolute camera position and orientation uncertainties 
            //todo omzetten naar float? 

            p.FilterPage(3); //voor de zekerheid dat hij op pagina 3 zit
            string AbsoluteMeanX = p.Find("Mean").Right().Text;
            string AbsoluteMeanY = p.Right().Text;
            string AbsoluteMeanZ = p.Right().Text;

            string AbsoluteSigmaX = p.Find("Sigma").Right().Text;
            string AbsoluteSigmaY = p.Right().Text;
            string AbsoluteSigmaZ = p.Right().Text;
            p.FilterClear();

            Console.WriteLine("0.008 ?: " + AbsoluteMeanX);
            Console.WriteLine("0.008 ?: " + AbsoluteMeanY);
            Console.WriteLine("0.014 ?: " + AbsoluteMeanZ);
            Console.WriteLine("0.001 ?: " + AbsoluteSigmaX);
            Console.WriteLine("0.001 ?: " + AbsoluteSigmaY);
            Console.WriteLine("0.001 ?: " + AbsoluteSigmaZ);

            //absolute camera position and orientation uncertainties 
            //todo omzetten naar float? 
            p.FilterPage(6);
            string RelativeMeanX = p.Find("Mean").Right().Text;
            string RelativeMeanY = p.Right().Text;
            string RelativeMeanZ = p.Right().Text;

            string RelativeSigmaX = p.Find("Sigma").Right().Text;
            string RelativeSigmaY = p.Right().Text;
            string RelativeSigmaZ = p.Right().Text;


            Console.WriteLine("RelativeMeanX 0.007: " + RelativeMeanX);
            Console.WriteLine("RelativeMeanY 0.006: " + RelativeMeanY);
            Console.WriteLine("RelativeMeanZ 0.008: " + RelativeMeanZ);

            Console.WriteLine("RelativeSigmaX 0.002: " + RelativeSigmaX);
            Console.WriteLine("RelativeSigmaY 0.002: " + RelativeSigmaY);
            Console.WriteLine("RelativeSigmaZ 0.004: " + RelativeSigmaZ);


            //Ground control points 
            string MeanErrorX = p.Find("Mean").Right().Text;
            string MeanErrorY = p.Right().Text;
            string MeanErrorZ = p.Right().Text;

            string SigmaErrorX = p.Find("Sigma").Right().Text;
            string SigmaErrorY = p.Right().Text;
            string SigmaErrorZ = p.Right().Text;

            string RMSErrorX = p.Find("RMS Error").Right().Text;
            string RMSErrorY = p.Right().Text;
            string RMSErrorZ = p.Right().Text;

            Console.WriteLine("MeanErrorX -0.000051: " + MeanErrorX);
            Console.WriteLine("MeanErrorY -0.000158: " + MeanErrorY);
            Console.WriteLine("MeanErrorZ -0.000181: " + MeanErrorZ);

            Console.WriteLine("SigmaErrorX 0.007058: " + SigmaErrorX);
            Console.WriteLine("SigmaErrorY 0.009429: " + SigmaErrorY);
            Console.WriteLine("SigmaErrorZ 0.005903: " + SigmaErrorZ);

            Console.WriteLine("RMSErrorX 0.007058: " + RMSErrorX);
            Console.WriteLine("RMSErrorY 0.009430: " + RMSErrorY);
            Console.WriteLine("RMSErrorZ 0.005906: " + RMSErrorZ);

            //absolute geolocation variance 
            string MeanGeolocationErrorX = p.Find("Mean").Right().Text;
            string MeanGeolocationErrorY = p.Right().Text;
            string MeanGeolocationErrorZ = p.Right().Text;

            string SigmaGeolocationErrorX = p.Find("Sigma").Right().Text;
            string SigmaGeolocationErrorY = p.Right().Text;
            string SigmaGeolocationErrorZ = p.Right().Text;

            string RMSGeolocationErrorX = p.Find("RMS Error").Right().Text;
            string RMSGeolocationErrorY = p.Right().Text;
            string RMSGeolocationErrorZ = p.Right().Text;
            p.FilterClear(); //eind pagina 6

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
            string Hardware = p.FilterPage(7).FilterWindow(133, 431, 539, 465).ExtractText();
            p.FilterClear();
            string OperatingSystem = p.Find("Operating System").Right().Text;

            Console.WriteLine("hardware: " + Hardware);
            Console.WriteLine("OperatingSystem: " + OperatingSystem);

            //coordinate systems 
            string ImageCoordinateSystem = p.Find("Image Coordinate System").Right().Text;
            string GroundControlPointCoordinateSystem = p.Find("Ground Control Point (GCP) Coordinate System").Right().Text;
            string OutputCoordinateSystem = p.Find("Output Coordinate System").Right().Text;

            Console.WriteLine("ImageCoordinateSystem: " + ImageCoordinateSystem);
            Console.WriteLine("GroundControlPointCoordinateSystem: " + GroundControlPointCoordinateSystem);
            Console.WriteLine("OutputCoordinateSystem: " + OutputCoordinateSystem);

            //results
            //omzetten naar int/floats? 
            string NumberOfGeneratedTiles = p.Find("Number of Generated Tiles").Right().Text;
            string NumberOf3DDensifiedPoints = p.Find("Number of 3D Densified Points").Right().Text;
            string AverageDensity = p.FindPattern("Average Density*").Right().Right().Right().Text;

            Console.WriteLine("NumberOfGeneratedTiles: " + NumberOfGeneratedTiles);
            Console.WriteLine("NumberOf3DDensifiedPoints: " + NumberOf3DDensifiedPoints);
            Console.WriteLine("AverageDensity: " + AverageDensity);


        }
    }
}