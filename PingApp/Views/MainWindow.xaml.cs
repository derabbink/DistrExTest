using System.Windows;
using PingPong.ViewModels;

namespace PingApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new PingerViewModel();
            InitializeComponent();
        }
    }
}
