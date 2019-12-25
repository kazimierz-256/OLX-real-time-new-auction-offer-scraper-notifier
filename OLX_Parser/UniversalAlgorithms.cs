using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace OLX_Parser
{
    class UniversalAlgorithms
    {
        public static Timer PlanExecution(TimeSpan time, ElapsedEventHandler elapsedEventHandler, bool autoReset = false)
        {
            var timer = new Timer()
            {
                AutoReset = autoReset,
                Interval = time.TotalMilliseconds,
            };
            timer.Elapsed += elapsedEventHandler;
            if (!autoReset)
                timer.Elapsed += (object sender, ElapsedEventArgs e) => timer.Dispose();
            timer.Enabled = true;
            return timer;
        }

        internal async static Task<bool> IsOnlineAsync()
        {
            PingReply ping = null;
            try
            {
                ping = await new Ping().SendPingAsync("8.8.8.8", 5000);
            }
            catch (PingException e)
            {
                return false;
            }

            return ping.Status == IPStatus.Success;
        }
    }
}
