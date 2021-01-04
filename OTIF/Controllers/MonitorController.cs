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
    public class MonitorController : Controller
    {
        private Entities DbFile = new Entities();
        // GET: Monitor
        public ActionResult MonitorMach()
        {
            return View();
        }

        public ActionResult MonitorTV1()
        {
            ViewBag.MS_Machine = DbFile.Master_Machine.ToList().GetRange(0,9);
            return View();
        }

        public ActionResult MonitorTV2()
        {
            ViewBag.MS_Machine = DbFile.Master_Machine.ToList().GetRange(9, 9);
            return View();
        }
        public ActionResult MonitorTV3()
        {
            ViewBag.MS_Machine = DbFile.Master_Machine.ToList().GetRange(18, 9);
            return View();
        }
        public ActionResult MonitorTV4()
        {
            ViewBag.MS_Machine = DbFile.Master_Machine.ToList().GetRange(27, 9);
            return View();
        }
        public string CheckMach(string MACH)
        {
            try
           {               
                var dt =DateTime.UtcNow.Date; 
            var data = (from TR_Process in DbFile.OTIF_TR_Process
                            join MSBs in DbFile.Master_Backlog on TR_Process.Pro_SO equals MSBs.B_SO into MSB1
                            from MSB in MSB1.DefaultIfEmpty()
                           // join MSMAPLs in DbFile.Master_Planner.Where(y => y.PL_Tag == TR_Process.Bar_Kan).AsEnumerable().OrderByDescending(k => k.PL_Time).GroupBy(k => new { k.PL_Status }).Select(k => k.First()).OrderByDescending(k => k.PL_Status).FirstOrDefault()
                            where TR_Process.Machine.Equals(MACH) && TR_Process.Date_Actual == dt && TR_Process.RT_QTY!=null
                            select new
                            {
                                TR_Process.ID,
                                TR_Process.Bar_Kan,
                                TR_Process.Machine,
                                TR_Process.QTY,
                                TR_Process.RT_QTY,
                                TR_Process.Date_Actual,
                                TR_Process.Time_Actual,
                                TR_Process.TR_DateTime,
                                TR_Process.Process_Speed,
                                TR_Process.Pro_SO,
                                TR_Process.Stop_Process,
                                TR_Process.User,
                               // CheckMASPL.PL_Type,
                                CheckMASPL="",
                                MSB.B_Customer,
                                MSB.B_Order_QTY,
                                MSB.B_Order_Unit,
                                CalTotalQTYs=""
                            }).AsEnumerable().Select(x=> new 
                            {
                                ID=x.ID,
                                Bar_Kan = x.Bar_Kan,
                                Machine=x.Machine,
                                QTY=x.QTY,
                                RT_QTY=x.RT_QTY,
                                Date_Actual=x.Date_Actual,
                                Time_Actual=x.Time_Actual,
                                TR_DateTime=x.TR_DateTime,
                                Process_Speed=x.Process_Speed,
                                Pro_SO=x.Pro_SO,
                                Stop_Process=x.Stop_Process,
                                User=x.User,
                                // CheckMASPL.PL_Type,
                                PL_Type = MAT(x.Bar_Kan).ToString(),
                                B_Customer=x.B_Customer,
                                B_Order_QTY=x.B_Order_QTY,
                                B_Order_Unit=x.B_Order_Unit,
                                CalTotalQTYs= CalTotalQTY(x.Pro_SO)
                            }).OrderBy(c=>c.ID).ToList();
                string jsonlog = new JavaScriptSerializer().Serialize(data);
                return jsonlog;
            }
            catch { return null; }
        }//CheckLocation  //CheckUser

        public string CalTotalQTY(string SO)
        {
            var data = (from TR_Process in DbFile.OTIF_TR_Process
                        join MSPLs in DbFile.Master_Planner on TR_Process.Bar_Kan equals MSPLs.PL_Tag into MSB1
                        from MSPL in MSB1.DefaultIfEmpty()
                        where TR_Process.Pro_SO.Equals(SO)&&MSPL.PL_Status.Equals("F")&&MSPL.PL_Process_Status!="No"
                        select new
                        {
                            TR_Process.RT_QTY                        
                        }).ToList();
            var SUM = data.Sum(a => a.RT_QTY);
            return SUM.ToString();
        }
        public string MAT(string TAG)
        {
            var CheckMASPLs = DbFile.Master_Planner.Where(z => z.PL_Tag.Equals(TAG)).AsEnumerable().OrderByDescending(k => k.PL_Time).GroupBy(k => new { k.PL_Status }).Select(k => k.First()).OrderByDescending(k => k.PL_Status).FirstOrDefault();

            return CheckMASPLs.PL_Type;
        }
       
        public string Machine(int START,int END)
        {
            try
            { 
                var data = (from MS_Machine in DbFile.Master_Machine
                           // where MS_Machine.MC_Process.Equals("Y")
                            select new
                            {
                                MS_Machine.MC_Machine 
                            }).ToList().GetRange(START, END);
                string jsonlog = new JavaScriptSerializer().Serialize(data);
                return jsonlog;
            }
            catch { return null; }
        }//CheckLocation  //CheckUser
    }
}