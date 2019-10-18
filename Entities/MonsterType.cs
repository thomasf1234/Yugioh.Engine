using System;
using System.Collections.Generic;

namespace Yugioh.Engine.Entities
{
    public partial class MonsterType
    {
        public long MonsterTypeId { get; set; }
        public string Name { get; set; }
        public long? CardId { get; set; }

        public Card Card { get; set; }
    }
}
