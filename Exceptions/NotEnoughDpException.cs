using System;

namespace Yugioh.Engine.Exceptions
{
  public class NotEnoughDpException : Exception
  {
    public int RequiredDp { get; }
    public int UserDp { get; }
    public NotEnoughDpException(int _requiredDp, int _userDp)
    {
      this.RequiredDp = _requiredDp;
      this.UserDp = _userDp;
    }
  }
}
