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
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Signup_Click(object sender, EventArgs e)
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["Login"].ConnectionString);
            connect.Open();

            int I = 1;
            I++;
            SqlCommand INSERT = new SqlCommand("insert into Users (ID,User,Password,Email,Phone) VALUES (@ID,@User,@Password,@Email,@Phone)", connect);
            INSERT.Parameters.AddWithValue("ID",I.ToString() );
            INSERT.Parameters.AddWithValue("User", UserName.Text);
            INSERT.Parameters.AddWithValue("Password", Password.Text);
            INSERT.Parameters.AddWithValue("Email", Email.Text);
            INSERT.Parameters.AddWithValue("Phone", PhoneNum.Text);

            INSERT.ExecuteNonQuery();
            Response.Write("Insert Sucessful!");

        }
    }
}