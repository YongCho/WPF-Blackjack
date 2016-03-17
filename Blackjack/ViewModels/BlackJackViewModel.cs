using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Input;
using BlackJack.Models;

namespace BlackJack.ViewModels
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

        private string resultText = string.Empty;
        public string ResultText
        {
            get { return this.resultText; }
            set { SetProperty(ref this.resultText, value); }
        }

        public ICommand DealCommand { get; set; }
        public ICommand HitCommand { get; set; }
        public ICommand StandCommand { get; set; }
        public ICommand SplitCommand { get; set; }

        private Deck deck;

        public BlackJackViewModel()
        {
            int numDecks = 3;
            this.deck = new Deck(numDecks);

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
            if (this.deck.CardCount < 20)
            {
                this.deck.Reset();
                this.deck.Shuffle();
            }

            PlayerCards.Clear();
            DealerCards.Clear();

            PlayerCards.Add(this.deck.DealCard());
            Card c = this.deck.DealCard();
            c.IsFaceDown = true;
            DealerCards.Add(c);

            PlayerCards.Add(this.deck.DealCard());
            DealerCards.Add(this.deck.DealCard());
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
