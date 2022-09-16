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

            dataGrid.ItemsSource = rep.GetClientsData();
            dataGrid.SelectedIndex = 0;
            
            pageClient.DataContext = rep;
            this.DataContext = rep;
            pageClient.SaveClientButton.Click += SaveClientButton_Click;



        }

        private void SaveClientButton_Click(object sender, RoutedEventArgs e)
        {
            rep.SaveCurrentClient(dataGrid);
        }

        private void dataGrid_Selected(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            Client client;
            if (dataGrid.SelectedItems.Count > 1)
            {   // Выбрано несколько ячеек
                client = new Client();
            }
            else
            {
                if (dataGrid.SelectedItem.GetType() == typeof(Client))
                {   // Выбрана одна ячейка (нормальный режим)
                    client = (Client)dataGrid.SelectedItem;
                }
                else
                {   // Выбрана одна последняя пустая ячейка
                    client = new Client(); ////////
                }
                
            }
            rep.ClientSelect(client);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(rep.CurrentClient);
        }
    }
}
