using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using OTIF.Models;


namespace OTIF.Controllers
{
    public class PlannerController : Controller
    {
        private Entities DbFile = new Entities();
        // GET: Planner
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Planner()
        {
            return View();
        }
        public ActionResult ChangePlan()
        {
            ViewBag.Machine = DbFile.Master_Machine.OrderBy(a=>a.MC_Machine).ToList();
            return View();
        }
        public string CheckMachine(string BAR_KAN)
        {
            try
            {
                var data = (from Ms_Pl in DbFile.Master_Planner
                            where Ms_Pl.PL_Tag.Equals(BAR_KAN)// && Ms_Pl.PL_Machine.Equals(MACHINE)
                            select new
                            {
                                Ms_Pl.PL_Tag,
                                Ms_Pl.PL_Machine,
                                Ms_Pl.PL_Qty_T,
                                Ms_Pl.PL_Qty,
                                Ms_Pl.PL_Time,
                                Ms_Pl.PL_Status
                            }).AsEnumerable().OrderByDescending(k => k.PL_Time).GroupBy(k => new { k.PL_Machine }).Select(k => k.First()).OrderByDescending(k => k.PL_Time).ToList();
                string jsonlog = new JavaScriptSerializer().Serialize(data);
                return jsonlog;
            }
            catch { return null; }
        }//CheckLocation  //CheckUser
        [HttpPost]
        public string SaveChangePlan(string TAG, string MACHIN_O, string MACHIN_N,string USER)
        {
            try
            {
                var User = DbFile.Master_Member.Where(y => y.Mem_Name == USER || y.Code_Mem == USER).FirstOrDefault();
                var CheckMASPL = DbFile.Master_Planner.Where(y => y.PL_Tag == TAG).Where(z => z.PL_Machine.Equals(MACHIN_O)).AsEnumerable().OrderByDescending(k => k.PL_Time).GroupBy(k => new { k.PL_Status }).Select(k => k.First()).OrderByDescending(k => k.PL_Status).FirstOrDefault();
                CheckMASPL.PL_Machine = MACHIN_N;
                CheckMASPL.PL_User_Change = User.Code_Mem;
                DbFile.SaveChanges();
                return "S";
            }
            catch {

                return null;
            }
        }

        [HttpPost]
        public string SaveData(string SO, int QTY, string USER, string Date_Metal, string Time_Metal, string Date_Compound, string Time_Compound, string Date_Oh_RM, string Time_Oh_RM, string Date_CutL, string CUTL, string Time_CutL, string Date_Mg, string Time_Mg, string DATE_PLAN, string TIME_PLAN, TimeSpan TIME_ACTUAL)
        {
            try
            {

                var User = DbFile.Master_Member.Where(y => y.Mem_Name == USER || y.Code_Mem == USER).FirstOrDefault();
                var CheckSO = DbFile.OTIF_TR_Planner.Where(y => y.B_SO == SO).FirstOrDefault();
                var MasterSO = DbFile.Master_Backlog.Where(y => y.B_SO == SO).FirstOrDefault();
                var TR_Plan = new OTIF_TR_Planner(); 
                TR_Plan.B_SO = SO;
                TR_Plan.B_PO = MasterSO.B_PO;
                TR_Plan.B_Customer = MasterSO.B_Customer;
                TR_Plan.User = User.Code_Mem;
                TR_Plan.B_Material = MasterSO.B_Material;
                TR_Plan.B_Material_Des = MasterSO.B_Material_Des;

              //  TR_Plan.Date_Create = MasterSO.Date_Create;
               // TR_Plan.Date_PFD = MasterSO.Date_PFD;
              //  TR_Plan.Date_Delivery = MasterSO.Date_Delivery;
                TR_Plan.B_Order_QTY = QTY;
                TR_Plan.B_Order_Unit = MasterSO.B_Order_Unit;
                if (DATE_PLAN != "" && TIME_PLAN != "")
                {
                    DateTime DATE_PLANs = Convert.ToDateTime(DATE_PLAN);
                    TimeSpan TIME_PLANs = TimeSpan.Parse(DATE_PLAN);
                    TR_Plan.Date_Plan = DATE_PLANs;
                    TR_Plan.Time_Plan = TIME_PLANs;
                }

                if (Date_Metal != "" && Time_Metal != "")
                {
                    DateTime Date_Metals = Convert.ToDateTime(Date_Metal);
                    TimeSpan Time_Metals = TimeSpan.Parse(Time_Metal);
                    TR_Plan.Date_Metal = Date_Metals;
                    TR_Plan.Time_Metal = Time_Metals;
                }
                if (Date_Compound != "" && Time_Compound != "")
                {
                    DateTime Date_Compounds = Convert.ToDateTime(Date_Compound);
                    TimeSpan Time_Compounds = TimeSpan.Parse(Time_Compound);
                    TR_Plan.Date_Compound = Date_Compounds;
                    TR_Plan.Time_Compound = Time_Compounds;
                }

                if (Date_Oh_RM != "" && Time_Oh_RM != "")
                {
                    DateTime Date_Oh_RMs = Convert.ToDateTime(Date_Oh_RM);
                    TimeSpan Time_Oh_RMs = TimeSpan.Parse(Time_Oh_RM);
                    TR_Plan.Date_Oh_RM = Date_Oh_RMs;
                    TR_Plan.Time_Oh_RM = Time_Oh_RMs;
                }

                if (Date_CutL != "" && Time_CutL != "")
                {
                    DateTime Date_CutLs = Convert.ToDateTime(Date_CutL);
                    TimeSpan Time_CutLs = TimeSpan.Parse(Time_CutL);
                    TR_Plan.Date_CutL = Date_CutLs;
                    TR_Plan.Time_CutL = Time_CutLs;
                    if (CUTL != "")
                    {
                        TR_Plan.B_Cutting_Length = CUTL;
                    }
                    else {
                        TR_Plan.B_Cutting_Length = MasterSO.B_Cutting_Length;
                    }
                }
                else
                {
                    TR_Plan.B_Cutting_Length = MasterSO.B_Cutting_Length;
                }
                if (Date_Mg != "" && Time_Mg != "")
                {
                    DateTime Date_Mgs = Convert.ToDateTime(Date_Mg);
                    TimeSpan Time_Mgs = TimeSpan.Parse(Time_Mg);
                    TR_Plan.Date_Mg = Date_Mgs;
                    TR_Plan.Time_Mg = Time_Mgs;
                }
                TR_Plan.Date_Actual = DateTime.Now;
                //   TR_Pro.Time_Actual = TimeSpan.Parse(DateTime.Now.ToString("hh:mm:ss"));
                TR_Plan.Time_Actual = TIME_ACTUAL;
                //   TR_Plan.Status = "T";
               
                DbFile.OTIF_TR_Planner.Add(TR_Plan);
                DbFile.SaveChanges();
                return "S";
                //   }
            }
            catch { return "N"; }
        }
    }
}