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
            string targetserver = (from serverrow in _serverdata.AsEnumerable()
                                 where serverrow.Field<string>("ServerName") == servername
                                 select (string)serverrow["ServerName"]).LastOrDefault();

            DateTime lastCollection = (from serverrow in _serverdata.AsEnumerable()
                                       where serverrow.Field<string>("ServerName") == servername
                                       select (DateTime)serverrow["CollectionTime"]).LastOrDefault();

            if (string.IsNullOrEmpty(targetserver))
                return ServerConfiguration.Error;
            if (DateTime.Now.Subtract(TimeSpan.FromHours(12)) > lastCollection)
                return ServerConfiguration.ReportingError;
            if (PercentageChange(servername) > 15)
                return ServerConfiguration.RapidChange;            
            return ServerConfiguration.OK;

        }

        private int PercentageChange(string servername)
        {
            decimal measure1;
            decimal measure2;
            int c = 0;
            IEnumerable<DataRow> results = _serverdata.AsEnumerable()
                .Where(s => s.Field<string>("ServerName") == servername)
                .OrderByDescending(o => o.Field<DateTime>("CollectionTime"));
            if (results != null && results.Count<DataRow>() > 1)
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
