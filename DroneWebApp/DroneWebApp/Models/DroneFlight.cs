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
            this.CTRLPoints = new HashSet<CTRLPoint>();
            this.DroneLogEntries = new HashSet<DroneLogEntry>();
            this.GroundControlPoints = new HashSet<GroundControlPoint>();
            this.PointCloudXYZs = new HashSet<PointCloudXYZ>();
            this.RawImages = new HashSet<RawImage>();
        }
    
        public int FlightId { get; set; }
        public int DroneId { get; set; }
        public int ProjectId { get; set; }
        public int PilotId { get; set; }
        public string Location { get; set; }
        public System.DateTime Date { get; set; }
        public bool hasTFW { get; set; }
        public bool hasGCPs { get; set; }
        public bool hasCTRLs { get; set; }
        public bool hasDepInfo { get; set; }
        public bool hasDestInfo { get; set; }
        public bool hasQR { get; set; }
        public bool hasXYZ { get; set; }
        public bool hasDroneLog { get; set; }
        public bool hasRawImages { get; set; }
        public string TypeOfActivity { get; set; }
        public string Other { get; set; }
        public string Simulator { get; set; }
        public string Instructor { get; set; }
        public string Remarks { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTRLPoint> CTRLPoints { get; set; }
        public virtual DepartureInfo DepartureInfo { get; set; }
        public virtual DestinationInfo DestinationInfo { get; set; }
        public virtual Drone Drone { get; set; }
        public virtual DroneAttributeValue DroneAttributeValue { get; set; }
        public virtual Pilot Pilot { get; set; }
        public virtual Project Project { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DroneLogEntry> DroneLogEntries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroundControlPoint> GroundControlPoints { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PointCloudXYZ> PointCloudXYZs { get; set; }
        public virtual QualityReport QualityReport { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RawImage> RawImages { get; set; }
        public virtual TFW TFW { get; set; }
    }
}
