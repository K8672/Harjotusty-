using System;
using BlackJack.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Players
{
    public class Player
    {
        //Luodaan pelaaja
        private List<Card> hand = new List<Card>();
        private int myScore;

        public bool Busted { get; set; }
        
        public Player()
        {
            Busted = false;
        }
        //jakaa pelaajalle kortin
        public void HitMe(Card dealtCard)
        {
            hand.Add(dealtCard);
        }

        public List<Card> ShowHand()
        {
            return hand;
        }

        public Card LastCard()
        {
            return hand[hand.Count - 1];
        }

        public string ShoutHand()
        {
            StringBuilder handString = new StringBuilder();
            foreach (Card card in hand)
            {
                handString.AppendLine(card.Suit.ToString() + ' ' + card.Value.ToString());
            }
            return handString.ToString();
        }
        //Hakee käden arvon
        public int GetScore()
        {
            myScore = 0;
            for (int i = 0; i < hand.Count; i++)
            {
                if ((int)hand[i].Value < 10)
                {
                    //jos kädessä on ässä laittaa sen arvoksi 11
                    if ((int)hand[i].Value == 1)
                    {
                        myScore += 10;
                    }
                    myScore += (int)hand[i].Value;
                }
                else
                {
                    myScore += 10;
                }
            }
            //Blackjack palauttaa 21
            if (hand.Count == 2 && ((hand[0].Value == CardValue.Ace && (int)hand[1].Value >= 10) || (hand[1].Value == CardValue.Ace && (int)hand[0].Value >= 10)))
            {
                return 21;
            }
            //jos score yli 21 mutta kädessä on ässä muutta sen arvoksi 1
            if (myScore > 21)
            {
                for (int i = 0; i < hand.Count; i++)
                {

                    if ((int)hand[i].Value == 1)
                    {
                        myScore -= 10;
                    }
                }
            }
            //jos score yli 21 = häviö
            if (myScore > 21)
            {
                Busted = true;
            }

            return myScore;
        }
        //Luovutus (ei käytössä)
        public void ThrowCards()
        {
            hand.Clear();
            Busted = false;
        }
    }
}
