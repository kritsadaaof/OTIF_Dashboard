//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OTIF.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OTIF_PD_Process_Actual
    {
        public int ID { get; set; }
        public string SO { get; set; }
        public Nullable<int> QTY { get; set; }
        public Nullable<int> RT_QTY { get; set; }
        public string PL_Tag { get; set; }
        public string Machine { get; set; }
        public Nullable<System.DateTime> DateActual { get; set; }
        public string Material { get; set; }
        public string Process { get; set; }
        public string MasterProcess { get; set; }
        public string PL_Status { get; set; }
    }
}
