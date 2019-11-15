using System;
using System.Collections.Generic;

using Yugioh.Engine.Entities;

using Yugioh.Engine.Models.SubDecks;

namespace Yugioh.Engine.Models
{
    public class Deck
    {
        public string Name { get; set; }
        public MainSubDeck Main { get; set; }
        public SideSubDeck Side { get; set; }
        public ExtraSubDeck Extra { get; set; }
    }
}
