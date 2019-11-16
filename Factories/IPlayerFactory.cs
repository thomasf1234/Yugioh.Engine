using Yugioh.Engine.Models;

namespace Yugioh.Engine.Factories
{
  public interface IPlayerFactory
  {
    Player Build(string playerName);
    Player Build(Save save);
  }
}
