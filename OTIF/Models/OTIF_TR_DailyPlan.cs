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
    
    public partial class OTIF_TR_DailyPlan
    {
        public string Con { get; set; }
        public string Resource_name { get; set; }
        public string Parent_Name { get; set; }
        public Nullable<double> O_Qty { get; set; }
        public string Item_name { get; set; }
        public string Cutting_length { get; set; }
        public Nullable<double> Prd_Qty { get; set; }
        public string Operation_code { get; set; }
        public Nullable<System.DateTime> Start_time { get; set; }
        public Nullable<System.DateTime> End_time { get; set; }
        public Nullable<int> Setup_time { get; set; }
        public Nullable<System.DateTime> Date_Create { get; set; }
        public int IDs { get; set; }
    }
}