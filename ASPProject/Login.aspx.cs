using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPProject
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Signup_Click(object sender, EventArgs e)
        {
            Response.Redirect ("Signup.aspx");
        }

        protected void Log_Click(object sender, EventArgs e)
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["Login"].ConnectionString);
            connect.Open();
            SqlCommand INSERT = new SqlCommand("select User,Password where User = " + UserName.Text +" and Password =" + Password.Text, connect);
            int count = Convert.ToInt32(INSERT.ExecuteScalar());
            if (count == 1)
            {
                Response.Redirect("Signup.aspx");
            }
            else
            { }
            connect.Close();


        }
    }
}