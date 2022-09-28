using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12_6_BankA
{
    internal class Departament: IComparable
    {
        public int ID { get; private set; }
        public string nameOfDepartament { get; private set; }
        public DataChangeAtributes dataChangeAtributes;

        public string NameOfDepartament { get { return nameOfDepartament; } set { nameOfDepartament = value; } }

        public Departament(string NameOfDepartament, Employer employer)
        {
            if (employer.Permission.SetDepartamentsData != Permission.EDataMode.All)
            {
                throw new Exception("Нет привелегий");
            }
            dataChangeAtributes = DataChangeAtributes.NewChangeAtributes(employer);
            this.nameOfDepartament = NameOfDepartament;
            ID = ++IDs.DepartamentsIDCount;
        }
        public Departament(int Id, string nameOfDepartament)
        {
            ID = Id;
            NameOfDepartament = nameOfDepartament;
        }
        public override string ToString()
        {
            return nameOfDepartament;
        }

        public int CompareTo(object obj)
        {
            return nameOfDepartament.CompareTo(((Departament)obj).nameOfDepartament);
        }
    }
}
