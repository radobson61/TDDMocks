using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using TDDMocks; 

namespace TDDMocksTest
{
    /// <summary>
    /// ServerCheck Code Coverage @ 96.83%
    /// Maintainability 78
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ServerCheckerTests
    {
        

        ///<summary>
        /// ServerConfigurationResult Is Error when ServerName is NotFoundinDB
        ///</summary>
        ///<remarks>AC 1. When there is no entry in the database for a monitored server name I want a server configuration error message</remarks>
        [TestMethod()]
        [TestCategory("Programmer")]
        public void ServerConfigurationResult_Is_Error_When_ServerName_is_NotFoundinDB()
        {
            //assemble
            string servername = "Server";
            DataTable table = MakeServerInformationTable();
            DataRow dr;
            dr = table.NewRow();
            dr["ServerName"] = string.Concat(servername);
            dr["CollectionTime"] = DateTime.Now;
            dr["Measurement"] = 0;
            table.Rows.Add(dr);

            ServerConfiguration expected = ServerConfiguration.Error;
            ServerCheck target = new ServerCheck(table);
            //act
            ServerConfiguration actual = target.Result(string.Concat(servername, "A"));
            //assert
            Assert.AreEqual(expected, actual);
        }

        
        ///<summary>
        /// ServerConfigurationResult Is ReportingError when LastDBEntry is MoreThan12HoursOld
        ///</summary>
        ///<remarks>AC 2. When the most recent entry in the database is more than 12 hours old I want a server reporting error message</remarks>
        [TestMethod()]
        [TestCategory("Programmer")]
        public void ServerConfigurationResult_Is_ReportingError_When_LastDBEntry_is_MoreThan12HoursOld()
        {
            //assemble
            string servername = "Server";
            DataTable serverInformation = MakeServerInformationTable();
            DataRow dr;
            dr = serverInformation.NewRow();
            dr["ServerName"] = string.Concat(servername);
            dr["CollectionTime"] = DateTime.Now.Subtract(TimeSpan.FromHours(13));
            dr["Measurement"] = 0;
            serverInformation.Rows.Add(dr);
            ServerConfiguration expected = ServerConfiguration.ReportingError;
            ServerCheck target = new ServerCheck(serverInformation);
            //act
            ServerConfiguration actual = target.Result(servername);
            //assert
            Assert.AreEqual(expected, actual);

        }

        ///<summary>
        /// ServerCheckResult Is RapidChange when RecentValue is 15PercentGreaterThanPrevious
        ///</summary>
        ///<remarks>3. When the most recent error in the database is more than 15% above the average I want rapid change information message.</remarks>
        [TestMethod()]
        [TestCategory("Programmer")]
        public void ServerCheckResult_Is_RapidChange_When_RecentValue_is_15PercentGreaterThanPrevious()
        {
            //assemble
            string servername = "Server";
            DataTable serverInformation = MakeServerInformationTable();
            DataRow dr;
            dr = serverInformation.NewRow();
            // older measurement
            dr["ServerName"] = servername;
            dr["CollectionTime"] = DateTime.Now.Subtract(TimeSpan.FromHours(13));
            dr["Measurement"] = 100;
            serverInformation.Rows.Add(dr);
            dr = serverInformation.NewRow();
            dr["ServerName"] = servername;
            dr["CollectionTime"] = DateTime.Now.Subtract(TimeSpan.FromHours(2));
            dr["Measurement"] = 116;
            serverInformation.Rows.Add(dr);
            ServerConfiguration expected = ServerConfiguration.RapidChange;
            ServerCheck target = new ServerCheck(serverInformation);
            //act
            ServerConfiguration actual = target.Result(servername);
            //assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Create a private method to increase code use.
        /// </summary>
        /// <returns></returns>
        private DataTable MakeServerInformationTable()
        {
            DataTable table = new DataTable("ServerInformation");
            DataColumn dc = new DataColumn() { DataType = System.Type.GetType("System.String"), ColumnName = "ServerName" };
            table.Columns.Add(dc);
            dc = new DataColumn() { DataType = System.Type.GetType("System.DateTime"), ColumnName = "CollectionTime" };
            table.Columns.Add(dc);
            dc = new DataColumn() { DataType = System.Type.GetType("System.Int32"), ColumnName = "Measurement" };
            table.Columns.Add(dc);
            return table;
        }

        
        ///<summary>
        /// ServerCheckResults Is OK when ItsARecentMeasurementAndChange is LessThan15Percent
        ///</summary>
        [TestMethod()]
        [TestCategory("Programmer")]
        public void ServerCheckResults_Is_OK_When_ItsARecentMeasurementAndChange_is_LessThan15Percent()
        {
            //assemble
            string servername = "Server";
            DataTable serverInformation = MakeServerInformationTable();
            DataRow dr;
            dr = serverInformation.NewRow();
            // older measurement
            dr["ServerName"] = servername;
            dr["CollectionTime"] = DateTime.Now.Subtract(TimeSpan.FromHours(13));
            dr["Measurement"] = 100;
            serverInformation.Rows.Add(dr);
            dr = serverInformation.NewRow();
            dr["ServerName"] = servername;
            dr["CollectionTime"] = DateTime.Now.Subtract(TimeSpan.FromHours(2));
            dr["Measurement"] = 114;
            serverInformation.Rows.Add(dr);
            ServerConfiguration expected = ServerConfiguration.OK;
            ServerCheck target = new ServerCheck(serverInformation);
            //act
            ServerConfiguration actual = target.Result(servername);
            //assert
            Assert.AreEqual(expected, actual);
        }


    }
}
