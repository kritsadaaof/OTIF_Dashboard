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
    
    public partial class Master_Planner
    {
        public int ID { get; set; }
        public string PL_SO { get; set; }
        public string PL_Machine { get; set; }
        public string PL_Process { get; set; }
        public string PL_Type { get; set; }
        public Nullable<double> PL_Qty { get; set; }
        public string PL_Tag { get; set; }
        public Nullable<double> PL_Qty_T { get; set; }
        public string PL_Color { get; set; }
        public Nullable<int> PL_Level { get; set; }
        public Nullable<System.DateTime> PL_Date { get; set; }
        public string PL_Reel_Size { get; set; }
        public Nullable<System.DateTime> PL_Creation_Date { get; set; }
        public Nullable<System.DateTime> PL_Modification_Date { get; set; }
        public string PL_Status { get; set; }
        public string PL_Process_Status { get; set; }
        public Nullable<System.DateTime> PL_Time { get; set; }
        public string PL_QTY_Cus { get; set; }
        public string PL_Shop_Packing { get; set; }
        public string PL_User_Change { get; set; }
    }
}