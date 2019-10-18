using System;
using System.Collections.Generic;

namespace Yugioh.Engine.Models
{
    public class Turn
    {
        // enum Phase : int { DrawPhase, StandbyPhase, MainPhase1, BattlePhase, MainPhase2, EndPhase };

        public int Index { get; set; }
        public Player Player { get; set; }
        public Turn(int turnIndex, Player turnPlayer)
        {
            this.Index = turnIndex;
            this.Player = turnPlayer;
        }
    }
}
