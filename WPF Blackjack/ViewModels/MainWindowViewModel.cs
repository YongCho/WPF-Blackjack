using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using WPF_Blackjack.Models;

namespace WPF_Blackjack.ViewModels
{
    public class MainWindowViewModel
    {
        public ICommand HitCommand { get; set; }
        public ICommand StandCommand { get; set; }
        public ICommand SplitCommand { get; set; }

        public MainWindowViewModel()
        {
            HitCommand = new DelegateCommand(DoHit);
            StandCommand = new DelegateCommand(DoStand);
            SplitCommand = new DelegateCommand(DoSplit);
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
