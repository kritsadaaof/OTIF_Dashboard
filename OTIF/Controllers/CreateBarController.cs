using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.Ajax.Utilities;
using OTIF.Models;
using Rotativa;
using ZXing;

namespace OTIF.Controllers
{
    public class CreateBarController : Controller
    {
        private Entities DbFile = new Entities();
        // GET: CreateBar
        public ActionResult CreateBar()
        {
            return View();
        }
        public ActionResult PrintViewToPdf(string id)
        {
            try
            {
                int IDs =int.Parse(id);
               // var data = DbFile.Master_Planner.Where(a => a.ID.Equals(IDs)).FirstOrDefault();
                ViewBag.TAG = DbFile.Master_Planner.Where(a => a.ID.Equals(IDs)).ToList();
                //SavePrint(IDs); 
                return new PartialViewAsPdf("~/Views/CreateBar/FormPrintBar.cshtml")
                {
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Landscape,
                    PageMargins = { Top = 1, Bottom = 0 },
                    PageHeight = 100,
                    PageWidth = 30 
                };
            }
            catch { return null; }
        }

        public ActionResult RenderBarcode(string id)
        { 
            Image img = null;
            using (var ms = new MemoryStream())
            {

                var writer = new ZXing.BarcodeWriter() { Format = BarcodeFormat.CODE_128 };
                writer.Options.Height = 70;
                writer.Options.Width = 600;
                writer.Options.PureBarcode = true;
                img = writer.Write(id);
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                return File(ms.ToArray(), "image/jpeg");
            }
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
                var data = (from d in DbFile.Master_Planner
                                //  join Loc in DbFile.WMS_PD_Master_Location on d.Location equals Loc.Lo_Code into Locs
                                //  from Loc1 in Locs.DefaultIfEmpty()
                                //  where d.PRO_Status != ""
                            select new
                            {
                                // d.Bar_Kan,
                                d.ID,
                                PL_SO = d.PL_SO.Equals("") ? "<i style='font-size:16px; color: green'>-</i>" : d.PL_SO.Equals(null) ? "<i style='font-size:16px; color: green'>-</i>" : d.PL_SO,
                                PL_Machine = d.PL_Machine.Equals("") ? "<i style='font-size:16px; color: green'>-</i>" : d.PL_Machine.Equals(null) ? "<i style='font-size:16px; color: green'>-</i>" : d.PL_Machine,
                                PL_Process = d.PL_Process.Equals("") ? "<i style='font-size:16px; color: green'>-</i>" : d.PL_Process.Equals(null) ? "<i style='font-size:16px; color: green'>-</i>" : d.PL_Process,
                                PL_Tag = d.PL_Tag.Equals("") ? "<i style='font-size:16px; color: green'>-</i>" : d.PL_Tag.Equals(null) ? "<i style='font-size:16px; color: green'>-</i>" : d.PL_Tag,
                                PL_Qty_T =d.PL_Qty_T.Equals(null) ? "<i style='font-size:16px; color: green'>-</i>" : d.PL_Qty_T.ToString(),
                                PL_Type = d.PL_Type.Equals("") ? "<i style='font-size:16px; color: green'>-</i>" : d.PL_Type.Equals(null) ? "<i style='font-size:16px; color: green'>-</i>" : d.PL_Type,
                                PL_Reel_Size = d.PL_Reel_Size.Equals("") ? "<i style='font-size:16px; color: green'>-</i>" : d.PL_Reel_Size.Equals(null) ? "<i style='font-size:16px; color: green'>-</i>" : d.PL_Reel_Size,

                                // PL_Time = d.PL_Time
                                PL_Time = SqlFunctions.Replicate("0", 2 - SqlFunctions.DateName("dd", d.PL_Time).Trim().Length) +
                                SqlFunctions.DateName("dd", d.PL_Time) +
                                SqlFunctions.Replicate("/", 2 - SqlFunctions.StringConvert((double)d.PL_Time.Value.Month).TrimStart().Length) +
                                SqlFunctions.Replicate("0", 2 - SqlFunctions.StringConvert((double)d.PL_Time.Value.Month).TrimStart().Length) +
                                SqlFunctions.StringConvert((double)d.PL_Time.Value.Month).TrimStart() +
                                SqlFunctions.Replicate("/", 2 - SqlFunctions.StringConvert((double)d.PL_Time.Value.Month).TrimStart().Length) +
                                SqlFunctions.DateName("year", d.PL_Time).Trim()+" "+
                                SqlFunctions.Replicate("0", 2 - SqlFunctions.DateName("hh", d.PL_Time).Trim().Length) + SqlFunctions.DateName("hh", d.PL_Time)+":"+
                                SqlFunctions.Replicate("0", 2 - SqlFunctions.DateName("n", d.PL_Time).Trim().Length)+ SqlFunctions.DateName("n", d.PL_Time),

                            }).AsEnumerable().OrderByDescending(k=>k.PL_Time).GroupBy(k=>new { k.PL_Tag,k.PL_Machine }).Select(k=>k.First()).ToList();
                //.DistinctBy(a => a.PL_Tag) 
                //var fal_Start = DbFile.Master_Distance.Select(m => new { m.Dis_Start, m.ID }).AsEnumerable().GroupBy(x => x.Dis_Start).Select(x => x.First()).Where(f => f.Dis_Start != null).ToList();

                int totalRecords = data.Count;
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    data = data.Where(p => p.PL_SO.ToString().ToLower().Contains(search.ToLower()) ||
                        p.PL_Machine.ToString().ToLower().Contains(search.ToLower()) ||
                        p.PL_Process.ToString().ToLower().Contains(search.ToLower()) ||
                        p.PL_Tag.ToString().ToLower().Contains(search.ToLower()) ||
                        p.PL_Qty_T.ToString().ToLower().Contains(search.ToLower()) ||
                        p.PL_Type.ToString().ToLower().Contains(search.ToLower()) ||
                        p.PL_Reel_Size.ToString().ToLower().Contains(search.ToLower()) ||
                        p.PL_Time.ToString().ToLower().Contains(search.ToLower())
                     ).ToList();
                }
                if (!string.IsNullOrEmpty(preFix))
                {
                    data = data.Where(a => a.PL_SO.ToString().ToLower().Contains(preFix.ToLower())).ToList();
                }
                switch (order)
                {
                    case "0":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            data.OrderByDescending(p => p.PL_SO).ToList()
                            : data.OrderBy(p => p.PL_SO).ToList();
                        break;
                    case "1":
                        data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ?
                            data.OrderByDescending(p => p.PL_Machine).ToList()
                            : data.OrderBy(p => p.PL_Machine).ToList();
                        break;
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
                    //*/


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
                }

                int recFilter = data.Count;
                data = data.Skip(startRec).Take(pageSize).ToList();
                var modifiedData = data.Select(d =>
                    new
                    {
                      //  d.Bar_Kan,
                        d.ID,
                        d.PL_SO,
                        d.PL_Machine,
                        d.PL_Process,
                        d.PL_Tag,
                        d.PL_Time,
                        d.PL_Qty_T,
                        d.PL_Type,
                        d.PL_Reel_Size
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
        private string SavePrint(int ID)//กวาดข้อมูล Backlog,Planner ไปลงตาราง Master Data
        {
            try
            {
                var MS_PL = DbFile.Master_Planner.Where(y => y.ID.Equals(ID)).FirstOrDefault();
                var MS_BackLog = DbFile.Master_Backlog.Where(y => y.B_SO == MS_PL.PL_SO).FirstOrDefault();
                var MS_Production = DbFile.Master_Production.Where(y => y.PD_SO == MS_PL.PL_SO).FirstOrDefault();
                var CheckMData = DbFile.Master_Data.Where(y => y.PL_Tag == MS_PL.PL_Tag).FirstOrDefault();
                var MData = new Master_Data();
                if (CheckMData != null) //Edit
                {
                    if (MS_BackLog != null)
                    {
                        CheckMData.B_SO = MS_BackLog.B_SO;
                        CheckMData.B_SO = MS_BackLog.B_SO;
                        CheckMData.B_Customer = MS_BackLog.B_Customer;
                        CheckMData.B_Material = MS_BackLog.B_Material;
                        CheckMData.B_Material_Des = MS_BackLog.B_Material_Des;
                      //  CheckMData.Date_Create = MS_BackLog.Date_Create;
                      //  CheckMData.Date_PFD = MS_BackLog.Date_PFD;
                      //  CheckMData.Date_Delivery = MS_BackLog.Date_Delivery;
                        CheckMData.B_Cutting_Length = MS_BackLog.B_Cutting_Length;
                        CheckMData.B_Order_QTY = MS_BackLog.B_Order_QTY;
                        CheckMData.B_Order_Unit = MS_BackLog.B_Order_Unit;
                    }
                    if (MS_Production != null)
                    {
                        CheckMData.B_Cutting_Length = MS_Production.PD_CuttingLength;
                    }
                    
                    CheckMData.PL_Machine = MS_PL.PL_Machine;
                    CheckMData.PL_Process = MS_PL.PL_Process;
                    CheckMData.PL_Type = MS_PL.PL_Type;
                    CheckMData.PL_Qty = MS_PL.PL_Qty;
                    CheckMData.PL_Tag = MS_PL.PL_Tag;
                    CheckMData.PL_Qty_T = MS_PL.PL_Qty_T;
                    CheckMData.PL_Color = MS_PL.PL_Color; 
                    DbFile.SaveChanges();
                    return "S";
                }
                else //Add
                {
                    if (MS_BackLog != null)
                    {
                        MData.B_SO = MS_BackLog.B_SO;
                        MData.B_SO = MS_BackLog.B_SO;
                        MData.B_Customer = MS_BackLog.B_Customer;
                        MData.B_Material = MS_BackLog.B_Material;
                        MData.B_Material_Des = MS_BackLog.B_Material_Des;
                   //     MData.Date_Create = MS_BackLog.Date_Create;
                    //    MData.Date_PFD = MS_BackLog.Date_PFD;
                     //   MData.Date_Delivery = MS_BackLog.Date_Delivery;
                        MData.B_Cutting_Length = MS_BackLog.B_Cutting_Length;
                        MData.B_Order_QTY = MS_BackLog.B_Order_QTY;
                        MData.B_Order_Unit = MS_BackLog.B_Order_Unit;
                    }
                    else 
                    {
                        MData.B_SO = MS_PL.PL_SO;
                    }

                    if (MS_Production != null)
                    {
                        MData.B_Cutting_Length = MS_Production.PD_CuttingLength;
                    }
                    MData.PL_Machine = MS_PL.PL_Machine;
                    MData.PL_Process = MS_PL.PL_Process;
                    MData.PL_Type = MS_PL.PL_Type;
                    MData.PL_Qty = MS_PL.PL_Qty;
                    MData.PL_Tag = MS_PL.PL_Tag;
                    MData.PL_Qty_T = MS_PL.PL_Qty_T;
                    MData.PL_Color = MS_PL.PL_Color;
                    DbFile.Master_Data.Add(MData);
                    DbFile.SaveChanges();
                    return "S";
                }

            }
            catch { return "N"; }
        }//*/


    }
}