using System.Windows.Controls;
using System.Windows.Input;
using Blackjack.ViewModels;

namespace Blackjack.Views
{
    /// <summary>
    /// Interaction logic for BlackjackView.xaml
    /// </summary>
    public partial class BlackjackView : UserControl
    {
        public BlackjackView()
        {
            InitializeComponent();
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BlackjackViewModel viewModel = this.DataContext as BlackjackViewModel;
            if (viewModel.RestartCommand.CanExecute(null))
            {
                viewModel.RestartCommand.Execute(null);
            }
        }
    }
}
