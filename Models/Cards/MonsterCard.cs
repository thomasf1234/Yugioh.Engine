
using System.Collections.Generic;

namespace Yugioh.Engine.Models.Cards
{
  public class MonsterCard : Card
  {
    public int? Level { get; set; }
    public int? Rank { get; set; }
    public int? Link { get; set; }
    public int? PendulumScale { get; set; }
    public string PendulumEffect { get; set; }
    public string Attack { get; set; }
    public string Defense { get; set; }
    public IList<string> Types { get; set; }
  }
}
