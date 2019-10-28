using System;

namespace Yugioh.Engine.Models
{
  public class Dice
  {
    public enum States : int { One = 0, Two = 1, Three = 2, Four = 3, Five = 4, Six = 5 };

    private States _state;

    public void Roll()
    {
      Random random = new Random();
      int result = random.Next(0, 6);
      this._state = (States) result;
    }

    public States GetState()
    {
        return this._state;
    }

    public bool IsOne()
    {
        return this._state == States.One;
    }

    public bool IsTwo()
    {
        return this._state == States.Two;
    }

    public bool IsThree()
    {
        return this._state == States.Three;
    }

    public bool IsFour()
    {
        return this._state == States.Four;
    }

    public bool IsFive()
    {
        return this._state == States.Five;
    }

    public bool IsSix()
    {
        return this._state == States.Six;
    }
  }
}
