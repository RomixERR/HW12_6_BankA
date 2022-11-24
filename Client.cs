using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace HW12_6_BankA
{
    public class Client : IEquatable<Client>
    {
        public int ID { get; set; }
        private FIO fio;
        private string phoneNum;
        private string pasportNum;
        private Departament departament;
        public DataChangeAtributes dataChangeAtributes;

        public ClientBillWPF ClientBill { get; set; }


        public FIO Fio { get { return fio; } set { fio = value; } }
        public string PhoneNum { get { return phoneNum; } set { phoneNum = value; } }
        public string PasportNum { get { return pasportNum; } set { pasportNum = value; } }
        public Departament Departament { get { return departament; } set { departament = value;} }

        public Client(FIO name, string phoneNum, string pasportNum, Employer employer, Departament departament)
        {
            if (employer.Permission.SetClientsData != Permission.EDataMode.All)
            {
                throw new Exception("Нет привелегий");
            }
            
            this.fio = name;
            this.phoneNum = phoneNum;
            this.pasportNum = pasportNum;
            this.Departament = departament;
            dataChangeAtributes = DataChangeAtributes.NewChangeAtributes(employer);
            ID = ++IDs.ClientsIDCount;
            ClientBill = new ClientBillWPF(ID);
        }

        /// <summary>
        /// Пустой клиент !!!
        /// </summary>
        public Client() 
        {
            this.fio = new FIO("", "", "");
            this.phoneNum = "";
            this.pasportNum = "";
            this.ID = -1;
            ClientBill = new ClientBillWPF();
            this.Departament = new Departament(-1,"");
        }

        public Client(FIO name, string phoneNum, string pasportNum)
        {
            this.fio = name;
            this.phoneNum = phoneNum;
            this.pasportNum = pasportNum;
            ID = ++IDs.ClientsIDCount;
            ClientBill = new ClientBillWPF(ID);
        }
        /// <summary>
        /// Дубликат клиента (клон)
        /// </summary>
        /// <param name="ForCopy"></param>
        public Client(Client ForCopy)
        {
            this.fio = new FIO(ForCopy.Fio.FirstName, ForCopy.Fio.LastName, ForCopy.Fio.MiddleName);
            this.phoneNum =ForCopy.PhoneNum;
            this.pasportNum = ForCopy.PasportNum;
            this.ID = ForCopy.ID;
            this.Departament = new Departament( ForCopy.Departament.ID, ForCopy.Departament.NameOfDepartament);
            this.ClientBill = ForCopy.ClientBill;
        }

        public override string ToString()
        {
            return $"{Fio} {PhoneNum} {PasportNum} {Departament}";
        }

        public bool Equals(Client other)
        {
            if (other.ID == this.ID) return true; else return false;
        }
        public static Client FindClientByID(List<Client> list, int ID)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ID == ID) return list[i];
            }
            return null;
        }
        /// <summary>
        /// Проверка полноты данных
        /// </summary>
        /// <returns></returns>
        public (bool check, string errorMsg) Check()
        {
            string errorMsg = "";
            bool check = true;
            if (PhoneNum.Length < 6) { check = false; errorMsg += "PhoneNum.Length" + " "; }
            if (PasportNum.Length < 9) { check = false; errorMsg += "PasportNum.Length" + " "; }
            var K = this.Fio.Check();
            if (!K.check) { check = false; errorMsg += K.errorMsg + " "; }
            K = this.Departament.Check();
            if (!K.check) { check = false; errorMsg += K.errorMsg + " "; }
            return (check, errorMsg);
        }

        public static void DebugPrintList(List<Client> testClients)
        {
            foreach (var item in testClients)
            {
                Debug.WriteLine(item);
            }
        }

    }

    public class FIO : IComparable
    {
        
        private string firstName;
        private string lastName;
        private string middleName;
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get { return firstName; } set { firstName = value; } }
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get { return lastName; } set { lastName = value;  } }
        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get { return middleName; } set { middleName = value; } }
        public FIO(string FirstName, string LastName, string MiddleName)
        {
            this.firstName = FirstName;
            this.lastName = LastName;
            this.middleName = MiddleName;
        }

        public override string ToString()
        {
            //return $"Имя: {FirstName}, Фамилия: {LastName}, Отчество: {MiddleName}";
            return $"{LastName} {FirstName} {MiddleName}";
        }

        public int CompareTo(object obj)
        {
            FIO fio = (FIO)obj;
            return lastName.CompareTo(fio.lastName);
        }
        /// <summary>
        /// Проверка полноты данных
        /// </summary>
        /// <returns></returns>
        public (bool check, string errorMsg) Check()
        {
            string errorMsg = "";
            bool check = true;
            if (FirstName.Length < 2) { check = false; errorMsg += "FirstName.Length < 2" + " "; }
            if (LastName.Length < 2) { check = false; errorMsg += "LastName.Length < 2" + " "; }
            if (MiddleName.Length < 2) { check = false; errorMsg += "MiddleName.Length < 2" + " "; }
            return (check, errorMsg);
        }

    }
}
