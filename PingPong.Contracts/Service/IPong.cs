using System.ServiceModel;

namespace PingPong.Contracts.Service
{
    [ServiceContract(Name = "Ping", Namespace = "http://schemas.fugro/distrextest/service/ping")]
    public interface IPing
    {
        [OperationContract(IsOneWay = true)]
        void Ping();
    }
}
