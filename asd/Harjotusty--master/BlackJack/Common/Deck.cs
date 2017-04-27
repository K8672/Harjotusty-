﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Common
{
    public class Deck
    {
        //Luodaan lista Card muuttujista
        Queue<Card> deckOfCards;

        List<int> cardsAsInt;

        Random generator = new Random();

        private void GenerateCardsAsInt()
        {
            cardsAsInt = new List<int>();
            for (int i = 0; i < 52; i++)
            {
                cardsAsInt.Add(i);
            }
        }
        //Luodaan korttien sekoitus muuttuja
        public void Shuffle()
        {
            deckOfCards = new Queue<Card>();
            GenerateCardsAsInt();
            for (int i = 51; i >= 0; i--)
            {
                int index = generator.Next(0, i);
                int temp = cardsAsInt[i];
                cardsAsInt[i] = cardsAsInt[index];
                cardsAsInt[index] = temp;
            }
            FillDeck();
        }

        private void FillDeck()
        {
            for (int i = 0; i < cardsAsInt.Count; i++)
            {
                CardSuit suit = (CardSuit)(cardsAsInt[i] % 4);
                CardValue value = (CardValue)(cardsAsInt[i] % 13 + 1);
                deckOfCards.Enqueue(new Card(suit, value));
            }
        }

        public Card DealCard()
        {
            return deckOfCards.Dequeue();
        }
    }
}
