using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Yugioh.Engine.Entities
{
  public partial class YugiohContext : DbContext
  {
    private readonly ILoggerFactory loggerFactory;

    public YugiohContext(ILoggerFactory _loggerFactory)
    {
      this.loggerFactory = _loggerFactory;
    }

    public YugiohContext(DbContextOptions<YugiohContext> options)  
        : base(options)  
    {  
          
    }  

    public virtual DbSet<Artwork> Artwork { get; set; }
    public virtual DbSet<BaseBoosterPack> BaseBoosterPack { get; set; }
    public virtual DbSet<BaseBoosterPackCard> BaseBoosterPackCard { get; set; }
    public virtual DbSet<BaseCard> BaseCard { get; set; }
    public virtual DbSet<ForbiddenLimitedList> ForbiddenLimitedList { get; set; }
    public virtual DbSet<ForbiddenLimitedListCard> ForbiddenLimitedListCard { get; set; }
    public virtual DbSet<MonsterType> MonsterType { get; set; }
    public virtual DbSet<UnusableCard> UnusableCard { get; set; }
    public virtual DbSet<User> User { get; set; }
    public virtual DbSet<UserCard> UserCard { get; set; }
    public virtual DbSet<UserDeck> UserDeck { get; set; }
    public virtual DbSet<UserDeckCard> UserDeckCard { get; set; }
    public virtual DbSet<ActiveUserDeck> ActiveUserDeck { get; set; }
    public virtual DbSet<Rarity> Rarity { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        optionsBuilder.UseSqlite("DataSource=/Users/tfisher/Documents/workspace/Yugioh.Engine/Data/Yugioh.sqlite3;Read Only=True;");
        optionsBuilder.UseLoggerFactory(this.loggerFactory).EnableSensitiveDataLogging()  ;
      }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Artwork>(entity =>
      {
        entity.Property(e => e.ArtworkId).ValueGeneratedOnAdd();

        entity.Property(e => e.ImagePath).HasColumnType("varchar");

        entity.Property(e => e.SourceUrl).HasColumnType("varchar");
      });

      modelBuilder.Entity<BaseBoosterPack>(entity =>
      {
        entity.HasIndex(e => e.DbName)
                  .HasName("IX_BaseBoosterPackOnDbName")
                  .IsUnique();

        entity.Property(e => e.BaseBoosterPackId).ValueGeneratedOnAdd();

        entity.Property(e => e.DbName).HasColumnType("varchar");

        entity.Property(e => e.ImagePath).HasColumnType("varchar");

        entity.Property(e => e.Name).HasColumnType("varchar");
      });

      modelBuilder.Entity<BaseBoosterPackCard>(entity =>
      {
        entity.Property(e => e.BaseBoosterPackCardId).ValueGeneratedOnAdd();
      });

      modelBuilder.Entity<BaseCard>(entity =>
      {
        entity.HasIndex(e => e.DbName)
                  .HasName("IX_CardOnDbName")
                  .IsUnique();

        entity.HasIndex(e => e.SerialNumber)
                  .HasName("IX_CardOnSerialNumber");

        entity.Property(e => e.BaseCardId).ValueGeneratedNever();

        entity.Property(e => e.Attack).HasColumnType("varchar");

        entity.Property(e => e.CardAttribute).HasColumnType("varchar");

        entity.Property(e => e.CardType).HasColumnType("varchar");

        entity.Property(e => e.Category).HasColumnType("varchar");

        entity.Property(e => e.DbName).HasColumnType("varchar");

        entity.Property(e => e.Defense).HasColumnType("varchar");

        entity.Property(e => e.Description).HasColumnType("varchar");

        entity.Property(e => e.Name).HasColumnType("varchar");

        entity.Property(e => e.Property).HasColumnType("varchar");

        entity.Property(e => e.SerialNumber).HasColumnType("varchar");
      });

      modelBuilder.Entity<ForbiddenLimitedList>(entity =>
      {
        entity.HasIndex(e => e.EffectiveFrom)
                  .HasName("IX_ForbiddenLimitedListOnEffectiveFrom")
                  .IsUnique();

        entity.Property(e => e.ForbiddenLimitedListId).ValueGeneratedOnAdd();

        entity.Property(e => e.EffectiveFrom).HasColumnType("date");

        entity.HasMany(fll => fll.ForbiddenLimitedListCards).WithOne(fllc => fllc.ForbiddenLimitedList);
      });

      modelBuilder.Entity<ForbiddenLimitedListCard>(entity =>
      {
        entity.HasIndex(e => new { e.ForbiddenLimitedListId, e.BaseCardId })
                  .HasName("UIX_ForbiddenLimitedListCardOnFLLIdAndBaseCardId")
                  .IsUnique();

        entity.Property(e => e.ForbiddenLimitedListCardId).ValueGeneratedOnAdd();

        entity.Property(e => e.LimitedStatus).HasColumnType("varchar");
      });

      modelBuilder.Entity<MonsterType>(entity =>
      {
        entity.Property(e => e.MonsterTypeId).ValueGeneratedOnAdd();

        entity.Property(e => e.Name).HasColumnType("varchar");
      });

      modelBuilder.Entity<UnusableCard>(entity =>
      {
        entity.HasIndex(e => e.DbName)
                  .HasName("IX_UnusableCardOnDbName")
                  .IsUnique();

        entity.Property(e => e.UnusableCardId).ValueGeneratedOnAdd();

        entity.Property(e => e.DbName).HasColumnType("varchar");

        entity.Property(e => e.Reason).HasColumnType("varchar");
      });

      modelBuilder.Entity<User>(entity =>
      {
        entity.HasIndex(e => e.Username)
                  .HasName("IX_UserOnUsername")
                  .IsUnique();

        entity.Property(e => e.UserId).ValueGeneratedOnAdd();

        entity.Property(e => e.Dp).HasDefaultValueSql("0");

        entity.Property(e => e.Username).HasColumnType("varchar");
      });

      modelBuilder.Entity<UserCard>(entity =>
      {
        entity.HasIndex(e => new { e.UserId, e.BaseCardId })
                  .HasName("IX_UserCardOnUserIdAndBaseCardId")
                  .IsUnique();

        entity.Property(e => e.UserCardId).ValueGeneratedOnAdd();
      });

      modelBuilder.Entity<UserDeck>(entity =>
      {
        entity.HasIndex(e => e.UserId)
                  .HasName("IX_UserDeckOnUserId");

        entity.Property(e => e.UserDeckId).ValueGeneratedOnAdd();

        entity.Property(e => e.Name).HasColumnType("varchar");
      });

      modelBuilder.Entity<Rarity>(entity =>
      {
        entity.HasIndex(e => e.RarityId)
                  .HasName("IX_RarityOnRarityId");

        entity.Property(e => e.RarityId).ValueGeneratedOnAdd();

        entity.Property(e => e.Name).HasColumnType("varchar");
        entity.Property(e => e.Special).HasColumnType("boolean");
      });
    }
  }
}
