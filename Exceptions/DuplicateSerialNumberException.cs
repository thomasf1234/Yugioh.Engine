using System;

namespace Yugioh.Engine.Exceptions
{
  public class DuplicateSerialNumberException : Exception
  {
    public string SerialNumber { get; }
    public DuplicateSerialNumberException(string _serialNumber, string message) : base(message)
    {
      this.SerialNumber = _serialNumber;
    }
  }
}
