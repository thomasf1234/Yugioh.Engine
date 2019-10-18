using System;
using System.Collections.Generic;

namespace Yugioh.Engine.Entities
{
    public partial class ForbiddenLimitedList
    {
        public long ForbiddenLimitedListId { get; set; }
        public string EffectiveFrom { get; set; }

        public virtual ICollection<ForbiddenLimitedListCard> ForbiddenLimitedListCards { get; set; }
    }
}
