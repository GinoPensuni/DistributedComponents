using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppLogic.ServerLogic
{
    public class DataGate
    {
        private object payload;
        private EventWaitHandle waitGate;

        public DataGate()
        {
            this.payload = null;
            this.waitGate = new EventWaitHandle(false, EventResetMode.ManualReset);
        }

        public void SendData(object payload)
        {
            this.payload = payload;
            this.waitGate.Set();
        }

        public object ReceiveData()
        {
            this.waitGate.WaitOne();
            return this.payload;
        }
    }
}
