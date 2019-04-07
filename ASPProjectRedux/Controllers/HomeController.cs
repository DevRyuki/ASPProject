    using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPProjectRedux.Models;
using System.Text.RegularExpressions;

namespace ASPProject.Contollers
{
    public class HomeController : Controller
    {
        private LoginEntities db = new LoginEntities();
        
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
            if (HttpContext.Session["Username"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Login", "Home", new { signout=1 });

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
                string query = "SELECT COUNT(*) from Users where Usr=@un AND Password=@pw";
                SqlCommand Login = new SqlCommand(query, connect);
                Login.Parameters.AddWithValue("@un", Request.Form["UserName"]);
                Login.Parameters.AddWithValue("@pw", Request.Form["Password"]);
                object exec = Login.ExecuteScalar();
                Console.WriteLine(exec);
                int count = Convert.ToInt32(exec);

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
                Regex rxp = new Regex(@"^\D?(\d{3})\D?\D?(\d{3})\D?(\d{4})$")
                Regex rxe = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                if (!rxe.IsMatch(Request.Form["Email"]))
                {
                    return RedirectToAction("Signup", "Home", new { email = 1 });
                }
                if (!rxp.IsMatch(Request.Form["Phone"]))
                {
                    return RedirectToAction("Signup", "Home", new { phone = 1 });
                }
                string un = Request.Form["UserName"];
                if (db.Users.Any(x => x.Usr == un))
                {
                    return RedirectToAction("Signup", "Home", new { exists = 1 });
                }
                Guid id = Guid.NewGuid();
                SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["Login"].ConnectionString);
                connect.Open();
                SqlCommand INSERT = new SqlCommand("insert into Users (ID,Usr,Password,Phone,Email) values (@ID,@Usr,@Password,@Phone,@Email)", connect);
                INSERT.Parameters.AddWithValue("@ID", id);
                INSERT.Parameters.AddWithValue("@Usr", un);
                INSERT.Parameters.AddWithValue("@Password", Request.Form["Password"]);
                INSERT.Parameters.AddWithValue("@Email", Request.Form["Email"]);
                INSERT.Parameters.AddWithValue("@Phone", Request.Form["Phone"]);
                INSERT.ExecuteNonQuery();
                //Response.Write("Insert Sucessful!");
            }
            return RedirectToAction("Login", "Home", new { signup=1 });
        }

        public ActionResult Delete()
        {
            if (HttpContext.Session["Username"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if(Request.QueryString["id"] != null)
            {
                SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["Login"].ConnectionString);
                connect.Open();
                string query = "DELETE from Tasks where Id=@ID";
                SqlCommand taskdel = new SqlCommand(query, connect);
                taskdel.Parameters.AddWithValue("@ID", Request.QueryString["id"].ToString());
                //TODO: Message confirmation
                taskdel.ExecuteNonQuery();
                connect.Close();
            }
            return RedirectToAction("Index", "Home");
        }



        [HttpPost]
        public ActionResult AddTask()
        {
            if (HttpContext.Session["Username"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            string validate;
            if (Request.Form["addname"] == null || Request.Form["adddesc"] == null || Request.Form["adddate"] == null
            || Request.Form["addname"].Trim() == "" || Request.Form["adddesc"].Trim() == "" || Request.Form["adddate"].Trim() == "")
            {
                validate = "<div class='erralert'>Some fields are missing!</div>";
            }
            else
            {
                SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["Login"].ConnectionString);
                connect.Open();
                string insq = "INSERT INTO Tasks values(@an,@ads,@ada,@un)";
                SqlCommand ins = new SqlCommand(insq, connect);
                ins.Parameters.AddWithValue("@an", Request.Form["addname"]);
                ins.Parameters.AddWithValue("@ads", Request.Form["adddesc"]);
                ins.Parameters.AddWithValue("@ada", Request.Form["adddate"]);
                ins.Parameters.AddWithValue("@un", HttpContext.Session["Username"].ToString());
                if (ins.ExecuteNonQuery() <= 0)
                {
                    validate = "<div class='erralert'>Failed to insert task.</div>";
                }
                else
                {
                    validate = "<div class='succalert'>Successfully created task.</div>";
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

        public ActionResult Edit(int? id)
        {
            if (HttpContext.Session["Username"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            try
            {
                if (Request.Cookies["validatEdit"] != null)
                {
                    ViewBag.validate = new HtmlString(HttpUtility.UrlDecode(Request.Cookies.Get("validatEdit").Value));
                    Response.Cookies.Remove("validatEdit");
                }
            }
            catch (Exception e)
            {
                Response.Cookies.Remove("validatEdit");
            }
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "Home");
            }
            string usern = (string)HttpContext.Session["Username"];
            Task t = db.Tasks.First(x => x.usr == usern && x.Id == id.Value);
            if(t != null) {
                ViewBag.tname = t.name;
                ViewBag.tdesc = t.desc;
                ViewBag.tdate = t.date.ToString("yyyy-MM-dd");
                ViewBag.tid = id.Value;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult EditTask()
        {
            if (HttpContext.Session["Username"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (Request.Form["can"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            string validate = "";
            if (Request.Form["tid"] == null || Request.Form["tid"].Trim() == "" || Request.Form["tname"] == null || Request.Form["tdesc"] == null || Request.Form["tdate"] == null
            || Request.Form["tname"].Trim() == "" || Request.Form["tdesc"].Trim() == "" || Request.Form["tdate"].Trim() == "")
            {
                validate = "<div class='erralert'>Some fields are missing!</div>";
            }

            if (validate == "")
            {
                int id = 0;
                if (int.TryParse(Request.Form["tid"], out id))
                {
                    string usern = (string)HttpContext.Session["Username"];
                    ASPProjectRedux.Models.Task t = db.Tasks.First(x => x.usr == usern && x.Id == id);
                    if (t != null)
                    {
                        DateTime td = new DateTime();
                        t.name = Request.Form["tname"];
                        t.desc = Request.Form["tdesc"];
                        if (DateTime.TryParse(Request.Form["tdate"], out td))
                        {
                            t.date = td;
                            db.SaveChanges();
                        }
                        else
                        {
                            validate = "<div class='erralert'>Invalid date!</div>";
                        }
                    }
                    else
                    {
                        validate = "<div class='erralert'>Failed to update task!</div>";
                    }
                }
                else
                {
                    validate = "<div class='erralert'>Failed to update task!</div>";
                }
            }

            if (validate != "")
            {
                HttpCookie k = new HttpCookie("validatEdit", HttpUtility.UrlEncode(validate));
                k.Expires = DateTime.Now.AddSeconds(5);
                Response.Cookies.Add(k);
                return RedirectToAction("Edit", "Home");
            }

            return RedirectToAction("Index", "Home");
        }


    }
}