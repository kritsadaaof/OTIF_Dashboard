﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Master_Backlog> Master_Backlog { get; set; }
        public virtual DbSet<Master_Backlog_Temp> Master_Backlog_Temp { get; set; }
        public virtual DbSet<Master_Data> Master_Data { get; set; }
        public virtual DbSet<Master_Location> Master_Location { get; set; }
        public virtual DbSet<Master_Machine> Master_Machine { get; set; }
        public virtual DbSet<Master_Member> Master_Member { get; set; }
        public virtual DbSet<Master_Metal> Master_Metal { get; set; }
        public virtual DbSet<Master_Planner> Master_Planner { get; set; }
        public virtual DbSet<Master_Production> Master_Production { get; set; }
        public virtual DbSet<Master_Reason_Code> Master_Reason_Code { get; set; }
        public virtual DbSet<Master_Reel_Bom> Master_Reel_Bom { get; set; }
        public virtual DbSet<OTIF_TR_Cal> OTIF_TR_Cal { get; set; }
        public virtual DbSet<OTIF_TR_Machine_Monitor> OTIF_TR_Machine_Monitor { get; set; }
        public virtual DbSet<OTIF_TR_MoveLocation> OTIF_TR_MoveLocation { get; set; }
        public virtual DbSet<OTIF_TR_Packing> OTIF_TR_Packing { get; set; }
        public virtual DbSet<OTIF_TR_Packing_Log> OTIF_TR_Packing_Log { get; set; }
        public virtual DbSet<OTIF_TR_Process> OTIF_TR_Process { get; set; }
        public virtual DbSet<OTIF_TR_Process_Log> OTIF_TR_Process_Log { get; set; }
        public virtual DbSet<OTIF_TR_ShopFloor> OTIF_TR_ShopFloor { get; set; }
        public virtual DbSet<OTIF_TR_Planner> OTIF_TR_Planner { get; set; }
        public virtual DbSet<TEST> TEST { get; set; }
    }
}
