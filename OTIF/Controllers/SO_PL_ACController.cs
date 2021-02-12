using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using OTIF.Models;


namespace OTIF.Controllers
{
    public class SO_PL_ACController : Controller
    {
        // GET: SO_PL_AC
        //   private TranferDATAEntities DbTranfer = new TranferDATAEntities();
        private Entities DbOtif = new Entities();
        private SQSLXEntities DbSQS = new SQSLXEntities(); //Routing 
       // private TranferDATAEntities DbTranfer = new TranferDATAEntities(); //Routing 
        private WMS_PDEntities DbWMS_PD = new WMS_PDEntities();//Blacklog SO Now
        public ActionResult Index()
        {

            return View();
        }

        public string CheckSOPL(string MAT)
        {
            try
            {
                var data = (from PLSO in DbSQS.sqProduction
                            where PLSO.ItemNumber.Equals(MAT)
                            select new
                            {
                                PLSO.Operation,
                                PLSO.ItemNumber,
                                PLSO.SequenceNumber
                            }).AsEnumerable().OrderBy(k => k.SequenceNumber).GroupBy(k => new { k.Operation }).Select(k => k.First()).ToList();

                string jsonlog = new JavaScriptSerializer().Serialize(data);
                return jsonlog;
            }
           catch { return null; }
        }//CheckLocation  //CheckUse
        public string CheckActual(string SO)
        {
            // try
            //   {
        /*    var data = (from TR_PRO in DbOtif.OTIF_TR_Process
                        join MS_Ps in DbOtif.Master_Planner on TR_PRO.Bar_Kan equals MS_Ps.PL_Tag into MS_P1
                        from MS_P in MS_P1.DefaultIfEmpty()
                        where TR_PRO.Pro_SO.Equals(SO)&&MS_P.PL_Status !=null&&MS_P.PL_Process!="-"
                        select new
                        {
                            TR_PRO.Bar_Kan,
                            TR_PRO.RT_QTY 
                        }).ToList();

            string jsonlog = new JavaScriptSerializer().Serialize(data); 
            return  jsonlog; */ 
             var data = DbOtif.OTIF_PD_Process_Actual.Where(a=>a.SO.Equals(SO)).AsEnumerable()
                 .GroupBy(a => a.MasterProcess)
                 .Select(a => new
                 {

                     MasterProcess = a.Key,
                     QTY = a.Sum(b => b.RT_QTY)
                 }).ToList(); 

             string jsonlog = new JavaScriptSerializer().Serialize(data); 
            return  jsonlog;
            //  }
            //  catch { return null; }
        }//CheckLocation  //CheckUse
        public string CheckFG(string SO)
        {
            // try
            //   {
                var data = (from WMS_Product in DbWMS_PD.WMS_PD_Product
                            where WMS_Product.PRO_SO.Equals(SO)
                            select new
                            {
                                WMS_Product.PRO_Tag_No,
                                WMS_Product.PRO_Quantity
                            }).ToList();

                string jsonlog = new JavaScriptSerializer().Serialize(data); 
                return  jsonlog; 
           
            //  }
            //  catch { return null; }
        }//CheckLocation  //CheckUse
        public string Time(string SF_Time)
        {
            var time = SF_Time.Substring(0, 8);
            return time;
        }

        public string MAT(string MATs)
        {
            //var MAT = DbTranfer.GroupBy_Mat_SQS.Where(a=>a.ItemNumber.Equals(MATs)).FirstOrDefault();
            return "JAAGK04C004BA0";
        }
        public ActionResult LoadData()
        {
            JsonResult result = new JsonResult();
            try
            {
                // var MAT = DbTranfer.GroupBy_Mat_SQS.ToList();
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                var preFix = Request.Form.GetValues("columns[1][search][value]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                var data = (from Wms_MB in DbOtif.MASTER_SO
                            where Wms_MB.Customer != null && !Wms_MB.Material.StartsWith("T0") && Wms_MB.Unit != "EA" && Wms_MB.Unit != "TO"
                            //&& 
                            select new
                            {
                                // d.Bar_Kan,
                                SO = Wms_MB.SO,
                                Customer = Wms_MB.Customer,
                                Material = Wms_MB.Material.Equals("") ? "<i style='font-size:16px; color: green'>-</i>" : Wms_MB.Material.Equals(null) ? "<i style='font-size:16px; color: green'>-</i>" : Wms_MB.Material,
                                Material_Des = Wms_MB.Material_Des.Equals("") ? "<i style='font-size:16px; color: green'>-</i>" : Wms_MB.Material_Des.Equals(null) ? "<i style='font-size:16px; color: green'>-</i>" : Wms_MB.Material_Des
                                // Process = Otif_MB.Process.Equals("") ? "<i style='font-size:16px; color: green'>-</i>" : d.Process.Equals(null) ? "<i style='font-size:16px; color: green'>-</i>" : d.Process,
                                ,
                                Date_Delivery = Wms_MB.Date_Delivery.ToString(),
                                Order_QTY = Wms_MB.Order_QTY,
                                Wms_MB.Unit

                            }).ToList();
                int totalRecords = data.Count;
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    data = data.Where(p => p.SO.ToString().ToLower().Contains(search.ToLower()) //||
                                                                                                // p.Material_Des.ToString().ToLower().Contains(search.ToLower())// ||
                                                                                                //  p.PL_Reel_Size.ToString().ToLower().Contains(search.ToLower()) ||
                                                                                                //   p.PL_Time.ToString().ToLower().Contains(search.ToLower())
                     ).ToList();
                }
                if (!string.IsNullOrEmpty(preFix))
                {
                    data = data.Where(a => a.Material.ToString().ToLower().Contains(preFix.ToLower())).ToList();
                }
                switch (order)
                {
                    case "0":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            data.OrderByDescending(p => p.SO).ToList()
                            : data.OrderBy(p => p.SO).ToList();
                        break;
                    case "1":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            data.OrderByDescending(p => p.Material).ToList()
                            : data.OrderBy(p => p.Material).ToList();
                        break;
                    default:
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            data.OrderByDescending(p => p.Material).ToList()
                            : data.OrderBy(p => p.Material).ToList();
                        break;
                        /*
                    case "2":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            data.OrderByDescending(p => p.PL_Process).ToList()
                            : data.OrderBy(p => p.PL_Process).ToList();
                        break;
                    case "3":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            data.OrderByDescending(p => p.PL_Qty_T).ToList()
                            : data.OrderBy(p => p.PL_Qty_T).ToList();
                        break;
                    case "4":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            data.OrderByDescending(p => p.PL_Type).ToList()
                            : data.OrderBy(p => p.PL_Type).ToList();
                        break;
                    case "5":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            data.OrderByDescending(p => p.PL_Reel_Size).ToList()
                            : data.OrderBy(p => p.PL_Reel_Size).ToList();
                        break;
                    case "6":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            data.OrderByDescending(p => p.PL_Tag).ToList()
                            : data.OrderBy(p => p.PL_Tag).ToList();
                        break;



                    case "7":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            data.OrderByDescending(p => p.ID).ToList()
                            : data.OrderBy(p => p.ID).ToList();
                        break;

                    default:
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            data.OrderByDescending(p => p.ID).ToList()
                            : data.OrderBy(p => p.ID).ToList();
                        break; 
                        */
                }

                int recFilter = data.Count;
                data = data.Skip(startRec).Take(pageSize).ToList();
                var modifiedData = data.Select(d =>
                    new
                    {
                        d.SO,
                        d.Customer,
                        d.Material,
                        d.Material_Des,
                        d.Date_Delivery,
                        d.Order_QTY,
                        d.Unit

                        //  d.SequenceNumber
                        //  d.Material_Des
                        /*  d.PL_Machine,
                          d.PL_Process,
                          d.PL_Tag,
                          d.PL_Time,
                          d.PL_Qty_T,
                          d.PL_Type,
                          d.PL_Reel_Size */
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
        }
    }
}