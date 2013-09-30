//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ECTunes.Database.ConfigLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class Car
    {
        public Car()
        {
            this.Param = new HashSet<Param>();
            this.Signal = new HashSet<Signal>();
        }
    
        public int customerId { get; set; }
        public int carId { get; set; }
        public string name { get; set; }
        public System.DateTime version { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Param> Param { get; set; }
        public virtual ICollection<Signal> Signal { get; set; }
    }
}