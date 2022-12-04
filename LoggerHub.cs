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
        public static MainWindow Window { get; private set; }

        public LoggerHub(Repository rep, Employer employer, MainWindow window)
        {
            Rep = rep;
            Employer = employer;
            Window = window;

            LogEvent += LogToDebugConsole;
            LogEvent += LogToWindowForm;


            LoggerHub.Log(this, "============Старт ЛОГГЕРА============", LoggerHub.LogEventType.dontDisplayOnForm);
            LoggerHub.Log(this, $"С базой данных работает {Employer}.", LoggerHub.LogEventType.dontDisplayOnForm);
        }

        public static void Log(object obj, string msg, LogEventType logEventType)
        {
            string message = $"{DateTime.Now}: {Employer}: {msg}";
            LogEvent?.Invoke(obj, message, logEventType);
        }

        public static void LogToDebugConsole(object obj, string msg, LogEventType logEventType)
        {
            Debug.WriteLine(msg);
        }

        public static void LogToWindowForm(object obj, string msg, LogEventType logEventType)
        {
            if (logEventType == LogEventType.DisplayOnForm) {
                Window.tBLog.Text += msg + "\n";
            }
        }




    }
}