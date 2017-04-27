using System;
using BlackJack.Common;
using BlackJack.Players;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.GameManager
{
        public class Manager
    {
        //Luodaan Manager oli jakaja
        private Deck playingDeck = new Deck();
        private List<Player> players = new List<Player>();
        private List<int> playerScores = new List<int>();
        private Player dealer = new Player();
        public bool Playing { get; set; }

        public Card DealerVisibleCard
        {
            get
            {
                return dealer.ShowHand()[0];
            }
        }

        public Card DealerLastCard
        {
            get
            {
                return dealer.ShowHand()[dealer.ShowHand().Count - 1];
            }
        }

        public List<Card> getDealerCards()
        {
            return dealer.ShowHand();
        }

        public Manager()
        {
            Playing = false;
        }

        public Manager(Player player1)
        {
            players.Add(player1);
        }

        public void AddPlayer(Player newPlayer)
        {
            players.Add(newPlayer);
        }
        //jakaja nostaa kaksi korttia
        public void DealFirstTwoCards()
        {
            playingDeck.Shuffle();

            dealer.HitMe(playingDeck.DealCard());
            dealer.HitMe(playingDeck.DealCard());

            for (int i = 0; i < players.Count; i++)
            {
                players[i].HitMe(playingDeck.DealCard());
                players[i].HitMe(playingDeck.DealCard());
            }
        }
        //jakaa kortin
        public Card DealCard()
        {
            return playingDeck.DealCard();
        }
        //uusi peli
        public void StartNewDeal()
        {
            playingDeck = new Deck();
            for (int i = 0; i < players.Count; i++)
            {
                players[i].ThrowCards();
                dealer.ThrowCards();
            }
            DealFirstTwoCards();
        }
        //Laskee käden arvon
        private int GetHandScore(List<Card> cards)
        {
            int score = 0;
            for (int i = 0; i < cards.Count; i++)
            {
                int cardValue = (int)cards[i].Value;
                //ässän arvon 11 jos score alle 22
                if (cardValue == 1 && score < 22)
                {
                    cardValue = 11;
                }
                //laittaa kuvakortit kympeiksi
                if (cardValue > 10)
                {
                    cardValue = 10;
                }
                
                score += cardValue;



            }

            if (score > 21)
            {
                score = -1;
            }

            //Blackjack palauttaa 0
            if (cards.Count == 2 && ((cards[0].Value == CardValue.Ace && (int)cards[1].Value >= 10) || (cards[1].Value == CardValue.Ace && (int)cards[0].Value >= 10)))
            {
                return 0;
            }

            return score;
        }
        
        public string DisplayScores()
        {
            StringBuilder showScores = new StringBuilder();
            for (int i = 0; i < players.Count; i++)
            {
                showScores.AppendLine(String.Format("Player {0} score: {1}", i, playerScores[i]));
            }
            return showScores.ToString();
        }
        //Antaa jakajalle kortin
        public void GiveDealerACard()
        {
            dealer.HitMe(playingDeck.DealCard());
        }
        //Hakee jakajan scoren
        public int GetDealerScore()
        {
            return dealer.GetScore();
        }
    }
}
