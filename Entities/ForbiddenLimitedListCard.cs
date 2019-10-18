using System;
using System.Collections.Generic;

namespace Yugioh.Engine.Entities
{
    public partial class ForbiddenLimitedListCard
    {
        public long ForbiddenLimitedListCardId { get; set; }
        public long? ForbiddenLimitedListId { get; set; }
        public long? CardId { get; set; }
        public string LimitedStatus { get; set; }

        public ForbiddenLimitedList ForbiddenLimitedList { get; set; }
    }
}
