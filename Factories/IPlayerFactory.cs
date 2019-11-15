using Yugioh.Engine.Models;

namespace Yugioh.Engine.Factories
{
  public interface IPlayerFactory
  {
    Player Build(Save save);
  }
}
