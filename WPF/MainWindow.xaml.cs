using FakeUsersLite;
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
        BillWindow billWindow;
        Repository rep;
        public MainWindow(Employer employer)
        {
            InitializeComponent();
            pageClient = new PageClient();
            LeftFrame.Content = pageClient;
            rep = new Repository("baza.json", employer);
            pageClient.Bills.IsEnabled = false;
            RefreshDataGrid();
            
            pageClient.DataContext = rep;
            this.DataContext = rep;
            pageClient.SaveClientButton.Click += SaveClientButton_Click;
            pageClient.DeleteButton.Click += DeleteButton_Click;
            pageClient.AddButton.Click += AddButton_Click;
            pageClient.FIO_Btn.Click += BtnRandom_Click;
            pageClient.TLP_Btn.Click += BtnRandom_Click;
            pageClient.PSP_Btn.Click += BtnRandom_Click;
            pageClient.Bills.Click += Bills_Click;

            comboBox.ItemsSource = rep.GetDepartamentsData();
        }

        private void Bills_Click(object sender, RoutedEventArgs e)
        {
            if (rep.CurrentClient == null) return;
            billWindow = new BillWindow(rep,this);
            this.Hide();
            billWindow.Show();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.SelectedIndex = dataGrid.Items.Count - 1;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0) return;

                string s = (dataGrid.SelectedItems.Count > 1) ? "нескольких клиентов" : $"клиента {dataGrid.SelectedItems[0]}";
                ModalWindowDialog modalWindow = new ModalWindowDialog($"Вы собираетесь удалить {s}\nНапишите ДА", "УДАЛЕНИЕ КЛИЕНТОВ");
            if (!(bool)modalWindow.ShowDialog()) return;
            if (modalWindow.InputText.ToUpper() != "ДА") return;
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
            //rep.OnPropertyChanged("CurrentClient");
            if (comboBox.SelectedItem != null)
            {
                rep.SaveCurrentClient((Departament)comboBox.SelectedItem); //это необходимо, если клиент НОВЫЙ (тоесть когда данных о департаменте просто нет, берётся тот департамент который выбран)
            }
            else
            {
                rep.SaveCurrentClient();
            }
            RefreshDataGrid();
        }

        private void dataGrid_Selected(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            Client client;
            pageClient.Bills.IsEnabled = false;
            if (dataGrid.SelectedItems.Count > 1)
            {   // Выбрано несколько ячеек
                client = new Client();
            }
            else if(dataGrid.SelectedItems.Count != 0) //при переключении департаментов возникает такая ситуация
            {
                if (dataGrid.SelectedItem.GetType() == typeof(Client))
                {   // Выбрана одна ячейка (нормальный режим)
                    client = (Client)dataGrid.SelectedItem;
                    pageClient.Bills.IsEnabled = true;
                }
                else
                {   // Выбрана одна последняя пустая ячейка
                    client = new Client(); 
                }
                
            } else { return; }
            rep.ClientSelect(client);
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
            pageClient.Bills.IsEnabled = false;
            dataGrid.Items.Refresh();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            var K= rep.SaveToFile();
            if(!K.result) { Debug.WriteLine(K.error); }
        }


        private void BtnRandom_Click(object sender, RoutedEventArgs e)
        {
            FakeUser fu = new FakeUser();
            switch (((Button)sender).Name)
            {
                case "FIO_Btn":
                    rep.CurrentClient.Fio.LastName = fu.GetLName();
                    rep.CurrentClient.Fio.FirstName = fu.GetFName();
                    rep.CurrentClient.Fio.MiddleName = fu.GetMName();
                    break;
                case "TLP_Btn":
                    rep.CurrentClient.PhoneNum = fu.GetPhone();
                    break;
                case "PSP_Btn":
                    rep.CurrentClient.PasportNum = fu.GetPasport();
                    break;
                default:
                    break;
            }
            rep.OnPropertyChanged("CurrentClient");
        }
    }
}
