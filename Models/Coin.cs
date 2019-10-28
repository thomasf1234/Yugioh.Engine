using System;

namespace Yugioh.Engine.Models
{
  public class Coin
  {
    public enum States : int { Heads = 0, Tails = 1 };

    private States _state;

    public void Flip()
    {
      Random random = new Random();
      int result = random.Next(0, 2);
      this._state = (States) result;
    }

    public States GetState()
    {
        return this._state;
    }

    public bool IsHeads()
    {
        return this._state == States.Heads;
    }

    public bool IsTails()
    {
        return this._state == States.Tails;
    }
  }
}
