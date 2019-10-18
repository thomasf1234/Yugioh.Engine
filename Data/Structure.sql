BEGIN TRANSACTION;

CREATE TABLE IF NOT EXISTS User (
  UserId integer PRIMARY KEY AUTOINCREMENT NOT NULL, 
  Username varchar, 
  EncryptedPassword blob, 
  Dp integer DEFAULT 0
);

CREATE TABLE IF NOT EXISTS UserCard (
  UserCardId integer PRIMARY KEY AUTOINCREMENT NOT NULL, 
  UserId integer,
  CardId integer, 
  ArtworkId integer, 
  Count integer DEFAULT 1
);

CREATE TABLE IF NOT EXISTS UserDeck (
  UserDeckId integer PRIMARY KEY AUTOINCREMENT NOT NULL, 
  UserId integer
);

CREATE TABLE IF NOT EXISTS UserDeckCard (
  UserDeckCardId integer PRIMARY KEY AUTOINCREMENT NOT NULL, 
  UserDeckId integer NOT NULL, 
  SubDeck varchar CHECK(SubDeck IN ('Main', 'Side', 'Extra')),
  CardId integer NOT NULL
);

CREATE TABLE IF NOT EXISTS Card (
  CardId integer PRIMARY KEY AUTOINCREMENT NOT NULL, 
  DbName varchar, 
  CardType varchar, 
  Category varchar, 
  Name varchar, 
  Level integer, 
  Rank integer, 
  PendulumScale integer, 
  CardAttribute varchar, 
  Property varchar, 
  Attack varchar, 
  Defense varchar, 
  SerialNumber varchar, 
  Description varchar
);

CREATE TABLE IF NOT EXISTS UnusableCard (
  UnusableCardId integer PRIMARY KEY AUTOINCREMENT NOT NULL, 
  DbName varchar, 
  Reason varchar
);

CREATE TABLE IF NOT EXISTS MonsterType (
  MonsterTypeId integer PRIMARY KEY AUTOINCREMENT NOT NULL, 
  Name varchar, 
  CardId integer
);

CREATE TABLE IF NOT EXISTS Artwork (
  ArtworkId integer PRIMARY KEY AUTOINCREMENT NOT NULL, 
  SourceUrl varchar, 
  ImagePath varchar, 
  CardId integer
);

CREATE TABLE IF NOT EXISTS BoosterPack (
  BoosterPackId integer PRIMARY KEY AUTOINCREMENT NOT NULL, 
  DbName varchar, 
  Name varchar, 
  ImagePath varchar, 
  Cost integer
);

CREATE TABLE IF NOT EXISTS BoosterPackCard (
  BoosterPackCardId integer PRIMARY KEY AUTOINCREMENT NOT NULL, 
  BoosterPackId integer, 
  CardId integer, 
  Rarity varchar
);

CREATE TABLE IF NOT EXISTS ForbiddenLimitedList (
  ForbiddenLimitedListId integer PRIMARY KEY AUTOINCREMENT NOT NULL, 
  EffectiveFrom date
);
CREATE TABLE IF NOT EXISTS ForbiddenLimitedListCard (
  ForbiddenLimitedListCardId integer PRIMARY KEY AUTOINCREMENT NOT NULL, 
  ForbiddenLimitedListId integer, 
  CardId integer, 
  LimitedStatus varchar
);

CREATE UNIQUE INDEX IX_UserOnUsername ON User (Username);
CREATE UNIQUE INDEX IX_UserCardOnUserIdAndCardId ON UserCard (UserId, CardId);
CREATE INDEX IX_UserDeckOnUserId ON UserDeck (UserId);
CREATE INDEX IX_CardOnSerialNumber ON Card (SerialNumber);
CREATE UNIQUE INDEX IX_CardOnDbName ON Card (DbName);
CREATE UNIQUE INDEX IX_UnusableCardOnDbName ON UnusableCard (DbName);
CREATE UNIQUE INDEX IX_BoosterPackOnDbName ON BoosterPack (DbName);
CREATE UNIQUE INDEX IX_ForbiddenLimitedListOnEffectiveFrom ON ForbiddenLimitedList (EffectiveFrom);
CREATE UNIQUE INDEX UIX_ForbiddenLimitedListCardOnFLLIdAndCardId ON ForbiddenLimitedListCard (ForbiddenLimitedListId, CardId);
COMMIT;