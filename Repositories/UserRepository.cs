using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using Yugioh.Engine.Entities;
using Yugioh.Engine.Exceptions;

namespace Yugioh.Engine.Repositories
{
  public class UserRepository : BaseRepository, IUserRepository
  {
    public UserRepository(YugiohContext _yugiohContext) : base(_yugiohContext)
    {
    }

    public User Get(string username)
    {
      var users = this.YugiohContext.User
          .Include(u => u.UserCards).ThenInclude(uc => uc.BaseCard)
          .Include(u => u.UserCards).ThenInclude(uc => uc.Artwork)
          .Include(u => u.UserCards).ThenInclude(uc => uc.Rarity)
          .Include(u => u.UserDecks).ThenInclude(ud => ud.UserDeckCards).ThenInclude(udc => udc.UserCard).ThenInclude(uc => uc.BaseCard)
          .Include(u => u.UserDecks).ThenInclude(ud => ud.UserDeckCards).ThenInclude(udc => udc.UserCard).ThenInclude(uc => uc.Artwork)
          .Include(u => u.UserDecks).ThenInclude(ud => ud.UserDeckCards).ThenInclude(udc => udc.UserCard).ThenInclude(uc => uc.Rarity)
          .Include(u => u.ActiveUserDecks).ThenInclude(aud => aud.UserDeck).ThenInclude(ud => ud.UserDeckCards).ThenInclude(udc => udc.UserCard).ThenInclude(uc => uc.BaseCard)
          .Include(u => u.ActiveUserDecks).ThenInclude(aud => aud.UserDeck).ThenInclude(ud => ud.UserDeckCards).ThenInclude(udc => udc.UserCard).ThenInclude(uc => uc.Artwork)
          .Include(u => u.ActiveUserDecks).ThenInclude(aud => aud.UserDeck).ThenInclude(ud => ud.UserDeckCards).ThenInclude(udc => udc.UserCard).ThenInclude(uc => uc.Rarity)
          .Where(u => u.Username == username);

      int userCount = users.Count();

      if (userCount == 1)
      {
        User user = users.First();
        return user;
      }
      else if (userCount == 0)
      {
        throw new UserNotFoundException(username);
      }
      else
      {
        throw new DuplicateUserException(username);
      }
    }

    public IList<User> All()
    {
      IList<User> users = this.YugiohContext.User
          .Include(u => u.UserCards).ThenInclude(uc => uc.BaseCard)
          .Include(u => u.UserCards).ThenInclude(uc => uc.Artwork)
          .Include(u => u.UserCards).ThenInclude(uc => uc.Rarity)
          .Include(u => u.UserDecks).ThenInclude(ud => ud.UserDeckCards).ThenInclude(udc => udc.UserCard).ThenInclude(uc => uc.BaseCard)
          .Include(u => u.UserDecks).ThenInclude(ud => ud.UserDeckCards).ThenInclude(udc => udc.UserCard).ThenInclude(uc => uc.Artwork)
          .Include(u => u.UserDecks).ThenInclude(ud => ud.UserDeckCards).ThenInclude(udc => udc.UserCard).ThenInclude(uc => uc.Rarity)
          .Include(u => u.ActiveUserDecks).ThenInclude(aud => aud.UserDeck).ThenInclude(ud => ud.UserDeckCards).ThenInclude(udc => udc.UserCard).ThenInclude(uc => uc.BaseCard)
          .Include(u => u.ActiveUserDecks).ThenInclude(aud => aud.UserDeck).ThenInclude(ud => ud.UserDeckCards).ThenInclude(udc => udc.UserCard).ThenInclude(uc => uc.Artwork)
          .Include(u => u.ActiveUserDecks).ThenInclude(aud => aud.UserDeck).ThenInclude(ud => ud.UserDeckCards).ThenInclude(udc => udc.UserCard).ThenInclude(uc => uc.Rarity)
          .ToList();

      return users;
    }

    public User Create(string username)
    {
      User user = new User();
      user.Username = username;
      user.Dp = 0;

      this.YugiohContext.User.Add(user);
      return user;
    }

    public UserDeck CreateDeck(User user, string deckName)
    {
      UserDeck userDeck = new UserDeck();
      userDeck.Name = deckName;

      userDeck.User = user;
      userDeck.UserId = user.UserId;

      user.UserDecks.Add(userDeck);
      this.YugiohContext.UserDeck.Add(userDeck);

      return userDeck;
    }
  }
}
