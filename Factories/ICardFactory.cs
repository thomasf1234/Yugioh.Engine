using Yugioh.Engine.Entities;
using Yugioh.Engine.Models;

namespace Yugioh.Engine.Factories
{
  public interface ICardFactory
  {
    Models.Cards.Card Build(SetCard setCard);
  }
}
