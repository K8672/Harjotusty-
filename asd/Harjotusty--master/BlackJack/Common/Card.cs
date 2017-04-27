using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Common
{
    public struct Card
    {
        // Luodaan kortti muutaja, johon tuodaan CardSuit ja CardValue.
        private CardSuit suit;
        private CardValue value;

        public CardSuit Suit
        {
            get
            {
                return this.suit;
            }
        }

        public CardValue Value
        {
            get
            {
                return this.value;
            }
        }

        public Card(CardSuit suit, CardValue value)
            : this()
        {
            this.suit = suit;
            this.value = value;
        }

        public override string ToString()
        {
            return string.Format("Suit: {0}, Value: {1}", this.suit, this.value);
        }
    }
}
