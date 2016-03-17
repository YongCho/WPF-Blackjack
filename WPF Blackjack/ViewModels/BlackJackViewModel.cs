using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Input;
using WPF_Blackjack.Models;

namespace WPF_Blackjack.ViewModels
{
    public class BlackJackViewModel : BindableBase
    {
        public ObservableCollection<Card> PlayerCards { get; set; }
        public ObservableCollection<Card> DealerCards { get; set; }

        private string playerHandValue = string.Empty;
        public string PlayerHandValue
        {
            get { return this.playerHandValue; }
            set { SetProperty(ref this.playerHandValue, value); }
        }

        private string dealerHandValue = string.Empty;
        public string DealerHandValue
        {
            get { return this.dealerHandValue; }
            set { SetProperty(ref this.dealerHandValue, value); }
        }

        public ICommand DealCommand { get; set; }
        public ICommand HitCommand { get; set; }
        public ICommand StandCommand { get; set; }
        public ICommand SplitCommand { get; set; }

        public BlackJackViewModel()
        {
            Deck deck = new Deck(3);

            PlayerCards = new ObservableCollection<Card>();
            DealerCards = new ObservableCollection<Card>();
            PlayerCards.CollectionChanged += HandlePlayerHandChange;
            DealerCards.CollectionChanged += HandleDealerHandChange;

            DealCommand = new DelegateCommand(DoDeal);
            HitCommand = new DelegateCommand(DoHit);
            StandCommand = new DelegateCommand(DoStand);
            SplitCommand = new DelegateCommand(DoSplit);
        }

        private void HandlePlayerHandChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            PlayerHandValue = Convert.ToString(CalculateValue(PlayerCards));
        }

        private void HandleDealerHandChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            DealerHandValue = Convert.ToString(CalculateValue(DealerCards));
        }

        private int CalculateValue(ObservableCollection<Card> hand)
        {
            int value = 0;

            return value;
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
