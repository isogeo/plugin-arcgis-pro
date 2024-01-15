using System.IO;
using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository.Hierarchy;

namespace Isogeo.Utils.LogManager
{
    public static class Log
    {
        public static ILog Logger => log4net.LogManager.GetLogger("Log");

        public static void InitializeLogPath(string logDirectory)
        {
            //get the current logging repository for this application
            var repository = log4net.LogManager.GetRepository();
            //get all of the appenders for the repository
            var appenders = repository.GetAppenders();
            //only change the file path on the 'FileAppenders'
            foreach (var appender in (from iAppender in appenders
                where iAppender is FileAppender
                select iAppender))
            {
                //set the path to your logDirectory using the original file name defined
                //in configuration
                if (appender is not FileAppender fileAppender) 
                    continue;
                fileAppender.File = Path.Combine(logDirectory, Path.GetFileName(fileAppender.File));
                //make sure to call fileAppender.ActivateOptions() to notify the logging
                //sub system that the configuration for this appender has changed.
                fileAppender.ActivateOptions();
            }
        }

        public static void InitializeLogManager(string configFile)
        {
            using var fs = new FileStream(configFile, FileMode.Open);
            XmlConfigurator.Configure(fs);
        }

        public static string GetLogFilePath()
        {
            var rootAppender = ((Hierarchy)log4net.LogManager.GetRepository())
                .Root.Appenders.OfType<FileAppender>()
                .FirstOrDefault();

            return (rootAppender != null ? rootAppender.File : string.Empty);
        }
    }
}
