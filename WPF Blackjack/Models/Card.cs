using Prism.Mvvm;
using System;

namespace WPF_Blackjack.Models
{
    public class Card : BindableBase
    {
        public Suits Suit { get; private set; }

        private int faceNumber;
        public int FaceNumber
        {
            get { return this.faceNumber; }
            private set
            {
                SetProperty(ref this.faceNumber, value);
                if (this.faceNumber >= 2 && this.faceNumber <= 10)
                {
                    FaceLetter = Convert.ToString(this.faceNumber);
                }
                else
                {
                    switch (this.faceNumber)
                    {
                        case 1:
                            FaceLetter = "A";
                            break;
                        case 11:
                            FaceLetter = "J";
                            break;
                        case 12:
                            FaceLetter = "Q";
                            break;
                        case 13:
                            FaceLetter = "K";
                            break;
                    }
                }
            }
        }

        private string faceLetter;
        public string FaceLetter { get; private set; }

        public Card(Suits suit, int faceNumber)
        {
            Suit = suit;
            FaceNumber = faceNumber;
        }
    }
}
