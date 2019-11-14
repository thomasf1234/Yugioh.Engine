using System;

namespace Yugioh.Engine.Exceptions
{
  public class DuplicateUserException : Exception
  {
    public string Username { get; }
    public DuplicateUserException(string _username)
    {
      this.Username = _username;
    }
  }
}
