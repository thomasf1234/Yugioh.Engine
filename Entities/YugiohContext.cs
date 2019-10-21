using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Yugioh.Engine.Entities
{
  public partial class YugiohContext : DbContext
  {
    public YugiohContext()
    {
    }

    public YugiohContext(DbContextOptions<YugiohContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Artwork> Artwork { get; set; }
    public virtual DbSet<BoosterPack> BoosterPack { get; set; }
    public virtual DbSet<BoosterPackCard> BoosterPackCard { get; set; }
    public virtual DbSet<Card> Card { get; set; }
    public virtual DbSet<ForbiddenLimitedList> ForbiddenLimitedList { get; set; }
    public virtual DbSet<ForbiddenLimitedListCard> ForbiddenLimitedListCard { get; set; }
    public virtual DbSet<MonsterType> MonsterType { get; set; }
    public virtual DbSet<UnusableCard> UnusableCard { get; set; }
    public virtual DbSet<User> User { get; set; }
    public virtual DbSet<UserCard> UserCard { get; set; }
    public virtual DbSet<UserDeck> UserDeck { get; set; }
    public virtual DbSet<UserDeckCard> UserDeckCard { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        optionsBuilder.UseSqlite("DataSource=/Users/tfisher/Documents/workspace/Yugioh.Engine/Data/Yugioh.sqlite3");
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

      modelBuilder.Entity<BoosterPack>(entity =>
      {
        entity.HasIndex(e => e.DbName)
                  .HasName("IX_BoosterPackOnDbName")
                  .IsUnique();

        entity.Property(e => e.BoosterPackId).ValueGeneratedOnAdd();

        entity.Property(e => e.DbName).HasColumnType("varchar");

        entity.Property(e => e.ImagePath).HasColumnType("varchar");

        entity.Property(e => e.Name).HasColumnType("varchar");
      });

      modelBuilder.Entity<BoosterPackCard>(entity =>
      {
        entity.Property(e => e.BoosterPackCardId).ValueGeneratedOnAdd();

        entity.Property(e => e.Rarity).HasColumnType("varchar");
      });

      modelBuilder.Entity<Card>(entity =>
      {
        entity.HasIndex(e => e.DbName)
                  .HasName("IX_CardOnDbName")
                  .IsUnique();

        entity.HasIndex(e => e.SerialNumber)
                  .HasName("IX_CardOnSerialNumber");

        entity.Property(e => e.CardId).ValueGeneratedNever();

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
        entity.HasIndex(e => new { e.ForbiddenLimitedListId, e.CardId })
                  .HasName("UIX_ForbiddenLimitedListCardOnFLLIdAndCardId")
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
        entity.HasIndex(e => new { e.UserId, e.CardId })
                  .HasName("IX_UserCardOnUserIdAndCardId")
                  .IsUnique();

        entity.Property(e => e.UserCardId).ValueGeneratedOnAdd();
      });

      modelBuilder.Entity<UserDeck>(entity =>
      {
        entity.HasIndex(e => e.UserId)
                  .HasName("IX_UserDeckOnUserId");

        entity.Property(e => e.UserDeckId).ValueGeneratedOnAdd();
      });

      modelBuilder.Entity<UserDeckCard>(entity =>
      {
        entity.HasIndex(e => e.UserDeckId)
                  .HasName("IX_UserDeckCardOnUserDeckId");

        entity.Property(e => e.UserDeckCardId).ValueGeneratedOnAdd();
      });
    }
  }
}
