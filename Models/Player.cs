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
        public IList<UserCard> Hand { get; set; }
        public IList<UserCard> RemovedFromPlay { get; set; }
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

            this.Hand = new List<UserCard>();
            this.RemovedFromPlay = new List<UserCard>();
            this.Turns = new List<Turn>();
        }
    }
}
