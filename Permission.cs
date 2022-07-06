using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12_6_BankA
{
    internal class Permission
    {

        public  Permission( EDataMode GetDepartamentsData,
                            EDataMode SetDepartamentsData,
                            EDataMode GetClientsData,
                            EDataMode SetClientsData)
        {
            this.GetDepartamentsData = GetDepartamentsData;
            this.SetDepartamentsData = SetDepartamentsData;
            this.GetClientsData = GetClientsData;
            this.SetClientsData = SetClientsData;
        }
          

        public EDataMode GetDepartamentsData { get; private set; }
        public EDataMode SetDepartamentsData { get; private set; }
        public EDataMode GetClientsData { get; private set; }
        public EDataMode SetClientsData { get; private set; }

        public enum EDataMode 
        {
            No,
            All,
            AllExclusivePasportNum,
            OnlyPhoneNumber
        }
    }
}
