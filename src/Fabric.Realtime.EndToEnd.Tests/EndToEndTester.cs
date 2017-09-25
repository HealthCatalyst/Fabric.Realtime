using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fabric.Realtime.EndToEnd.Tests
{
    [TestClass]
    public class EndToEndTester
    {
        [TestMethod]
        public void TestSendingHL7()
        {
            // from http://www.mieweb.com/wiki/Sample_HL7_Messages#ADT.5EA01
            var message =
                @"MSH|^~\&|SENDING_APPLICATION|SENDING_FACILITY|RECEIVING_APPLICATION|RECEIVING_FACILITY|20110613083617||ADT^A01|934576120110613083617|P|2.3||||
EVN|A01|20110613083617|||
PID|1||135769||MOUSE^MICKEY^||19281118|M|||123 Main St.^^Lake Buena Vista^FL^32830||(407)939-1289^^^theMainMouse@disney.com|||||1719|99999999||||||||||||||||||||
PV1|1|O|||||^^^^^^^^|^^^^^^^^";

            var result = HL7Sender.SendHL7("localhost", 6661, message);

            Assert.IsTrue(result);
            // HL7Sender.SendHL7("127.0.0.1", 6661, message);
        }
    }
}
