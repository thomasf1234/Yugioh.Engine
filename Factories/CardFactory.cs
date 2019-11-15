
using Microsoft.Extensions.Logging;

using Yugioh.Engine.Entities;
using Yugioh.Engine.Models.Cards;

namespace Yugioh.Engine.Factories
{
  public class CardFactory : ICardFactory
  {
    private readonly ILogger<CardFactory> _logger;
    public CardFactory(ILoggerFactory loggerFactory)
    {
      this._logger = loggerFactory.CreateLogger<CardFactory>();
    }

    public Models.Cards.Card Build(SetCard setCard)
    {
      if (setCard.CardAttribute == "SPELL")
      {
        SpellCard spellCard = new SpellCard();
        spellCard.Number = setCard.Number;
        spellCard.Name = setCard.Name;
        spellCard.CardAttribute = setCard.CardAttribute;
        spellCard.Property = setCard.Property;
        spellCard.Passcode = setCard.Passcode;
        spellCard.Description = setCard.Description;
        
        return spellCard;
      }
      else if (setCard.CardAttribute == "TRAP")
      {
        TrapCard trapCard = new TrapCard();
        trapCard.Number = setCard.Number;
        trapCard.Name = setCard.Name;
        trapCard.CardAttribute = setCard.CardAttribute;
        trapCard.Property = setCard.Property;
        trapCard.Passcode = setCard.Passcode;
        trapCard.Description = setCard.Description;
        
        return trapCard;
      } 
      else 
      {
        MonsterCard monsterCard = new MonsterCard();
        monsterCard.Number = setCard.Number;
        monsterCard.Name = setCard.Name;
        monsterCard.CardAttribute = setCard.CardAttribute;
        monsterCard.Level = setCard.Level;
        monsterCard.Attack = setCard.Attack;
        monsterCard.Defense = setCard.Defense;
        monsterCard.Passcode = setCard.Passcode;
        monsterCard.Description = setCard.Description;
        
        return monsterCard;       
      }
    }
  }
}