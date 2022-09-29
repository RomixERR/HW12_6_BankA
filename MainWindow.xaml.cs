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
            Permission permission = new Permission(Permission.EDataMode.All, Permission.EDataMode.All, Permission.EDataMode.All, Permission.EDataMode.All);
            //Permission permission = new Permission( Permission.EDataMode.All, Permission.EDataMode.No, Permission.EDataMode.AllExclusivePasportNum, Permission.EDataMode.OnlyPhoneNumber);
            Employer employer = new Employer("Вася", "Менеджер", permission);
            rep = new Repository("baza.json", employer);

            RefreshDataGrid();
            
            pageClient.DataContext = rep;
            this.DataContext = rep;
            pageClient.SaveClientButton.Click += SaveClientButton_Click;
            pageClient.DeleteButton.Click += DeleteButton_Click;
            pageClient.AddButton.Click += AddButton_Click;

            comboBox.ItemsSource = rep.GetDepartamentsData();

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.SelectedIndex = dataGrid.Items.Count - 1;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0) return;
            for (int i = 0; i < dataGrid.SelectedItems.Count; i++)
            {
                if (dataGrid.SelectedItems[i] != null && dataGrid.SelectedItems[i].GetType() == typeof(Client))
                {
                    int id = ((Client)dataGrid.SelectedItems[i]).ID;
                    rep.DeleteClient(id);
                }
            }
            RefreshDataGrid();
        }

        private void SaveClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox.SelectedItem != null)
            {
                rep.SaveCurrentClient(dataGrid, (Departament)comboBox.SelectedItem);
            }
            else
            {
                rep.SaveCurrentClient(dataGrid);
            }
            RefreshDataGrid();
        }

        private void dataGrid_Selected(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            Client client;
            if (dataGrid.SelectedItems.Count > 1)
            {   // Выбрано несколько ячеек
                client = new Client();
            }
            else if(dataGrid.SelectedItems.Count != 0) //при переключении департаментов возникает такая ситуация
            {
                if (dataGrid.SelectedItem.GetType() == typeof(Client))
                {   // Выбрана одна ячейка (нормальный режим)
                    client = (Client)dataGrid.SelectedItem;
                }
                else
                {   // Выбрана одна последняя пустая ячейка
                    client = new Client(); ////////
                }
                
            } else { return; }
            rep.ClientSelect(client);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(rep.CurrentClient);
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshDataGrid();
        }

        private void RefreshDataGrid()
        {
            if (comboBox.SelectedIndex == -1) dataGrid.ItemsSource = rep.GetClientsData();
            else dataGrid.ItemsSource = rep.GetClientsData((Departament)(comboBox.SelectedItem)); //применить фильтр
            dataGrid.SelectedIndex = 0;
            dataGrid.Items.Refresh();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            var K= rep.SaveToFile();
            if(!K.result) { Debug.WriteLine(K.error); }
        }
    }
}
