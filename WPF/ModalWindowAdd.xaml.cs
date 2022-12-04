﻿using System;
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

namespace HW12_6_BankA
{
    /// <summary>
    /// Логика взаимодействия для ModalWindowAdd.xaml
    /// </summary>
    public partial class ModalWindowAdd : Window
    {
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
        public ModalWindowAdd(string info="Введите значение", string caption="Диалоговое окно")
        {
            InitializeComponent();
            this.Title = caption;
            tbInfo.Text = info;
            LoggerHub.Log(this, $"Создано модальное окно {this.Name}, {caption}, {info}", LoggerHub.LogEventType.dontDisplayOnForm);
            tbSum.Focus();
        }

        private void btnСonfirm_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

       
    }
}