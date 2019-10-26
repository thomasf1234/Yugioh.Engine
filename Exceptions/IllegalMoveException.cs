using System;
using System.Collections.Generic;

using Yugioh.Engine.Models;

namespace Yugioh.Engine.Exceptions
{
  public class IllegalMoveException : Exception
  {
    public IllegalMoveException(string message) : base(message)
    {
        
    }
  }
}
