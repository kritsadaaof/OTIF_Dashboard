using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.Ajax.Utilities;
using OTIF.Models;

namespace OTIF.Controllers
{
    public class ClassController : Controller
    {
        private Entities DbFile = new Entities();
        
        // GET: Class
        public ActionResult Index()
        {
            return View();
        }

        public string CheckMACH(string MACH)
        {
            try
            {
                var data = (from Ms_MC in DbFile.Master_Machine
                            where Ms_MC.MC_Code.Equals(MACH)|| Ms_MC.MC_Machine.Equals(MACH)
                            select new
                            {
                                Ms_MC.MC_Machine
                            }).ToList();
                string jsonlog = new JavaScriptSerializer().Serialize(data);
                return jsonlog;
            }
            catch { return null; }
        }//CheckLocation  //CheckUser
        public string CheckUser(string USER)
        {
            try
            {
                var data = (from Ms_M in DbFile.Master_Member
                            where Ms_M.Code_Mem.Equals(USER)
                            select new
                            {
                                Ms_M.Mem_Name
                            }).ToList();
                string jsonlog = new JavaScriptSerializer().Serialize(data);
                return jsonlog;
            }
            catch { return null; }
        }//CheckLocation  //CheckUser
        public string CheckBar(string BAR)
        {
            try
            {
                var data = (from TR_Pro in DbFile.OTIF_TR_Process
                            where TR_Pro.Bar_Kan.Equals(BAR)
                            select new
                            {
                                TR_Pro.QTY
                            }).ToList();
                string jsonlog = new JavaScriptSerializer().Serialize(data);
                return jsonlog;
            }
            catch { return null; }
        } //CheckLoc

        public string CheckLoc(string LOC)
        {
            try
            {
                var data = (from MS_L in DbFile.Master_Location
                            where (MS_L.Lo_Code.Equals(LOC) && MS_L.Lo_Status == "T")||(MS_L.Lo_Name.Equals(LOC) && MS_L.Lo_Status == "T")
                            select new
                            {
                                MS_L.Lo_Name,
                                MS_L.Lo_Des
                            }).ToList();
                string jsonlog = new JavaScriptSerializer().Serialize(data);
                return jsonlog;
            }
            catch { return null; }
        } //CheckLoc

        public string CheckSO(string SO)
        {
            try
            {

                var data = DbFile.Master_Backlog.Where(a => a.B_SO.Equals(SO)).Take(1).ToList();


                string jsonlog = new JavaScriptSerializer().Serialize(data);
                return jsonlog;
            }
            catch { return null; }
        }
        public string Calculator(string SO, string MAC)
        {//เพื่อคำนวนหา QTY Balance ของ Process TAG IN
            try
            {
                var dataOUT = DbFile.OTIF_TR_Cal.Where(a => a.Cal_SO.Equals(SO)).Where(b => b.Cal_Machine.Equals(MAC)).Where(c => c.Cal_Status.Equals("OUT")).ToList();
                var SUM_OUT = dataOUT.Sum(a => a.Cal_QTY_Balance);
                var result = DbFile.OTIF_TR_Cal.Where(a => a.Cal_SO.Equals(SO)).Where(b => b.Cal_Machine.Equals(MAC)).Where(c => c.Cal_Status.Equals("IN")).ToList();
                result.ForEach(x => x.Cal_QTY_Balance = (x.Cal_QTY - SUM_OUT));
                DbFile.SaveChanges();
                return null;
            }
            catch { return null; }
        } //CheckLoc

        public string BackLogTOMasterData()
        {//โยกข้อมูลจาก Backlog ==> MasterData
          //  try
         //   {
                /*    var UpdateMSData = (from MS_B in DbFile.Master_Backlog.DefaultIfEmpty()
                                        join MD_D1 in DbFile.Master_Data on MS_B.B_SO equals MD_D1.B_SO into MDs
                                        from MD_D in MDs
                                        select new
                                        {
                                            MS_B.B_SO,
                                            D_SO = MD_D.B_SO
                                        }).ToList();
                */
             //   var filteredSO = DbFile.Master_Backlog.Where(a => !DbFile.Master_Data.Select(b => b.B_SO).Contains(a.B_SO)).ToList();
                //var filteredSO = DbFile.Master_Backlog.ToList();

                 /* var  filteredSO = (from a in DbFile.Master_Backlog
                                    where !DbFile.Master_Data.Any(b => b.B_SO == a.B_SO)
                                    select a).ToList();
               // */
                List<Master_Backlog> MSB = DbFile.Master_Backlog.Where(a => !DbFile.Master_Data.Select(b => b.B_SO).Contains(a.B_SO)).ToList();
            var data1 = new Master_Data();
              
                foreach (var MSBs in MSB)
                { 
                    data1.B_SO = MSBs.B_SO;
                    data1.B_PO = MSBs.B_PO;
                    data1.B_Customer = MSBs.B_Customer;
                    data1.B_Material = MSBs.B_Material;
                    data1.B_Material_Des = MSBs.B_Material_Des;
                 //   data1.Date_Create = MSBs.Date_Create;
                 //   data1.Date_PFD = MSBs.Date_PFD;
                  //  data1.Date_Delivery = MSBs.Date_Delivery;
                    data1.B_Cutting_Length = MSBs.B_Cutting_Length;
                    data1.B_Order_QTY = MSBs.B_Order_QTY;
                    data1.B_Order_Unit = MSBs.B_Order_Unit;
                    DbFile.Master_Data.Add(data1); 
                DbFile.SaveChanges();
            }
                
                //  DbFile.Master_Data.Add();


                /*
                                var Data = new List<Master_Data>();
                                for (int i = 0; i < resultList.Count; i++)
                                {
                                    Data.Add(new Master_Data
                                    { 

                                    });
                                    // Data.B_SO = resultList.,
                                    //  Field_Value_EN = product.ListProductFields[i].Field_Value_EN 
                                };
                */
                // DbFile.Master_Data.Add(resultList); 
                //  var SUM_OUT = dataOUT.Sum(a => a.Cal_QTY_Balance);
                // var result = DbFile.OTIF_TR_Cal.Where(a => a.Cal_SO.Equals(SO)).Where(b => b.Cal_Machine.Equals(MAC)).Where(c => c.Cal_Status.Equals("IN")).ToList();
                // result.ForEach(x => x.Cal_QTY_Balance = (x.Cal_QTY - SUM_OUT));
                // DbFile.SaveChanges();
                return null;
          //  }
          //  catch { return null; }
        } //CheckLoc
    }
}