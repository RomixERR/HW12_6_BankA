using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HW12_6_BankA
{
    public abstract class ClientBill
    {
       
        public List<Bill> bills { get; set; }
        
        //public int clientID { get; set; }

        public ClientBill()
        {
            bills = new List<Bill>();
        }

        public ClientBill(int clientID)
        {
            bills = new List<Bill>();
            //this.clientID = clientID;
        }

        public bool OpenBill(Type typeBill, int clientID)
        {
            foreach (var bill in bills)
            {
                if (bill == null) continue;
                if (bill.GetType() == typeBill) return false; //если вдруг такой счёт уже есть то нельзя открыть такой-же
            }
            if (typeBill == typeof(BillDeposit))
            {
                bills.Add(new BillDeposit(clientID));
                LoggerHub.Log(
                    this,
                    $"Открытие {bills.Last<Bill>().GetTypeBillString()}, {bills.Last<Bill>().ID} для клиента ID:{bills.Last<Bill>().ClientID}",
                    LoggerHub.LogEventType.DisplayOnForm);
                return true;
            }
            else if (typeBill == typeof(BillCredit))
            {
                bills.Add(new BillCredit(clientID));
                LoggerHub.Log(
                    this,
                    $"Открытие {bills.Last<Bill>().GetTypeBillString()}, {bills.Last<Bill>().ID} для клиента ID:{bills.Last<Bill>().ClientID}",
                    LoggerHub.LogEventType.DisplayOnForm);
                return true;
            }
            else return false;
        }

        public bool CloseBill(Bill bill, int clientID)
        {
            if (bill.Money == 0)
            {
                bills.Remove(bill);
                LoggerHub.Log(
                    this,
                    $"Закрытие {bill.GetTypeBillString()}, {bill.ID} для клиента ID:{bill.ClientID}",
                    LoggerHub.LogEventType.DisplayOnForm);
                return true;
            }
            else return false;
        }

        public BillDeposit GetBillDeposit()
        {
            foreach (var item in bills)
            {
                if (item == null) continue;
                if (item.GetType() == typeof(BillDeposit)) return (BillDeposit)item;
            }
            return null;
        }

        public BillCredit GetBillCredit()
        {
            foreach (var item in bills)
            {
                if (item == null) continue;
                if (item.GetType() == typeof(BillCredit)) return (BillCredit)item;
            }
            return null;
        }



        public override string ToString()
        {
            string S = "BILLS: \n";
            foreach (var item in bills)
            {
                S += $"{item.GetType().Name} \t{item}\n";
            }

            return S;
        }



        public abstract class Bill
        {
            public decimal Money { get; set; }
            public string ID { get; set; }
            public int nativeID { get; set; }
            public int ClientID { get; set; }

            protected Bill(int clientID, int nativeID)
            {
                this.ID = $"BILL{nativeID}#{clientID}";
                this.nativeID = nativeID;
                this.ClientID = clientID;
            }
            public override string ToString()
            {
                return $"# ID = {ID,10}; \tMoney = {Money,10}";
            }
            /// <summary>
            /// Положить (перевести) на этот счет деньги
            /// </summary>
            /// <param name="billFromTake"></param>
            /// <param name="amount"></param>
            /// <returns></returns>
            public bool Put(Bill billFromTake, decimal amount)
            {
                if (billFromTake == null)
                {
                    Debug.WriteLine("Счёт для снятия не найден");
                    return false;
                }
                if (amount <= billFromTake.Money)
                {
                    billFromTake.Money -= amount;
                    Money += amount;
                    return true;
                }
                else return false;
            }
            /// <summary>
            /// Положить на этот счет деньги
            /// </summary>
            /// <param name="amount"></param>
            /// <returns></returns>
            public bool Put(decimal amount)
            {
                Money += amount;
                return true;
            }
            /// <summary>
            /// Снять с этого счёта деньги (перевести на другой счёт)
            /// </summary>
            /// <param name="billForPut"></param>
            /// <param name="amount"></param>
            /// <returns></returns>
            public abstract bool Take(Bill billForPut, decimal amount);

            /// <summary>
            /// Снять с этого счёта деньги
            /// </summary>
            /// <param name="amount"></param>
            /// <returns></returns>
            public abstract bool Take(decimal amount);

            public Type GetTypeBill()
            {
                if (this.nativeID == 1) { return typeof(BillDeposit); }
                else if (this.nativeID == 2) { return typeof(BillCredit); }
                else return null;
            }
            public string GetTypeBillString()
            {
                //return GetTypeBill().Name;
                if (this.nativeID == 1) { return "депозитный счёт"; }
                else if (this.nativeID == 2) { return "кредитный счёт"; }
                else return $"!!! счёт неизвестного типа, nativeID={this.nativeID}";
            }

        }

        public class BillDeposit : Bill
        {
            public BillDeposit(int clientID) : base(clientID, 1) { }
            /// <summary>
            /// Снять с этого счёта деньги (перевести на другой счёт)
            /// </summary>
            /// <param name="billForPut"></param>
            /// <param name="amount"></param>
            /// <returns></returns>
            public override bool Take(Bill billForPut, decimal amount)
            {
                if (billForPut == null)
                {
                    Debug.WriteLine("Счёт для передачи не найден");
                    return false;
                }
                if (Money >= amount)
                {
                    Money -= amount;
                    billForPut.Money += amount;
                    return true;
                }
                else return false;
            }
            /// <summary>
            /// Снять с этого счёта деньги
            /// </summary>
            /// <param name="amount"></param>
            /// <returns></returns>
            public override bool Take(decimal amount)
            {
                if (Money >= amount)
                {
                    Money -= amount;
                    return true;
                }
                else return false;
            }
        }

        public class BillCredit : Bill
        {
            public BillCredit(int clientID) : base(clientID, 2) { }
            /// <summary>
            /// Снять с этого счёта деньги (перевести на другой счёт)
            /// </summary>
            /// <param name="billForPut"></param>
            /// <param name="amount"></param>
            /// <returns></returns>
            public override bool Take(Bill billForPut, decimal amount)
            {
                if (billForPut == null)
                {
                    Debug.WriteLine("Счёт для передачи не найден");
                    return false;
                }
                Money -= amount;
                billForPut.Money += amount;
                return true;
            }
            /// <summary>
            /// Снять с этого счёта деньги
            /// </summary>
            /// <param name="amount"></param>
            /// <returns></returns>
            public override bool Take(decimal amount)
            {
                Money -= amount;
                return true;
            }
        }
    }
}
