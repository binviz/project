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
    class Calls
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
        public Calls(SqlConnection stringConn)
        {
            con = stringConn;
        }
        public SqlDataReader FillDataGrid(string table)
        {
            SqlCommand com;
            Connection();
            using (com = new SqlCommand("select * from " + table, con))
            {
                SqlDataReader reader = com.ExecuteReader();
                return reader;
            }
        }
        public SqlDataReader FillDataGridCall()
        {
            SqlCommand com;
            Connection();
            using (com = new SqlCommand("select Звонки.id_звонка, Звонки.Дата, Город, Звонки.Номер_телефона, Длительность, Отправка_счета, Оплата_счета, Клиенты.Номер_телефона as Исходящий_номер, Область, Район, Населенный_пункт from Клиенты, Тарифы, Звонки where Клиенты.id_клиента=Звонки.id_клиента and Звонки.id_тарифа=Тарифы.id_тарифа", con))
            {
                SqlDataReader reader = com.ExecuteReader();
                return reader;
            }
        }
        public SqlDataReader SearchDataGridCall(string date, string City, string Number, string Duration, int mess, int payment, string NumberOut, string Region, string District, string Locality)
        {
            SqlCommand com;
            Connection();
            using (com = new SqlCommand("select Звонки.id_звонка, Звонки.Дата, Город, Звонки.Номер_телефона, Длительность, Отправка_счета, Оплата_счета, Клиенты.Номер_телефона as Исходящий_номер, Область, Район, Тарифы.Населенный_пункт from Клиенты, Тарифы, Звонки where Клиенты.id_клиента=Звонки.id_клиента and Звонки.id_тарифа=Тарифы.id_тарифа and Звонки.Дата like '%" + date + "%' and Город like '%" + City + "%' and Звонки.Номер_телефона like '%" + Number + "%' and Длительность like '%" + Duration + "%' and Отправка_счета like '%" + mess + "%' and Оплата_счета like '%" + payment + "%' and Клиенты.Номер_телефона like '%" + NumberOut + "%' and Область like '%" + Region + "%' and Район like '%" + District + "%' and Населенный_пункт like '%" + Locality + "%'", con))
            {
                SqlDataReader reader = com.ExecuteReader();
                return reader;
            }
        }
        public SqlDataReader SearchDataGridClient(string ClientNumber, string ClientSurname, string ClientName, string ClientPatronymic, string date)
        {
            SqlCommand com;
            Connection();
            using (com = new SqlCommand("select * from Клиенты where Номер_телефона like '%" + ClientNumber + "%' and Фамилия like '%" + ClientSurname + "%' and Имя like '%" + ClientName + "%' and Отчество like '%" + ClientPatronymic + "%' and Дата_регистрации like '%" + date + "%'", con))
            {
                SqlDataReader reader = com.ExecuteReader();
                return reader;
            }
        }
        public SqlDataReader SearchDataGridService(string ServiceRegion, string ServiceDistrict, string ServiceLocality, string ServicePrice, string ServicePreferential)
        {
            SqlCommand com;
            Connection();
            using (com = new SqlCommand("select * from Тарифы where Область like '%" + ServiceRegion + "%' and Район like '%" + ServiceDistrict + "%' and Населенный_пункт like '%" + ServiceLocality + "%' and Цена like '%" + ServicePrice + "%' and Льготная_цена like '%" + ServicePreferential + "%'", con))
            {
                SqlDataReader reader = com.ExecuteReader();
                return reader;
            }
        }
        public void AddCall(string CallDate, string CallCity, string idClient, string CallNumber, string CallDuration, string mess, string payment, string idService)
        {
            Connection();
            SqlCommand com;
            com = new SqlCommand("Insert into Звонки (Дата, Город, id_клиента, Номер_телефона, Длительность, Отправка_счета, Оплата_счета, id_тарифа) values('" + CallDate + "','" + CallCity + "','" + idClient + "','" + CallNumber + "','" + CallDuration + "','" + mess + "','" + payment + "','" + idService + "')", con);
            com.ExecuteNonQuery();
        }
        public void ChangeCall(string CallDate, string CallCity, string idClient, string CallNumber, string CallDuration, string mess, string payment, string idService, string idCall)
        {
            Connection();
            SqlCommand com;
            com = new SqlCommand("Update Звонки set Дата = '" + CallDate + "' , Город = '" + CallCity + "', id_клиента = '" + idClient + "' , Номер_телефона = '" + CallNumber + "', Длительность = '" + CallDuration + "', Отправка_счета = '" + mess + "',  Оплата_счета = '" + payment + "',  id_тарифа = '" + idService + "' where id_звонка = '" + idCall + "'", con);
            com.ExecuteNonQuery();
        }
        public void DeleteCall(string idCall)
        {
            Connection();
            SqlCommand com;
            com = new SqlCommand("Delete from Звонки where id_звонка = '" + idCall + "'", con);
            com.ExecuteNonQuery();
        }
        public int idClient(string idCall)
        {
            SqlCommand comClient = new SqlCommand("select id_клиента from Звонки where id_звонка='" + idCall + "'", con);
            return Convert.ToInt32(comClient.ExecuteScalar());
        }
        public int idService(string idCall)
        {
            SqlCommand comSer = new SqlCommand("select id_тарифа from Звонки where id_звонка='" + idCall.ToString() + "'", con);
            return Convert.ToInt32(comSer.ExecuteScalar());
        }
    }
}
