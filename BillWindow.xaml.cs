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

namespace HW12_6_BankA
{
    /// <summary>
    /// Логика взаимодействия для BillWindow.xaml
    /// </summary>
    public partial class BillWindow : Window
    {
        public BillWindow(Repository rep)
        {
            InitializeComponent();

            rep.CurrentClient.ClientBill.Refresh();
            this.DataContext = rep;

        }
    }
}
