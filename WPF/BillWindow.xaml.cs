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
using static HW12_6_BankA.ClientBill;

namespace HW12_6_BankA
{
    /// <summary>
    /// Логика взаимодействия для BillWindow.xaml
    /// </summary>
    public partial class BillWindow : Window
    {
        Repository rep;
        ClientBillWPF bills;
        Window mainWindow;
        public BillWindow(Repository _rep, Window mainWindow)
        {
            InitializeComponent();
            this.rep = _rep;
            bills = rep.CurrentClient.ClientBill;
            rep.CurrentClient.ClientBill.Refresh(rep);
            this.DataContext = rep;
            btnOpenDeb.Click += BtnOpenDeb_Click;
            btnOpenCred.Click += BtnOpenCred_Click;
            btnCloseDeb.Click += BtnCloseDeb_Click;
            btnCloseCred.Click += BtnCloseCred_Click;

            btnAddDeb.Click += BtnAddDeb_Click;
            btnTakeDeb.Click += BtnTakeDeb_Click;
            btnSendDeb.Click += BtnSendDeb_Click;

            btnAddCred.Click += BtnAddCred_Click;
            btnTakeCred.Click += BtnTakeCred_Click;
            btnSendCred.Click += BtnSendCred_Click;

            this.mainWindow = mainWindow;
            //tbFilter.TextChanged += TbFilter_TextChanged;
            //RefreshDataGrid();
        }

        protected override void OnClosed(EventArgs e)
        {
            mainWindow.Show();
            base.OnClosed(e);
        }

        private void BtnSendCred_Click(object sender, RoutedEventArgs e)
        {
            BillCredit clientBill = rep.CurrentClient.ClientBill.GetBillCredit();
            string d = "отправить перевод";
            string minfo = $"Вы собираетесь {d} со счёта {clientBill.ID}, клиента {rep.CurrentClient.Fio}.\n" +
                         $"На счёт ->>> (ВЫБРАТЬ СПРАВА В СПИСКЕ) ->>>\n" +
                         $"Введите сумму:";

            ModalWindowSend modalWindowSend = new ModalWindowSend(rep, clientBill.ID, minfo, $"{d}");

            if ((bool)modalWindowSend.ShowDialog())
            {
                int? sum = modalWindowSend.Sum;
                if ((sum == null) || (sum == 0)) return;
                if (modalWindowSend.ClientForSend == null) return;
                if (modalWindowSend.BillForSend == null) return;

                clientBill.Take(modalWindowSend.BillForSend, (decimal)sum);

                bills.Refresh(rep);
                //RefreshDataGrid();
                Debug.WriteLine($"OK {d} Sum={sum}");
                return;
            }
        }

        private void BtnTakeCred_Click(object sender, RoutedEventArgs e)
        {
            string d = "снять со счёта";
            ModalWindowAdd modalWindowAdd = new ModalWindowAdd(
                $"Вы собираетесь {d} {rep.CurrentClient.ID}, клиента {rep.CurrentClient.Fio}.\nВведите сумму:", $"{d}");
            if ((bool)modalWindowAdd.ShowDialog())
            {
                int? sum = modalWindowAdd.Sum;
                if ((sum == null) || (sum == 0)) return;
                ClientBillWPF clientBill = rep.CurrentClient.ClientBill;
                clientBill.GetBillCredit().Take((decimal)sum);
                bills.Refresh(rep);
                //RefreshDataGrid();
                Debug.WriteLine($"OK {d} Sum={sum}");
                return;
            }
        }

        private void BtnAddCred_Click(object sender, RoutedEventArgs e)
        {
            string d = "пополнить счёт";
            ModalWindowAdd modalWindowAdd = new ModalWindowAdd(
                $"Вы собираетесь {d} {rep.CurrentClient.ID}, клиента {rep.CurrentClient.Fio}.\nВведите сумму:", $"{d}");
            if ((bool)modalWindowAdd.ShowDialog())
            {
                int? sum = modalWindowAdd.Sum;
                if ((sum == null) || (sum == 0)) return;
                ClientBillWPF clientBill = rep.CurrentClient.ClientBill;
                clientBill.GetBillCredit().Put((decimal)sum);
                bills.Refresh(rep);
               // RefreshDataGrid();
                Debug.WriteLine($"OK {d} Sum={sum}");
                return;
            }
        }

        private void BtnSendDeb_Click(object sender, RoutedEventArgs e)
        {
            BillDeposit clientBill = rep.CurrentClient.ClientBill.GetBillDeposit();
            string d = "отправить перевод";
            string minfo = $"Вы собираетесь {d} со счёта {clientBill.ID}, клиента {rep.CurrentClient.Fio}.\n" +
                         $"На счёт ->>> (ВЫБРАТЬ СПРАВА В СПИСКЕ) ->>>\n" +
                         $"Введите сумму:";

            ModalWindowSend modalWindowSend = new ModalWindowSend(rep, clientBill.ID, minfo,$"{d}");

            if ((bool)modalWindowSend.ShowDialog())
            {
                int? sum = modalWindowSend.Sum;
                if ((sum == null) || (sum == 0)) return;
                if (modalWindowSend.ClientForSend == null) return;
                if (modalWindowSend.BillForSend == null) return;

                clientBill.Take(modalWindowSend.BillForSend, (decimal)sum);

                bills.Refresh(rep);
                //RefreshDataGrid();
                Debug.WriteLine($"OK {d} Sum={sum}");
                return;
            }
        }

        private void BtnTakeDeb_Click(object sender, RoutedEventArgs e)
        {
            string d = "снять со счёта";
            ModalWindowAdd modalWindowAdd = new ModalWindowAdd(
                $"Вы собираетесь {d} {rep.CurrentClient.ID}, клиента {rep.CurrentClient.Fio}.\nВведите сумму:", $"{d}");
            if ((bool)modalWindowAdd.ShowDialog())
            {
                int? sum = modalWindowAdd.Sum;
                if ((sum == null) || (sum == 0)) return;
                ClientBillWPF clientBill = rep.CurrentClient.ClientBill;
                if (clientBill.GetBillDeposit() == null)
                {
                    Debug.WriteLine($"У клиента для перевода нет счёта");
                    return;
                }
                clientBill.GetBillDeposit().Take((decimal)sum);
                bills.Refresh(rep);
                //RefreshDataGrid();
                Debug.WriteLine($"OK {d} Sum={sum}");
                return;
            }
        }

        private void BtnAddDeb_Click(object sender, RoutedEventArgs e)
        {
            string d = "пополнить счёт";
            ModalWindowAdd modalWindowAdd = new ModalWindowAdd(
                $"Вы собираетесь {d} {rep.CurrentClient.ID}, клиента {rep.CurrentClient.Fio}.\nВведите сумму:", $"{d}");
           if((bool)modalWindowAdd.ShowDialog())
            {
                int? sum = modalWindowAdd.Sum;
                if ((sum == null) || (sum == 0)) return;
                ClientBillWPF clientBill = rep.CurrentClient.ClientBill;
                clientBill.GetBillDeposit().Put((decimal)sum);
                bills.Refresh(rep);
                //RefreshDataGrid();
                Debug.WriteLine($"OK {d} Sum={sum}");
                return;
            }
        }



        //private void TbFilter_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    string text = ((TextBox)sender).Text;
        //    if (text.Length > 3)
        //    {
        //        RefreshDataGrid(rep.FilterNames(rep.GetClientsData(), text));
        //    }
        //    else
        //    {
        //        RefreshDataGrid();
        //    }
        //}



        private void BtnCloseCred_Click(object sender, RoutedEventArgs e)
        {
            BillCredit bill = bills.GetBillCredit();
            if (bill == null) return;
            if (bills.CloseBill(bill)) Debug.Write("Bill Close!"); else Debug.Write("Bill NOT Close!");
            bills.Refresh(rep);
            //RefreshDataGrid();
        }

        private void BtnCloseDeb_Click(object sender, RoutedEventArgs e)
        {
            BillDeposit bill = bills.GetBillDeposit();
            if (bill == null) return;
            if (bills.CloseBill(bill)) Debug.Write("Bill Close!"); else Debug.Write("Bill NOT Close!");
            bills.Refresh(rep);
            //RefreshDataGrid();
        }

        private void BtnOpenCred_Click(object sender, RoutedEventArgs e)
        {
            if (bills.OpenBill(typeof(BillCredit))) Debug.Write("BillCredit OPEN"); else { Debug.Write("BillCredit not open!"); return; };
            bills.Refresh(rep);
            //RefreshDataGrid();
        }

        private void BtnOpenDeb_Click(object sender, RoutedEventArgs e)
        {
            if (bills.OpenBill(typeof(BillDeposit))) Debug.Write("BillDeposit OPEN"); else { Debug.Write("BillDeposit not open!"); return; };
            bills.Refresh(rep);
            //RefreshDataGrid();
        }

        //private void RefreshDataGrid(List<Client> clients = null)
        //{
        //    if (clients == null)
        //    {
        //        dataGrid.ItemsSource = rep.GetClientsData();
        //    }
        //    else
        //    {
        //        dataGrid.ItemsSource = clients;
        //    }
        //    dataGrid.SelectedIndex = 0;
        //    dataGrid.Items.Refresh();
        //}

        //private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}
    }
}
