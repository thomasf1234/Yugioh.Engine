using System.Collections.Generic;

using Yugioh.Engine.Entities;

namespace Yugioh.Engine.Repositories
{
  public interface ISetCardRepository
  {
    SetCard Get(string cardNumber);
    IList<SetCard> Get(IList<string> cardNumbers);
    IList<SetCard> All();
  }
}