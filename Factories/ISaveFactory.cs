using Yugioh.Engine.Models;

namespace Yugioh.Engine.Factories
{
  public interface ISaveFactory
  {
    Save Build(Player player);
  }
}
