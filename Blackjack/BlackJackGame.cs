using System;
using System.Collections.Generic;

namespace Blackjack.Models
{
    public class BlackjackGame
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

        public BlackjackGame()
        {
            this.deck = new Deck(DeckCount);
            this.deck.Shuffle();
        }

        public int CalculateValue(List<Card> hand)
        {
            int handValue = 0;
            int aceCount = 0;

            foreach (Card card in hand)
            {
                // Face-down card doesn't add to the hand's value.
                if (card.IsFaceDown)
                {
                    continue;
                }

                if (card.FaceNumber <= 10)
                {
                    handValue += card.FaceNumber;
                    if (card.FaceNumber == 1)
                    {
                        ++aceCount;
                    }
                }
                else
                {
                    handValue += 10;
                }
            }

            // Raise one ace to 11 if it will not bust the hand.
            if (handValue <= 11 && aceCount > 0)
            {
                handValue += 10;
            }

            return handValue;
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
