using System.Windows.Controls;
using System.Windows.Input;
using BlackJack.ViewModels;

namespace BlackJack.Views
{
    /// <summary>
    /// Interaction logic for BlackJackView.xaml
    /// </summary>
    public partial class BlackJackView : UserControl
    {
        public BlackJackView()
        {
            InitializeComponent();
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BlackJackViewModel viewModel = this.DataContext as BlackJackViewModel;
            if (viewModel.RestartCommand.CanExecute(null))
            {
                viewModel.RestartCommand.Execute(null);
            }
        }
    }
}
