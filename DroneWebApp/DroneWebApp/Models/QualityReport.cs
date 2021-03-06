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
    
    public partial class QualityReport
    {
        public int QualityReportId { get; set; }
        public Nullable<System.DateTime> Processed { get; set; }
        public string CameraModelName { get; set; }
        public Nullable<double> AverageGSD { get; set; }
        public Nullable<double> AreaCovered { get; set; }
        public Nullable<System.TimeSpan> InitialProcessingTime { get; set; }
        public string Dataset { get; set; }
        public Nullable<int> DatasetAmountCalibrated { get; set; }
        public Nullable<int> DatasetAmountTotal { get; set; }
        public string DatasetStatus { get; set; }
        public string CameraOptimization { get; set; }
        public Nullable<double> CameraOptimizationAmount { get; set; }
        public string CameraOptimizationStatus { get; set; }
        public string Georeferencing { get; set; }
        public Nullable<int> GeoreferencingGCPs { get; set; }
        public Nullable<double> GeoreferencingRMS { get; set; }
        public string GeoreferencingStatus { get; set; }
        public string CPU { get; set; }
        public Nullable<int> RAM { get; set; }
        public string GPU { get; set; }
        public string OS { get; set; }
        public string ImageCoordinateSystem { get; set; }
        public string GCPCoordinateSystem { get; set; }
        public string OutputCoordinateSystem { get; set; }
        public Nullable<int> GeneratedTiles { get; set; }
        public Nullable<int> DensifiedPoints3D { get; set; }
        public Nullable<double> AverageDensity { get; set; }
    
        public virtual AbsoluteGeolocationVariance AbsoluteGeolocationVariance { get; set; }
        public virtual DroneFlight DroneFlight { get; set; }
        public virtual GCPError GCPError { get; set; }
        public virtual Uncertainty Uncertainty { get; set; }
    }
}
