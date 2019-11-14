using System;

namespace Yugioh.Engine.Exceptions
{
  public class UserNotFoundException : Exception
  {
    public string Username { get; }
    public UserNotFoundException(string _username)
    {
      this.Username = _username;
    }
  }
}
