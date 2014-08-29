using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDMocks
{
    public enum ServerConfiguration { OK, Error, ReportingError, RapidChange};

    public class ServerCheck
    {
        DataTable _serverdata;
        public ServerCheck(DataTable table)
        {
            _serverdata = table;
        }

        public ServerConfiguration Result(string servername)
        {
            IEnumerable<DataRow> results = _serverdata.AsEnumerable()
    .Where(s => s.Field<string>("ServerName") == servername)
    .OrderByDescending(o => o.Field<DateTime>("CollectionTime"));



            return FindResult(results);

        }

        private ServerConfiguration FindResult(IEnumerable<DataRow> results)
        {
            ServerConfiguration result = ServerConfiguration.OK;

            if (results.Count() == 0)
                return ServerConfiguration.Error;
            if (DateTime.Now.Subtract(TimeSpan.FromHours(12)) > results.First().Field<DateTime>("CollectionTime"))
                return ServerConfiguration.ReportingError;
            if (PercentageChange(results.First().Field<string>("ServerName"), results) > 15)
                return ServerConfiguration.RapidChange;
            
            return result;
        }

        private int PercentageChange(string servername, IEnumerable<DataRow> results)
        {
            decimal measure1;
            decimal measure2;
            int c = 0;

  
            if (results.Count<DataRow>() > 1)
            {
                measure1 = (decimal)results.ElementAt(0).Field<Int32>("Measurement");
                measure2 = (decimal)results.ElementAt(1).Field<Int32>("Measurement");
                decimal r = (measure1 / measure2);
                c = (int)(r * 100) - 100;
            }

            return c;

        }

    }
}
