using Prism.Mvvm;
using System;

namespace BlackJack.Models
{
    public class Card : BindableBase
    {
        private Suits suit;
        public Suits Suit
        {
            get { return this.suit; }
            private set
            {
                SetProperty(ref this.suit, value);
                switch (this.suit)
                {
                    case Suits.Spades:
                        SuitString = "Spades";
                        break;
                    case Suits.Diamonds:
                        SuitString = "Diamonds";
                        break;
                    case Suits.Clubs:
                        SuitString = "Clubs";
                        break;
                    case Suits.Hearts:
                        SuitString = "Hearts";
                        break;
                }
            }
        }

        private string suitString = string.Empty;
        public string SuitString
        {
            get { return this.suitString; }
            private set { SetProperty(ref this.suitString, value); }
        }

        private int faceNumber;
        public int FaceNumber
        {
            get { return this.faceNumber; }
            private set
            {
                if (value < 1 || value > 13)
                {
                    throw new ArgumentOutOfRangeException("FaceNumber", value, "Face value must be within [1, 13].");
                }

                SetProperty(ref this.faceNumber, value);
                switch (this.faceNumber)
                {
                    case 1:
                        FaceLetter = "A";
                        FaceString = "Ace";
                        break;
                    case 2:
                        FaceLetter = Convert.ToString(this.faceNumber);
                        FaceString = "Two";
                        break;
                    case 3:
                        FaceLetter = Convert.ToString(this.faceNumber);
                        FaceString = "Three";
                        break;
                    case 4:
                        FaceLetter = Convert.ToString(this.faceNumber);
                        FaceString = "Four";
                        break;
                    case 5:
                        FaceLetter = Convert.ToString(this.faceNumber);
                        FaceString = "Five";
                        break;
                    case 6:
                        FaceLetter = Convert.ToString(this.faceNumber);
                        FaceString = "Six";
                        break;
                    case 7:
                        FaceLetter = Convert.ToString(this.faceNumber);
                        FaceString = "Seven";
                        break;
                    case 8:
                        FaceLetter = Convert.ToString(this.faceNumber);
                        FaceString = "Eight";
                        break;
                    case 9:
                        FaceLetter = Convert.ToString(this.faceNumber);
                        FaceString = "Nine";
                        break;
                    case 10:
                        FaceLetter = Convert.ToString(this.faceNumber);
                        FaceString = "Ten";
                        break;
                    case 11:
                        FaceLetter = "J";
                        FaceString = "Jack";
                        break;
                    case 12:
                        FaceLetter = "Q";
                        FaceString = "Queen";
                        break;
                    case 13:
                        FaceLetter = "K";
                        FaceString = "King";
                        break;
                }
            }
        }

        private string faceLetter = string.Empty;
        public string FaceLetter
        {
            get { return this.faceLetter; }
            private set { SetProperty(ref this.faceLetter, value); }
        }

        private string faceString = string.Empty;
        public string FaceString
        {
            get { return this.faceString; }
            private set { SetProperty(ref this.faceString, value); }
        }

        private bool isFaceDown = false;
        public bool IsFaceDown
        {
            get { return this.isFaceDown; }
            set { SetProperty(ref this.isFaceDown, value); }
        }

        public Card(Suits suit, int faceNumber)
        {
            Suit = suit;
            FaceNumber = faceNumber;
        }

        public override string ToString()
        {
            return FaceString + " of " + SuitString;
        }
    }
}
