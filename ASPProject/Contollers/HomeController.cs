using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPProject.Contollers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult Signup()
        {
            return View();
        }

        public string Delete()
        {
            if(Request.QueryString["id"] == null)
            {
                Response.Redirect("~/Home/Index");
            }
            else
            {
                SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["Login"].ConnectionString);
                connect.Open();
                string query = "DELETE from Tasks where Id=" + Request.QueryString["id"].ToString();
                SqlCommand taskdel = new SqlCommand(query, connect);
                //TODO: Message confirmation
                taskdel.ExecuteNonQuery();
                connect.Close();
                Response.Redirect("~/Home/Index");
            }
            return "";
        }
    }
}