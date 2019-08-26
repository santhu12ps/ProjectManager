using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Logger
{
    public class LoggerConstants
    {
        public enum Info
        {
            APIInfo = 100,
            ServiceLayerInfo = 200,
            DALInfo = 300
        }

        public enum InfoLevel
        {
            High = 1,
            Medium = 2,
            Low = 2
        }
    }
}
