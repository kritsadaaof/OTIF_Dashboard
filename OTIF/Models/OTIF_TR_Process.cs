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
    
    public partial class OTIF_TR_Process
    {
        public int ID { get; set; }
        public string Bar_Kan { get; set; }
        public string Machine { get; set; }
        public string User { get; set; }
        public Nullable<int> QTY { get; set; }
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
        public string Status { get; set; }
        public Nullable<System.DateTime> Date_Actual { get; set; }
        public Nullable<System.TimeSpan> Time_Actual { get; set; }
        public Nullable<System.DateTime> Date_Plan { get; set; }
        public Nullable<System.TimeSpan> Time_Plan { get; set; }
        public Nullable<int> SF_Remain_Qty { get; set; }
        public Nullable<int> RT_QTY { get; set; }
        public string Move_Status { get; set; }
        public string Location { get; set; }
        public Nullable<double> Process_Speed { get; set; }
        public Nullable<System.DateTime> Date_Return { get; set; }
        public Nullable<System.TimeSpan> Time_Return { get; set; }
        public string Pro_SO { get; set; }
        public string Stop_Process { get; set; }
        public Nullable<System.DateTime> TR_DateTime { get; set; }
    }
}
