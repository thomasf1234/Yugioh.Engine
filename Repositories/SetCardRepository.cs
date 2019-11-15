using System.Collections.Generic;
using System.Linq;

using Yugioh.Engine.Entities;

namespace Yugioh.Engine.Repositories
{
  public class SetCardRepository : BaseRepository, ISetCardRepository
  {
    public SetCardRepository(YugiohContext _yugiohContext) : base(_yugiohContext)
    {
    }

    public IList<SetCard> All()
    {
      return this.YugiohContext.SetCard.ToList();
    }

    public SetCard Get(string cardNumber)
    {
      return this.YugiohContext.SetCard.Find(cardNumber);
    }

    public IList<SetCard> Get(string[] cardNumbers)
    {
      return this.YugiohContext.SetCard.Where(sc => cardNumbers.Contains(sc.Number)).ToList();
    }
  }
}