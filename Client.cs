﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace HW12_6_BankA
{
    internal class Client : IEquatable<Client>
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
        public Departament Departament { get { return departament; } set { departament = value;} }

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

        public Client(FIO name, string phoneNum, string pasportNum)
        {
            this.fio = name;
            this.phoneNum = phoneNum;
            this.pasportNum = pasportNum;
            ID = ++IDs.ClientsIDCount;
        }

        public Client(Client ForCopy)
        {
            this.fio = new FIO(ForCopy.Fio.FirstName, ForCopy.Fio.LastName, ForCopy.Fio.MiddleName);
            this.phoneNum =ForCopy.PhoneNum;
            this.pasportNum = ForCopy.PasportNum;
            this.ID = ForCopy.ID;
            this.Departament = new Departament( ForCopy.Departament.ID, ForCopy.Departament.NameOfDepartament);
        }

        public override string ToString()
        {
            return $"{Fio} {PhoneNum} {PasportNum} {Departament}";
        }

        public bool Equals(Client other)
        {
            if (other.ID == this.ID) return true; else return false;
        }
    }

    internal class FIO
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
            return $"{FirstName} {LastName} {MiddleName}";
        }
    }
}
