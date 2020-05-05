//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DroneWebApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class RawImage
    {
        public int RawImageKey { get; set; }
        public string FileName { get; set; }
        public byte[] RawData { get; set; }
        public byte[] RawDataDownsized { get; set; }
        public string ExposureTime { get; set; }
        public Nullable<int> FNumber { get; set; }
        public Nullable<int> Iso { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string ShutterSpeedValue { get; set; }
        public string ApertureValue { get; set; }
        public Nullable<int> ExposureCompensation { get; set; }
        public string MaxApertureValue { get; set; }
        public Nullable<double> SpeedX { get; set; }
        public Nullable<double> SpeedY { get; set; }
        public Nullable<double> SpeedZ { get; set; }
        public Nullable<double> Pitch { get; set; }
        public Nullable<double> Yaw { get; set; }
        public Nullable<double> Roll { get; set; }
        public Nullable<double> CameraPitch { get; set; }
        public Nullable<double> CameraYaw { get; set; }
        public Nullable<double> CameraRoll { get; set; }
        public Nullable<double> AbsoluteAltitude { get; set; }
        public Nullable<double> RelativeAltitude { get; set; }
        public Nullable<int> RtkFlag { get; set; }
        public string GpsAltitude { get; set; }
        public string GpsLatitude { get; set; }
        public string GpsLongitude { get; set; }
        public string GpsPosition { get; set; }
        public string GPSLatRef { get; set; }
        public string GPSLongRef { get; set; }
        public string GPSAltitudeRef { get; set; }
        public int FlightId { get; set; }
    
        public virtual DroneFlight DroneFlight { get; set; }
    }
}
