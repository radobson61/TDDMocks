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
            return FindResult(_serverdata.AsEnumerable()
                .Where(s => s.Field<string>("ServerName") == servername)
                .OrderByDescending(d => d.Field<DateTime>("CollectionTime")));

        }

        private ServerConfiguration FindResult(IEnumerable<DataRow> results)
        {

            if (results.Count() == 0)
                return ServerConfiguration.Error;
            if (DateTime.Now.Subtract(TimeSpan.FromHours(12)) > results.First().Field<DateTime>("CollectionTime"))
                return ServerConfiguration.ReportingError;
            if (PercentageChange(results.First().Field<string>("ServerName"), results) > 15)
                return ServerConfiguration.RapidChange;

            return ServerConfiguration.OK;
        }

        private int PercentageChange(string servername, IEnumerable<DataRow> results)
        {
            if (results.Count<DataRow>() > 1)
            {
                decimal r = ((decimal)results.ElementAt(0).Field<Int32>("Measurement") / (decimal)results.ElementAt(1).Field<Int32>("Measurement"));
                return (int)(r * 100) - 100;
            }

            return 0;
        }

    }
}
