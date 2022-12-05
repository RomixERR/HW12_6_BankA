using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HW12_6_BankA
{   /// <summary>
    /// Класс всецело занимается ведением журналов. Подключение к классу осуществляется только через метод Log
    /// </summary>
    public class LoggerHub
    {
        public enum LogEventType
        {
            DisplayOnForm,
            dontDisplayOnForm
        }

        private static event Action<object, string, LogEventType> LogEvent; //Тот самый ЭКШОН, ну куда его еще впихнуть...

        private static Repository Rep { get; set; }
        private static Employer Employer { get; set; }
        private static MainWindow Window { get; set; }
        private static string FileName { get; set; }

        public LoggerHub(Repository rep, Employer employer, MainWindow window, string fileName)
        {
            Rep = rep;
            Employer = employer;
            Window = window;
            FileName = fileName;

            LogEvent += LogToDebugConsole;
            LogEvent += LogToWindowForm;
            LogEvent += LogToFile;

            LoggerHub.Log(this, "============Старт ЛОГГЕРА============", LoggerHub.LogEventType.dontDisplayOnForm);
            LoggerHub.Log(this, $"С базой данных работает {Employer}.", LoggerHub.LogEventType.dontDisplayOnForm);
        }
        /// <summary>
        /// Создаёт событие "запись лога" внутри этого класса
        /// </summary>
        /// <param name="obj">текущий объект this</param>
        /// <param name="msg">сообщение</param>
        /// <param name="logEventType">тип записи (где отображать)</param>
        public static void Log(object obj, string msg, LogEventType logEventType)
        {
            string message = $"{DateTime.Now}: {Employer}: {msg}";
            LogEvent?.Invoke(obj, message, logEventType);
        }

        private static void LogToDebugConsole(object obj, string msg, LogEventType logEventType)
        {
            Debug.WriteLine(msg);
        }

        private static void LogToWindowForm(object obj, string msg, LogEventType logEventType)
        {
            if (logEventType == LogEventType.DisplayOnForm) {
                Window.tBLog.Text += msg + "\n";
            }
        }

        private static void LogToFile(object obj, string msg, LogEventType logEventType)
        {
            File.AppendAllText(FileName, msg + "\n");
        }
    }
}