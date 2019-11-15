
using System.Collections.Generic;

namespace Yugioh.Engine.Models.Cards
{
  public abstract class Card
  {
      public string Number { get; set; }

      public string Name { get; set; }
      public string CardAttribute { get; set; }
      public string Passcode { get; set; }
      public string Description { get; set; }
  }
}
