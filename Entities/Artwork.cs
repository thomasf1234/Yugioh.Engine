using System;
using System.Collections.Generic;

namespace Yugioh.Engine.Entities
{
  public partial class Artwork
  {
    public int ArtworkId { get; set; }
    public string SourceUrl { get; set; }
    public string ImagePath { get; set; }
    public int? CardId { get; set; }
  }
}
