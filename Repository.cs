﻿using Newtonsoft.Json;
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
        public static int find<Client>(this List<Client> list, Client target)
        {
            return list.IndexOf(target);
        }
    }

    public class Repository: INotifyPropertyChanged
    {
        /// <summary>
        /// текущая КОПИЯ выбранного клиента
        /// </summary>
        public Client CurrentClient { get; set; }
        //public List<Departament> departaments
        //{
        //    get
        //    {
        //        return GetDepartamentsData();
        //    }
        //}
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

            //ClientSelect(db.clients.First());
        }
        /// <summary>
        /// Выполняет загрузку базы из файла
        /// </summary>
        /// <param name="pathFileName">Путь к файлу или / и его имя</param>
        /// <returns></returns>
        public (bool result, string error) LoadFromFile()
        {
            db = new DataBase();
            JSONConverter[] converters = { new JSONConverter() };
            try
            {
                StreamReader streamReader = new StreamReader(pathFileName);
                db = (DataBase)JsonConvert.DeserializeObject(streamReader.ReadToEnd(), typeof(DataBase), converters);
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
            catch (Exception e) { return (false, "При сохранении возникла проблема "+e.Message); }
            return (true, "Успешно сохранён");
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
        /// <summary>
        /// Создаёт лист клиентов для ОТОБРАЖЕНИЯ (работаем только по ID)
        /// Лист без фильтра или с фильтром по департаменту или по имени
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Client> GetClientsData<T>(T filterObject = null)
            where T : class
        {
            List<Client> L = new List<Client>();
            if (employer.Permission.GetClientsData == Permission.EDataMode.All)
            {
                L = new List<Client>(db.clients);
            }else if (employer.Permission.GetClientsData == Permission.EDataMode.AllExclusivePasportNum)
            {
                L = new List<Client>();

                for (int i = 0; i < db.clients.Count; i++)
                {
                    L.Add(new Client(db.clients[i]));
                    L[i].PasportNum = "**** - *******";
                }
                
                
                //foreach (var item in L)
                //{
                //    item.PasportNum = "**** - *******";
                //}
            }else
            {
                throw new Exception("Нет привелегий GetClientsData");
            }

           // if (departament != null) L = FilterDepartaments(L, departament); //Применяем фильтр по департаментам если нужно
            if (filterObject != null)
            {
                Type typeFilter = filterObject.GetType();
                if (typeFilter == typeof( Departament)) {
                    L = FilterDepartaments(L, filterObject as Departament); //Применяем фильтр по департаментам если нужно
                }


            }


            return L;
        }
        public List<Client> GetClientsData()
        {
            return GetClientsData<object>(null);
        }

        /// <summary>
        /// Фильтр по департаментам
        /// На выходе получаем новый список только с нужными департаментами
        /// </summary>
        /// <param name="L">Изначальный список</param>
        /// <param name="departament">Какой департамент показывать</param>
        /// <returns></returns>
        private List<Client> FilterDepartaments(List<Client> L, Departament departament)
        {
            List<Client> resL = new List<Client>();
            foreach (Client item in L)
            {
                if (item.Departament.ID == departament.ID)
                {
                    resL.Add(item);
                }
            }
            return resL;
        }
        /// <summary>
        /// Фильтр по именам (полностью по фамилии имени и отчеству)
        /// На выходе получаем новый список только с нужными именами (например все Ивановы)
        /// </summary>
        /// <param name="L">Изначальный список</param>
        /// <param name="name">имя клиентов</param>
        /// <returns></returns>
        private List<Client> FilterNames(List<Client> L, String name)
        {
            List<Client> resL = new List<Client>();

            name = name.ToUpper();
            string[] LFMNames = name.Split(' ');
            if (LFMNames.Length == 0) return resL;
            if (LFMNames[0].Length < 2) return resL; //минимальная длина для поиска

            foreach (Client item in L)
            {
                string Fn = item.Fio.FirstName.ToUpper();
                string Mn = item.Fio.MiddleName.ToUpper();
                string Ln = item.Fio.LastName.ToUpper();

                switch (LFMNames.Length)
                {
                  case 1: //если введено одно слово
                        if (Ln.Contains(LFMNames[0])) resL.Add(item);
                        break;
                  case 2: //если введено два слова
                        if (Ln.Contains(LFMNames[0]))
                        {
                            if (Fn.Contains(LFMNames[1])) resL.Add(item);
                        }
                        break;
                  case 3: //если введено 3 слова
                        if (Ln.Contains(LFMNames[0]))
                        {
                            if (Fn.Contains(LFMNames[1])) 
                            {
                                if (Mn.Contains(LFMNames[2])) resL.Add(item);
                            };
                        }
                        break;
                }
            }
            return resL;
        }

        /// <summary>
        /// обновление ТЕКУЩЕГО редактируемого клиента
        /// </summary>
        /// <param name="client"></param>
        public void ClientSelect(Client client)
        {
            CurrentClient = new Client(client);
            OnPropertyChanged("CurrentClient");
        }
        /// <summary>
        /// Запись в базу (не в файл!) текущего клиента (изменение)
        /// </summary>
        public void SaveCurrentClient(Departament departament = null)
        {
            Client client;

            if (db.clients.find(CurrentClient) >= 0) //выбрана ли пустая последняя ячейка в таблице или существующая
            {  //существующая ячейка - ищем клиента и изменяем данные
                client = db.clients[db.clients.find(CurrentClient)];
                var K = ClientDataCompletenessCheck(CurrentClient);
                if (!K.check) { Debug.WriteLine("ОШИБКА В ДАННЫХ КЛИЕНТА ПРИ РЕДАКТИРОВАНИИ! " + K.errorMsg); return; }
                if (employer.Permission.SetClientsData == Permission.EDataMode.No)
                {
                    //throw new Exception("Нет привелегий в редактировании");
                    Debug.WriteLine("Нет привелегий в редактировании");
                    return;
                }
                client.PhoneNum = CurrentClient.PhoneNum;
                if (employer.Permission.SetClientsData == Permission.EDataMode.All)
                {
                    client.Fio = CurrentClient.Fio;
                    client.PasportNum = CurrentClient.PasportNum;
                }
            }
            else{
                //выбрана пустая ячейка - добавляем нового клиента
                if (employer.Permission.SetClientsData != Permission.EDataMode.All)
                {
                    //throw new Exception("Нет привелегий в добавлении");
                    Debug.WriteLine("Нет привелегий в добавлении");
                    return;
                }
                if (departament == null) departament = db.departaments[0]; //Если департамент не был указан то берём первый
                client = new Client(CurrentClient.Fio, CurrentClient.PhoneNum, CurrentClient.PasportNum, employer, departament);
                var K = ClientDataCompletenessCheck(client);
                if (!K.check) { Debug.WriteLine("ОШИБКА В ДАННЫХ КЛИЕНТА ПРИ ДОБАВЛЕНИИ! " + K.errorMsg); return; }
                db.clients.Add(client);
            }
        }
        /// <summary>
        /// Проверяет - все ли данные имеются у данного экземпляра клиента
        /// check = true - все поля заполненны
        /// check = false - не все поля заполнены
        /// errorMsg - сообщение о том какие поля следует заполнить
        /// </summary>
        /// <param name="client">Проверяемый экземпляр клиента</param>
        /// <returns></returns>
        public (bool check, string errorMsg) ClientDataCompletenessCheck(Client client)
        {
            string errorMsg="";
            bool check = true;
            if(client.Departament == null) { check = false; errorMsg += "client.Departament == null"+" "; return (check, errorMsg); }
            if(client.Fio == null) { check = false; errorMsg += "client.Fio == null" + " "; return (check, errorMsg); }
            var K = client.Check();
            if (!K.check) { check = false; errorMsg += K.errorMsg + " "; }
            return (check, errorMsg);
        }


        /// <summary>
        /// Удаление из базы (не в файл!) по ID
        /// </summary>
        public void DeleteClient(int ID)
        {
            Client client = Client.FindClientByID(db.clients, ID);
            if (client == null) return;
            if (employer.Permission.SetClientsData == Permission.EDataMode.No) throw new Exception("Нет привелегий в удалении!!!");
            if (employer.Permission.SetClientsData == Permission.EDataMode.All)
            {
                db.clients.Remove(client);
            }
        }



        


    }
}
