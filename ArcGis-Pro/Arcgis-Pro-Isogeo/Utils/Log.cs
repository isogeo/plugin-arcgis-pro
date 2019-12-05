using log4net;
using System;

namespace ArcMapAddinIsogeo.Utils
{
    internal class Log
    {
        public static ILog DockableWindowLogger
        {
            get
            {
                return LogManager.GetLogger("DockableWindow");
            }
        }
    }
}
