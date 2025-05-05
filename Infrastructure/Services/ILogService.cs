namespace Aelfcraeft.Infrastructure.Services
{
    public interface ILogService
    {
        void Log(string message, string fileName = "", int lineNumber = 0);
        void LogErr(string errorMessage, string fileName = "", int lineNumber = 0);
    }
}