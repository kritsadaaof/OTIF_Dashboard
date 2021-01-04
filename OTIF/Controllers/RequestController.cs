using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using OTIF.Models;

namespace OTIF.Controllers
{
    public class RequestController : Controller
    {
        private Entities DbFile = new Entities();
        // GET: Request
        public ActionResult Requests()
        {
            return View();
        }
        [HttpPost]
        public string SaveRQ(string MACHIN, string BAR_KAN, int QTY, string USER,TimeSpan TIME_ACTUAL,float PROCESS_SPEED)
        {
            try
            { 
                var User = DbFile.Master_Member.Where(y => y.Mem_Name == USER || y.Code_Mem == USER).FirstOrDefault();
                var CheckBar = DbFile.OTIF_TR_Process.Where(y => y.Bar_Kan == BAR_KAN).Where(z => z.Machine.Equals(MACHIN)).FirstOrDefault();
                var CheckCal = DbFile.OTIF_TR_Cal.Where(y => y.Cal_Tag == BAR_KAN).Where(z => z.Cal_Machine.Equals(MACHIN)).FirstOrDefault();
               // var CheckMASPL = DbFile.Master_Planner.Where(y => y.PL_Tag == BAR_KAN).Where(z => z.PL_Machine.Equals(MACHIN)).FirstOrDefault();
                var CheckMASPL = DbFile.Master_Planner.Where(y => y.PL_Tag == BAR_KAN).Where(z => z.PL_Machine.Equals(MACHIN)).AsEnumerable().OrderByDescending(k => k.PL_Time).GroupBy(k => new { k.PL_Status }).Select(k => k.First()).OrderByDescending(k => k.PL_Status).FirstOrDefault();
                var TR_Pro = new OTIF_TR_Process();
                var TR_CAL = new OTIF_TR_Cal();
                if (CheckBar != null)//Edit
                {
                    CheckMASPL.PL_Status = "T";
                    CheckCal.Cal_QTY = QTY;
                    CheckCal.Cal_QTY_Balance= CheckMASPL.PL_Qty_T;
                    CheckCal.Cal_Update = DateTime.Now;
                    CheckCal.Cal_Update_time = TIME_ACTUAL;
                    CheckBar.Machine = MACHIN; 
                    CheckBar.QTY = QTY;
                    CheckBar.User = User.Code_Mem; 
                    CheckBar.Date_Actual = DateTime.Now;
                    CheckBar.Time_Actual = TIME_ACTUAL; 
                    CheckBar.Process_Speed = PROCESS_SPEED;
                    CheckBar.Status = "T";
                    CheckBar.SF_Remain_Qty = null;
                    CheckBar.TR_DateTime = DateTime.Now;
                    DbFile.SaveChanges();
                    return "S";
                }
                else //Add new
                {
                    CheckMASPL.PL_Status = "T";
                    TR_CAL.Cal_SO = CheckMASPL.PL_SO;
                    TR_CAL.Cal_Machine = CheckMASPL.PL_Machine;
                    TR_CAL.Cal_Tag = CheckMASPL.PL_Tag;
                    TR_CAL.Cal_Status = "IN";
                    TR_CAL.Cal_QTY = CheckMASPL.PL_Qty_T;
                    TR_CAL.Cal_QTY_Balance = CheckMASPL.PL_Qty_T;
                    TR_CAL.Cal_LVL = CheckMASPL.PL_Level;
                    TR_CAL.Cal_Create = DateTime.Now;
                    TR_CAL.Cal_Create_time = TIME_ACTUAL;
                    TR_Pro.Machine = MACHIN;
                    TR_Pro.Bar_Kan = BAR_KAN;
                    TR_Pro.QTY = QTY;
                    TR_Pro.User = User.Code_Mem; 
                    TR_Pro.Date_Actual = DateTime.Now; 
                    TR_Pro.Time_Actual = TIME_ACTUAL;
                    TR_Pro.Status = "T"; 
                    TR_Pro.Process_Speed = PROCESS_SPEED;
                    TR_Pro.Pro_SO = CheckMASPL.PL_SO;
                    TR_Pro.TR_DateTime = DateTime.Now;
                    DbFile.OTIF_TR_Cal.Add(TR_CAL);
                    DbFile.OTIF_TR_Process.Add(TR_Pro);
                    DbFile.SaveChanges();
                    return "S";
                }
            }
            catch { return "N"; }
        }
        public string CheckMachineKanban(string BAR_KAN,string MACHINE)
        {
            try
            {
                var data = (from Ms_Pl in DbFile.Master_Planner
                            where Ms_Pl.PL_Tag.Equals(BAR_KAN)&&Ms_Pl.PL_Machine.Equals(MACHINE)
                            select new
                            {
                                Ms_Pl.PL_Tag,
                                Ms_Pl.PL_Machine,
                                Ms_Pl.PL_Qty_T,
                                Ms_Pl.PL_Qty,
                                Ms_Pl.PL_Time,
                                Ms_Pl.PL_Status
                            }).AsEnumerable().OrderByDescending(k => k.PL_Time).GroupBy(k => new { k.PL_Status }).Select(k => k.First()).OrderByDescending(k=>k.PL_Status).ToList();
                string jsonlog = new JavaScriptSerializer().Serialize(data);
                return jsonlog;
            }
            catch { return null; }
        }//CheckLocation  //CheckUser
    }
}