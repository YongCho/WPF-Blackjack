using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Input;
using BlackJack.Models;
using System.Linq;

namespace BlackJack.ViewModels
{
    public class BlackJackViewModel : BindableBase
    {
        public ObservableCollection<Card> PlayerCards { get; set; }
        public ObservableCollection<Card> DealerCards { get; set; }

        private string playerHandValueString = string.Empty;
        public string PlayerHandValueString
        {
            get { return this.playerHandValueString; }
            set { SetProperty(ref this.playerHandValueString, value); }
        }

        private string dealerHandValueString = string.Empty;
        public string DealerHandValueString
        {
            get { return this.dealerHandValueString; }
            set { SetProperty(ref this.dealerHandValueString, value); }
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

            DealCommand = new DelegateCommand(DoDeal, CanDeal);
            HitCommand = new DelegateCommand(DoHit, CanHit);
            StandCommand = new DelegateCommand(DoStand, CanStand);
            SplitCommand = new DelegateCommand(DoSplit, CanSplit);

            this.blackjack = new BlackJackGame();
            this.blackjack.DeckCount = 3;
        }

        private void HandlePlayerHandChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            int handValue = this.blackjack.CalculateValue(PlayerCards.ToList());
            PlayerHandValueString = Convert.ToString(handValue);
        }

        private void HandleDealerHandChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            int handValue = this.blackjack.CalculateValue(DealerCards.ToList());
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

        private bool CanDeal()
        {
            return true;
        }

        private void DoHit()
        {
            PlayerCards.Add(this.blackjack.DealCard());
        }

        private bool CanHit()
        {
            return true;
        }

        private void DoStand()
        {
            Trace.WriteLine("Stand");
        }

        private bool CanStand()
        {
            return CanHit();
        }

        private void DoSplit()
        {
            Trace.WriteLine("Split");
        }

        private bool CanSplit()
        {
            return false;
        }

        private void RaiseCanExecuteChanged()
        {
            (DealCommand as DelegateCommand).RaiseCanExecuteChanged();
            (HitCommand as DelegateCommand).RaiseCanExecuteChanged();
            (StandCommand as DelegateCommand).RaiseCanExecuteChanged();
            (SplitCommand as DelegateCommand).RaiseCanExecuteChanged();
        }
    }
}
