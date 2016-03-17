using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using WPF_Blackjack.Models;

namespace WPF_Blackjack.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public ObservableCollection<Card> PlayerCards { get; set; }
        public ObservableCollection<Card> DealerCards { get; set; }

        public ICommand DealCommand { get; set; }
        public ICommand HitCommand { get; set; }
        public ICommand StandCommand { get; set; }
        public ICommand SplitCommand { get; set; }

        public MainWindowViewModel()
        {
            PlayerCards = new ObservableCollection<Card>();
            DealerCards = new ObservableCollection<Card>();

            DealCommand = new DelegateCommand(DoDeal);
            HitCommand = new DelegateCommand(DoHit);
            StandCommand = new DelegateCommand(DoStand);
            SplitCommand = new DelegateCommand(DoSplit);
        }

        private void DoDeal()
        {
            Trace.WriteLine("Deal");
        }

        private void DoHit()
        {
            Trace.WriteLine("Hit");
        }

        private void DoStand()
        {
            Trace.WriteLine("Stand");
        }

        private void DoSplit()
        {
            Trace.WriteLine("Split");
        }
    }
}
