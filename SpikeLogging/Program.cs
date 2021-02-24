using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace SpikeLogging
{
    class Program
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       

        static private int i = -1;
        static private string str;
        static void Main(string[] args)
        {
            try
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(@"myLog4Net.xml"); //Enklare lösning på kopieringsproblemet fås genom filens Property-fönstro
                if (fi.Exists)
                {
                    log4net.Config.XmlConfigurator.Configure(fi);
                    logger.Info("File myLog4Net.xml exists");
                }
                else
                {
                    log4net.Config.BasicConfigurator.Configure(); // Den enklaste konfiguratorn kräver ej att du specar en konfigureringsfil
                    logger.Error("Missing file myLog4Net.xml");
                }
            }
            catch (Exception e)
            {
                log4net.Config.BasicConfigurator.Configure(); // Den enklaste konfiguratorn kräver ej att du specar en konfigureringsfil
                logger.Error("Missing file myLog4Net.xml", e);
            }
            if (logger.IsDebugEnabled) { logger.Debug("Test av DEBUG-logg"); }
            if (logger.IsInfoEnabled) { logger.Info("Test av INFO-logg"); }
            if (logger.IsWarnEnabled) { logger.Warn("Test av WARN-logg"); }
            if (logger.IsErrorEnabled) { logger.Error("Test av ERROR-logg"); }
            if (logger.IsFatalEnabled) { logger.Fatal("Test av FATAL-logg"); }
            while (i != 0)
            {
                Console.WriteLine("Enter a number");
                str = Console.ReadLine();
                Int32.TryParse(str,out i);
                if( i == 1)
                {
                    Console.WriteLine("Full in");
                    logger.Debug("Number 1 pressed");
                
                }
                else if (i == 2)
                {
                    Console.WriteLine("Half in");
                    logger.Debug("Number 2 pressed");
                   
                }

                else
                {
                    Console.WriteLine("Not in");
                    logger.Debug("Other pressed");
                }
            }
        }
    }
}
