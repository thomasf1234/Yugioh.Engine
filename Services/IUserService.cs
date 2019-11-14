using System.Collections.Generic;

using Yugioh.Engine.Entities;

namespace Yugioh.Engine.Services
{
  public interface IUserService
  {
    User Get(string username);
    IList<User> All();
    User Create(string username);

    UserDeck CreateDeck(User user, string deckName, IList<UserCard> mainUserCards, IList<UserCard> extraUserCards, IList<UserCard> sideUserCards);
  }
}

// using Yugioh.Engine.Entities;

// namespace Yugioh.Engine.Services
// {
//   public interface IUserService
//   {
//     User Create(string username);
//     User Get(string username);

//     void ReduceDp(User user);

//     void AddCard(User user, BaseCard baseCard, Rarity rarity, Artwork artwork)
//   }
// }

// // Create new User
// // Add cards to User
// // Update 
// // ShopService
// //   Buy - UserRepository.AddCards(User user, UserCards userCards)
// //   PasswordMachine
// // 
// // UserRepository.Save()