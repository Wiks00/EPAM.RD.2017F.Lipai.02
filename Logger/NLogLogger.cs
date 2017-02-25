using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Logger
{
    public class NLogLogger : ILogger
    {
        private static readonly Lazy<NLogLogger> Adapter = new Lazy<NLogLogger>(() => new NLogLogger());
        private readonly NLog.Logger logger;

        private NLogLogger()
        {
            this.logger = LogManager.GetCurrentClassLogger();
        }

        public static NLogLogger Logger => Adapter.Value;

        public void Error(string message)
        {
            this.logger.Error(message);
        }

        public void Error(Exception ex, string message)
        {
            this.logger.Error(ex, message);
        }

        public void Info(string message)
        {
            this.logger.Info(message);
        }
    }
}
