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
    
    public partial class AbsoluteGeolocationVariance
    {
        public int AbsoluteGeolocationVarianceId { get; set; }
        public Nullable<double> AGVMeanError_x { get; set; }
        public Nullable<double> AGVMeanError_y { get; set; }
        public Nullable<double> AGVMeanError_z { get; set; }
        public Nullable<double> AGVSigma_x { get; set; }
        public Nullable<double> AGVSigma_y { get; set; }
        public Nullable<double> AGVSigma_z { get; set; }
        public Nullable<double> AGVRMS_x { get; set; }
        public Nullable<double> AGVRMS_y { get; set; }
        public Nullable<double> AGVRMS_z { get; set; }
        public Nullable<int> QualityReportId { get; set; }
    
        public virtual QualityReport QualityReport { get; set; }
    }
}
