using System;
using System.ServiceModel;

namespace PingPong.Proxy
{
    public class Client<TService> : IDisposable
    {
        public Client(TService channel)
        {
            Channel = channel;
        }

        public TService Channel { get; private set; }

        public void Dispose()
        {
            //Dispose() on ICommunicationObject can throw
            var channel = Channel as ICommunicationObject;
            try
            {
                channel.Close();
            }
            catch (CommunicationException)
            {
                channel.Abort();
            }
            catch (TimeoutException)
            {
                channel.Abort();
            }
        }
    }
}
