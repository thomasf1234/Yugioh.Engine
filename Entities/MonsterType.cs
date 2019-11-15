using System;
using System.Collections.Generic;

namespace Yugioh.Engine.Entities
{
  public partial class MonsterType
  {
    public int MonsterTypeId { get; set; }
    public string Name { get; set; }
    public int? CardId { get; set; }

    public virtual Card Card { get; set; }
  }
}
