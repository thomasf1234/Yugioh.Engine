using System;

namespace Yugioh.Engine.Exceptions
{
  public class SaveNotFoundException : Exception
  {
    public string Path { get; }
    public SaveNotFoundException(string _path)
    {
      this.Path = _path;
    }
  }
}
