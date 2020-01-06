using log4net;
using System;

namespace IsogeoLibrary.Utils
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
