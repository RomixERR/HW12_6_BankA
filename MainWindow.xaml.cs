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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HW12_6_BankA
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PageClient pageClient;
        Repository rep;
        public MainWindow()
        {
            InitializeComponent();
            pageClient = new PageClient();
            LeftFrame.Content = pageClient;
            //Permission permission = new Permission(Permission.EDataMode.All, Permission.EDataMode.All, Permission.EDataMode.All, Permission.EDataMode.All);
            Permission permission = new Permission( Permission.EDataMode.All, Permission.EDataMode.No, Permission.EDataMode.AllExclusivePasportNum, Permission.EDataMode.OnlyPhoneNumber);
            Employer employer = new Employer("Вася", "Менеджер", permission);
            rep = new Repository("baza.json", employer);
            pageClient.DataContext = rep;
            this.DataContext = rep;

        }
    }
}
