using Yugioh.Engine.Entities;
using Yugioh.Engine.Models;

namespace Yugioh.Engine.Services
{
  public interface ISaveService
  {
    void Save(Player player);
    Player Load(string playerName);
  }
}
