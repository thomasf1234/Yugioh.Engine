using System;
using System.Collections.Generic;

using Yugioh.Engine.Entities;
using Yugioh.Engine.Exceptions;

namespace Yugioh.Engine.Models
{
    public class DuelCard
    {
        public readonly Duel Duel;
        public readonly UserCard UserCard;
        public Player Owner { get; set; }

        public bool FaceDown { get; set; }

        public DuelCard(Duel _duel, UserCard _userCard, Player _owner)
        {
            this.Duel = _duel;
            this.UserCard = _userCard;
            this.Owner = _owner;
        }

        public bool IsFaceDown()
        {
            return this.FaceDown;
        }
    }
}
