using System;

namespace Yugioh.Engine.Exceptions
{
  public class IllegalDeckException : Exception
  {
    public IllegalDeckException(string message) : base(message)
    {
        
    }
  }
}
