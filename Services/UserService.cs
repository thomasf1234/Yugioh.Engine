using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

using Yugioh.Engine.Constants;
using Yugioh.Engine.Entities;
using Yugioh.Engine.Exceptions;
using Yugioh.Engine.Repositories;

namespace Yugioh.Engine.Services
{
  public class UserService : IUserService
  {
    private readonly ILogger<UserService> _logger;
    private readonly IUserRepository _userRepository;
    public UserService(ILoggerFactory loggerFactory, IUserRepository userRepository)
    {
      this._logger = loggerFactory.CreateLogger<UserService>();
      this._userRepository = userRepository;
    }
    public IList<User> All()
    {
      return this._userRepository.All();
    }

    public User Create(string username)
    {
      return this._userRepository.Create(username);
    }

    public UserDeck CreateDeck(User user, string deckName, IList<UserCard> mainUserCards, IList<UserCard> extraUserCards, IList<UserCard> sideUserCards)
    {
      if (mainUserCards.Count >= 40 && mainUserCards.Count <= 60)
      {
        if (extraUserCards.Count <= 15)
        {
          if (sideUserCards.Count <= 15)
          {
            UserDeck userDeck = this._userRepository.CreateDeck(user, deckName);

            foreach (UserCard userCard in mainUserCards)
            {
              UserDeckCard userDeckCard = new UserDeckCard();
              userDeckCard.UserCard = userCard;
              userDeckCard.UserCardId = userCard.UserCardId;
              userDeckCard.UserDeck = userDeck;
              userDeckCard.UserDeckId = userDeck.UserDeckId;
              userDeckCard.SubDeck = SubDecks.Main;
              userDeck.UserDeckCards.Add(userDeckCard);
              this._logger.LogDebug($"Adding Card '{userCard.BaseCard.Name}' to {userDeckCard.SubDeck} Deck '{userDeck.Name}' for '{user.Username}'");
            }

            foreach (UserCard userCard in sideUserCards)
            {
              UserDeckCard userDeckCard = new UserDeckCard();
              userDeckCard.UserCard = userCard;
              userDeckCard.UserCardId = userCard.UserCardId;
              userDeckCard.UserDeck = userDeck;
              userDeckCard.UserDeckId = userDeck.UserDeckId;
              userDeckCard.SubDeck = SubDecks.Side;
              userDeck.UserDeckCards.Add(userDeckCard);
              this._logger.LogDebug($"Adding Card '{userCard.BaseCard.Name}' to {userDeckCard.SubDeck} Deck '{userDeck.Name}' for '{user.Username}'");
            }

            foreach (UserCard userCard in extraUserCards)
            {
              UserDeckCard userDeckCard = new UserDeckCard();
              userDeckCard.UserCard = userCard;
              userDeckCard.UserCardId = userCard.UserCardId;
              userDeckCard.UserDeck = userDeck;
              userDeckCard.UserDeckId = userDeck.UserDeckId;
              userDeckCard.SubDeck = SubDecks.Extra;
              userDeck.UserDeckCards.Add(userDeckCard);
              this._logger.LogDebug($"Adding Card '{userCard.BaseCard.Name}' to {userDeckCard.SubDeck} Deck '{userDeck.Name}' for '{user.Username}'");
            }

            return userDeck;
          }
          else
          {
            throw new IllegalDeckException($"Outside of SideDeck card count limit. There are {sideUserCards.Count} cards when the rules specify no more than 15");
          }
        }
        else
        {
          throw new IllegalDeckException($"Outside of ExtraDeck card count limit. There are {extraUserCards.Count} cards when the rules specify no more than 15");
        }
      }
      else
      {
        throw new IllegalDeckException($"Outside of MainDeck card count limit. There are {mainUserCards.Count} cards when the rules specify an inclusive range with a lower limit 40, and upper limit of 60");
      }
    }

    public User Get(string username)
    {
      return this._userRepository.Get(username);
    }

    public void SetActiveDeck(User user, UserDeck userDeck)
    {
      this._userRepository.CreateActiveDeck(user, userDeck);
    }
  }
}
