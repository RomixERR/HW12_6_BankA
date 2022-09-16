using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace HW12_6_BankA
{
    public static class Extensions
    {
        public static int find<T>(this List<T> list, T target)
        {
            return list.IndexOf(target);
        }
    }

    internal class Repository: INotifyPropertyChanged
    {
        /// <summary>
        /// текущая КОПИЯ выбранного клиента
        /// </summary>
        public Client CurrentClient { get; set; }
        private DataBase db;
        private string pathFileName;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        /// <summary>
        /// Текущий рабочий сотрудник банка - пользователь базы данных (не клиент!)
        /// </summary>
        public Employer employer { get; private set; }
        public int GetClientsCount()
        {
            return db.clients.Count;
        }
        /// <summary>
        /// Создаём объект ГЛАВНЫЙ репозиотрий с базой данных, загружаем БД или создаём новую, если нет
        /// </summary>
        /// <param name="pathFileName"></param>
        public Repository(string pathFileName, Employer employer)
        {
            this.pathFileName = pathFileName;
            var R = LoadFromFile();
            if (!R.result || db == null)
            {
                Console.WriteLine($"Загрузка БД не выполнена. Ошибка {R.error}, будет создана пустая БД");
                db = new DataBase(15,5);
            }
            else
            {
                Console.WriteLine(  $"Загрузка БД выполнена! БД Содержит {db.clients.Count} клиентов," +
                                    $"{db.departaments.Count} департаментов.");
            }
            this.employer = employer;
            IDs.SetCounts(db);
            ClientSelect(db.clients.First());
        }
        /// <summary>
        /// Выполняет загрузку базы из файла
        /// </summary>
        /// <param name="pathFileName">Путь к файлу или / и его имя</param>
        /// <returns></returns>
        public (bool result, string error) LoadFromFile()
        {
            db = new DataBase();
            try
            {
                StreamReader streamReader = new StreamReader(pathFileName);
                db = (DataBase)JsonConvert.DeserializeObject(streamReader.ReadToEnd(), typeof(DataBase));
                streamReader.Close();

            }
            catch (Exception e) { return (false, e.Message); }
            return (true, "OK");
        }

        /// <summary>
        /// Сохраняет базу в файл
        /// </summary>
        /// <param name="pathFileName"></param>
        /// <param name="clients"></param>
        /// <returns></returns>
        public (bool result, string error) SaveToFile()
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(pathFileName);
                streamWriter.Write(JsonConvert.SerializeObject(db, Formatting.Indented));
                streamWriter.Close();
            }
            catch (Exception e) { return (false, e.Message); }
            return (true, "OK");
        }
        public List<Departament> GetDepartamentsData()
        {
            List<Departament> L = new List<Departament>(db.departaments);
            if (employer.Permission.GetDepartamentsData != Permission.EDataMode.All)
            {
                throw new Exception("Нет привелегий");
            }

            return L;
        }
        public List<Client> GetClientsData()
        {
            List<Client> L = new List<Client>();
            if (employer.Permission.GetClientsData == Permission.EDataMode.All)
            {
                L = new List<Client>(db.clients);
            }else if (employer.Permission.GetClientsData == Permission.EDataMode.AllExclusivePasportNum)
            {
                L = new List<Client>(db.clients);
                foreach (var item in L)
                {
                    item.PasportNum = "**** - *******";
                }
            }else
            {
                throw new Exception("Нет привелегий");
            }
            return L;
        }

        public void ClientSelect(Client client)
        {
            CurrentClient = new Client(client);
            OnPropertyChanged("CurrentClient");
        }
        /// <summary>
        /// Запись в базу (не в файл!) текущего клиента (изменение)
        /// </summary>
        public void SaveCurrentClient(DataGrid dataGrid)
        {
            Client client = db.clients[db.clients.find(CurrentClient)];
            if (employer.Permission.SetClientsData == Permission.EDataMode.No) throw new Exception("Нет привелегий");
            if (employer.Permission.SetClientsData == Permission.EDataMode.OnlyPhoneNumber)
            {
                if (CurrentClient.PhoneNum.Length < 4 || CurrentClient.PhoneNum.Length > 30)
                {
                    //ВЫДАТь СООБЩЕНИЕ
                    Debug.WriteLine("PhoneNum.Length is INCORRECT!");
                    return;
                }
                client.PhoneNum = CurrentClient.PhoneNum;
            }

            if (employer.Permission.SetClientsData == Permission.EDataMode.All)
            {
                client.Fio = CurrentClient.Fio;
                client.PasportNum = CurrentClient.PasportNum;
            }

            dataGrid.Items.Refresh();
        }
    }
}
