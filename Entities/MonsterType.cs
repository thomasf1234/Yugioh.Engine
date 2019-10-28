using System;
using System.Collections.Generic;

namespace Yugioh.Engine.Entities
{
    public partial class MonsterType
    {
        public long MonsterTypeId { get; set; }
        public string Name { get; set; }
        public long? BaseCardId { get; set; }

        public virtual BaseCard BaseCard { get; set; }
    }
}
