using Yugioh.Engine.Entities;

namespace Yugioh.Engine.Repositories
{
  public abstract class BaseRepository
  {
    public YugiohContext YugiohContext { get; }
    
    public BaseRepository(YugiohContext _yugiohContext)
    {
      this.YugiohContext = _yugiohContext;
    }
  }
}
