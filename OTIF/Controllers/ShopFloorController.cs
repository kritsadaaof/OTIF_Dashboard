using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using OTIF.Models;



namespace OTIF.Controllers
{
    public class ShopFloorController : Controller
    {
        private Entities DbFile = new Entities();
        // GET: ShopFloor
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ShopFloor()
        {
            ViewBag.ReasonCode = DbFile.Master_Reason_Code.Where(a => a.Status.Equals("T")).ToList();
            return View();
        }

        public ActionResult ShopFloorFst()
        {
            ViewBag.ReasonCode = DbFile.Master_Reason_Code.Where(a => a.Status.Equals("T")).ToList();
            return View();
        }
        [HttpPost] //รายงาน Shopfloor 
        public string SaveSHF(string BAR_KAN, string MACHINE, int TRAGET, int QTY, string USER, TimeSpan TIME_ACTUAL, int REMAIN, String ACTUAL_SPEED, string STP, string STC)
        {
            try
            { //เพิ่มข้อมูลใหม่ไป  OTIF_TR_ShopFloor ทุกกรณี
                var User = DbFile.Master_Member.Where(y => y.Mem_Name == USER || y.Code_Mem == USER).FirstOrDefault();
                var CheckBar = DbFile.OTIF_TR_Process.Where(y => y.Bar_Kan == BAR_KAN).Where(z => z.Machine.Equals(MACHINE)).FirstOrDefault();
                var CheckMASPL = DbFile.Master_Planner.Where(y => y.PL_Tag == BAR_KAN).Where(z => z.PL_Machine.Equals(MACHINE)).AsEnumerable().OrderByDescending(k => k.PL_Time).GroupBy(k => new { k.PL_Status }).Select(k => k.First()).OrderByDescending(k => k.PL_Status).FirstOrDefault();
                var CheckCal = DbFile.OTIF_TR_Cal.Where(y => y.Cal_Tag == BAR_KAN).Where(z => z.Cal_Machine.Equals(MACHINE)).FirstOrDefault();
                var CheckPack = DbFile.OTIF_TR_Packing.Where(y => y.Pack_Tag == BAR_KAN).Where(z => z.Pack_Machine.Equals(MACHINE)).FirstOrDefault();
                var CheckMachine = DbFile.Master_Machine.Where(y => y.MC_Machine.Equals(MACHINE)).Where(z => z.MC_Process.Equals("Y")).FirstOrDefault();
                var CheckMetal = DbFile.Master_Metal.Where(y => y.Metal_Material_Des.Contains(CheckMASPL.PL_Type)).FirstOrDefault();

                ClassController CAL = new ClassController();
                var TR_SF = new OTIF_TR_ShopFloor();
                var TR_PRO = new OTIF_TR_Process();
                var TR_CAL = new OTIF_TR_Cal();
                var TR_Pack = new OTIF_TR_Packing();
                if (CheckCal != null) //Update Data OTIF_TR_Cal
                {
                    //  CheckCal.Cal_QTY = QTY;
                    CheckCal.Cal_Update = DateTime.Now;
                    CheckCal.Cal_Update_time = TIME_ACTUAL;
                   // CheckCal.Cal_QTY = CheckMASPL.PL_Qty_T;///////////////////////////////
                    CheckCal.Cal_QTY_Balance = QTY;
                }
                else //Create Data OTIF_TR_Cal
                {
                    
                    TR_CAL.Cal_SO = CheckMASPL.PL_SO;
                    TR_CAL.Cal_Machine = CheckMASPL.PL_Machine;
                    TR_CAL.Cal_Tag = CheckMASPL.PL_Tag;
                    TR_CAL.Cal_Status = "OUT";
                    TR_CAL.Cal_QTY = CheckMASPL.PL_Qty_T;
                    TR_CAL.Cal_QTY_Balance = QTY;
                    TR_CAL.Cal_LVL = CheckMASPL.PL_Level;
                    TR_CAL.Cal_Create = DateTime.Now;
                    TR_CAL.Cal_Create_time = TIME_ACTUAL;
                    TR_CAL.Cal_Speed = Double.Parse(ACTUAL_SPEED);
                    DbFile.OTIF_TR_Cal.Add(TR_CAL);
                }
                if (CheckMASPL.PL_Process_Status == "Yes") //ถ้าเป็นล้อ Process สุดท้าย 
                {
                    if (CheckPack != null) //Update Data OTIF_TR_Packing
                    {
                        CheckPack.Pack_Speed = Double.Parse(ACTUAL_SPEED);
                        CheckPack.Pack_Update = DateTime.Now;
                        CheckPack.Pack_QC_Update = DateTime.Now;
                        //  CheckPack.Pack_QTY = CheckMASPL.PL_Qty_T;////////////////////////////
                        CheckPack.Pack_QTY_Balance = QTY;
                        if (CheckMetal != null)
                        {
                            CheckPack.Pack_Weight = (CheckPack.Pack_QTY_Balance) * CheckMetal.Metal_Netweight_W_U;
                        }
                        if (STP == "T")
                        {
                            CheckPack.Pack_Status = "STOP";
                        }
                        else { CheckPack.Pack_Status = "OUT"; }
                    }
                    else //Create Data OTIF_TR_Packing
                    {
                        TR_Pack.Pack_SO = CheckMASPL.PL_SO;
                        TR_Pack.Pack_Machine = CheckMASPL.PL_Machine;
                        TR_Pack.Pack_Tag = CheckMASPL.PL_Tag;
                        TR_Pack.Pack_Status = "OUT";
                        TR_Pack.Pack_QTY = CheckMASPL.PL_Qty_T;
                        TR_Pack.Pack_QTY_Balance = QTY;
                        TR_Pack.Pack_LVL = CheckMASPL.PL_Level;
                        TR_Pack.Pack_Create = DateTime.Now;
                        TR_Pack.Pack_Update = DateTime.Now;
                        TR_Pack.Pack_QC_Update = DateTime.Now;
                        TR_Pack.Pack_Create_time = TIME_ACTUAL;
                        TR_Pack.Pack_Speed = Double.Parse(ACTUAL_SPEED);
                        TR_Pack.Pack_DateTime = DateTime.Now;
                        TR_Pack.Pack_Reel_Size = CheckMASPL.PL_Reel_Size;
                        if (CheckMetal != null)
                        {
                            TR_Pack.Pack_Weight = CheckMetal.Metal_Netweight_W_U;
                        }
                        if (CheckMachine != null)
                        {
                            TR_Pack.Pack_Base = "P1";
                        }
                       
                        DbFile.OTIF_TR_Packing.Add(TR_Pack);
                    }
                }

                if (CheckBar != null)
                { //เงื่อนไขนี้ แก้ไขข้อมูลใน OTIF_TR_Process
                    CheckBar.SF_Remain_Qty = REMAIN;
                    if (CheckBar.RT_QTY == null)
                    {
                        CheckBar.RT_QTY = QTY;
                    }
                    else
                    {
                        CheckBar.RT_QTY = QTY;
                    }
                    CheckBar.Process_Speed = Double.Parse(ACTUAL_SPEED);
                    CheckBar.Stop_Process = STP;
                   // CheckBar.TR_DateTime = DateTime.Now;
                    TR_SF.SF_Kanban = CheckBar.Bar_Kan;
                    TR_SF.SF_Machine = CheckBar.Machine;
                }
                else
                { //เงื่อนไขนี้เพิ่มข้อมูลใหม่ ใน OTIF_TR_Process 
                    TR_PRO.Machine = MACHINE;
                    TR_PRO.Pro_SO = CheckMASPL.PL_SO;
                    TR_PRO.Bar_Kan = BAR_KAN;
                    TR_SF.SF_Kanban = BAR_KAN;
                    TR_SF.SF_Machine = MACHINE;
                    TR_PRO.QTY = TRAGET;
                    TR_PRO.SF_Remain_Qty = REMAIN;
                    TR_PRO.User = User.Code_Mem;
                    TR_PRO.Date_Actual = DateTime.Now;
                    TR_PRO.Time_Actual = TIME_ACTUAL;
                    TR_PRO.Status = "T";
                    TR_PRO.RT_QTY = QTY;
                    TR_PRO.Stop_Process = STP;
                    TR_PRO.Process_Speed = Double.Parse(ACTUAL_SPEED);
                    TR_PRO.TR_DateTime = DateTime.Now;
                    DbFile.OTIF_TR_Process.Add(TR_PRO);
                }
                CheckMASPL.PL_Status = "F";
                TR_SF.SF_User = User.Code_Mem;
                TR_SF.SF_QTY = QTY;
                TR_SF.SF_Date = DateTime.Now;
                TR_SF.SF_Time = TIME_ACTUAL;
                TR_SF.SF_Actual_Speed = Double.Parse(ACTUAL_SPEED);
                TR_SF.SF_Stop_Process = STP;
                TR_SF.SF_Stop_Code = STC;
                DbFile.OTIF_TR_ShopFloor.Add(TR_SF);
                DbFile.SaveChanges();  //*/
                                       // CAL.Calculator(CheckMASPL.PL_SO, MACHINE);
                return "S";
            }
            catch { return "N"; }
        }
        [HttpPost]//รายงาน Shopfloor เมตรแรก
        public string SaveSHFFst(string BAR_KAN, string MACHINE, int TRAGET, int QTY, string USER, TimeSpan TIME_ACTUAL, int REMAIN, String ACTUAL_SPEED, string STP, string STC, TimeSpan TIME_SF_FST)
        {
            try
            { //เพิ่มข้อมูลใหม่ไป  OTIF_TR_ShopFloor ทุกกรณี
                var User = DbFile.Master_Member.Where(y => y.Mem_Name == USER || y.Code_Mem == USER).FirstOrDefault();
                var CheckBar = DbFile.OTIF_TR_Process.Where(y => y.Bar_Kan == BAR_KAN).Where(z => z.Machine.Equals(MACHINE)).FirstOrDefault();
                var CheckMASPL = DbFile.Master_Planner.Where(y => y.PL_Tag == BAR_KAN).Where(z => z.PL_Machine.Equals(MACHINE)).AsEnumerable().OrderByDescending(k => k.PL_Time).GroupBy(k => new { k.PL_Status }).Select(k => k.First()).OrderByDescending(k => k.PL_Status).FirstOrDefault();
                var CheckCal = DbFile.OTIF_TR_Cal.Where(y => y.Cal_Tag == BAR_KAN).Where(z => z.Cal_Machine.Equals(MACHINE)).FirstOrDefault();
                var CheckPack = DbFile.OTIF_TR_Packing.Where(y => y.Pack_Tag == BAR_KAN).Where(z => z.Pack_Machine.Equals(MACHINE)).FirstOrDefault();
                var CheckMachine = DbFile.Master_Machine.Where(y => y.MC_Machine.Equals(MACHINE)).Where(z => z.MC_Process.Equals("Y")).FirstOrDefault();
                var CheckMetal = DbFile.Master_Metal.Where(y=>y.Metal_Material_Des.Equals(CheckMASPL.PL_Type)).FirstOrDefault();
                ClassController CAL = new ClassController();
                var TR_SF = new OTIF_TR_ShopFloor();
                var TR_PRO = new OTIF_TR_Process();
                var TR_CAL = new OTIF_TR_Cal();
                var TR_Pack = new OTIF_TR_Packing();
                // Data to table Cal//////////////////////////////////////////////////////////
                if (CheckCal != null)
                {
                    //  CheckCal.Cal_QTY = QTY;
                    CheckCal.Cal_Update = DateTime.Now;
                    CheckCal.Cal_Update_time = TIME_ACTUAL;
                    // CheckCal.Cal_QTY = CheckMASPL.PL_Qty_T;///////////////////////////////
                    CheckCal.Cal_QTY_Balance = QTY;
                }
                else
                {

                    TR_CAL.Cal_SO = CheckMASPL.PL_SO;
                    TR_CAL.Cal_Machine = CheckMASPL.PL_Machine;
                    TR_CAL.Cal_Tag = CheckMASPL.PL_Tag;
                    TR_CAL.Cal_Status = "OUT";
                    TR_CAL.Cal_QTY = CheckMASPL.PL_Qty_T;
                    TR_CAL.Cal_QTY_Balance = QTY;
                    TR_CAL.Cal_LVL = CheckMASPL.PL_Level;
                    TR_CAL.Cal_Create = DateTime.Now;
                    TR_CAL.Cal_Create_time = TIME_ACTUAL;
                    TR_CAL.Cal_Speed = Double.Parse(ACTUAL_SPEED);
                    DbFile.OTIF_TR_Cal.Add(TR_CAL);
                }
                // Data to table Cal //////////////////////////////////////////////////////////
                // Data to table Packing //////////////////////////////////////////////////////
                if (CheckMASPL.PL_Process_Status == "Yes")
                {
                    if (CheckPack != null)
                    {
                        CheckPack.Pack_Speed = Double.Parse(ACTUAL_SPEED);
                        CheckPack.Pack_Update = DateTime.Now;
                        CheckPack.Pack_QC_Update = DateTime.Now;
                        //  CheckPack.Pack_QTY = CheckMASPL.PL_Qty_T;////////////////////////////
                        CheckPack.Pack_QTY_Balance = QTY;
                        if (CheckMetal != null)
                        {
                            CheckPack.Pack_Weight = (CheckPack.Pack_QTY_Balance) * CheckMetal.Metal_Netweight_W_U;
                        }
                    }
                    else
                    {
                        TR_Pack.Pack_SO = CheckMASPL.PL_SO;
                        TR_Pack.Pack_Machine = CheckMASPL.PL_Machine;
                        TR_Pack.Pack_Tag = CheckMASPL.PL_Tag;
                        TR_Pack.Pack_Status = "OUT";
                        TR_Pack.Pack_QTY = CheckMASPL.PL_Qty_T;
                        TR_Pack.Pack_QTY_Balance = QTY;
                        TR_Pack.Pack_LVL = CheckMASPL.PL_Level;
                        TR_Pack.Pack_Create = DateTime.Now;
                        TR_Pack.Pack_Update = DateTime.Now;
                        TR_Pack.Pack_QC_Update = DateTime.Now;
                        TR_Pack.Pack_Create_time = TIME_ACTUAL;
                        TR_Pack.Pack_Speed = Double.Parse(ACTUAL_SPEED);
                        TR_Pack.Pack_DateTime = DateTime.Now;
                        TR_Pack.Pack_Reel_Size = CheckMASPL.PL_Reel_Size;
                        if (CheckMetal != null)
                        {
                            TR_Pack.Pack_Weight = CheckMetal.Metal_Netweight_W_U;
                        }
                        if (CheckMachine != null)
                        {
                            TR_Pack.Pack_Base = "P1";
                        }
                        DbFile.OTIF_TR_Packing.Add(TR_Pack);
                    }
                }
                // Data to table Packing //////////////////////////////////////////////////////

                // Data to table Process //////////////////////////////////////////////////////
                if (CheckBar != null)
                { //เงื่อนไขนี้ แก้ไขข้อมูลใน OTIF_TR_Process
                    CheckBar.SF_Remain_Qty = REMAIN;
                    if (CheckBar.RT_QTY == null)
                    {
                        CheckBar.RT_QTY = QTY;
                    }
                    else
                    {
                        CheckBar.RT_QTY = QTY;
                    }
                    TR_SF.SF_Kanban = CheckBar.Bar_Kan;
                    TR_SF.SF_Machine = CheckBar.Machine;
                   // CheckBar.TR_DateTime = DateTime.Now;
                }
                else
                { //เงื่อนไขนี้เพิ่มข้อมูลใหม่ ใน OTIF_TR_Process 
                    TR_PRO.Machine = MACHINE;
                    TR_PRO.Pro_SO = CheckMASPL.PL_SO;
                    TR_PRO.Bar_Kan = BAR_KAN;
                    TR_SF.SF_Kanban = BAR_KAN;
                    TR_SF.SF_Machine = MACHINE;
                    TR_PRO.QTY = TRAGET;
                    TR_PRO.SF_Remain_Qty = REMAIN;
                    TR_PRO.User = User.Code_Mem;
                    TR_PRO.Date_Actual = DateTime.Now;
                    TR_PRO.Time_Actual = TIME_ACTUAL;
                    TR_PRO.Status = "T";
                    TR_PRO.RT_QTY = QTY;
                    TR_PRO.Process_Speed = Double.Parse(ACTUAL_SPEED);
                    TR_PRO.TR_DateTime = DateTime.Now;
                    DbFile.OTIF_TR_Process.Add(TR_PRO);
                }

                // Data to table Process //////////////////////////////////////////////////////

                // Data to table ShopFloor ////////////////////////////////////////////////////
                CheckMASPL.PL_Status = "F";
                TR_SF.SF_User = User.Code_Mem;
                TR_SF.SF_QTY = QTY;
                TR_SF.SF_Date = DateTime.Now;
                TR_SF.SF_Time = TIME_ACTUAL;
                TR_SF.SF_Actual_Speed = Double.Parse(ACTUAL_SPEED);
                TR_SF.SF_Stop_Process = STP;
                TR_SF.SF_Stop_Code = STC;
                TR_SF.SF_Time_Fst = TIME_SF_FST;
                DbFile.OTIF_TR_ShopFloor.Add(TR_SF);

                // Data to table ShopFloor ////////////////////////////////////////////////////

                DbFile.SaveChanges();  // สั่ง Save ข้อมูลตามข้างบน
                                       // CAL.Calculator(CheckMASPL.PL_SO, MACHINE);
                return "S";
            } catch { return "N"; }
        }
        public string CheckBarSF(string BAR, string MACHINE)
        {
            try
            {
                var data = (from TR_Pro in DbFile.OTIF_TR_Process
                            where TR_Pro.Bar_Kan.Equals(BAR) && TR_Pro.Status.Equals("T") && TR_Pro.Machine.Equals(MACHINE)
                            select new
                            {
                                TR_Pro.QTY,
                                TR_Pro.SF_Remain_Qty,
                                TR_Pro.RT_QTY
                            }).ToList();
                if (data.Count == 0)
                {
                    var data1 = (from MS_PL in DbFile.Master_Planner
                                 where MS_PL.PL_Tag.Equals(BAR) && MS_PL.PL_Machine.Equals(MACHINE)
                                 select new
                                 {
                                     MS_PL.PL_Time,
                                     MS_PL.PL_Status,
                                     QTY = MS_PL.PL_Qty_T,
                                     SF_Remain_Qty = MS_PL.PL_Qty_T,
                                     RT_QTY=0
                                 }).AsEnumerable().OrderByDescending(k => k.PL_Time).GroupBy(k => new { k.PL_Status }).Select(k => k.First()).OrderByDescending(k => k.PL_Status).ToList();
                    string jsonlog = new JavaScriptSerializer().Serialize(data1);
                    return jsonlog;
                }
                else
                {
                    string jsonlog = new JavaScriptSerializer().Serialize(data);
                    return jsonlog;
                }
            }
            catch { return null; }
        }
    }
}