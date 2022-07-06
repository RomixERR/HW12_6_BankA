using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12_6_BankA
{
    internal class Client 
    {
        public int ID { get; private set; }
        private FIO fio;
        private string phoneNum;
        private string pasportNum;
        private Departament departament;
        public DataChangeAtributes dataChangeAtributes;


        public FIO Fio { get { return fio; } set { fio = value; } }
        public string PhoneNum { get { return phoneNum; } set { phoneNum = value; } }
        public string PasportNum { get { return pasportNum; } set { pasportNum = value; } }
        public Departament Departament { get { return departament; } set { departament = value; } }

        public Client(FIO name, string phoneNum, string pasportNum, Employer employer)
        {
            if (employer.Permission.SetClientsData != Permission.EDataMode.All)
            {
                throw new Exception("Нет привелегий");
            }
            this.fio = name;
            this.phoneNum = phoneNum;
            this.pasportNum = pasportNum;
            dataChangeAtributes = DataChangeAtributes.NewChangeAtributes(employer);
            ID = ++IDs.ClientsIDCount;
        }

        public class FIO
        {   /// <summary>
            /// Имя
            /// </summary>
            public string FirstName;
            /// <summary>
            /// Фамилия
            /// </summary>
            public string LastName;
            /// <summary>
            /// Отчество
            /// </summary>
            public string MiddleName;
            public FIO(string FirstName, string LastName,string MiddleName)
            {
                this.FirstName = FirstName;
                this.LastName = LastName;
                this.MiddleName = MiddleName;
            }

            public override string ToString()
            {
                //return $"Имя: {FirstName}, Фамилия: {LastName}, Отчество: {MiddleName}";
                return $"{FirstName} {LastName} {MiddleName}";
            }
        }
    }
}
