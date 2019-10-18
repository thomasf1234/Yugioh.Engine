using System;
using System.Collections.Generic;

namespace Yugioh.Engine.Entities
{
    public partial class BoosterPack
    {
        public long BoosterPackId { get; set; }
        public string DbName { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public long? Cost { get; set; }

        public virtual ICollection<BoosterPackCard> BoosterPackCard { get; set; }
    }
}
