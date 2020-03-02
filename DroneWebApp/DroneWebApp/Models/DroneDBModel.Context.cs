﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DroneDBEntities : DbContext
    {
        public DroneDBEntities()
            : base("name=DroneDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AbsoluteGeoreferencingVariance> AbsoluteGeoreferencingVariances { get; set; }
        public virtual DbSet<AbsoluteUncertainty> AbsoluteUncertainties { get; set; }
        public virtual DbSet<DepartureInfo> DepartureInfoes { get; set; }
        public virtual DbSet<DestinationInfo> DestinationInfoes { get; set; }
        public virtual DbSet<Drone> Drones { get; set; }
        public virtual DbSet<DroneFlight> DroneFlights { get; set; }
        public virtual DbSet<GCPError> GCPErrors { get; set; }
        public virtual DbSet<GroundControlPoint> GroundControlPoints { get; set; }
        public virtual DbSet<Pilot> Pilots { get; set; }
        public virtual DbSet<PointCloudXYZ> PointCloudXYZs { get; set; }
        public virtual DbSet<QualityReport> QualityReports { get; set; }
        public virtual DbSet<RawImage> RawImages { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TFW> TFWs { get; set; }
        public virtual DbSet<Uncertainty> Uncertainties { get; set; }
        public virtual DbSet<DroneAttributeValue> DroneAttributeValues { get; set; }
        public virtual DbSet<DroneGP> DroneGPS { get; set; }
        public virtual DbSet<DroneIMU_ATTI> DroneIMU_ATTI { get; set; }
        public virtual DbSet<DroneLog> DroneLogs { get; set; }
        public virtual DbSet<DroneMotor> DroneMotors { get; set; }
        public virtual DbSet<DroneOA> DroneOAs { get; set; }
        public virtual DbSet<DroneRC> DroneRCs { get; set; }
        public virtual DbSet<DroneRTKData> DroneRTKDatas { get; set; }
    }
}
