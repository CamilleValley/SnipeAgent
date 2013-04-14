using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using System.IO;

namespace UltimateSniper_Logger
{
    public static class Logger
    {
        private static readonly log4net.ILog log = LogManager.GetLogger(typeof (Logger));
        private static readonly string LOG_CONFIG_FILE = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase) + "\\log4net.config";

        static Logger()
        {
            //// TODO: Do exception handling for File access issues and supply sane defaults if it's unavailable.   
            LOG_CONFIG_FILE = LOG_CONFIG_FILE.Replace("file:\\", "");
            XmlConfigurator.ConfigureAndWatch(new FileInfo(LOG_CONFIG_FILE));
        }

        public static void CreateLog(string message, EnumLogLevel level)
        {
            Logger.CreateLog(message, string.Empty, null, level);
        }

        public static void CreateLog(string message, string context, Exception exception, EnumLogLevel level)
        {
            using (NDC.Push(context))
            {
                switch (level)
                {
                    case EnumLogLevel.DEBUG:
                        if (exception != null) Logger.log.Debug(message, exception);
                        else Logger.log.Debug(message);
                        break;

                    case EnumLogLevel.ERROR:
                        if (exception != null) Logger.log.Error(message, exception);
                        else Logger.log.Fatal(message);
                        break;

                    case EnumLogLevel.FATAL:
                        if (exception != null) Logger.log.Fatal(message, exception);
                        else Logger.log.Fatal(message);
                        break;

                    case EnumLogLevel.INFO:
                        if (exception != null) Logger.log.Info(message, exception);
                        else Logger.log.Info(message);
                        break;
                }
            }
        }
    }

    public enum EnumLogLevel
    {
        DEBUG, 
        INFO, 
        WARN, 
        ERROR, 
        FATAL
    }


}
