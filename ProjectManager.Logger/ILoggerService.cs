using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Logger
{
    public interface ILoggerService
    {
        void LogInfo(string infoMsg, LoggerConstants.Info code);
        void LogDebug(string infoMsg, LoggerConstants.Info code);
        void LogException(Exception exception, LoggerConstants.Info code);
    }
}
