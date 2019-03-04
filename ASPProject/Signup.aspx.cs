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
            Guid id = Guid.NewGuid();
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["Login"].ConnectionString);
            connect.Open();
            SqlCommand INSERT = new SqlCommand("insert into Users (ID,Usr,Password,Phone,Email) values (@ID,@Usr,@Password,@Phone,@Email)", connect);
            INSERT.Parameters.AddWithValue("ID",id );
            INSERT.Parameters.AddWithValue("Usr", UserName.Text);
           INSERT.Parameters.AddWithValue("Password", Password.Text);
            INSERT.Parameters.AddWithValue("Email", Email.Text);
            INSERT.Parameters.AddWithValue("Phone", PhoneNum.Text);
            INSERT.ExecuteNonQuery();
            Response.Write("Insert Sucessful!");

        }
    }
}