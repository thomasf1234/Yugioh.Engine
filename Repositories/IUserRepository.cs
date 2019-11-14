using System.Collections.Generic;

using Yugioh.Engine.Entities;

namespace Yugioh.Engine.Repositories
{
  public interface IUserRepository
  {
    User Get(string username);
    IList<User> All();
    User Create(string username);
    UserDeck CreateDeck(User user, string deckName);
  }
}
