using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using PingPong.ViewModels;

namespace PingPong.Commands
{
    public class PingerPingCommand : ICommand
    {
        private readonly PingerViewModel _viewModel;
        private readonly SynchronizationContext _synchronizationContext;

        public PingerPingCommand(PingerViewModel viewModel)
        {
            _viewModel = viewModel;
            _synchronizationContext = SynchronizationContext.Current;
        }

        #region ICommand Members

        public event EventHandler CanExecuteChanged;

        public virtual void OnCanExecuteChanged()
        {
            EventHandler handler = CanExecuteChanged;
            if (handler != null)
                _synchronizationContext.Send(_ => handler(this, EventArgs.Empty), null);
        }

        public void Execute(object parameter)
        {
            _viewModel.Ping();
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.CanPing;
        }

        #endregion
    }
}
