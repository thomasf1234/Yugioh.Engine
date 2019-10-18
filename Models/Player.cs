using System;
using System.Collections.Generic;

using Yugioh.Engine.Entities;
using Yugioh.Engine.Exceptions;

namespace Yugioh.Engine.Models
{
    public class Player
    {
        public User User { get; set; }
        public int Lp { get; set; }
        public IList<Card> Hand { get; set; }
        public IList<Card> RemovedFromPlay { get; set; }
        public Player Opponent { get; set; }
        public FieldSide FieldSide { get; set; }
        public Deck MainDeck { get; set; }
        public Deck ExtraDeck { get; set; }
        public Deck SideDeck { get; set; }
        public IList<Turn> Turns { get; set; }

        public Player(User _user, Deck mainDeck,  Deck extraDeck, Deck sideDeck)
        {
            this.User = _user;

            this.MainDeck = mainDeck;
            this.ExtraDeck = extraDeck;
            this.SideDeck = sideDeck;

            this.Hand = new List<Card>();
            this.RemovedFromPlay = new List<Card>();
            this.Turns = new List<Turn>();
        }

        public void Shuffle(Deck deck)
        {
            Random _random = new Random();
            int n = deck.Cards.Count;
            for (int i = 0; i < n; i++)
            {
                // Use Next on random instance with an argument.
                // ... The argument is an exclusive bound.
                //     So we will not go past the end of the array.
                int r = i + _random.Next(n - i);
                Card card = deck.Cards[r];

                // Swap cards
                deck.Cards[r] = deck.Cards[i];
                deck.Cards[i] = card;
            }
        }

        public void Draw(Deck deck)
        {
            if (deck.Cards.Count == 0)
            {
                throw new RunOutOfCardsException(this);
            }
            else 
            {
                this.Hand.Add(deck.Cards[0]);
                deck.Cards.RemoveAt(0);
            }
        }
    }
}
