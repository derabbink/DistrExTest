using System;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.ServiceModel;
using System.Windows.Input;
using PingPong.Annotations;
using PingPong.Commands;
using PingPong.Contracts.Service;
using PingPong.Events;
using PingPong.Models;
using PingPong.Proxy;
using PingPong.Service.Pong;

namespace PingPong.ViewModels
{
    public class PingerViewModel : INotifyPropertyChanged, IDisposable
    {
        private PongService _pongService;
        private ServiceHost _pongHost;
        private ClientFactory<IPing> _pingClientFactory;
        private Client<IPing> _pingClient;
        private IObservable<PongEventArgs> _pongs;
        private readonly PingerPingCommand _pingCommand;

        public PingerViewModel()
        {
            Pinger = new Pinger(true);
            _pingCommand = new PingerPingCommand(this);

            InitPongService();
        }

        private void InitPongService()
        {
            _pongService = new PongService();
            _pongHost = new ServiceHost(_pongService);
            _pongHost.Open();
            _pingClientFactory = new ClientFactory<IPing>("*");
            _pingClient = _pingClientFactory.GetClient();

            _pongs = Observable.FromEventPattern<Service.Events.PongEventArgs>(handler => _pongService.PongRequest += handler,
                                                                               handler => _pongService.PongRequest -= handler)
                               .Select(_ => new PongEventArgs())
                               .ObserveOn(Scheduler.Default);
            _pongs.Subscribe(OnPong);
        }

        public bool CanPing
        {
            get
            {
                if (Pinger == null)
                    return false;
                return Pinger.CanPing;
            }
            private set
            {
                Pinger.CanPing = value;
                OnPropertyChanged("CanPing");
                _pingCommand.OnCanExecuteChanged();
            }
        }

        public void Ping()
        {
            _pingClient.Channel.Ping();
            CanPing = false;
        }

        protected virtual void OnPong(PongEventArgs e)
        {
            CanPing = true;
        }

        public Pinger Pinger { get; private set; }

        public ICommand PingCommand { get { return _pingCommand; } }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _pongHost.Close();
            _pingClient.Dispose();
        }

        #endregion
    }
}
