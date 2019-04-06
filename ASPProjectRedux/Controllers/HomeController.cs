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
            try
            {
                if (Request.Cookies["validat"] != null)
                {
                    ViewBag.validate = new HtmlString(HttpUtility.UrlDecode(Request.Cookies.Get("validat").Value));
                    Response.Cookies.Remove("validat");
                }
            }
            catch (Exception e)
            {
                Response.Cookies.Remove("validat");
            }
            return View();
        }

        public ActionResult Login(int? f)
        {
            if (f.HasValue)
            {
                ViewBag.failed = true;
            }
            return View();
        }

        public ActionResult Signout()
        {

            return View();

        }

        [HttpPost]
        public ActionResult LogoutPOST()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");

        }

        public ActionResult LoginPOST()
        {
            if (Request.Form["Signup"] != null)
            {
                return RedirectToAction("Signup", "Home");
            }
            else if (Request.Form["Login"] != null)
            {
                SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["Login"].ConnectionString);
                connect.Open();
                string query = "SELECT COUNT(*) from Users where Usr='" + Request.Form["UserName"] + "' AND Password='" + Request.Form["Password"] + "'";
                SqlCommand Login = new SqlCommand(query, connect);

                int count = Convert.ToInt32(Login.ExecuteScalar());

                connect.Close();
                if (count == 1)
                {
                    HttpContext.Session["Username"] = Request.Form["UserName"];
                    if (Request.Form["UserName"] == "admin")
                    {
                        return RedirectToAction("Index", "Users");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Home", new { f = 1 });
                }
            }
            return RedirectToAction("Login", "Home");
        }

     

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignupPOST()
        {
            if (Request.Form["Signup"] != null)
            {

                Guid id = Guid.NewGuid();
                SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["Login"].ConnectionString);
                connect.Open();
                SqlCommand INSERT = new SqlCommand("insert into Users (ID,Usr,Password,Phone,Email) values (@ID,@Usr,@Password,@Phone,@Email)", connect);
                INSERT.Parameters.AddWithValue("ID", id);
                INSERT.Parameters.AddWithValue("Usr", Request.Form["UserName"]);
                INSERT.Parameters.AddWithValue("Password", Request.Form["Password"]);
                INSERT.Parameters.AddWithValue("Email", Request.Form["Email"]);
                INSERT.Parameters.AddWithValue("Phone", Request.Form["Phone"]);
                INSERT.ExecuteNonQuery();
                Response.Write("Insert Sucessful!");
            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Delete()
        {
            if(Request.QueryString["id"] != null)
            {
                SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["Login"].ConnectionString);
                connect.Open();
                string query = "DELETE from Tasks where Id=" + Request.QueryString["id"].ToString();
                SqlCommand taskdel = new SqlCommand(query, connect);
                //TODO: Message confirmation
                taskdel.ExecuteNonQuery();
                connect.Close();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult AddTask()
        {
            string validate;
            if (Request.Form["addname"] == null || Request.Form["adddesc"] == null || Request.Form["adddate"] == null
            || Request.Form["addname"].Trim() == "" || Request.Form["adddesc"].Trim() == "" || Request.Form["adddate"].Trim() == "")
            {
                validate = "Some fields are missing! <br>";
            }
            else
            {
                SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["Login"].ConnectionString);
                connect.Open();
                string insq = "INSERT INTO Tasks values('" + Request.Form["addname"] + "','" + Request.Form["adddesc"] + "','" + Request.Form["adddate"] + "','" + HttpContext.Session["Username"].ToString() + "')";
                SqlCommand ins = new SqlCommand(insq, connect);
                if (ins.ExecuteNonQuery() <= 0)
                {
                    validate = "Failed to insert task. <br>";
                }
                else
                {
                    validate = "Successfully inserted task. <br>";
                }
                connect.Close();
            }
            if (validate != null && validate != "")
            {
                HttpCookie k = new HttpCookie("validat", HttpUtility.UrlEncode(validate));
                k.Expires = DateTime.Now.AddSeconds(5);
                Response.Cookies.Add(k);
            }
            return RedirectToAction("Index", "Home");
        } // 

        public ActionResult About()
        {
            return View();
        }


    }
}