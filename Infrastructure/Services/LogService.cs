using Godot;
using System; // Added to resolve the CS0103 error for 'DateTime'.

namespace Aelfcraeft.Infrastructure.Services
{
    public class LogService : BaseService, ILogService
    {
        private readonly string _logFilePath;

        public LogService() : base()
        {
            string runNumber = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            // Create a new log file for this run
            var logDirectory = "Temp/Logs";
            if (!System.IO.Directory.Exists(logDirectory))
            {
                System.IO.Directory.CreateDirectory(logDirectory);
            }
            _logFilePath = System.IO.Path.Combine(logDirectory, $"Run_{runNumber}.log");
            System.IO.File.WriteAllText(_logFilePath, $"Log for Run {runNumber}\n\n");
        }

        private string FormatLogMessage(string message, string fileName, int lineNumber)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffffff");
            var formattedFileName = fileName.Length > 25 ? fileName.Substring(0, 25) : fileName.PadRight(25);
            return $"[{timestamp}] [{formattedFileName}:{lineNumber:3}] {message}";
        }

        public virtual void Log(string message, [System.Runtime.CompilerServices.CallerFilePath] string fileName = "", [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            var formattedMessage = FormatLogMessage(message, System.IO.Path.GetFileName(fileName), lineNumber);
            
            GD.Print(formattedMessage);
            System.IO.File.AppendAllText(_logFilePath, formattedMessage + "\n");
        }

        public void LogErr(string message, [System.Runtime.CompilerServices.CallerFilePath] string fileName = "", [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            var formattedMessage = FormatLogMessage(message, System.IO.Path.GetFileName(fileName), lineNumber);
           
            GD.PrintErr($"\u001b[31m{formattedMessage}\u001b[0m"); // Print error in red to the console
            System.IO.File.AppendAllText(_logFilePath, formattedMessage + "\n");
        }
    }
}