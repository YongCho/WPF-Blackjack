using System;
using System.Collections.Generic;

namespace WPF_Blackjack.Models
{
    public class Deck
    {
        private List<Card> cards;
        private Random random;
        private int deckCount;
        public int DeckCount
        {
            get { return this.deckCount; }
            private set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("DeckCount", value, "Deck count must be 1 or higher.");
                }
                this.deckCount = value;
            }
        }

        public int CardCount
        {
            get { return this.cards.Count; }
        }

        public Deck(int numDecks)
        {
            this.random = new Random();
            this.cards = new List<Card>(numDecks * 52);
            Reset(numDecks);
        }

        public void Reset()
        {
            Reset(DeckCount);
        }

        public void Reset(int numDecks)
        {
            DeckCount = numDecks;
            this.cards.Clear();

            for (int i = 0; i < numDecks; ++i)
            {
                foreach (Suits suit in Enum.GetValues(typeof(Suits)))
                {
                    for (int faceVal = 1; faceVal <= 13; ++faceVal)
                    {
                        this.cards.Add(new Card(suit, faceVal));
                    }
                }
            }
        }

        public void Shuffle()
        {
            // Swap each card with another random card in the deck.
            for (int i = 0; i < this.cards.Count; ++i)
            {
                int randomIndex = this.random.Next(0, this.cards.Count - 1);
                Card randomCard = this.cards[randomIndex];
                this.cards[randomIndex] = this.cards[i];
                this.cards[i] = randomCard;
            }
        }

        public Card DealCard()
        {
            Card c = this.cards[0];
            this.cards.RemoveAt(0);
            return c;
        }
    }
}
