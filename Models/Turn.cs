namespace Yugioh.Engine.Models
{
  public class Turn
    {
        public int Index { get; set; }
        public Player Player { get; set; }

        public Turn(int turnIndex, Player turnPlayer)
        {
            this.Index = turnIndex;
            this.Player = turnPlayer;
        }

        public bool IsFirstTurn()
        {
            return this.Index == 0;
        }
    }
}
