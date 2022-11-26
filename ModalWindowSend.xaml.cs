using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace HW12_6_BankA
{
    /// <summary>
    /// Логика взаимодействия для ModalWindowSend.xaml
    /// </summary>
    public partial class ModalWindowSend : Window
    {
        public Repository rep;
        public Client ClientForSend { get; private set; }
        public int? Sum
        {
            get
            {
                int s = 0;
                try
                {
                    s = Convert.ToInt32(tbSum.Text);
                    return s;
                }
                catch
                {
                    return null;
                }
            }
        }
        private string BillTakeID;

        public ModalWindowSend(Repository rep, string BillTakeID, string info = "Введите значение", string caption = "Диалоговое окно")
        {
            InitializeComponent();
            this.Title = caption;
            tbInfo.Text = info;
            tbSum.Focus();
            this.rep = rep;
            tbFilter.TextChanged += TbFilter_TextChanged;
            this.BillTakeID = BillTakeID;
            RefreshDataGrid();
        }

        private void RefreshInfo()
        {
            string d = "отправить перевод";
            tbInfo.Text= $"Вы собираетесь {d} со счёта {BillTakeID}, клиента {rep.CurrentClient.Fio}.\n" +
                         $"На счёт {ClientForSend.ClientBill.DepositID}\n" +
                         $"Введите сумму:";
        }

        private void btnСonfirm_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void RefreshDataGrid(List<Client> clients = null)
        {
            if (clients == null)
            {
                dataGrid.ItemsSource = rep.GetClientsData();
            }
            else
            {
                dataGrid.ItemsSource = clients;
            }
            dataGrid.SelectedIndex = 0;
            dataGrid.Items.Refresh();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            if (dataGrid.SelectedItems.Count > 1)
            {   // Выбрано несколько ячеек
                return;
            }
            else if (dataGrid.SelectedItems.Count != 0) //при переключении департаментов возникает такая ситуация
            {
                if (dataGrid.SelectedItem.GetType() == typeof(Client))
                {   // Выбрана одна ячейка (нормальный режим)
                    ClientForSend = (Client)dataGrid.SelectedItem;
                    Debug.WriteLine(ClientForSend.ToString());
                    return;
                }
            }
            else { return; }
        }

        private void TbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            if (text.Length > 3)
            {
                RefreshDataGrid(rep.FilterNames(rep.GetClientsData(), text));
            }
            else
            {
                RefreshDataGrid();
            }
        }


    }
}
