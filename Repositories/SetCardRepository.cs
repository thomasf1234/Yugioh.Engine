using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Logging;

using Yugioh.Engine.Entities;

namespace Yugioh.Engine.Repositories
{
  public class SetCardRepository : BaseRepository, ISetCardRepository
  {
    private readonly ILogger<SetCardRepository> _logger;
    public SetCardRepository(ILoggerFactory loggerFactory, YugiohContext _yugiohContext) : base(_yugiohContext)
    {
      this._logger = loggerFactory.CreateLogger<SetCardRepository>();
    }

    public IList<SetCard> All()
    {
      return this.YugiohContext.SetCard.ToList();
    }

    public SetCard Get(string cardNumber)
    {
      return this.YugiohContext.SetCard.Find(cardNumber);
    }

    public IList<SetCard> Get(IList<string> cardNumbers)
    {
      return this.YugiohContext.SetCard.Where(sc => cardNumbers.Contains(sc.Number)).ToList();
    }
  }
}