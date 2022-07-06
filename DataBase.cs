using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeUsersLite;

namespace HW12_6_BankA
{
    internal class DataBase
    {
        public List<Client> clients;
        public List<Departament> departaments;
        /// <summary>
        /// Конструктор по умолчанию. Применяется, если база данных пустая и надо создать БД по умолчанию
        /// </summary>
        /// <param name="AddFakeUsersForTest">Добавить фейковых пользователей и департаменты для теста? Укажите количество или 0</param>
        public DataBase(int AddFakeUsersForTest=0, int AddFakeDepartForTest=0)
        {
            clients = new List<Client>();
            departaments = new List<Departament>();
            if (AddFakeUsersForTest>0)
            {
                AddFakeClients(AddFakeUsersForTest);
            }
            if (AddFakeDepartForTest > 0)
            {
                AddFakeDeps(AddFakeDepartForTest);
            }
        }

        private void AddFakeClients(int amount)
        {
            Permission permission = new Permission(Permission.EDataMode.All, Permission.EDataMode.All, Permission.EDataMode.All, Permission.EDataMode.All);
            Employer fakeEmployer = new Employer("fakeEmployer", "My Gode", permission);
            for (int i = 0; i < amount; i++)
            {
                clients.Add(DataBase.GetFakeClient(fakeEmployer));
            }
        }
        private void AddFakeDeps(int amount)
        {
            Permission permission = new Permission(Permission.EDataMode.All, Permission.EDataMode.All, Permission.EDataMode.All, Permission.EDataMode.All);
            Employer fakeEmployer = new Employer("fakeEmployer", "My Gode", permission);
            for (int i = 0; i < amount; i++)
            {
                departaments.Add(DataBase.GetFakeDepartament(fakeEmployer));
            }
        }
        public static Client GetFakeClient(Employer employer)
        {
            FakeUser fu;
            fu = new FakeUser();
            return new Client(new Client.FIO(fu.GetFName(), fu.GetLName(), fu.GetMName()), fu.GetPhone(), fu.GetPasport(), employer);
        }

        public static Departament GetFakeDepartament(Employer employer)
        {
            int ID = ++IDs.DepartamentsIDCount;
            return new Departament($"Департамент {ID}", employer);
        }
    }
}
