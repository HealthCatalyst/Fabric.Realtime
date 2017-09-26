using System;
using Xunit;

namespace Fabric.Realtime.EndToEndTests
{
    using System.Threading;
    using System.Threading.Tasks;

    public class EndToEndTester
    {
        [Fact]
        public void TestSendingHL7()
        {
            // from http://www.mieweb.com/wiki/Sample_HL7_Messages#ADT.5EA01
            var message =
                @"MSH|^~\&|SENDING_APPLICATION|SENDING_FACILITY|RECEIVING_APPLICATION|RECEIVING_FACILITY|20110613083617||ADT^A01|934576120110613083617|P|2.3||||
EVN|A01|20110613083617|||
PID|1||135769||MOUSE^MICKEY^||19281118|M|||123 Main St.^^Lake Buena Vista^FL^32830||(407)939-1289^^^theMainMouse@disney.com|||||1719|99999999||||||||||||||||||||
PV1|1|O|||||^^^^^^^^|^^^^^^^^";

            // set up the queue first
            var rabbitMqListener = new RabbitMqListener();

            rabbitMqListener.SetupExchange();

            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;

            var waitHandle = new AutoResetEvent(false);

            var task = Task.Run(() => rabbitMqListener.GetMessage(token, waitHandle), token);

            var result = HL7Sender.SendHL7("localhost", 6661, message);

            Assert.True(result);
            // HL7Sender.SendHL7("127.0.0.1", 6661, message);

            waitHandle.WaitOne();

            tokenSource.Cancel();
            task.Wait();

            var r = task.Result;

            Assert.NotNull(r);
            Assert.NotEmpty(r);


        }
    }

}
