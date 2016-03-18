using System;

namespace BlackJack.Models
{
    public class BlackJackGame
    {
        private static int defaultDeckCount = 1;

        private Deck deck;
        private int deckCount = defaultDeckCount;
        public int DeckCount
        {
            get { return this.deckCount; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("DeckCount", value, "DeckCount must be 1 or higher.");
                }
                this.deck.Reset(this.deckCount);
                this.deck.Shuffle();
            }
        }

        public BlackJackGame()
        {
            this.deck = new Deck(DeckCount);
            this.deck.Shuffle();
        }

        public int CalculateValue(Card[] hand)
        {
            int value = 0;

            return value;
        }

        public Card DealCard()
        {
            if (this.deck.CardCount < 20)
            {
                this.deck.Reset();
                this.deck.Shuffle();
            }
            return this.deck.DealCard();
        }
    }
}
