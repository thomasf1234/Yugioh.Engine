namespace Yugioh.Engine.Models
{
  public class Counter
    {
        public enum Types : int { Spell }
        public Types Type { get; set; }

        public Counter(Types type)
        {
            this.Type = type;
        }
    }
}
