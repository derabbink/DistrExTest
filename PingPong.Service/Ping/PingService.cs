using System;
using System.ServiceModel;
using PingPong.Contracts.Service;
using PingPong.Service.Events;

namespace PingPong.Service.Ping
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class PingService : IPing
    {
        public event EventHandler<PingEventArgs> PingRequest;

        public void Ping()
        {
            OnPingRequest(new PingEventArgs());
        }

        protected virtual void OnPingRequest(PingEventArgs e)
        {
            EventHandler<PingEventArgs> handler = PingRequest;
            if (handler != null) handler(this, e);
        }
    }
}
