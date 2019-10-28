using System;
using System.Collections.Generic;

namespace Yugioh.Engine.Entities
{
    public partial class BaseBoosterPack
    {
        public long BaseBoosterPackId { get; set; }
        public string DbName { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public long? Cost { get; set; }

        public virtual ICollection<BaseBoosterPackCard> BaseBoosterPackCards { get; set; }
    }
}
