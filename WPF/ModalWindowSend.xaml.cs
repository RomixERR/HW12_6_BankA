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
using static HW12_6_BankA.ClientBill;

namespace HW12_6_BankA
{
    /// <summary>
    /// Логика взаимодействия для ModalWindowSend.xaml
    /// </summary>
    public partial class ModalWindowSend : Window
    {
        public Repository rep;
        public Client ClientForSend { get; private set; }
        public Bill BillForSend { get; private set; }
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
                catch (Exception e)
                {
                    LoggerHub.Log(this, $"Вызвано исключение {e.Message}", LoggerHub.LogEventType.DisplayOnForm);
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
            LoggerHub.Log(this, $"Создано модальное окно {this.Name}, {caption}, {info}", LoggerHub.LogEventType.dontDisplayOnForm);
            RefreshDataGrid();
        }

        private void RefreshInfo()
        {
            string d = "отправить перевод";
            string bs;
            
            if(cbBillForSend.SelectedItem == null)
            {
                bs = "(НЕ ВЫБРАНО)";
            }
            else
            {
                bs = cbBillForSend.SelectedItem.ToString();
            }
            
            tbInfo.Text= $"Вы собираетесь {d} со счёта {BillTakeID}, клиента {rep.CurrentClient.Fio}. " +
                         $"На счёт {bs}. " +
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
            RefreshInfo();
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
                    RefreshInfo();
                    return;
                }
            }
            else { return; }
        }

        private void TbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            if (text.Length > 2)
            {
                RefreshDataGrid(rep.FilterNames(rep.GetClientsData(), text));
            }
            else
            {
                RefreshDataGrid();
            }
        }



        private void cbBillForSend_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshInfo();
            BillForSend = (Bill)cbBillForSend.SelectedItem;
        }
    }
}
