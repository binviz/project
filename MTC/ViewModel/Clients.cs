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
    class Clients
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
        public Clients(SqlConnection stringConn)
        {
            con = stringConn;
        }
        public SqlDataReader FillDataGrid(string table)
        {
            SqlCommand com;
            Connection();
            using (com = new SqlCommand("select * from "+table, con))
            {
                SqlDataReader reader = com.ExecuteReader();
                return reader;
            }
        }
        public bool AvailabilityClient(string Number)
        {
            Connection();
            SqlCommand com = new SqlCommand("Select * from Клиенты where Номер_телефона='" + Number + "'", con);
            if (com.ExecuteScalar() == null)
                return false;
            else
                return true;
        }
        public void AddClient(string Number, string Surname, string Name, string Patronymic, string Date)
        {
            Connection();
            SqlCommand com;
            com = new SqlCommand("Insert into Клиенты (Номер_телефона, Фамилия, Имя, Отчество, Дата_регистрации) values('" + Number + "','" + Surname + "','" + Name + "','" + Patronymic + "','" + Date + "')", con);
            com.ExecuteNonQuery();
        }
        public void ChangeClient(string Number, string Surname, string Name, string Patronymic, string Date, int id)
        {
            Connection();
            SqlCommand com;
            com = new SqlCommand("Update Клиенты set Номер_телефона = " + Number + " , Фамилия = '" + Surname + "', Имя = '" + Name + "' , Отчество = '" + Patronymic + "', Дата_регистрации = '" + Date + "' where id_клиента = " + id, con);
            com.ExecuteNonQuery();
        }
        public void DeleteClient(int id)
        {
            Connection();
            SqlCommand com;
            com = new SqlCommand("Delete from Звонки  where id_клиента = " + id, con);
            com.ExecuteNonQuery();
            com = new SqlCommand("Delete from Клиенты  where id_клиента = " + id, con);
            com.ExecuteNonQuery();
        }
        public bool AvailabilityService(string Region, string District, string Locality)
        {
            Connection();
            SqlCommand com = new SqlCommand("Select * from Тарифы where Область='" + Region + "' and Район='" + District + "' and Населенный_пункт='" + Locality + "'", con);
            if (com.ExecuteScalar() == null)
                return false;
            else
                return true;
        }
        public void AddService(string Region, string District, string Locality, string Price, string Concession)
        {
            Connection();
            SqlCommand com;
            com = new SqlCommand("Insert into Тарифы (Область, Район, Населенный_пункт, Цена, Льготная_цена) values('" + Region + "','" + District + "','" + Locality + "','" + Price + "','" + Concession + "')", con);
            com.ExecuteNonQuery();
        }
        public void ChangeService(string Region, string District, string Locality, string Price, string Concession, int id)
        {
            Connection();
            SqlCommand com;
            com = new SqlCommand("Update Тарифы set Область = '" + Region + "' , Район = '" + District + "', Населенный_пункт = '" + Locality + "' , Цена = '" + Price + "', Льготная_цена = '" + Concession + "' where id_тарифа = '" + id + "'", con);
            com.ExecuteNonQuery();
        }
        public void DeleteService(int id)
        {
            Connection();
            SqlCommand com;
            com = new SqlCommand("Delete from Звонки  where id_тарифа = " + id, con);
            com.ExecuteNonQuery();
            com = new SqlCommand("Delete from Тарифы  where id_тарифа = " + id, con);
            com.ExecuteNonQuery();
        }
    }
}
