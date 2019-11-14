using Yugioh.Engine.Entities;
using Yugioh.Engine.Models;

namespace Yugioh.Engine.Factories
{
  public interface IBoosterPackFactory
  {
    BoosterPack Build(BaseBoosterPack baseBoosterPack);
  }
}
