using System.Collections.Generic;

using Yugioh.Engine.Models.Cards;

namespace Yugioh.Engine.Models.SubDecks
{
  public abstract class SubDeck
  {
    public IList<Card> Cards { get; set; }
  }
}
