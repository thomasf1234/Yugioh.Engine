using System;

namespace Yugioh.Engine.Models
{
  public class Coin
  {
    public enum States : int { Heads = 0, Tails = 1 };

    private States _state;
    private Random _random;

    public Coin()
    {
      this._random = new Random();
    }

    public void Flip()
    {
      int result = this._random.Next(0, 2);
      this._state = (States)result;
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
