﻿using System;
using System.Collections.Generic;

namespace Yugioh.Engine.Entities
{
    public partial class Artwork
    {
        public long ArtworkId { get; set; }
        public string SourceUrl { get; set; }
        public string ImagePath { get; set; }
        public long? CardId { get; set; }

        public Card Card { get; set; }
    }
}
