using System.ServiceModel;

namespace PingPong.Contracts.Service
{
    [ServiceContract(Name = "Pong", Namespace = "http://schemas.fugro/distrextest/service/pong")]
    public interface IPong
    {
        [OperationContract(IsOneWay = true)]
        void Pong();
    }
}
