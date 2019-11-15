using System;
using System.Collections.Generic;

using Yugioh.Engine.Exceptions;
using Yugioh.Engine.Models.Cards;

namespace Yugioh.Engine.Models
{
    public class Player
    {
        public string Name { get; set; }
        public int DuelistPoints { get; set; }
        public IList<Card> Cards { get; set; }
        public IList<Deck> Decks { get; set; }
        public string ActiveDeckName { get; set; }

        public Player() 
        {
            this.Cards = new List<Card>();
            this.Decks = new List<Deck>();
        }
    }
}
