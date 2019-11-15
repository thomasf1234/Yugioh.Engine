using System;
using System.Collections.Generic;

namespace Yugioh.Engine.Entities
{
  public partial class CardPrint
  {
    public int CardPrintId { get; set; }
    public int Number { get; set; }
    public int? CardId { get; set; }
    public int? ArtworkId { get; set; }
    public int? RarityId { get; set; }
    public int? ProductId { get; set; }


    public virtual Card Card { get; set; }
    public virtual Artwork Artwork { get; set; }
    public virtual Rarity Rarity { get; set; }
    public virtual Product Product { get; set; }
  }
}
