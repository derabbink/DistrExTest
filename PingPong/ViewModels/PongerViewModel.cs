﻿using System;
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
using PingPong.Service.Ping;

namespace PingPong.ViewModels
{
    public class PongerViewModel : INotifyPropertyChanged, IDisposable
    {
        private PingService _pingService;
        private ServiceHost _pingHost;
        private ClientFactory<IPong> _pongClientFactory;
        private Client<IPong> _pongClient;
        private IObservable<PingEventArgs> _pings;
        private readonly PongerPongCommand _pongCommand;

        public PongerViewModel()
        {
            Ponger = new Ponger(false);
            _pongCommand = new PongerPongCommand(this);

            InitPingService();
        }

        private void InitPingService()
        {
            _pingService = new PingService();
            _pingHost = new ServiceHost(_pingService);
            _pingHost.Open();
            _pongClientFactory = new ClientFactory<IPong>("*");
            _pongClient = _pongClientFactory.GetClient();

            _pings = Observable.FromEventPattern<Service.Events.PingEventArgs>(handler => _pingService.PingRequest += handler,
                                                                               handler => _pingService.PingRequest -= handler)
                               .Select(_ => new PingEventArgs())
                               .ObserveOn(Scheduler.Default);
            _pings.Subscribe(OnPing);
        }

        public bool CanPong
        {
            get
            {
                if (Ponger == null)
                    return false;
                return Ponger.CanPong;
            }
            private set
            {
                Ponger.CanPong = value;
                OnPropertyChanged("CanPong");
                _pongCommand.OnCanExecuteChanged();
            }
        }

        public void Pong()
        {
            _pongClient.Channel.Pong();
            CanPong = false;
        }

        protected virtual void OnPing(PingEventArgs e)
        {
            CanPong = true;
        }

        public Ponger Ponger { get; private set; }

        public ICommand PongCommand { get { return _pongCommand; } }

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
            _pingHost.Close();
            _pongClient.Dispose();
        }

        #endregion
    }
}
