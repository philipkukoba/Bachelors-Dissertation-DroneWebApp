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
    
    public partial class DroneFlight
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DroneFlight()
        {
            this.DepartureInfoes = new HashSet<DepartureInfo>();
            this.DestinationInfoes = new HashSet<DestinationInfo>();
            this.GroundControlPoints = new HashSet<GroundControlPoint>();
            this.PointCloudXYZs = new HashSet<PointCloudXYZ>();
            this.QualityReports = new HashSet<QualityReport>();
            this.RawImages = new HashSet<RawImage>();
            this.TFWs = new HashSet<TFW>();
        }
    
        public int FlightId { get; set; }
        public int DroneId { get; set; }
        public string Location { get; set; }
        public System.DateTime Date { get; set; }
        public string PilotName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DepartureInfo> DepartureInfoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DestinationInfo> DestinationInfoes { get; set; }
        public virtual Drone Drone { get; set; }
        public virtual Pilot Pilot { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroundControlPoint> GroundControlPoints { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PointCloudXYZ> PointCloudXYZs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QualityReport> QualityReports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RawImage> RawImages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TFW> TFWs { get; set; }
    }
}
