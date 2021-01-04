using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using OTIF.Models;
using ZXing.OneD;

namespace OTIF.Controllers
{
    public class MoveController : Controller
    {
        private Entities DbFile = new Entities();
        // GET: Move
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Move()
        {
            return View();
        }
        [HttpPost]
        public string SaveMove(string BAR_KAN, string LOCATION, string USER, TimeSpan TIME_ACTUAL,string OPTION,string BASE)
        {
            try
            {
                var User = DbFile.Master_Member.Where(y => y.Mem_Name == USER || y.Code_Mem == USER).FirstOrDefault();

                var CheckMove = DbFile.OTIF_TR_MoveLocation.Where(y => y.Move_TAG == BAR_KAN).OrderByDescending(x => x.ID).FirstOrDefault();
                var CheckPack = DbFile.OTIF_TR_Packing.Where(y => y.Pack_Tag == BAR_KAN).FirstOrDefault();
               // var TR_Pro = new OTIF_TR_Process();
                var Move = new OTIF_TR_MoveLocation();
                if (CheckMove != null)
                {
                    if ((CheckMove.Move_Status == null || CheckMove.Move_Status == "F")&& OPTION=="รับ")
                    {
                        Move.Move_Status = "T"; //T=ยกขึ้นรถ
                    }
                    else
                    {
                        Move.Move_Status = "F";
                    }
                    Move.Move_TAG = BAR_KAN;
                    Move.Move_User = User.Code_Mem;
                    Move.Move_Date = DateTime.Now;
                    Move.Move_time = TIME_ACTUAL;
                    Move.Move_Loca = LOCATION;
                    if (CheckPack!=null&& BASE != null)
                    {
                        CheckPack.Pack_Base = BASE;
                    }
                    DbFile.OTIF_TR_MoveLocation.Add(Move);
                    DbFile.SaveChanges();  //*/
                   // return "S";
                }
                else
                {
                    if (OPTION == "ส่ง")
                    {
                        Move.Move_Status = "F"; //F=ยกลงรถ
                    }
                    else {
                        Move.Move_Status = "T"; //T=ยกขึ้นรถ
                    }
                       
                    Move.Move_TAG = BAR_KAN;
                    Move.Move_User = User.Code_Mem;
                    Move.Move_Date = DateTime.Now;
                    Move.Move_time = TIME_ACTUAL;
                    Move.Move_Loca = LOCATION;
                    if (BASE != null)
                    {
                        CheckPack.Pack_Base = BASE;
                    }
                    DbFile.OTIF_TR_MoveLocation.Add(Move);
                    DbFile.SaveChanges();
                   // return "S";
                }
            
                return "S";
            }
            catch { return "N"; }
        }

   /*     public string CheckBar111111(string BAR)
        {
            try
            {
                var data = (from TR_Move in DbFile.OTIF_TR_MoveLocation 
                            where TR_Move.Move_TAG.Equals(BAR)
                            orderby TR_Move.ID descending
                            select new
                            {
                                TR_Move.ID,
                                TR_Move.Move_Status
                                //  TR_Move.Bar_Kan, 
                            }).Take(1).ToList();
                string jsonlog = new JavaScriptSerializer().Serialize(data);
                return jsonlog;
            }
            catch { return null; }
        }
   */
        public string CheckBar(string BAR)
        {
            try
            {
                var data = (from MS_Plan in DbFile.Master_Planner
                            join TR_Move in DbFile.OTIF_TR_MoveLocation on MS_Plan.PL_Tag equals TR_Move.Move_TAG into TR_Move1
                            from TR_Moves in TR_Move1.DefaultIfEmpty()
                            where MS_Plan.PL_Tag.Equals(BAR)
                            orderby TR_Moves.ID descending
                            select new
                            {
                                MS_Plan.PL_Process_Status,
                                TR_Moves.ID,
                                TR_Moves.Move_Status
                                //  TR_Move.Bar_Kan, 
                            }).Take(1).ToList();
                string jsonlog = new JavaScriptSerializer().Serialize(data);
                return jsonlog;
            }
            catch {
                var data = (from MS_Plan in DbFile.Master_Planner 
                            where MS_Plan.PL_Tag.Equals(BAR) 
                            select new
                            {
                                MS_Plan.PL_Process_Status 
                                //  TR_Move.Bar_Kan, 
                            }).Take(1).ToList();
                string jsonlog = new JavaScriptSerializer().Serialize(data);
                return jsonlog;
            }
        }
    }
}