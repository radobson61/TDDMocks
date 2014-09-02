using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TDDMocks
{
    public class Log
    {
        private StreamWriter _logfile;

        public Log(StreamWriter logfile)
        {
            _logfile = logfile;
        }

        public void LogLine(string logline)
        {
            _logfile.WriteLine(logline);
        }
    }
}
