using System;
using System.ServiceModel;
using PingPong.Contracts.Service;
using PingPong.Service.Events;

namespace PingPong.Service.Pong
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class PongService : IPong
    {
        public event EventHandler<PongEventArgs> PongRequest;

        public void Pong()
        {
            OnPongRequest(new PongEventArgs());
        }

        protected virtual void OnPongRequest(PongEventArgs e)
        {
            EventHandler<PongEventArgs> handler = PongRequest;
            if (handler != null) handler(this, e);
        }
    }
}
