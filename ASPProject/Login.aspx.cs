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
            Response.Write(UserName.Text );
            Response.Write(Password.Text);
 
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["Login"].ConnectionString);
            connect.Open();
            string query = "SELECT COUNT(*) from Users where User='" + UserName.Text + "' AND Password='" + Password.Text + "'";
            SqlCommand Login = new SqlCommand(query, connect);

            int count = Convert.ToInt32(Login.ExecuteScalar());
            
            if (count == 1)
            {
                Status.Text = "Complete";
            }
            else
            {
                Status.Text = "F";
            }
            connect.Close();


        }
    }
}