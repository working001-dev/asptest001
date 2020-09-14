using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using asptest001.Models;
using System.Security.Cryptography;
using System.Text;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Sparkline;
using Newtonsoft.Json.Linq;
using asptest001.App_Start;
using System.Web; 

namespace asptest001.Controllers
{
    public class HomeController : Controller
    {
        dbtestEntities1 db = new dbtestEntities1();
        public ActionResult Index()
        {

            //Response.Redirect("Home", false);
            ViewBag.Login = ( Session["InsuredKey"] != null && Session["InsuredKey"].ToString() == "1" ) ? "login" : "logout";
            return View();
        }

        [HttpPost]
        public ActionResult Login(string uid, string pass)
        {
            //string ps = encryption(pass).ToString();
            //ps = EasyEncryption.MD5.ComputeMD5Hash(pass).ToString();
            pass = MD5Hash(pass);
            var listemp = db.Employee_member.Where(w => w.Emp_username.Equals(uid) && w.Emp_password.Equals(pass)).ToList();
            string login = "logout";
            if(listemp.Count > 0)
            {
                Session["InsuredKey"] = "1";
                Session["MemberCode"] = listemp[0].Emp_code;
                Session["MemberName"] = string.Format("{0}{1}", (listemp[0].Emp_efname != null) ? listemp[0].Emp_efname : "" , (listemp[0].Emp_elname != null) ? "  " + listemp[0].Emp_elname : "");
                login = "login";
                ViewBag.Login = login;
                return RedirectToAction("MainHome", "Home");
            }
            else
            {
                Session["InsuredKey"] = "0"; 
                ViewBag.Login = "login-fail";
                return RedirectToAction("Index", "Home");
            }

        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            ViewBag.Login = "logout";
            return RedirectToAction("Index", "Home");
        }


        [SessionAuthorize]
        public ActionResult MainHome()
        {
            ViewBag.Login = (Session["InsuredKey"] != null && Session["InsuredKey"].ToString() == "1") ? "login" : "logout";
            return View();
        }

        [SessionAuthorize]
        public ActionResult Issue()
        {
 
            return View();
        }

        [HttpGet]
        public ActionResult _get_Factory()
        {
            return Json(new { data = db.TM_Factory }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult _upd_Factory()
        {
            string fact_id = Request.QueryString["fact_code"];
            try
            {
                var tf = db.TM_Factory.Where( w => w.fact_id.Equals(fact_id) ).ToList();
                tf.ForEach(a =>
                {
                    a.fact_name = Request.QueryString["fact_name"];
                    a.fact_use =  Request.QueryString["fact_used"];
                    a.power =     Request.QueryString["fact_powe"];
                    a.mand =      Request.QueryString["fact_mand"];
                    a.money =     Request.QueryString["fact_mony"];
                    a.addess =    Request.QueryString["fact_adds"];
                    a.m =         Request.QueryString["fact_m"];
                    a.t =         Request.QueryString["fact_t"];
                    a.s =         Request.QueryString["fact_s"];
                    a.b =         Request.QueryString["fact_b"];
                    a.provinc =   Request.QueryString["fact_povc"];
                    a.city =      Request.QueryString["fact_city"];
                    a.phone =     Request.QueryString["fact_phon"];
                    a.create_date = DateTime.Now; 
                });  
                db.SaveChanges();
                return Json(new {status_id = "1", status_massage="success!...", updated = fact_id },JsonRequestBehavior.AllowGet);
            }catch(Exception e)
            {
                return Json(new { status_id = "0", status_massage = "Update Filed! : Error : " + e.Message }, JsonRequestBehavior.AllowGet);
            }

        } 


        public ActionResult Get_table()
        {
            var table = db.Seal_order.Take(10000); 
            ViewBag.dtable = table;
            //return Json(new { data = table } );
            return PartialView();
        }

        [SessionAuthorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page."; 
            return View();
        }

        [SessionAuthorize]
        public ActionResult Contact()
        {
            string flg = Request.QueryString["flg"];
            string atv = ( Request.QueryString["active"] != null ) ? Request.QueryString["active"] : "1";
            string msg = (Request.QueryString["msg"] != null) ? Request.QueryString["msg"] : "";
            ViewBag.Flg = flg;
            ViewBag.Atv = atv;
            ViewBag.Msg = msg; 
            return View();

        }

        public string encryption(String password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encrypt;
            UTF8Encoding encode = new UTF8Encoding();
            //encrypt the given password string into Encrypted data  
            encrypt = md5.ComputeHash(encode.GetBytes(password));
            StringBuilder encryptdata = new StringBuilder();
            //Create a new string by using the encrypted data  
            for (int i = 0; i < encrypt.Length; i++)
            {
                encryptdata.Append(encrypt[i].ToString());
            }
            return encryptdata.ToString();
        }
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
        public ActionResult CreateDocument()
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                //create the WorkSheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");
                
                //add some dummy data, note that row and column indexes start at 1
                for (int i = 1; i <= 30; i++)
                {
                    for (int j = 1; j <= 15; j++)
                    {
                        worksheet.Cells[i, j].Value = "Row " + i + ", Column " + j;
                    }
                }

                //fill column A with solid red color
                worksheet.Column(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Column(1).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FF0000"));
                worksheet.Cells["A1"].Hyperlink = new ExcelHyperLink("http://www.google.com") {  Display = "Click me!" };
                //set the font type for cells C1 - C30
                worksheet.Cells["C1:C30"].Style.Font.Size = 13;
                worksheet.Cells["C1:C30"].Style.Font.Name = "Calibri";
                worksheet.Cells["C1:C30"].Style.Font.Bold = true;
                worksheet.Cells["C1:C30"].Style.Font.Color.SetColor(Color.Blue);

                //fill row 4 with striped orange background
                worksheet.Row(4).Style.Fill.PatternType = ExcelFillStyle.DarkHorizontal;
                worksheet.Row(4).Style.Fill.BackgroundColor.SetColor(Color.Orange);

                //make the borders of cell F6 thick
                worksheet.Cells[6, 6].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                worksheet.Cells[6, 6].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                worksheet.Cells[6, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                worksheet.Cells[6, 6].Style.Border.Left.Style = ExcelBorderStyle.Thick;

                //make the borders of cells A18 - J18 double and with a purple color
                worksheet.Cells["A18:J18"].Style.Border.Top.Style = ExcelBorderStyle.Double;
                worksheet.Cells["A18:J18"].Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                worksheet.Cells["A18:J18"].Style.Border.Top.Color.SetColor(Color.Purple);
                worksheet.Cells["A18:J18"].Style.Border.Bottom.Color.SetColor(Color.Purple);


                worksheet.Cells["A17"].Value = "Stacked";
                worksheet.Cells["B17:Q17"].LoadFromArrays(new List<object[]> { new object[] { 2, -1, 3, -4, 8, 5, -12, 18, 99, 1, -4, 12, -8, 9, 0, -8 } });
                var sparklineStacked = worksheet.SparklineGroups.Add(eSparklineType.Line, worksheet.Cells["R17"], worksheet.Cells["B17:Q17"]);
                sparklineStacked.High = true;
                sparklineStacked.ColorHigh.SetColor(Color.Red);
                sparklineStacked.Low = true;
                sparklineStacked.ColorLow.SetColor(Color.Green);
                sparklineStacked.Negative = true;
                sparklineStacked.ColorNegative.SetColor(Color.Blue);

                //make all text fit the cells
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                //i use this to make all columms just a bit wider, text would sometimes still overflow after AutoFitColumns(). Bug?
                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    worksheet.Column(col).Width = worksheet.Column(col).Width + 1;
                }

                //make column H wider and set the text align to the top and right
                worksheet.Column(8).Width = 25;
                worksheet.Column(8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Column(8).Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                //convert the excel package to a byte array
                byte[] bin = excelPackage.GetAsByteArray();
                //clear the buffer stream
                Response.ClearHeaders();
                Response.Clear();
                Response.Buffer = true;
                //set the correct contenttype
                Response.ContentType = "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet";
                //set the correct length of the data being send
                Response.AddHeader("content-length", bin.Length.ToString());
                //set the filename for the excel package
                Response.AddHeader("content-disposition", "attachment; filename=\"ExcelDemo.xlsx\"");
                //send the byte array to the browser
                Response.OutputStream.Write(bin, 0, bin.Length);
                //cleanup
                Response.Flush();
                System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

            return RedirectToAction("Contact", "Home", new { flg = "1", active = "2", msg = "Upload file sucess!" });
        }

        public void Curexport_excel(List<EXCHANGERATES> dg)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int _head = 3;
            int _rows = 5;
            int _filt = _rows - 1;
            #region generate
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                
                
                List<object[]> col = new List<object[]> { new object[]
                {
                  #region header
                     "EXC_DATE"
                    ,"CAD"
                    ,"HKD"
                    ,"ISK"
                    ,"PHP"
                    ,"DKK"
                    ,"HUF"
                    ,"CZK"
                    ,"GBP"
                    ,"RON"
                    ,"SEK"
                    ,"IDR"
                    ,"INR"
                    ,"BRL"
                    ,"RUB"
                    ,"HRK"
                    ,"JPY"
                    ,"THB"
                    ,"CHF"
                    ,"EUR"
                    ,"MYR"
                    ,"BGN"
                    ,"TRY"
                    ,"CNY"
                    ,"NOK"
                    ,"NZD"
                    ,"ZAR"
                    ,"USD"
                    ,"MXN"
                    ,"SGD"
                    ,"AUD"
                    ,"ILS"
                    ,"KRW"
                    ,"PLN" }
                    #endregion
                };
                dbtestEntities1 db = new dbtestEntities1();
                //DateTime? dateT = DateTime.Parse("2020-07-31");
                var cr = db.EXCHANGERATES.Select(s => new
                {
                 #region content
                 s.EXC_DATE
                   ,s.CAD
                   ,s.HKD
                   ,s.ISK
                   ,s.PHP
                   ,s.DKK
                   ,s.HUF
                   ,s.CZK
                   ,s.GBP
                   ,s.RON
                   ,s.SEK
                   ,s.IDR
                   ,s.INR
                   ,s.BRL
                   ,s.RUB
                   ,s.HRK
                   ,s.JPY
                   ,s.THB
                   ,s.CHF
                   ,s.EUR
                   ,s.MYR
                   ,s.BGN
                   ,s.TRY
                   ,s.CNY
                   ,s.NOK
                   ,s.NZD
                   ,s.ZAR
                   ,s.USD
                   ,s.MXN
                   ,s.SGD
                   ,s.AUD
                   ,s.ILS
                   ,s.KRW
                   ,s.PLN
                 #endregion
                }).OrderByDescending(o => o.EXC_DATE);
                int _crow = col[0].Count();
                int _ccol = cr.Count();
                
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Exchenge report");
                //add all the content from the List<Book> collection, starting at cell A1
                worksheet.Cells["A"+ _head].LoadFromArrays(col);
                
                worksheet.Cells["A"+ _rows].LoadFromCollection(cr);
                worksheet.Column(1).Style.Numberformat.Format = "yyyy-MM-dd";
                #region Filter
                worksheet.Cells["A"+_filt].AutoFilter = true;
                #endregion
                #region width col
                worksheet.Column(1).Width = 15;
                for (int i=2;i<=_ccol;i++)
                {
                    worksheet.Column(i).Width = 8.11;
                }
                #endregion

                #region height
                worksheet.Row(_filt).Height = 10.25;
                #endregion


                byte[] bin = excelPackage.GetAsByteArray();
                //clear the buffer stream
                Response.ClearHeaders();
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet";
                Response.AddHeader("content-length", bin.Length.ToString());
                Response.AddHeader("content-disposition", "attachment; filename=\"ExcelDemo.xlsx\"");
                Response.OutputStream.Write(bin, 0, bin.Length);
                //cleanup
                Response.Flush();
                System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            #endregion
        }

        public ActionResult GetSamples()
        {
            
            return PartialView();
        }

        public ActionResult Pdf_export()
        {

            return new Rotativa.ActionAsPdf("GetSamples")
            {
                //FileName = "Test.pdf",
                PageMargins = { Left = 10, Bottom = 10, Right = 10, Top = 10 },
                PageOrientation = Rotativa.Options.Orientation.Landscape
            };
        }

        public ActionResult Curexport_report()
        {
            dbtestEntities1 db = new dbtestEntities1();
            DateTime? dateT = DateTime.Parse("2020-07-31");
            var cr = db.EXCHANGERATES;
            Curexport_excel( cr.ToList() );

            return RedirectToAction("Contact", "Home", new { flg = "1", active = "2", msg = "Upload file sucess!" });
        }
     
        public JsonResult Api_curentcy()
        {
            dbtestEntities1 db = new dbtestEntities1();
            DateTime? dateT = DateTime.Parse("2020-07-31");
            var cr = db.EXCHANGERATES.Select(s => new
            {
                   s.EXC_DATE
                  ,s.CAD
                  ,s.HKD
                  ,s.ISK
                  ,s.PHP
                  ,s.DKK
                  ,s.HUF
                  ,s.CZK
                  ,s.GBP
                  ,s.RON
                  ,s.SEK
                  ,s.IDR
                  ,s.INR
                  ,s.BRL
                  ,s.RUB
                  ,s.HRK
                  ,s.JPY
                  ,s.THB
                  ,s.CHF
                  ,s.EUR
                  ,s.MYR
                  ,s.BGN
                  ,s.TRY
                  ,s.CNY
                  ,s.NOK
                  ,s.NZD
                  ,s.ZAR
                  ,s.USD
                  ,s.MXN
                  ,s.SGD
                  ,s.AUD
                  ,s.ILS
                  ,s.KRW
                  ,s.PLN
            });
            return Json(cr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult setOnprogram()
        {
            Session["Online"] = "1";

            return Json(new { sta = "Ok" });
        }
    }
}