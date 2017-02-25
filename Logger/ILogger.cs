using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public interface ILogger
    {
        void Error(string message);

        void Error(Exception ex, string message);

        void Info(string message);
    }
}
