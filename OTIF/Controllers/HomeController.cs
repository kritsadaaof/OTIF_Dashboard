using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using OTIF.Models;


namespace OTIF.Controllers
{
    public class HomeController : Controller
    {
        private Entities DbFile = new Entities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CheckByMachines()
        {
            ViewBag.Machines = DbFile.Master_Machine.OrderBy(a => a.MC_Machine).ToList();
            return View();
        }

        public string DataIndexMach(DateTime DATE)
        {
            // try
            //  {
            var dt = DateTime.UtcNow.Date;

            var data = (from MS_M in DbFile.Master_Machine
                        join TR_P1 in DbFile.OTIF_TR_Process on MS_M.MC_Machine equals TR_P1.Machine into TR_Ps
                        from TR_P in TR_Ps.DefaultIfEmpty()

                            //  where TR_P.Date_Actual == dt // && Ms_Pl.PL_Machine.Equals(MACHINE)
                        select new
                        {
                            MS_M.MC_Machine,
                            TR_P.Pro_SO,
                            TR_P.Bar_Kan,
                            TR_P.Date_Actual,
                            TR_P.TR_DateTime,
                            TR_P.Time_Actual,
                            TR_P.SF_Remain_Qty,
                            TR_P.RT_QTY,
                            TR_P.QTY,
                            TR_P.User,
                            TR_P.Stop_Process

                        }).AsEnumerable().OrderByDescending(k => k.TR_DateTime).GroupBy(k => new { k.MC_Machine }).Select(k => k.First()).Select(x => new
                        {
                            Machine = x.MC_Machine,
                            Pro_SO = x.Date_Actual == dt ? x.Pro_SO : "-",
                            PL_Type = x.Date_Actual == dt ? x.Bar_Kan != null ? MAT(x.Bar_Kan).ToString() : "-" : "-",
                            Bar_Kan = x.Date_Actual == dt ? x.Bar_Kan : "-",
                            SF_Remain_Qty = x.Date_Actual == dt ? x.SF_Remain_Qty : null,
                            RT_QTY = x.Date_Actual == dt ? x.RT_QTY : null,
                            QTY = x.QTY,
                            User = x.Date_Actual == dt ? CheckUser(x.User) : "-",
                            Date_Actual = x.Date_Actual,
                            // CalTotalQTYs = CalTotalQTY(x.Machine),
                            Stop_Process = x.Date_Actual == dt ? x.Stop_Process == "N" ? "No Plan" : x.Stop_Process == "T" ? "<i style='font-size:16px; color: red'>เครื่องมีปัญหา</i>" : x.SF_Remain_Qty.Equals(0) ? "<i style='font-size:16px; color: #ff6a00'>เครื่องจอด</i>" : "<i style='font-size:16px; color: green'>เครื่องเดิน</i>" : x.Stop_Process == "N" ? "<i style='font-size:16px; color: #333399'>No Plan</i>" : "-"


                        }).ToList();
            string jsonlog = new JavaScriptSerializer().Serialize(data);
            return jsonlog;
            //   }
            //  catch { return null; }
        }//CheckLocation  //CheckUser
        public string DataIndexMachs(DateTime DATE)
        {
            // try
            //  {
            var dt = DateTime.UtcNow.Date;
            var data = (from TR_P in DbFile.OTIF_TR_Process
                        where TR_P.Date_Actual == dt // && Ms_Pl.PL_Machine.Equals(MACHINE)
                        select new
                        {
                            TR_P.Machine,
                            TR_P.Pro_SO,
                            TR_P.Bar_Kan,
                            TR_P.Date_Actual,
                            TR_P.Time_Actual,
                            TR_P.SF_Remain_Qty,
                            TR_P.RT_QTY,
                            TR_P.QTY,
                            TR_P.Stop_Process

                        }).AsEnumerable().OrderByDescending(k => k.Time_Actual).GroupBy(k => new { k.Machine }).Select(k => k.First()).OrderByDescending(k => k.Time_Actual).Select(x => new
                        {
                            Machine = x.Machine,
                            Pro_SO = x.Pro_SO,
                            PL_Type = MAT(x.Bar_Kan).ToString(),
                            Bar_Kan = x.Bar_Kan,
                            SF_Remain_Qty = x.SF_Remain_Qty,
                            RT_QTY = x.RT_QTY,
                            QTY = x.QTY,
                            // CalTotalQTYs = CalTotalQTY(x.Machine),
                            Stop_Process = x.Stop_Process == "T" ? "<i style='font-size:16px; color: red'>เครื่องมีปัญหา</i>" : x.SF_Remain_Qty.Equals(0) ? "<i style='font-size:16px; color: #ff6a00'>เครื่องจอด</i>" : "<i style='font-size:16px; color: green'>เครื่องเดิน</i>"


                        }).ToList();
            string jsonlog = new JavaScriptSerializer().Serialize(data);
            return jsonlog;
            //   }
            //  catch { return null; }
        }//CheckLocation  //CheckUser

        public string SelectMachines(String SELECTMAC, DateTime STARTDATE, DateTime ENDDATE)
        {
            //try
            {
                if (STARTDATE.Year < DateTime.Now.Year)
                {
                    STARTDATE = STARTDATE.AddYears(+543);
                }

                if (ENDDATE.Year < DateTime.Now.Year)
                {
                    ENDDATE = ENDDATE.AddYears(+543);
                }
                var startDate = STARTDATE;
                var endDate = ENDDATE;
                var selectMac = SELECTMAC;
                var data = (from TR_P in DbFile.OTIF_TR_Process
                            where (TR_P.Date_Actual >= startDate && TR_P.Date_Actual <= endDate && TR_P.Machine == selectMac) && TR_P.SF_Remain_Qty != null// && Ms_Pl.PL_Machine.Equals(MACHINE)
                            select new
                            {
                                TR_P.Machine,
                                TR_P.Pro_SO,
                                TR_P.Bar_Kan,
                                TR_P.Date_Actual,
                                TR_P.Time_Actual,
                                TR_P.SF_Remain_Qty,
                                TR_P.RT_QTY,
                                TR_P.QTY,
                                TR_P.User,
                                TR_P.Stop_Process,
                                TR_P.TR_DateTime

                            }).AsEnumerable().OrderBy(k => k.TR_DateTime).Select(x => new
                            {
                                Date_Actual = x.Date_Actual.ToString(),
                                Time_Actual = x.Time_Actual.ToString(),
                                Machine = x.Machine,
                                Pro_SO = x.Pro_SO,
                                PL_Type = MAT(x.Bar_Kan).ToString(),
                                Bar_Kan = x.Bar_Kan,
                                SF_Remain_Qty = x.SF_Remain_Qty,
                                RT_QTY = x.RT_QTY,
                                QTY = x.QTY,
                                User = CheckUser(x.User),
                                // CalTotalQTYs = CalTotalQTY(x.Machine),
                                Stop_Process = x.Stop_Process == "T" ? "<i style='font-size:16px; color: red'>เครื่องมีปัญหา</i>" : x.SF_Remain_Qty.Equals(0) ? "<i style='font-size:16px; color: #ff6a00'>เครื่องจอด</i>" : "<i style='font-size:16px; color: green'>เครื่องเดิน</i>"


                            }).ToList();
                string jsonlog = new JavaScriptSerializer().Serialize(data);
                return jsonlog;
            }
            //    catch { return "N"; }
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
                            }).FirstOrDefault();
                //  string jsonlog = new JavaScriptSerializer().Serialize(data);
                return data.Mem_Name;
            }
            catch { return null; }
        }//CheckLocation  //CheckUser
        public string CalTotalQTY(string Machine)
        {
            var dt = DateTime.UtcNow.Date;
            var data = (from TR_Process in DbFile.OTIF_TR_Process
                        where TR_Process.Date_Actual == dt && TR_Process.SF_Remain_Qty != null && TR_Process.Machine.Equals(Machine)

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
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult TestData()
        {

            return View(DbFile.Master_Data.ToList());
        }

        public ActionResult OverView()
        {
            return View();
        }
        public ActionResult ReportByDepart()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoadData()
        {
            JsonResult result = new JsonResult();
            try
            {
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                var preFix = Request.Form.GetValues("columns[1][search][value]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                var data = (from d in DbFile.OTIF_TR_Process
                                //  join Loc in DbFile.WMS_PD_Master_Location on d.Location equals Loc.Lo_Code into Locs
                                //  from Loc1 in Locs.DefaultIfEmpty()
                                //  where d.PRO_Status != ""
                            select new
                            {
                                // d.Bar_Kan,
                                Bar_Kan = d.Bar_Kan.Equals("") ? "<i style='font-size:16px; color: green'>-</i>" : d.Bar_Kan.Equals(null) ? "<i style='font-size:16px; color: green'>-</i>" : d.Bar_Kan,
                                //QTY= d.QTY,
                                QTY = d.QTY.ToString().Equals("") ? "<i style='font-size:16px; color: green'>-</i>" : d.QTY.ToString().Equals(null) ? "<i style='font-size:16px; color: green'>-</i>" : d.QTY.ToString(),
                                Location = d.Location.Equals("") ? "<i style='font-size:16px; color: green'>-</i>" : d.Location.Equals(null) ? "<i style='font-size:16px; color: green'>-</i>" : d.Location,
                                Machine = d.Machine.Equals("") ? "<i style='font-size:16px; color: green'>-</i>" : d.Machine.Equals(null) ? "<i style='font-size:16px; color: green'>-</i>" : d.Machine
                                //d.Location,
                                // d.Machine
                                //  d.Qty_Ship,
                                // d.PRO_Status,
                                // PRO_Status = d.PRO_Status.Equals("ALB") ? "<i class='fa fa-check-circle-o' style='font-size:16px; color: green'>Available</i>" : d.PRO_Status.Equals("WIP") ? "<i class='fa fa-retweet' style='font-size:16px; color: orange'>In-progress</i>" : d.PRO_Status.Equals("SHP") ? "<i class='fa fa-tasks' style='font-size:14px;color:dodgerblue'>โหลดสินค้าขึ้นรถ</i>" : null,
                                // Loc1.Lo_Des,
                                //d.PRO_SO,
                            }).ToList();

                int totalRecords = data.Count;
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    data = data.Where(p => p.Bar_Kan.ToString().ToLower().Contains(search.ToLower()) ||
                        p.QTY.ToString().ToLower().Contains(search.ToLower()) ||
                        p.Location.ToString().ToLower().Contains(search.ToLower()) ||
                        p.Machine.ToString().ToLower().Contains(search.ToLower())
                     ).ToList();
                }
                if (!string.IsNullOrEmpty(preFix))
                {
                    data = data.Where(a => a.Bar_Kan.ToString().ToLower().Contains(preFix.ToLower())).ToList();
                }
                switch (order)
                {
                    case "0":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            data.OrderByDescending(p => p.Bar_Kan).ToList()
                            : data.OrderBy(p => p.Bar_Kan).ToList();
                        break;
                    case "1":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            data.OrderByDescending(p => p.QTY).ToList()
                            : data.OrderBy(p => p.QTY).ToList();
                        break;
                    case "2":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            data.OrderByDescending(p => p.Location).ToList()
                            : data.OrderBy(p => p.Location).ToList();
                        break;

                    default:
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            data.OrderByDescending(p => p.Machine).ToList()
                            : data.OrderBy(p => p.Machine).ToList();
                        break;
                }

                int recFilter = data.Count;
                data = data.Skip(startRec).Take(pageSize).ToList();
                var modifiedData = data.Select(d =>
                    new
                    {
                        d.Bar_Kan,
                        d.QTY,
                        d.Location,
                        d.Machine
                    }
                    );
                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = modifiedData
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return result;
        }// */

    }
}