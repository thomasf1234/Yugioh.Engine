namespace Yugioh.Engine.Entities
{
  public partial class Rarity
    {
        public long RarityId { get; set; }
        public string Name { get; set; }
        public bool Special { get; set; }
        public double Ratio { get; set; }

        public bool IsSpecial()
        {
            return this.Special;
        }
    }
}
