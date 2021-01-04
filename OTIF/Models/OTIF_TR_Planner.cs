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
    
    public partial class OTIF_TR_Planner
    {
        public int ID { get; set; }
        public string B_SO { get; set; }
        public string B_PO { get; set; }
        public string B_Customer { get; set; }
        public string B_Material { get; set; }
        public string B_Material_Des { get; set; }
        public string Date_Create { get; set; }
        public string Date_PFD { get; set; }
        public string Date_Delivery { get; set; }
        public string B_Cutting_Length { get; set; }
        public Nullable<double> B_Order_QTY { get; set; }
        public string B_Order_Unit { get; set; }
        public Nullable<System.DateTime> Date_Metal { get; set; }
        public Nullable<System.TimeSpan> Time_Metal { get; set; }
        public Nullable<System.DateTime> Date_Compound { get; set; }
        public Nullable<System.TimeSpan> Time_Compound { get; set; }
        public Nullable<System.DateTime> Date_Oh_RM { get; set; }
        public Nullable<System.TimeSpan> Time_Oh_RM { get; set; }
        public Nullable<System.DateTime> Date_CutL { get; set; }
        public Nullable<System.TimeSpan> Time_CutL { get; set; }
        public Nullable<System.DateTime> Date_Mg { get; set; }
        public Nullable<System.TimeSpan> Time_Mg { get; set; }
        public Nullable<System.DateTime> Date_Plan { get; set; }
        public Nullable<System.TimeSpan> Time_Plan { get; set; }
        public Nullable<System.DateTime> Date_Actual { get; set; }
        public Nullable<System.TimeSpan> Time_Actual { get; set; }
        public string Move_Status { get; set; }
        public string Location { get; set; }
        public string User { get; set; }
    }
}