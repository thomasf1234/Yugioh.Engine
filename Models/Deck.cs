using System;
using System.Collections.Generic;
using Yugioh.Engine.Entities;

namespace Yugioh.Engine.Models
{
    public class Deck
    {
        public IList<UserCard> UserCards { get; set; }

        public Deck(IList<UserCard> _userCards)
        {
            this.UserCards = _userCards;
        }
    }
}
