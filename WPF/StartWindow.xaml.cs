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
    /// Логика взаимодействия для StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public MainWindow mainWindow;
        public List<Employer> employers;

        public StartWindow()
        {
            InitializeComponent();
            SetEmployersAndPerm();
            comboBox.ItemsSource = employers;
            //comboBox.DataContext = employers;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Employer employer = (Employer)(((ComboBox)sender).SelectedItem);
            Debug.WriteLine(employer);
            mainWindow = new MainWindow(employer);
            mainWindow.Show();
            mainWindow.Activate();
            this.Close();
        }

        public void SetEmployersAndPerm() 
        {
            employers = new List<Employer>();
            employers.Add(new Employer("Вася", "Manager",
                new Permission(Permission.EDataMode.All, Permission.EDataMode.All, Permission.EDataMode.All, Permission.EDataMode.All)));
            employers.Add(new Employer("Петя", "Helper",
                new Permission(Permission.EDataMode.All, Permission.EDataMode.No, Permission.EDataMode.AllExclusivePasportNum, Permission.EDataMode.OnlyPhoneNumber)));

            //Permission permission = new Permission(Permission.EDataMode.All, Permission.EDataMode.All, Permission.EDataMode.All, Permission.EDataMode.All);
            //Permission permission = new Permission(Permission.EDataMode.All, Permission.EDataMode.No, Permission.EDataMode.AllExclusivePasportNum, Permission.EDataMode.OnlyPhoneNumber);
            
        }
    }
}
