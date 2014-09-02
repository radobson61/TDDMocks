using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using TDDMocks;
using Rhino.Mocks;

namespace TDDMocksTest
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class LogTests
    {
        
        ///<summary>
        /// Log Writes TestString when TestString is SentToLog
        ///</summary>
        [TestMethod()]
        [TestCategory("Programmer")]
        public void Log_Writes_TestString_When_TestString_is_SentToLog()
        {
            //assemble
            StreamWriter logfile = MockRepository.GenerateMock<StreamWriter>();
            Log target = new Log(logfile);
            string logline = "TestString";
            //act
            target.LogLine(logline);
            //assert
            logfile.AssertWasCalled(l => l.WriteLine(logline));
        }
    }
}
