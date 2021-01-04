using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using OTIF.Models;


namespace OTIF.Controllers
{
    public class PDReturnController : Controller
    {
        private Entities DbFile = new Entities();
        // GET: PDReturn
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PDReturn()
        {
            return View();
        }
        [HttpPost]
        public string SaveRT(string BAR_KAN, int RT_QTY, string USER, TimeSpan TIME_ACTUAL)
        {
            try
            {

                var User = DbFile.Master_Member.Where(y => y.Mem_Name == USER || y.Code_Mem == USER).FirstOrDefault();
                var CheckBar = DbFile.OTIF_TR_Process.Where(y => y.Bar_Kan == BAR_KAN).FirstOrDefault(); 
                CheckBar.RT_QTY = RT_QTY;
                CheckBar.User = User.Code_Mem;
                CheckBar.Date_Actual = DateTime.Now;
                CheckBar.Time_Actual = TIME_ACTUAL;
                CheckBar.Status = "F";// F = Finish 
                DbFile.SaveChanges();  //*/
                return "S";
            }
            catch { return "N"; }
        }

    }
}