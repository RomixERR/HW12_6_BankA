﻿using System;
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
        public BillWindow(Repository _rep)
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
            tbFilter.TextChanged += TbFilter_TextChanged; ;
            RefreshDataGrid();
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



        private void BtnCloseCred_Click(object sender, RoutedEventArgs e)
        {
            BillCredit bill = bills.GetBillCredit();
            if (bill == null) return;
            if (bills.CloseBill(bill)) Debug.Write("Bill Close!"); else Debug.Write("Bill NOT Close!");
            bills.Refresh(rep);
            RefreshDataGrid();
        }

        private void BtnCloseDeb_Click(object sender, RoutedEventArgs e)
        {
            BillDeposit bill = bills.GetBillDeposit();
            if (bill == null) return;
            if (bills.CloseBill(bill)) Debug.Write("Bill Close!"); else Debug.Write("Bill NOT Close!");
            bills.Refresh(rep);
            RefreshDataGrid();
        }

        private void BtnOpenCred_Click(object sender, RoutedEventArgs e)
        {
            if (bills.OpenBill(typeof(BillCredit))) Debug.Write("BillCredit OPEN"); else { Debug.Write("BillCredit not open!"); return; };
            bills.Refresh(rep);
            RefreshDataGrid();
        }

        private void BtnOpenDeb_Click(object sender, RoutedEventArgs e)
        {
            if (bills.OpenBill(typeof(BillDeposit))) Debug.Write("BillDeposit OPEN"); else { Debug.Write("BillDeposit not open!"); return; };
            bills.Refresh(rep);
            RefreshDataGrid();
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

        }
    }
}
