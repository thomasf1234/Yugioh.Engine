using System;
using System.Collections.Generic;

namespace Yugioh.Engine.Models
{
    public class Turn
    {
        public int Index { get; set; }
        public Player Player { get; set; }
        public int SummonLimit { get; set; }
        public int SummonCount { get; set; }
        public Turn(int turnIndex, Player turnPlayer, int summonLimit)
        {
            this.Index = turnIndex;
            this.Player = turnPlayer;
            this.SummonLimit = summonLimit;
            this.SummonCount = 0;
        }

        public bool IsFirstTurn()
        {
            return this.Index == 0;
        }

        public bool ReachedSummonLimit()
        {
            return this.SummonCount == this.SummonLimit;
        }
    }
}
