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
    /// Логика взаимодействия для addCall.xaml
    /// </summary>
    public partial class addCall : Window
    {
        MTC.ViewModel.Calls calls;
        SqlConnection con;
        public addCall()
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=IVAN\SQLEXPRESS;Initial Catalog=MTC;Integrated Security=True;MultipleActiveResultSets=True");
            calls = new ViewModel.Calls(con);
            DateGridClient.ItemsSource = calls.FillDataGrid("Клиенты");
            DateGridService.ItemsSource = calls.FillDataGrid("Тарифы");
            DateGridCall.ItemsSource = calls.FillDataGridCall();
            tbCallDate.Text = DateTime.Now.ToString();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            int mess = 0, payment = 0;
            if (cbMess.IsChecked.Value)
                mess = 1;
            if (cbPayment.IsChecked.Value)
                payment = 1;
            String date = "";
            if (tbDate.Text != "")
                date = Convert.ToDateTime(tbDate.Text).ToString("yyyy'-'MM'-'dd");
            DateGridCall.ItemsSource = calls.SearchDataGridCall(date.ToString() , tbCity.Text, tbNumber.Text ,tbDuration.Text , mess ,payment,tbNumberOut.Text,tbRegion.Text,tbDistrict.Text,tbLocality.Text);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            DateGridCall.ItemsSource = calls.FillDataGridCall();
            tbCity.Text = "";
            tbDate.Text = "";
            tbDistrict.Text = "";
            tbDuration.Text = "";
            tbLocality.Text = "";
            tbNumber.Text = "";
            tbNumberOut.Text = "";
            tbRegion.Text = "";
            cbMess.IsChecked = false;
            cbPayment.IsChecked = false;
        }

        private void SearchClietnButton_Click(object sender, RoutedEventArgs e)
        {
            String date = "";
            if (tbClietnDate.Text != "")
                date = Convert.ToDateTime(tbClietnDate.Text).ToString("yyyy'-'MM'-'dd");
            DateGridClient.ItemsSource = calls.SearchDataGridClient(tbClientNumber.Text, tbClientSurname.Text, tbClientName.Text, tbClientPatronymic.Text, date.ToString());
        }
        void ClearClient()
        {
            DateGridClient.ItemsSource = calls.FillDataGrid("Клиенты");
            tbClientName.Text = "";
            tbClientNumber.Text = "";
            tbClientPatronymic.Text = "";
            tbClientSurname.Text = "";
            tbClietnDate.Text = "";
        }
        private void ClearClientButton_Click(object sender, RoutedEventArgs e)
        {
            ClearClient();
        }

        private void SearchServiceButton_Click(object sender, RoutedEventArgs e)
        {
            DateGridService.ItemsSource = calls.SearchDataGridService(tbServiceRegion.Text , tbServiceDistrict.Text , tbServiceLocality.Text ,tbServicePrice.Text ,tbServicePreferential.Text);
        }
        void ClearService()
        {
            DateGridService.ItemsSource = calls.FillDataGrid("Тарифы");
            tbServiceDistrict.Text = "";
            tbServiceLocality.Text = "";
            tbServicePreferential.Text = "";
            tbServicePrice.Text = "";
            tbServiceRegion.Text = "";
        }
        private void ClearServiceButton_Click(object sender, RoutedEventArgs e)
        {
            ClearService();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if ((tbCallDate.Text != "") && (tbCallCity.Text != "") && (tbCallDuration.Text != "") && (tbCallNumber.Text != ""))
            {
                if (operationList.SelectedIndex == 0)
                {
                    int mess = 0, payment = 0;
                    if (cbCallMess.IsChecked.Value)
                        mess = 1;
                    if (cbCallPayment.IsChecked.Value)
                        payment = 1;
                    try
                    {
                        calls.AddCall(tbCallDate.Text ,tbCallCity.Text , idClient.ToString() ,tbCallNumber.Text ,tbCallDuration.Text , mess.ToString() ,payment.ToString(),idService.ToString());
                        MessageBox.Show("Звонок добавлен");
                        DateGridCall.ItemsSource = calls.FillDataGridCall();
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
                    if (operationList.SelectedIndex == 1)
                    {
                        int mess = 0, payment = 0;
                        if (cbCallMess.IsChecked.Value)
                            mess = 1;
                        if (cbCallPayment.IsChecked.Value)
                            payment = 1;
                        try
                        {
                            calls.ChangeCall(tbCallDate.Text, tbCallCity.Text, idClient.ToString(), tbCallNumber.Text, tbCallDuration.Text, mess.ToString(), payment.ToString(), idService.ToString(), idCall.ToString());
                            MessageBox.Show("Звонок обновлен");
                            DateGridCall.ItemsSource = calls.FillDataGridCall();
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
                            calls.DeleteCall(idCall.ToString());
                            MessageBox.Show("Звонок удален");
                            DateGridCall.ItemsSource = calls.FillDataGridCall();
                        }
            }
            else
                MessageBox.Show("Все поля должны быть заполнены");

        }

        int idClient=0, idService=0, idCall=0;
        string pole(int i, int s)
        {
            if (s == 0)
            {
                int selectedColumn = i;
                var selectedCell = DateGridCall.SelectedCells[selectedColumn];
                var cellContent = selectedCell.Column.GetCellContent(selectedCell.Item);
                if (i<5)
                return (cellContent as TextBlock).Text;
                else
                    return (cellContent as CheckBox).IsChecked.Value.ToString();

            }
            else
            if (s == 1)
            {
                int selectedColumn = i;
                var selectedCell = DateGridClient.SelectedCells[selectedColumn];
                var cellContent = selectedCell.Column.GetCellContent(selectedCell.Item);
                return (cellContent as TextBlock).Text;
            }
            else
            {
                int selectedColumn = i;
                var selectedCell = DateGridService.SelectedCells[selectedColumn];
                var cellContent = selectedCell.Column.GetCellContent(selectedCell.Item);
                return (cellContent as TextBlock).Text;
            }
        }

        private void DateGridClient_MouseUp(object sender, MouseButtonEventArgs e)
        {
                idClient = Convert.ToInt32(pole(0, 1));
        }

        private void DateGridService_MouseUp(object sender, MouseButtonEventArgs e)
        {
            idService = Convert.ToInt32(pole(0, 2));
        }
        void SelectRowByIndex(DataGrid dataGrid, int rowIndex)
        {
            if (!dataGrid.SelectionUnit.Equals(DataGridSelectionUnit.FullRow))
                throw new ArgumentException("The SelectionUnit of the DataGrid must be set to FullRow.");

            if (rowIndex < 0 || rowIndex > (dataGrid.Items.Count - 1))
                throw new ArgumentException(string.Format("{0} is an invalid row index.", rowIndex));

            dataGrid.SelectedItems.Clear();
            object item = dataGrid.Items[rowIndex]; 
            dataGrid.SelectedItem = item;

            DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            if (row == null)
            {
                dataGrid.ScrollIntoView(item);
                row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            }
        }
        private void DateGridCall_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (operationList.SelectedIndex != 0)
            {
                idCall = Convert.ToInt32(pole(0, 0));
                tbCallDate.Text = pole(1, 0);
                tbCallCity.Text = pole(2, 0);
                tbCallNumber.Text = pole(3, 0);
                tbCallDuration.Text = pole(4, 0);
                if (pole(5, 0).ToString() == "True")
                    cbCallMess.IsChecked = true;
                else
                    cbCallMess.IsChecked = false;
                if (pole(6, 0).ToString() == "True")
                    cbCallPayment.IsChecked = true;
                else
                    cbCallPayment.IsChecked = false;
                idClient = calls.idClient(idCall.ToString());
                idService = calls.idService(idCall.ToString());
                for (int i = 0; i < DateGridClient.Items.Count; i++)
                {
                    TextBlock x = DateGridClient.Columns[0].GetCellContent(DateGridClient.Items[i]) as TextBlock;
                    if (x.Text == idClient.ToString())
                    {
                        SelectRowByIndex(DateGridClient, i);
                        break;
                    }
                }
                for (int i = 0; i < DateGridService.Items.Count; i++)
                {
                    TextBlock x = DateGridService.Columns[0].GetCellContent(DateGridService.Items[i]) as TextBlock;
                    if (x.Text == idService.ToString())
                    {
                        SelectRowByIndex(DateGridService, i);
                        break;
                    }
                }
            }
        }
        
        int ch = 0;
        private void operationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((operationList.SelectedIndex == 0) && (ch != 0))
            {
                SaveButton.Content = "Добавить";
                cbCallMess.IsEnabled = true;
                cbCallPayment.IsEnabled = true;
                tbCallCity.IsEnabled = true;
                tbCallDate.IsEnabled = true;
                tbCallDuration.IsEnabled = true;
                tbCallNumber.IsEnabled = true;
            }
            else
                if (operationList.SelectedIndex == 1)
                {
                    SaveButton.Content = "Изменить";
                    cbCallMess.IsEnabled = true;
                    cbCallPayment.IsEnabled = true;
                    tbCallCity.IsEnabled = true;
                    tbCallDate.IsEnabled = true;
                    tbCallDuration.IsEnabled = true;
                    tbCallNumber.IsEnabled = true;
                    ClearClient();
                    ClearService();
                }
                else
                    if (operationList.SelectedIndex == 2)
                    {
                        SaveButton.Content = "Удалить";
                        cbCallMess.IsEnabled = false;
                        cbCallPayment.IsEnabled = false;
                        tbCallCity.IsEnabled = false;
                        tbCallDate.IsEnabled = false;
                        tbCallDuration.IsEnabled = false;
                        tbCallNumber.IsEnabled = false;
                        ClearClient();
                        ClearService();
                    }
            ch++;

        }
    }
}
