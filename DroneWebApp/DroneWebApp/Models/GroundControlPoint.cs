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
    
    public partial class GroundControlPoint
    {
        public int GCPId { get; set; }
        public string GCPName { get; set; }
        public Nullable<double> X { get; set; }
        public Nullable<double> Y { get; set; }
        public Nullable<double> Z { get; set; }
        public Nullable<int> FlightId { get; set; }
    
        public virtual DroneFlight DroneFlight { get; set; }
    }
}
