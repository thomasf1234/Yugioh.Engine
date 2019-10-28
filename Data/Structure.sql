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
  BaseCardId integer, 
  ArtworkId integer,
  RarityId integer
);

CREATE TABLE IF NOT EXISTS UserDeck (
  UserDeckId integer PRIMARY KEY AUTOINCREMENT NOT NULL, 
  UserId integer
);

CREATE TABLE IF NOT EXISTS UserDeckCard (
  UserDeckCardId integer PRIMARY KEY AUTOINCREMENT NOT NULL, 
  UserDeckId integer NOT NULL, 
  SubDeck varchar CHECK(SubDeck IN ('Main', 'Side', 'Extra')),
  UserCardId integer NOT NULL
);

CREATE TABLE IF NOT EXISTS BaseCard (
  BaseCardId integer PRIMARY KEY AUTOINCREMENT NOT NULL, 
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
  BaseCardId integer
);

CREATE TABLE IF NOT EXISTS Artwork (
  ArtworkId integer PRIMARY KEY AUTOINCREMENT NOT NULL, 
  SourceUrl varchar, 
  ImagePath varchar, 
  BaseCardId integer
);

CREATE TABLE IF NOT EXISTS BaseBoosterPack (
  BaseBoosterPackId integer PRIMARY KEY AUTOINCREMENT NOT NULL, 
  DbName varchar, 
  Name varchar, 
  ImagePath varchar, 
  Cost integer
);

CREATE TABLE IF NOT EXISTS BaseBoosterPackCard (
  BaseBoosterPackCardId integer PRIMARY KEY AUTOINCREMENT NOT NULL, 
  BaseBoosterPackId integer, 
  BaseCardId integer, 
  RarityId integer
);

CREATE TABLE IF NOT EXISTS Rarity (
  RarityId integer PRIMARY KEY AUTOINCREMENT NOT NULL, 
  Name integer,
  Special boolean, 
  Ratio float
);

CREATE TABLE IF NOT EXISTS ForbiddenLimitedList (
  ForbiddenLimitedListId integer PRIMARY KEY AUTOINCREMENT NOT NULL, 
  EffectiveFrom date
);
CREATE TABLE IF NOT EXISTS ForbiddenLimitedListCard (
  ForbiddenLimitedListCardId integer PRIMARY KEY AUTOINCREMENT NOT NULL, 
  ForbiddenLimitedListId integer, 
  BaseCardId integer, 
  LimitedStatus varchar
);

CREATE UNIQUE INDEX IX_UserOnUsername ON User (Username);
CREATE INDEX IX_UserDeckOnUserId ON UserDeck (UserId);
CREATE INDEX IX_BaseCardOnSerialNumber ON BaseCard (SerialNumber);
CREATE UNIQUE INDEX IX_BaseCardOnDbName ON BaseCard (DbName);
CREATE UNIQUE INDEX IX_UnusableCardOnDbName ON UnusableCard (DbName);
CREATE UNIQUE INDEX IX_BaseBoosterPackOnDbName ON BaseBoosterPack (DbName);
CREATE UNIQUE INDEX IX_ForbiddenLimitedListOnEffectiveFrom ON ForbiddenLimitedList (EffectiveFrom);
CREATE UNIQUE INDEX UIX_ForbiddenLimitedListCardOnFLLIdAndBaseCardId ON ForbiddenLimitedListCard (ForbiddenLimitedListId, BaseCardId);
COMMIT;