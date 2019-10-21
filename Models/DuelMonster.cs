using System;
using System.Collections.Generic;

using Yugioh.Engine.Entities;
using Yugioh.Engine.Exceptions;

namespace Yugioh.Engine.Models
{
    public class DuelMonster : DuelCard
    {
        public bool AttackPosition { get; set; }

        public DuelMonster(UserCard _userCard, Player _owner) : base(_userCard, _owner)
        {
        }

        public bool IsInAttackPosition()
        {
            return this.AttackPosition;
        }
    }
}
