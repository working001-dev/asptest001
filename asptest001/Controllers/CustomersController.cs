using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asptest001.Models;
using System.IO;

namespace asptest001.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customers
        dbtestEntities1 dbt = new dbtestEntities1();
        TM_Factory fact = new TM_Factory();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add_customer()
        {
            TD_Tempolary_Factory tmf = new TD_Tempolary_Factory();
            int max_code = dbt.TD_Tempolary_Factory.Max( t => t.temp_id) + 1;
            tmf.temp_code = "N" + max_code.ToString().PadLeft(13, '0');
            tmf.temp_name = Request.QueryString["cust_name"];
            tmf.created_date = DateTime.Now;
            dbt.TD_Tempolary_Factory.Add(tmf);

            dbt.SaveChanges();

            return RedirectToAction("Contact", "Home", new { flg = "1", msg = "Add factory sucess!" });
        }


        public ActionResult Get_factory()
        {
            var fc = dbt.TM_Factory.Select(data => new { id = data.fact_id, name = data.fact_name }).ToList();
            var tf = dbt.TD_Tempolary_Factory.Select(data => new { id = data.temp_code, name = data.temp_name }).ToList();

            return Json(new { data = fc.Union(tf) });
        }
        public ActionResult Get_countrys()
        {
            var ct = dbt.TM_Countrys.Select(data => new { id = data.country_id, name = data.country_name_en, colors = data.color_tag }).ToList(); 

            return Json(new { data = ct });
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), "Master_excel.xlsx");
                    file.SaveAs(_path);
                }
                ViewBag.Message = "File Uploaded Successfully!!";
                return RedirectToAction("Contact", "Home", new { flg = "1", active = "2", msg = "Upload file sucess!" });
            }
            catch (Exception ex)
            {
                ViewBag.Message = "File upload failed!!";
                return RedirectToAction("Contact", "Home", new { flg = "0", active = "2", msg = "Upload fie fail! <hr>" + ex.Message });
            }
            
        }

    }
}
   