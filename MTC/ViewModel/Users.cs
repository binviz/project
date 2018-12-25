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
    class Users
    {
        public SqlConnection con;
        private void Connection()
        {
            if (con.State != System.Data.ConnectionState.Open)
            {
                con.Close();
                con.Open();
            }
        }
        public Users(SqlConnection stringConn)
        {
            con = stringConn;
        }
        public SqlDataReader FillDataGrid()
        {
            SqlCommand com;
            Connection();
            using (com = new SqlCommand("select * from Users", con))
            {
                SqlDataReader reader = com.ExecuteReader();
                return reader;
            }
        }
        public bool Availability(string Login)
        {
            Connection();
            SqlCommand com = new SqlCommand("Select * from Users where login='" + Login + "'", con);
            if (com.ExecuteScalar() == null)
                return false;
            else
                return true;
        }
        public void AddUsers(string Login, string Password, string role, string Surname, string Name, string Patronymic, string Date)
        {
            Connection();
            SqlCommand com;
            com = new SqlCommand("Insert into Users (login, password, Роль, Фамилия, Имя, Отчество, Дата_регистрации) values('" + Login + "','" + Password + "','" + role + "','" + Surname + "','" + Name + "','" + Patronymic + "','" + Date + "')", con);
            com.ExecuteNonQuery();
        }
        public void ChangeUsers(string Login, string Password, string role, string Surname, string Name, string Patronymic, string Date,int id)
        {
            Connection();
            SqlCommand com;
            com = new SqlCommand("Update Users set login = '" + Login + "' , password = '" + Password + "', Роль = '" + role + "' , Фамилия = '" + Surname + "', Имя = '" + Name + "', Отчество = '" + Patronymic + "', Дата_регистрации = '" + Date + "' where id_user = '" + id + "'", con);
            com.ExecuteNonQuery();
        }
        public void DeleteUsers(int id)
        {
            Connection();
            SqlCommand com;
            com = new SqlCommand("Delete from Users  where id_user = " + id, con);
            com.ExecuteNonQuery();
        }
    }
}
