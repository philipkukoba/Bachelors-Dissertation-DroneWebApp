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
    
    public partial class DroneLog
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DroneLog()
        {
            this.DroneGPS = new HashSet<DroneGP>();
            this.DroneIMU_ATTI = new HashSet<DroneIMU_ATTI>();
            this.DroneMotors = new HashSet<DroneMotor>();
            this.DroneOAs = new HashSet<DroneOA>();
            this.DroneRCs = new HashSet<DroneRC>();
            this.DroneRTKDatas = new HashSet<DroneRTKData>();
        }
    
        public int DroneLogId { get; set; }
        public Nullable<long> Tick_no { get; set; }
        public Nullable<double> OffsetTime { get; set; }
        public Nullable<int> FlightTime { get; set; }
        public Nullable<int> NavHealth { get; set; }
        public Nullable<double> GeneralRelHeight { get; set; }
        public string FlyCState { get; set; }
        public string ControllerCTRLMode { get; set; }
        public string BatteryStatus { get; set; }
        public Nullable<int> SmartBattGoHome { get; set; }
        public Nullable<int> SmartBattLand { get; set; }
        public string NonGPSCause { get; set; }
        public string CompassError { get; set; }
        public string ConnectedToRC { get; set; }
        public string BatteryLowVoltage { get; set; }
        public string GPSUsed { get; set; }
        public int FlightId { get; set; }
    
        public virtual DroneFlight DroneFlight { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DroneGP> DroneGPS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DroneIMU_ATTI> DroneIMU_ATTI { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DroneMotor> DroneMotors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DroneOA> DroneOAs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DroneRC> DroneRCs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DroneRTKData> DroneRTKDatas { get; set; }
    }
}
