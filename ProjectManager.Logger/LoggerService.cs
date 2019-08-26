using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Logger
{
    public class LoggerService : ILoggerService
    {
        #region Variable Declaration
        log4net.ILog logger = log4net.LogManager.GetLogger("LogFileAppender");
        private string infoMessage;
        #endregion

        #region Public Methods      

        public void LogInfo(string infoMsg, LoggerConstants.Info code)
        {
            this.infoMessage = infoMsg;
            logger.Info(" InfoCode:" + code + " Message :" + infoMsg);
        }
        public void LogDebug(string infoMsg, LoggerConstants.Info code)
        {
            this.infoMessage = infoMsg;
            logger.Debug(" InfoCode:" + code + " Message :" + infoMessage);
        }
        public void LogException(Exception exception, LoggerConstants.Info code)
        {
            this.infoMessage = exception.ToString();
            logger.Error(" InfoCode:" + code + " Message :" + infoMessage);
        }

        #endregion

    }
}
