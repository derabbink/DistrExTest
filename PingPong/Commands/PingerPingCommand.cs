using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using PingPong.ViewModels;

namespace PingPong.Commands
{
    public class PingerPingCommand : ICommand
    {
        private PingerViewModel _viewModel;
        public PingerPingCommand(PingerViewModel viewModel)
        {
            _viewModel = viewModel;
        }


        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
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
