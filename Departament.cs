using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12_6_BankA
{
    internal class Departament
    {
        public int ID { get; private set; }
        private string nameOfDepartament;
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
        public override string ToString()
        {
            return nameOfDepartament;
        }
    }
}
