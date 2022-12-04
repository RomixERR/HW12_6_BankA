using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW12_6_BankA
{
    public class LoggerHub
    {
        public enum LogEventType
        {
            DisplayOnForm,
            dontDisplayOnForm
        }

        public static event Action<object, string, LogEventType> LogEvent;

        public static Repository Rep { get; private set;}
        public static Employer Employer { get; private set; }

        public LoggerHub(Repository rep, Employer employer)
        {
            Rep = rep;
            Employer = employer;
            LogEvent += LogToDebugConsole;
        }

        public static void Log(object obj, string msg, LogEventType logEventType)
        {
            LogEvent?.Invoke(obj, msg, logEventType);
        }

        public static void LogToDebugConsole(object obj, string msg, LogEventType logEventType)
        {
            Debug.WriteLine(msg);
        }






}
}
