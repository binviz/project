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
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        int id;
        SqlConnection con;
        MTC.ViewModel.Users users;
        public AddUser()
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=IVAN\SQLEXPRESS;Initial Catalog=MTC;Integrated Security=True;MultipleActiveResultSets=True");
            users = new ViewModel.Users(con);
            UsertDataGrid.ItemsSource = users.FillDataGrid();
            tbDate.SelectedDate = DateTime.Today;
        }
        int ch = 0;
        private void operationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (operationList.SelectedIndex == 1)
            {
                tbDate.Text = "";
                tbLogin.Text = "";
                tbPassword.Text = "";
                tbSurname.Text = "";
                tbName.Text = "";
                tbPatronymic.Text = "";
                SaveButton.Content = "Изменить";
                tbLogin.IsEnabled = true;
                tbPassword.IsEnabled = true;
                tbSurname.IsEnabled = true;
                tbName.IsEnabled = true;
                tbPatronymic.IsEnabled = true;
                tbDate.IsEnabled = true;
                operationListRole.IsEnabled = true;
                SaveButton.IsEnabled = false;
            }
            else
                if (operationList.SelectedIndex == 2)
                {
                    tbDate.Text = "";
                    tbLogin.Text = "";
                    tbPassword.Text = "";
                    tbSurname.Text = "";
                    tbName.Text = "";
                    tbPatronymic.Text = "";
                    SaveButton.Content = "Удалить";
                    tbLogin.IsEnabled = false;
                    tbPassword.IsEnabled = false;
                    tbSurname.IsEnabled = false;
                    tbName.IsEnabled = false;
                    tbPatronymic.IsEnabled = false;
                    tbDate.IsEnabled = false;
                    operationListRole.IsEnabled = false;
                    SaveButton.IsEnabled = false;
                }
                else
                    if ((operationList.SelectedIndex == 0) && (ch != 0))
                    {
                        SaveButton.Content = "Добавить";
                        tbLogin.IsEnabled = true;
                        tbPassword.IsEnabled = true;
                        tbSurname.IsEnabled = true;
                        tbName.IsEnabled = true;
                        tbPatronymic.IsEnabled = true;
                        tbDate.IsEnabled = true;
                        operationListRole.IsEnabled = true;
                        tbDate.Text = "";
                        tbLogin.Text = "";
                        tbPassword.Text = "";
                        tbSurname.Text = "";
                        tbName.Text = "";
                        tbPatronymic.Text = "";
                        tbDate.SelectedDate = DateTime.Today;
                        SaveButton.IsEnabled = true;
                    }
            ch++;
        }
        string pole(int i)
        {
                int selectedColumn = i;
                var selectedCell = UsertDataGrid.SelectedCells[selectedColumn];
                var cellContent = selectedCell.Column.GetCellContent(selectedCell.Item);
                return (cellContent as TextBlock).Text;
        }
        private void UsertDataGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (operationList.SelectedIndex != 0)
            {
                id = Convert.ToInt32(pole(0));
                tbLogin.Text = pole(1);
                tbPassword.Text = pole(2);
                if (pole(3) == "Технолог            ")
                    operationListRole.SelectedIndex = 0;
                else
                    if (pole(3) == "Оператор            ")
                        operationListRole.SelectedIndex = 1;
                    else
                        operationListRole.SelectedIndex = 2;
                tbSurname.Text = pole(4);
                tbName.Text = pole(5);
                tbPatronymic.Text = pole(6);
                tbDate.Text = pole(7);
                SaveButton.IsEnabled = true;
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if ((tbLogin.Text != "") && (tbPassword.Text != "") && (tbSurname.Text != "") && (tbName.Text != "") && (tbDate.Text != ""))
            {
                string role;
                if (operationListRole.SelectedIndex == 0)
                    role = "Технолог";
                else
                    if (operationListRole.SelectedIndex == 1)
                        role = "Оператор";
                    else
                        role = "Администратор";
                if (operationList.SelectedIndex == 0)
                {
                    if (!users.Availability(tbLogin.Text))
                    {
                        try
                        {
                            users.AddUsers(tbLogin.Text ,tbPassword.Text ,role ,tbSurname.Text ,tbName.Text ,tbPatronymic.Text,tbDate.Text);
                            MessageBox.Show("Пользователь добавлен");
                            UsertDataGrid.ItemsSource = users.FillDataGrid();
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
                    else MessageBox.Show("Такой логин уже есть");
                }
                else
                    if (operationList.SelectedIndex == 1)
                    {
                        try
                        {
                            users.ChangeUsers(tbLogin.Text, tbPassword.Text, role, tbSurname.Text, tbName.Text, tbPatronymic.Text, tbDate.Text,id);
                            MessageBox.Show("Пользователь обновлен");
                            UsertDataGrid.ItemsSource = users.FillDataGrid();
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
                            users.DeleteUsers(id);
                            MessageBox.Show("Пользователь удален");
                            UsertDataGrid.ItemsSource = users.FillDataGrid();
                        }
            }
            else
                MessageBox.Show("Все поля кроме отчества должны быть заполнены");
        }
    }
}
