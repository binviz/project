using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.Sql;
using System.Data.SqlClient;
namespace MTC.ViewModel
{
    class Authorization
    {
        public SqlConnection con;
        public Authorization(SqlConnection stringConn)
        {
            con = stringConn;
        }
        public Authorization()
        {
            con = new SqlConnection(@"Data Source=IVAN\SQLEXPRESS;Initial Catalog=MTC;Integrated Security=True");
        }
        public bool Availability(string Login, string Password)
        {
            SqlCommand com = new SqlCommand("Select * from Users where login='" + Login + "' and password='" + Password + "'", con);
            if (com.ExecuteScalar() == null)
                return false;
            else
                return true;
        }
        public string Role(string Login, string Password)
        {
            SqlCommand comRole = new SqlCommand("select Роль from Users where login='" + Login + "' and password='" + Password + "'", con);
            return ((string)comRole.ExecuteScalar()).ToString().Replace(" ", "");
        }
    }
}
