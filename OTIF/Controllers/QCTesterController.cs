using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.Ajax.Utilities;
using OTIF.Models;

namespace OTIF.Controllers
{
    public class QCTesterController : Controller
    {
        private Entities DbFile = new Entities();
        // GET: QCTester
        public ActionResult QC()
        {
            return View();
        }
        [HttpPost]
        public string GetDATA()
        {
            var Get = (from TR_Pac in DbFile.OTIF_TR_Packing
                           // where TR_Pac.ID.Equals(AA)
                       orderby TR_Pac.Pack_QC_Update ascending
                       where (TR_Pac.Pack_Status.Equals("OUT") || TR_Pac.Pack_Status.Equals("Reject")) && TR_Pac.Pack_Status_QC.Equals("T")
                       select new
                       {
                           TR_Pac.ID,
                           TR_Pac.Pack_Tag,
                           TR_Pac.Pack_QTY,
                           TR_Pac.Pack_QTY_Balance,
                           TR_Pac.Pack_SO,
                           TR_Pac.Pack_Machine,
                           TR_Pac.Pack_DateTime,
                           TR_Pac.Pack_Speed,
                           TR_Pac.Pack_Status_QC,
                           TR_Pac.Pack_Base,
                           TR_Pac.Pack_Weight
                       }).ToList();
            string jsonlog = new JavaScriptSerializer().Serialize(Get);
            return jsonlog;
            //   return View();
        }

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
                var data = (from d in DbFile.OTIF_TR_Packing
                            select new
                            {
                                Bar_Kan = d.Pack_Tag.Equals("") ? "<i style='font-size:16px; color: green'>-</i>" : d.Pack_Tag.Equals(null) ? "<i style='font-size:16px; color: green'>-</i>" : d.Pack_Tag,
                                SO = d.Pack_SO.Equals("") ? "<i style='font-size:16px; color: green'>-</i>" : d.Pack_SO.Equals(null) ? "<i style='font-size:16px; color: green'>-</i>" : d.Pack_SO,
                                QTY = d.Pack_QTY.ToString().Equals("") ? "<i style='font-size:16px; color: green'>-</i>" : d.Pack_QTY.ToString().Equals(null) ? "<i style='font-size:16px; color: green'>-</i>" : d.Pack_QTY.ToString(),
                                Machine = d.Pack_Machine.Equals("") ? "<i style='font-size:16px; color: green'>-</i>" : d.Pack_Machine.Equals(null) ? "<i style='font-size:16px; color: green'>-</i>" : d.Pack_Machine,
                                Pack_Status = d.ID.ToString().Equals("") ? "<i style='font-size:16px; color: green'>-</i>" : d.ID.ToString().Equals(null) ? "<i style='font-size:16px; color: green'>-</i>" : d.ID.ToString(),
                                Pack_Base = d.Pack_Base.Equals("") ? "<i style='font-size:16px; color: green'>-</i>" : d.Pack_Base.Equals(null) ? "<i style='font-size:16px; color: green'>-</i>" : d.Pack_Base,
                                Pack_DateTime = d.Pack_DateTime.ToString()

                            }).ToList();

                int totalRecords = data.Count;
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    data = data.Where(p => p.Bar_Kan.ToString().ToLower().Contains(search.ToLower()) ||
                         p.SO.ToString().ToLower().Contains(search.ToLower()) ||
                        p.QTY.ToString().ToLower().Contains(search.ToLower()) ||
                        p.Machine.ToString().ToLower().Contains(search.ToLower()) ||
                        p.Pack_Status.ToString().ToLower().Contains(search.ToLower()) ||
                        p.Pack_Base.ToString().ToLower().Contains(search.ToLower()) ||
                        p.Pack_DateTime.ToString().ToLower().Contains(search.ToLower())
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
                            data.OrderByDescending(p => p.SO).ToList()
                            : data.OrderBy(p => p.SO).ToList();
                        break;
                    case "2":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            data.OrderByDescending(p => p.QTY).ToList()
                            : data.OrderBy(p => p.QTY).ToList();
                        break;
                    case "3":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            data.OrderByDescending(p => p.Machine).ToList()
                            : data.OrderBy(p => p.Machine).ToList();
                        break;
                    case "4":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            data.OrderByDescending(p => p.Machine).ToList()
                            : data.OrderBy(p => p.Machine).ToList();
                        break;


                    case "5":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            data.OrderByDescending(p => p.Pack_Base).ToList()
                            : data.OrderBy(p => p.Pack_Base).ToList();
                        break;

                    default:
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            data.OrderByDescending(p => p.Pack_DateTime).ToList()
                            : data.OrderBy(p => p.Pack_DateTime).ToList();
                        break;
                }

                int recFilter = data.Count;
                data = data.Skip(startRec).Take(pageSize).ToList();
                var modifiedData = data.Select(d =>
                    new
                    {
                        d.Bar_Kan,
                        d.SO,
                        d.QTY,
                        d.Machine,
                        d.Pack_Status,
                        d.Pack_Base,
                        d.Pack_DateTime
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

        [HttpPost]
        public string SaveQC(string TAG, string USER, string QC_Status, TimeSpan TIME)
        {
            try
            {
                var User = DbFile.Master_Member.Where(y => y.Mem_Name == USER || y.Code_Mem == USER).FirstOrDefault();

                var CheckPack = DbFile.OTIF_TR_Packing.Where(y => y.Pack_Tag == TAG).FirstOrDefault();

                if (CheckPack != null)
                {
                    if (CheckPack.Pack_Status_QC == null || CheckPack.Pack_Status_QC == "R")
                    {
                        CheckPack.Pack_Status_QC = "T";//ขึ้นสถานะตรวจสอบ
                    }
                    else
                    {
                        if (QC_Status == "R")//เช็คเงื่อนไขถ้าไม่CheckBoxไม่ผ่าน
                        {
                            CheckPack.Pack_Status_QC = "R";
                            if (CheckPack.Pack_Status == "Reject")
                            {
                                CheckPack.Pack_Status = "OUT";
                                CheckPack.Pack_Status_Shop = null;
                            }
                        }
                        else
                        {//เงื่อนไขถ้า CheckBox ไม่ผ่าน
                            CheckPack.Pack_Status_QC = "F";
                        }
                    }
                    CheckPack.Pack_QC_Update = DateTime.Now;
                    CheckPack.Pack_QC_User = User.Code_Mem;
                    DbFile.SaveChanges();  //*/ 
                    return "S";
                }
                else
                {
                    SaveDataByQA(TAG,USER,TIME);
                    return "S";
                }
            }
            catch { return "N"; }
        }
        public string CheckBarQC(string TAG)
        {
            try
            {
                var data = (from TR_Pack in DbFile.OTIF_TR_Packing
                            where TR_Pack.Pack_Tag.Equals(TAG)
                            select new
                            {
                                TR_Pack.Pack_Status_QC
                                //  TR_Move.Bar_Kan, 
                            }).Take(1).ToList();
                string jsonlog = new JavaScriptSerializer().Serialize(data);
                return jsonlog;
            }
            catch { return null; }
        }
        private string SaveDataByQA(string BAR_KAN, string USER, TimeSpan TIME)
        {
            try
            { //เพิ่มข้อมูลใหม่ไป  OTIF_TR_ShopFloor ทุกกรณี
                var User = DbFile.Master_Member.Where(y => y.Mem_Name == USER || y.Code_Mem == USER).FirstOrDefault();
                var CheckMASPL = DbFile.Master_Planner.Where(y => y.PL_Tag == BAR_KAN).AsEnumerable().OrderByDescending(k => k.PL_Time).GroupBy(k => new { k.PL_Status }).Select(k => k.First()).OrderByDescending(k => k.PL_Status).FirstOrDefault();
                var CheckMetal = DbFile.Master_Metal.Where(y => y.Metal_Material_Des.Contains(CheckMASPL.PL_Type)).FirstOrDefault();


                var TR_Pack = new OTIF_TR_Packing();


                if (CheckMASPL.PL_Process_Status == "Yes") //ถ้าเป็นล้อ Process สุดท้าย 
                {
                    TR_Pack.Pack_SO = CheckMASPL.PL_SO;
                    TR_Pack.Pack_Machine = CheckMASPL.PL_Machine;
                    TR_Pack.Pack_Tag = CheckMASPL.PL_Tag;
                    TR_Pack.Pack_Status = "OUT";
                    TR_Pack.Pack_QTY = CheckMASPL.PL_Qty_T;
                    TR_Pack.Pack_QTY_Balance = CheckMASPL.PL_Qty_T;
                    TR_Pack.Pack_LVL = CheckMASPL.PL_Level;
                    TR_Pack.Pack_Create = DateTime.Now;
                    TR_Pack.Pack_Update = DateTime.Now;
                    TR_Pack.Pack_QC_Update = DateTime.Now;
                    TR_Pack.Pack_Create_time = TIME;
                    TR_Pack.Pack_Speed = 19;
                    TR_Pack.Pack_DateTime = DateTime.Now;
                    TR_Pack.Pack_Reel_Size = CheckMASPL.PL_Reel_Size;
                    if (CheckMetal != null)
                    {
                        TR_Pack.Pack_Weight = CheckMASPL.PL_Qty_T * CheckMetal.Metal_Netweight_W_U;
                    }
                    TR_Pack.Pack_Base = "P1";
                    TR_Pack.Pack_Status_QC = "T";
                    CheckMASPL.PL_Status = "F";
                    DbFile.OTIF_TR_Packing.Add(TR_Pack);
                    DbFile.SaveChanges();  //*/
                }
                return "S";
            }
            catch { return "N"; }
        }
    }
}