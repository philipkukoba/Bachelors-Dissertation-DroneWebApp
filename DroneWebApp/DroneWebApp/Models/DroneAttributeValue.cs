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
    
    public partial class DroneAttributeValue
    {
        public int AttributeValueId { get; set; }
        public string ACType { get; set; }
        public Nullable<System.DateTime> FirmwareDate { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public Nullable<int> BatterySN { get; set; }
        public Nullable<double> GeoDeclination { get; set; }
        public Nullable<double> GeoInclination { get; set; }
        public Nullable<double> GeoIntensity { get; set; }
    
        public virtual DroneFlight DroneFlight { get; set; }
    }
}
