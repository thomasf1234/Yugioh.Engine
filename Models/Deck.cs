using System;
using System.Collections.Generic;
using Yugioh.Engine.Entities;

namespace Yugioh.Engine.Models
{
    public class Deck
    {
        public IList<Card> Cards { get; set; }

        public Deck(IList<Card> _cards)
        {
            this.Cards = _cards;
        }
    }
}
