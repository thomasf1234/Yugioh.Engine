using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Yugioh.Engine.Entities;
using Yugioh.Engine.Factories;
using Yugioh.Engine.Models;
using Yugioh.Engine.Models.Cards;
using Yugioh.Engine.Repositories;

namespace Yugioh.Engine.Services
{
  public class CardService : ICardService
  {
    private readonly ILogger<CardService> _logger;
    private readonly ISetCardRepository _setCardRepository;
    private readonly ICardFactory _cardFactory;
    public CardService(ILoggerFactory loggerFactory, ISetCardRepository setCardRepository, ICardFactory cardFactory)
    {
      this._logger = loggerFactory.CreateLogger<CardService>();
      this._setCardRepository = setCardRepository;
      this._cardFactory = cardFactory;
    }

    public Models.Cards.Card Get(string cardNumber)
    {
      SetCard setCard = this._setCardRepository.Get(cardNumber);
      Models.Cards.Card card = this._cardFactory.Build(setCard);
      
      return card;
    }

    public IList<Models.Cards.Card> Get(string[] cardNumbers)
    {
      IList<Models.Cards.Card> cards = new List<Models.Cards.Card>();
      IList<SetCard> setCards = this._setCardRepository.Get(cardNumbers);

      foreach (SetCard setCard in setCards)
      {
        Models.Cards.Card card = this._cardFactory.Build(setCard);
        cards.Add(card);
      }
      
      return cards;
    }
  }
}
