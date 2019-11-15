using System.Collections.Generic;

using Yugioh.Engine.Entities;

namespace Yugioh.Engine.Repositories
{
  public interface ISetCardRepository
  {
    SetCard Get(string cardNumber);
    IList<SetCard> Get(string[] cardNumbers);
    IList<SetCard> All();
  }
}