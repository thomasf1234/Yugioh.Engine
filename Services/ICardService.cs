using System.Collections.Generic;

using Yugioh.Engine.Models.Cards;

namespace Yugioh.Engine.Services
{
  public interface ICardService
  {
    Card Get(string cardNumber);
    IList<Card> Get(IList<string> cardNumbers);
  }
}
