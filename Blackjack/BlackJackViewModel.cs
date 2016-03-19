using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Input;
using Blackjack.Models;
using System.Linq;
using Stateless;
using System.Threading.Tasks;

namespace Blackjack.ViewModels
{
    public class BlackjackViewModel : BindableBase
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

        // Have some delay between events so things won't happen too fast for the user.
        private int actionDelayMillisec = Properties.Settings.Default.GlobalActionDelayMilliseconds;

        public ICommand DealCommand { get; set; }
        public ICommand HitCommand { get; set; }
        public ICommand StandCommand { get; set; }
        public ICommand SplitCommand { get; set; }
        public ICommand RestartCommand { get; set; }

        enum State { ReadyDeal, Dealing, PlayerTurn, AskingSplit, PlayingSplitted, DealerTurn, CheckingWinner }
        enum Trigger { DealingStarted, DealingDone, SameFace, Split, PlayerDone, DealerDone, Restart }

        State state = State.ReadyDeal;
        StateMachine<State, Trigger> stateMachine;

        private BlackjackGame blackjack;

        public BlackjackViewModel()
        {
            PlayerCards = new ObservableCollection<Card>();
            DealerCards = new ObservableCollection<Card>();
            PlayerCards.CollectionChanged += HandlePlayerHandChange;
            DealerCards.CollectionChanged += HandleDealerHandChange;

            DealCommand = new DelegateCommand(DealCommand_Execute, DealCommand_CanExecute);
            HitCommand = new DelegateCommand(HitCommand_Execute, HitCommand_CanExecute);
            StandCommand = new DelegateCommand(StandCommand_Execute, StandCommand_CanExecute);
            SplitCommand = new DelegateCommand(SplitCommand_Execute, SplitCommand_CanExecute);
            RestartCommand = new DelegateCommand(RestartCommand_Execute, RestartCommand_CanExecute);

            // Configure state machine.
            this.stateMachine = new StateMachine<State, Trigger>(() => this.state, s => this.state = s);

            this.stateMachine.Configure(State.ReadyDeal)
                .OnEntry(RaiseCanExecuteChanged)
                .OnEntry(SetupTable)
                .Permit(Trigger.DealingStarted, State.Dealing);

            this.stateMachine.Configure(State.Dealing)
                .OnEntry(RaiseCanExecuteChanged)
                .Permit(Trigger.DealingDone, State.PlayerTurn);

            this.stateMachine.Configure(State.PlayerTurn)
                .OnEntry(RaiseCanExecuteChanged)
                .Permit(Trigger.PlayerDone, State.DealerTurn);

            this.stateMachine.Configure(State.DealerTurn)
                .OnEntry(RaiseCanExecuteChanged)
                .OnEntry(PlayDealer)
                .Permit(Trigger.DealerDone, State.CheckingWinner);

            this.stateMachine.Configure(State.CheckingWinner)
                .OnEntry(RaiseCanExecuteChanged)
                .OnEntry(CheckWinner)
                .Permit(Trigger.Restart, State.ReadyDeal);

            this.blackjack = new BlackjackGame();
            this.blackjack.DeckCount = 3;
        }

        private void SetupTable()
        {
            PlayerCards.Clear();
            DealerCards.Clear();

            PlayerHandValue = 0;
            DealerHandValue = 0;

            ResultText = string.Empty;
        }

        private void HandlePlayerHandChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshScores();
        }

        private void HandleDealerHandChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshScores();
        }

        private void RefreshScores()
        {
            PlayerHandValue = this.blackjack.CalculateValue(PlayerCards.ToList());
            DealerHandValue = this.blackjack.CalculateValue(DealerCards.ToList());
        }

        private async void PlayDealer()
        {
            // Show the face-down card.
            foreach (Card card in DealerCards)
            {
                await Task.Delay(actionDelayMillisec);
                card.IsFaceDown = false;
                RefreshScores();
            }

            await Task.Delay(actionDelayMillisec);

            // Dealer only proceeds if player is not already busted.
            if (PlayerHandValue <= 21)
            {
                while (DealerHandValue < 17)
                {
                    DealerCards.Add(this.blackjack.DealCard());
                    await Task.Delay(actionDelayMillisec);
                }
            }

            await Task.Delay(actionDelayMillisec);
            this.stateMachine.Fire(Trigger.DealerDone);
        }

        private void CheckWinner()
        {
            if (PlayerHandValue > 21 || (DealerHandValue <= 21 && DealerHandValue > PlayerHandValue))
            {
                ResultText = "Dealer Wins!";
            }
            else if (PlayerHandValue == DealerHandValue)
            {
                ResultText = "Push";
            }
            else
            {
                ResultText = "Win!";
            }
        }

        private async void DealCommand_Execute()
        {
            this.stateMachine.Fire(Trigger.DealingStarted);

            PlayerCards.Add(this.blackjack.DealCard());
            await Task.Delay(actionDelayMillisec);

            DealerCards.Add(this.blackjack.DealCard());
            await Task.Delay(actionDelayMillisec);

            PlayerCards.Add(this.blackjack.DealCard());
            await Task.Delay(actionDelayMillisec);

            Card faceDownCard = this.blackjack.DealCard();
            faceDownCard.IsFaceDown = true;
            DealerCards.Add(faceDownCard);
            await Task.Delay(actionDelayMillisec);

            this.stateMachine.Fire(Trigger.DealingDone);

            // If player is dealt a Blackjack, skip their turn.
            if (PlayerHandValue == 21)
            {
                this.stateMachine.Fire(Trigger.PlayerDone);
            }
        }

        private bool DealCommand_CanExecute()
        {
            return this.state == State.ReadyDeal;
        }

        private void HitCommand_Execute()
        {
            PlayerCards.Add(this.blackjack.DealCard());
            if (PlayerHandValue >= 21)
            {
                // Player busted.
                this.stateMachine.Fire(Trigger.PlayerDone);
            }
        }

        private bool HitCommand_CanExecute()
        {
            return this.state == State.PlayerTurn;
        }

        private void StandCommand_Execute()
        {
            this.stateMachine.Fire(Trigger.PlayerDone);
        }

        private bool StandCommand_CanExecute()
        {
            return HitCommand_CanExecute();
        }

        private void SplitCommand_Execute()
        {
            // Split is not implemented. This is just a placeholder.
            Trace.WriteLine("Split");
        }

        private bool SplitCommand_CanExecute()
        {
            return this.state == State.AskingSplit;
        }

        private void RestartCommand_Execute()
        {
            this.stateMachine.Fire(Trigger.Restart);
        }

        private bool RestartCommand_CanExecute()
        {
            return this.state == State.CheckingWinner;
        }

        private void RaiseCanExecuteChanged()
        {
            (DealCommand as DelegateCommand).RaiseCanExecuteChanged();
            (HitCommand as DelegateCommand).RaiseCanExecuteChanged();
            (StandCommand as DelegateCommand).RaiseCanExecuteChanged();
            (SplitCommand as DelegateCommand).RaiseCanExecuteChanged();
            (RestartCommand as DelegateCommand).RaiseCanExecuteChanged();
        }
    }
}
