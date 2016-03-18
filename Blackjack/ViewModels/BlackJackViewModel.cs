using Prism.Commands;
using Prism.Mvvm;
using System;
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
        public string PlayerHandValueString
        {
            get { return this.playerHandValue; }
            set { SetProperty(ref this.playerHandValue, value); }
        }

        private string dealerHandValue = string.Empty;
        public string DealerHandValueString
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

        private BlackJackGame blackjack;

        public BlackJackViewModel()
        {
            PlayerCards = new ObservableCollection<Card>();
            DealerCards = new ObservableCollection<Card>();
            PlayerCards.CollectionChanged += HandlePlayerHandChange;
            DealerCards.CollectionChanged += HandleDealerHandChange;

            DealCommand = new DelegateCommand(DoDeal);
            HitCommand = new DelegateCommand(DoHit);
            StandCommand = new DelegateCommand(DoStand);
            SplitCommand = new DelegateCommand(DoSplit);

            this.blackjack = new BlackJackGame();
            this.blackjack.DeckCount = 3;
        }

        private void HandlePlayerHandChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            Card[] playerCardsArray = new Card[PlayerCards.Count];
            PlayerCards.CopyTo(playerCardsArray, 0);

            int handValue = this.blackjack.CalculateValue(playerCardsArray);
            PlayerHandValueString = Convert.ToString(handValue);
        }

        private void HandleDealerHandChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            Card[] dealerCardsArray = new Card[DealerCards.Count];
            DealerCards.CopyTo(dealerCardsArray, 0);

            int handValue = this.blackjack.CalculateValue(dealerCardsArray);
            DealerHandValueString = Convert.ToString(handValue);
        }

        private void DoDeal()
        {
            PlayerCards.Clear();
            DealerCards.Clear();

            PlayerCards.Add(this.blackjack.DealCard());

            Card faceDownCard = this.blackjack.DealCard();
            faceDownCard.IsFaceDown = true;
            DealerCards.Add(faceDownCard);

            PlayerCards.Add(this.blackjack.DealCard());
            DealerCards.Add(this.blackjack.DealCard());
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
