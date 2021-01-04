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
    public class PackingController : Controller
    {
        private Entities DbFile = new Entities();
        // GET: Packing
        public ActionResult PackingFG()
        {
            return View();
        }
        public ActionResult Packing()
        {
            return View();
        }
        public ActionResult Packing2()
        {
            return View();
        }

        public ActionResult PackingCheck()
        {
            return View();
        }

        public ActionResult PackingCheck2()
        {
            return View();
        }

        public ActionResult PackingReceive()
        {
            return View();
        }
        [HttpPost]
        public string SaveDataByPacking(string BAR_KAN, string USER, double REELNO, double GROSS, TimeSpan TIME)
        {
            try
            {
                var User = DbFile.Master_Member.Where(y => y.Mem_Name == USER || y.Code_Mem == USER).FirstOrDefault();
                var CheckMASPL = DbFile.Master_Planner.Where(y => y.PL_Tag == BAR_KAN).AsEnumerable().OrderByDescending(k => k.PL_Time).GroupBy(k => new { k.PL_Status }).Select(k => k.First()).OrderByDescending(k => k.PL_Status).FirstOrDefault();
                var CheckMetal = DbFile.Master_Metal.Where(y => y.Metal_Material_Des.Contains(CheckMASPL.PL_Type)).FirstOrDefault();
                var CheckPack = DbFile.OTIF_TR_Packing.Where(y => y.Pack_Tag == BAR_KAN).FirstOrDefault();
                var CheckReelBom = DbFile.Master_Reel_Bom.Where(y => y.Reel_Size.Contains(CheckMASPL.PL_Reel_Size)).FirstOrDefault();


                if (CheckPack != null)
                {
                    CheckPack.Pack_ReelNo = REELNO;
                    CheckPack.Pack_Gross = GROSS;
                    CheckPack.Pack_Status_Shop = "T";
                    if (CheckReelBom != null)
                    {
                        CheckPack.Pack_Gross = GROSS + CheckReelBom.Lag_Reel;
                    }
                    DbFile.SaveChanges();  //*/
                    return "S";
                }
                else
                {
                   

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
                        TR_Pack.Pack_ReelNo = REELNO;
                       // TR_Pack.Pack_Gross = GROSS;
                        if (CheckReelBom != null)
                        {
                            TR_Pack.Pack_Gross = GROSS + CheckReelBom.Lag_Reel;
                        }
                        if (CheckMetal != null)
                        {
                            TR_Pack.Pack_Weight = CheckMASPL.PL_Qty_T * CheckMetal.Metal_Netweight_W_U;
                        }
                        TR_Pack.Pack_Base = "P1";
                        TR_Pack.Pack_Status_Shop = "T";
                        CheckMASPL.PL_Status = "F";
                        DbFile.OTIF_TR_Packing.Add(TR_Pack);
                        DbFile.SaveChanges();  //*/
                    }

                    return "S";
                }
            }
            catch { return "N"; }
        }

        public string CheckBar(string BAR)
        {
            try
            {
                var CheckMASPL = DbFile.Master_Planner.Where(y => y.PL_Tag == BAR).AsEnumerable().OrderByDescending(k => k.PL_Time).GroupBy(k => new { k.PL_Status }).Select(k => k.First()).OrderByDescending(k => k.PL_Status).FirstOrDefault();
                string jsonlog = new JavaScriptSerializer().Serialize(CheckMASPL);
                return jsonlog;
            }
            catch { return null; }

        }
        public string SaveShop(int ID, string STATUS)
        {//บันทึกการ Shop
            try
            {
                var TR_Packing = DbFile.OTIF_TR_Packing.Where(a => a.ID.Equals(ID)).FirstOrDefault();
                if (TR_Packing.Pack_Status_QC == "F")
                {
                    TR_Packing.Pack_Status = "Reject";
                }
                TR_Packing.Pack_Status_Shop = STATUS;
                DbFile.SaveChanges();
                return null;
            }
            catch { return null; }
        }

        [HttpPost]
        public string GetDATA(string BASE) //ข้อมูล P1 || P2
        {
            var Get = (from TR_Pac in DbFile.OTIF_TR_Packing
                       join Bom in DbFile.Master_Reel_Bom on TR_Pac.Pack_Reel_Size equals Bom.Reel_Size into Bom1
                       from Boms in Bom1.DefaultIfEmpty()
                       join MS_PLs in DbFile.Master_Planner on TR_Pac.Pack_Tag equals MS_PLs.PL_Tag into MS_PL1
                       from MS_PL in MS_PL1.Where(a => a.PL_Status != null).DefaultIfEmpty()
                       where (TR_Pac.Pack_Status_QC == null || TR_Pac.Pack_Status_QC == "R" || (TR_Pac.Pack_Status_QC == "F" && TR_Pac.Pack_Status_Shop == "T")) && TR_Pac.Pack_Status.Equals("OUT") && TR_Pac.Pack_Base.Equals(BASE)
                       orderby TR_Pac.Pack_QC_Update ascending
                       select new
                       {
                           TR_Pac.ID,
                           TR_Pac.Pack_Tag,
                           TR_Pac.Pack_QTY,
                           TR_Pac.Pack_QTY_Balance,
                           TR_Pac.Pack_SO,
                           TR_Pac.Pack_Machine,//
                           TR_Pac.Pack_DateTime,
                           TR_Pac.Pack_Update,
                           TR_Pac.Pack_Speed,
                           TR_Pac.Pack_Reel_Size,
                           TR_Pac.Pack_Status_Shop,
                           TR_Pac.Pack_Status_QC,
                           TR_Pac.Pack_Weight,
                           MS_PL.PL_Type,//
                           Boms.Drum_High,
                           Boms.Drum_Length,
                           Boms.Size,
                           Boms.Q_Unit,
                           Boms.Thickness,
                           Boms.Reel_Weight
                       }).ToList();
            string jsonlog = new JavaScriptSerializer().Serialize(Get);
            return jsonlog;
            //   return View();
        }

        [HttpPost]
        public string GetDATAQCPass(string BASE)
        {
            var Get = (from TR_Pac in DbFile.OTIF_TR_Packing
                       join Bom in DbFile.Master_Reel_Bom on TR_Pac.Pack_Reel_Size equals Bom.Reel_Size into Bom1
                       from Boms in Bom1.DefaultIfEmpty()
                       where (TR_Pac.Pack_Status_QC == null || TR_Pac.Pack_Status_QC == "R" || TR_Pac.Pack_Status_QC == "F") && TR_Pac.Pack_Status.Equals("OUT") && TR_Pac.Pack_Base.Equals(BASE) && TR_Pac.Pack_Status_Shop.Equals("T")
                       orderby TR_Pac.Pack_QC_Update ascending
                       select new
                       {
                           TR_Pac.ID,
                           TR_Pac.Pack_Tag,
                           TR_Pac.Pack_QTY,
                           TR_Pac.Pack_QTY_Balance,
                           TR_Pac.Pack_SO,
                           TR_Pac.Pack_Machine,
                           TR_Pac.Pack_DateTime,
                           TR_Pac.Pack_Update,
                           TR_Pac.Pack_Speed,
                           TR_Pac.Pack_Reel_Size,
                           TR_Pac.Pack_Status_Shop,
                           TR_Pac.Pack_Status_QC,
                           TR_Pac.Pack_Weight,
                           TR_Pac.Pack_ReelNo,
                           TR_Pac.Pack_Gross,
                           Boms.Drum_High,
                           Boms.Drum_Length,
                           Boms.Size,
                           Boms.Q_Unit,
                           Boms.Thickness,
                           Boms.Reel_Weight
                       }).ToList();
            string jsonlog = new JavaScriptSerializer().Serialize(Get);
            return jsonlog;
            //   return View();
        }

        [HttpPost]
        public string GetDATAFG(string BASE)
        {
            var Get = (from TR_Pac in DbFile.OTIF_TR_Packing
                       join Bom in DbFile.Master_Reel_Bom on TR_Pac.Pack_Reel_Size equals Bom.Reel_Size into Bom1
                       from Boms in Bom1.DefaultIfEmpty()
                       join MS_PLs in DbFile.Master_Planner on TR_Pac.Pack_Tag equals MS_PLs.PL_Tag into MS_PL1
                       from MS_PL in MS_PL1.Where(a => a.PL_Status != null).DefaultIfEmpty()
                       where (TR_Pac.Pack_Status_QC == null || TR_Pac.Pack_Status_QC == "R" || TR_Pac.Pack_Status_QC == "F") && TR_Pac.Pack_Status.Equals("OUT") && TR_Pac.Pack_Status_Shop.Equals("T") && TR_Pac.Pack_Base.Equals(BASE)
                       orderby TR_Pac.Pack_QC_Update ascending
                       select new
                       {
                           TR_Pac.ID,
                           TR_Pac.Pack_Tag,
                           TR_Pac.Pack_QTY,
                           TR_Pac.Pack_QTY_Balance,
                           TR_Pac.Pack_SO,
                           TR_Pac.Pack_Machine,
                           TR_Pac.Pack_DateTime,
                           TR_Pac.Pack_Update,
                           TR_Pac.Pack_Speed,
                           TR_Pac.Pack_Reel_Size,
                           TR_Pac.Pack_Status_Shop,
                           TR_Pac.Pack_Status_QC,
                           TR_Pac.Pack_Weight,
                           TR_Pac.Pack_ReelNo,
                           TR_Pac.Pack_Gross,
                           MS_PL.PL_QTY_Cus,
                           MS_PL.PL_Shop_Packing,
                           Boms.Drum_High,
                           Boms.Drum_Length,
                           Boms.Size,
                           Boms.Q_Unit,
                           Boms.Thickness,
                           Boms.Reel_Weight
                       }).ToList();
            string jsonlog = new JavaScriptSerializer().Serialize(Get);
            return jsonlog;
            //   return View();
        }
    }
}