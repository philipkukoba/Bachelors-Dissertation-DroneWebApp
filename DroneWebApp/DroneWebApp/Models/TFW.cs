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
    
    public partial class TFW
    {
        public int TFWId { get; set; }
        public Nullable<double> xScale_X { get; set; }
        public Nullable<double> yRotationTerm_X { get; set; }
        public Nullable<double> TranslationTerm_X { get; set; }
        public Nullable<double> xRotationTerm_Y { get; set; }
        public Nullable<double> yNegativeScale_Y { get; set; }
        public Nullable<double> TranslationTerm_Y { get; set; }
        public Nullable<int> FlightId { get; set; }
    
        public virtual DroneFlight DroneFlight { get; set; }
    }
}
