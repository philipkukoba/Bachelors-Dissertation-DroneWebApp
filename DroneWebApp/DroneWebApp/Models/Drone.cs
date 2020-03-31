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
    using System.ComponentModel.DataAnnotations;

    public partial class Drone
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Drone()
        {
            this.DroneFlights = new HashSet<DroneFlight>();
        }
    
        public int DroneId { get; set; }
        [Required(ErrorMessage = "Registration is required")]
        public string Registration { get; set; }
        [Required(ErrorMessage = "DroneType is required")]
        public string DroneType { get; set; }
        public string DroneName { get; set; }
        public Nullable<System.TimeSpan> TotalFlightTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DroneFlight> DroneFlights { get; set; }
    }
}
