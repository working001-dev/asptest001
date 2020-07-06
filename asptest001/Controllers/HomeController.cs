using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asptest001.Models;
using System.Security.Cryptography;
using System.Text; 
namespace asptest001.Controllers
{
    public class HomeController : Controller
    {
        dbtestEntities db = new dbtestEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult QueryData(string uid, string pass)
        {
            //string ps = encryption(pass).ToString();
            //ps = EasyEncryption.MD5.ComputeMD5Hash(pass).ToString();
            
            var listemp = db.Employee_member.Where(w => w.Emp_username.Equals(uid) && w.Emp_password.Equals(pass));

            List < Employee_member > ls= listemp.ToList();
            ViewBag.dataLinQ = ls;
            return (ls.Count == 0) ? PartialView("Index", "ControllerName") : PartialView();
        }
        
        public ActionResult Get_table()
        {
            var table = db.Seal_order.Take(1000); 
            ViewBag.dtable = table;
            //return Json(new { data = table } );
            return PartialView();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page."; 
            return View();
        }

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
    }
}