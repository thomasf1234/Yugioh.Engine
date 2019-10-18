using System;
using System.Collections.Generic;

namespace Yugioh.Engine.Entities
{
    public partial class UnusableCard
    {
        public long UnusableCardId { get; set; }
        public string DbName { get; set; }
        public string Reason { get; set; }
    }
}
