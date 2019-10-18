using System;
using System.Collections.Generic;

using Yugioh.Engine.Models;

namespace Yugioh.Engine.Exceptions
{
    public class RunOutOfCardsException : Exception
    {
        public Player Player { get; set; }
        public RunOutOfCardsException(Player _player)
        {
            this.Player = _player;
        }
    }
}
