using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data.Sql;
using System.Data.SqlClient;

namespace MTC
{
    /// <summary>
    /// Логика взаимодействия для AddClient.xaml
    /// </summary>
    public partial class AddClient : Window
    {
        int id;
        MTCDataContext dc = new MTCDataContext(Properties.Settings.Default.MTCConnectionString);
        MTCDataContext dcSer = new MTCDataContext(Properties.Settings.Default.MTCConnectionString);
        MTC.ViewModel.Clients clients;
        SqlConnection con;
        public AddClient()
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=IVAN\SQLEXPRESS;Initial Catalog=MTC;Integrated Security=True;MultipleActiveResultSets=True");
            clients = new ViewModel.Clients(con);
            ClientDataGrid.ItemsSource = clients.FillDataGrid("Клиенты");
            ClientDataGridSer.ItemsSource = clients.FillDataGrid("Тарифы");
        }

        void _UpdateTable(int i)
        {
            if (i == 1)
            {
                ClientDataGrid.ItemsSource = clients.FillDataGrid("Клиенты");
            }
            else
            {
                ClientDataGridSer.ItemsSource = clients.FillDataGrid("Тарифы");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if ((tbNumber.Text != "")&&(tbSurname.Text!="")&&(tbName.Text!="")&&(tbDate.Text!=""))
            {
                if (operationList.SelectedIndex == 0)
                {
                    if (!clients.AvailabilityClient(tbNumber.Text))
                    {
                        try
                        {
                            clients.AddClient(tbNumber.Text , tbSurname.Text ,tbName.Text , tbPatronymic.Text , tbDate.Text);
                            MessageBox.Show("Клиент добавлен");
                            _UpdateTable(1);
                        }
                        catch (System.Data.SqlClient.SqlException ex) //Catch SqlException
                        {
                            MessageBox.Show("Введите корректные данные");
                        }
                        catch (Exception ex) //Catch Other Exception
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else MessageBox.Show("Номер занят");
                }
                else
                    if (operationList.SelectedIndex == 1)
                    {
                        try
                        {
                            clients.ChangeClient(tbNumber.Text, tbSurname.Text, tbName.Text, tbPatronymic.Text, tbDate.Text,id);
                            MessageBox.Show("Клиент обновлен");
                            _UpdateTable(1);
                        }
                        catch (System.Data.SqlClient.SqlException ex) //Catch SqlException
                        {
                            MessageBox.Show("Введите корректные данные");
                        }
                        catch (Exception ex) //Catch Other Exception
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                        if (operationList.SelectedIndex == 2)
                        {
                            clients.DeleteClient(id);
                            MessageBox.Show("Клиент удален");
                            _UpdateTable(1);
                        }
            }
            else
                MessageBox.Show("Все поля кроме отчества должны быть заполнены");

        }

        string pole (int i,int s)
        {
            if (s == 1)
            {
                int selectedColumn = i;
                var selectedCell = ClientDataGrid.SelectedCells[selectedColumn];
                var cellContent = selectedCell.Column.GetCellContent(selectedCell.Item);
                return (cellContent as TextBlock).Text;
            }
            else
            {
                int selectedColumn = i;
                var selectedCell = ClientDataGridSer.SelectedCells[selectedColumn];
                var cellContent = selectedCell.Column.GetCellContent(selectedCell.Item);
                return (cellContent as TextBlock).Text;
            }
        }

        private void ClientDataGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (operationList.SelectedIndex !=0)
            {
                id = Convert.ToInt32(pole(0,1));
                tbNumber.Text = pole(1,1);
                tbSurname.Text = pole(2,1);
                tbName.Text = pole(3,1);
                tbPatronymic.Text = pole(4,1);
                tbDate.Text = pole(5,1);
                SaveButton.IsEnabled = true;
            }

        }
        int ch = 0;
        private void operationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (operationList.SelectedIndex == 1)
            {
                tbNumber.Text = "";
                tbSurname.Text = "";
                tbName.Text = "";
                tbPatronymic.Text = "";
                SaveButton.Content = "Изменить";
                tbNumber.IsEnabled = true;
                tbSurname.IsEnabled = true;
                tbName.IsEnabled = true;
                tbPatronymic.IsEnabled = true;
                tbDate.IsEnabled = true;
                SaveButton.IsEnabled = false;
            }
            else
                if (operationList.SelectedIndex == 2)
                {
                    tbNumber.Text = "";
                    tbSurname.Text = "";
                    tbName.Text = "";
                    tbPatronymic.Text = "";
                    SaveButton.Content = "Удалить";
                    tbNumber.IsEnabled = false;
                    tbSurname.IsEnabled = false;
                    tbName.IsEnabled = false;
                    tbPatronymic.IsEnabled = false;
                    tbDate.IsEnabled = false;
                    SaveButton.IsEnabled = false;
                }
                else
                    if ((operationList.SelectedIndex == 0) && (ch != 0))
                    {
                        SaveButton.Content = "Добавить";
                        tbNumber.IsEnabled = true;
                        tbSurname.IsEnabled = true;
                        tbName.IsEnabled = true;
                        tbPatronymic.IsEnabled = true;
                        tbDate.IsEnabled = true;
                        tbNumber.Text = "";
                        tbSurname.Text = "";
                        tbName.Text = "";
                        tbPatronymic.Text = "";
                        tbDate.SelectedDate = DateTime.Today;
                        SaveButton.IsEnabled = true;
                    }
            ch++;
        }

        private void operationListSer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (operationListSer.SelectedIndex == 1)
            {
                SaveButtonSer.Content = "Изменить";
                tbLocality.IsEnabled = true;
                tbDistrict.IsEnabled = true;
                tbRegion.IsEnabled = true;
                tbPrice.IsEnabled = true;
                tbConcession.IsEnabled = true;
                tbLocality.Text = "";
                tbDistrict.Text = "";
                tbRegion.Text = "";
                tbPrice.Text = "";
                tbConcession.Text = "";
                SaveButtonSer.IsEnabled = false;
            }
            else
                if (operationListSer.SelectedIndex == 2)
                {
                    SaveButtonSer.Content = "Удалить";
                    tbLocality.IsEnabled = false;
                    tbDistrict.IsEnabled = false;
                    tbRegion.IsEnabled = false;
                    tbPrice.IsEnabled = false;
                    tbConcession.IsEnabled = false;
                    tbLocality.Text = "";
                    tbDistrict.Text = "";
                    tbRegion.Text = "";
                    tbPrice.Text = "";
                    tbConcession.Text = "";
                    SaveButtonSer.IsEnabled = false;
                }
                else
                    if ((operationListSer.SelectedIndex == 0) && (ch > 1))
                    {
                        SaveButtonSer.Content = "Добавить";
                        tbLocality.IsEnabled = true;
                        tbDistrict.IsEnabled = true;
                        tbRegion.IsEnabled = true;
                        tbPrice.IsEnabled = true;
                        tbConcession.IsEnabled = true;
                        tbLocality.Text = "";
                        tbDistrict.Text = "";
                        tbRegion.Text = "";
                        tbPrice.Text = "";
                        tbConcession.Text = "";
                        SaveButtonSer.IsEnabled = true;
                    }
            ch++;
        }

        private void ClientDataGridSer_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (operationListSer.SelectedIndex != 0)
            {
                id = Convert.ToInt32(pole(0,2));
                tbRegion.Text = pole(1,2);
                tbDistrict.Text = pole(2,2);
                tbLocality.Text = pole(3,2);
                tbPrice.Text = pole(4,2);
                tbConcession.Text = pole(5,2);
                SaveButtonSer.IsEnabled = true;
            }
        }

        private void SaveButtonSer_Click(object sender, RoutedEventArgs e)
        {
            if ((tbRegion.Text != "") && (tbLocality.Text != "") && (tbPrice.Text != "") && (tbConcession.Text != ""))
            {
                if (operationListSer.SelectedIndex == 0)
                {
                    if (!clients.AvailabilityService(tbRegion.Text,tbDistrict.Text,tbLocality.Text))
                    {
                        try
                        {
                            clients.AddService(tbRegion.Text,tbDistrict.Text ,tbLocality.Text ,tbPrice.Text , tbConcession.Text);
                            MessageBox.Show("Тариф добавлен");
                            _UpdateTable(2);
                        }
                        catch (System.Data.SqlClient.SqlException ex) //Catch SqlException
                        {
                            MessageBox.Show("Введите корректные данные");
                        }
                        catch (Exception ex) //Catch Other Exception
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else MessageBox.Show("Такой тариф уже есть");
                }
                else
                    if (operationListSer.SelectedIndex == 1)
                    {
                        try
                        {
                            clients.ChangeService(tbRegion.Text, tbDistrict.Text, tbLocality.Text, tbPrice.Text, tbConcession.Text,id);
                            MessageBox.Show("Тариф обновлен");
                            _UpdateTable(2);
                        }
                        catch (System.Data.SqlClient.SqlException ex) //Catch SqlException
                        {
                            MessageBox.Show("Введите корректные данные");
                        }
                        catch (Exception ex) //Catch Other Exception
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                        if (operationListSer.SelectedIndex == 2)
                        {
                            clients.DeleteService(id);
                            MessageBox.Show("Тариф удален");
                            _UpdateTable(2);
                        }
            }
            else
                MessageBox.Show("Все поля кроме района должны быть заполнены");
        }
    }
}
