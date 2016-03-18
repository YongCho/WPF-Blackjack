using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Input;
using BlackJack.Models;
using System.Linq;
using Stateless;
using System.Threading.Tasks;

namespace BlackJack.ViewModels
{
    public class BlackJackViewModel : BindableBase
    {
        public ObservableCollection<Card> PlayerCards { get; set; }
        public ObservableCollection<Card> DealerCards { get; set; }

        private int playerHandValue;
        public int PlayerHandValue
        {
            get { return this.playerHandValue; }
            set { SetProperty(ref this.playerHandValue, value); }
        }

        private int dealerHandValue;
        public int DealerHandValue
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

        enum State { ReadyDeal, PlayerTurn, AskingSplit, PlayingSplitted, DealerTurn, CheckingScore }
        enum Trigger { DealingDone, SameFace, Split, PlayerDone, DealerDone, Restart }

        State state = State.ReadyDeal;
        StateMachine<State, Trigger> stateMachine;

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

            // Configure state machine.
            this.stateMachine = new StateMachine<State, Trigger>(() => this.state, s => this.state = s);

            this.stateMachine.Configure(State.ReadyDeal)
                .OnEntry(RaiseCanExecuteChanged)
                .OnEntry(SetupTable)
                .Permit(Trigger.DealingDone, State.PlayerTurn);

            this.stateMachine.Configure(State.PlayerTurn)
                .OnEntry(RaiseCanExecuteChanged)
                .Permit(Trigger.PlayerDone, State.DealerTurn);

            this.stateMachine.Configure(State.DealerTurn)
                .OnEntry(RaiseCanExecuteChanged)
                .OnEntry(PlayDealerAsync)
                .Permit(Trigger.DealerDone, State.CheckingScore);

            this.stateMachine.Configure(State.CheckingScore)
                .OnEntry(RaiseCanExecuteChanged)
                .OnEntry(CheckScores)
                .Permit(Trigger.Restart, State.ReadyDeal);

            this.blackjack = new BlackJackGame();
            this.blackjack.DeckCount = 3;
        }

        private void SetupTable()
        {
            PlayerCards.Clear();
            DealerCards.Clear();

            PlayerHandValue = 0;
            DealerHandValue = 0;
        }

        private void HandlePlayerHandChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            PlayerHandValue = this.blackjack.CalculateValue(PlayerCards.ToList());
        }

        private void HandleDealerHandChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            DealerHandValue = this.blackjack.CalculateValue(DealerCards.ToList());
        }

        private async void PlayDealerAsync()
        {
            // Turn over the face-down card.
            foreach (Card card in DealerCards)
            {
                await Task.Delay(200);
                card.IsFaceDown = false;
            }

            while (DealerHandValue < 17)
            {
                await Task.Delay(200);
                DealerCards.Add(this.blackjack.DealCard());
            }

            this.stateMachine.Fire(Trigger.DealerDone);
        }

        private void CheckScores()
        {
            Trace.WriteLine("Checking Score");
        }

        private void DoDeal()
        {
            PlayerCards.Add(this.blackjack.DealCard());

            Card faceDownCard = this.blackjack.DealCard();
            faceDownCard.IsFaceDown = true;
            DealerCards.Add(faceDownCard);

            PlayerCards.Add(this.blackjack.DealCard());
            DealerCards.Add(this.blackjack.DealCard());

            this.stateMachine.Fire(Trigger.DealingDone);
        }

        private bool CanDeal()
        {
            return this.state == State.ReadyDeal;
        }

        private void DoHit()
        {
            PlayerCards.Add(this.blackjack.DealCard());
            int handValue = this.blackjack.CalculateValue(PlayerCards.ToList());
            if (handValue > 21)
            {
                this.stateMachine.Fire(Trigger.PlayerDone);
            }
        }

        private bool CanHit()
        {
            return this.state == State.PlayerTurn;
        }

        private void DoStand()
        {
            this.stateMachine.Fire(Trigger.PlayerDone);
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
            return this.state == State.AskingSplit;
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
