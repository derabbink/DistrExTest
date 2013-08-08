using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using PingPong.ViewModels;

namespace PingPong.Commands
{
    public class PongerPongCommand : ICommand
    {
        private PongerViewModel _viewModel;
        public PongerPongCommand(PongerViewModel viewModel)
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
            _viewModel.Pong();
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.CanPong;
        }

        #endregion
    }
}
