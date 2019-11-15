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
    public virtual DbSet<Card> Card { get; set; }
    public virtual DbSet<CardPrint> CardPrint { get; set; }
    public virtual DbSet<MonsterType> MonsterType { get; set; }
    public virtual DbSet<Product> Product { get; set; }
    public virtual DbSet<Rarity> Rarity { get; set; }
    public virtual DbSet<SetCard> SetCard { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        // optionsBuilder.UseSqlite("DataSource=/Users/tfisher/Documents/workspace/Yugioh.Engine/Data/Yugioh.sqlite3;Read Only=True;");
                optionsBuilder.UseSqlite("DataSource=/Users/tfisher/Documents/workspace/Yugioh.Engine/Data/Yugioh.sqlite3");

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

      modelBuilder.Entity<Card>(entity =>
      {
        entity.HasIndex(e => e.CardId)
                  .HasName("UIX_CardOnCardId")
                  .IsUnique();

        entity.Property(e => e.Attack).HasColumnType("varchar");

        entity.Property(e => e.CardAttribute).HasColumnType("varchar");

        entity.Property(e => e.PendulumEffect).HasColumnType("varchar");

        entity.Property(e => e.Defense).HasColumnType("varchar");

        entity.Property(e => e.Description).HasColumnType("varchar");

        entity.Property(e => e.Name).HasColumnType("varchar");

        entity.Property(e => e.Property).HasColumnType("varchar");

        entity.Property(e => e.Passcode).HasColumnType("varchar");
      });

      modelBuilder.Entity<CardPrint>(entity =>
      {
        entity.HasIndex(e => e.Number)
                  .HasName("UIX_CardPrintOnNumber")
                  .IsUnique();

        entity.Property(e => e.Number).HasColumnType("varchar");
      });

      modelBuilder.Entity<MonsterType>(entity =>
      {
        entity.Property(e => e.MonsterTypeId).ValueGeneratedOnAdd();

        entity.Property(e => e.Name).HasColumnType("varchar");
      });

      modelBuilder.Entity<Product>(entity =>
      {
        entity.HasIndex(e => e.Name)
                  .HasName("UIX_ProductOnName")
                  .IsUnique();           

        entity.Property(e => e.ProductId).ValueGeneratedOnAdd();

        entity.Property(e => e.Name).HasColumnType("varchar");
      });

      modelBuilder.Entity<Rarity>(entity =>
      {
        entity.HasIndex(e => e.Name)
                  .HasName("UIX_RarityOnName")
                  .IsUnique();           

        entity.Property(e => e.RarityId).ValueGeneratedOnAdd();

        entity.Property(e => e.Name).HasColumnType("varchar");
      });

      // View
      modelBuilder.Entity<SetCard>(entity =>
      {
        entity.Property(e => e.Number).HasColumnType("varchar");
    
        entity.Property(e => e.Attack).HasColumnType("varchar");

        entity.Property(e => e.CardAttribute).HasColumnType("varchar");

        entity.Property(e => e.PendulumEffect).HasColumnType("varchar");

        entity.Property(e => e.Defense).HasColumnType("varchar");

        entity.Property(e => e.Description).HasColumnType("varchar");

        entity.Property(e => e.Name).HasColumnType("varchar");

        entity.Property(e => e.Property).HasColumnType("varchar");

        entity.Property(e => e.Passcode).HasColumnType("varchar");

        entity.Property(e => e.RarityName).HasColumnType("varchar");
      });
    }
  }
}
