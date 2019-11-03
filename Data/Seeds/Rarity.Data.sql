BEGIN;
INSERT INTO Rarity(RarityId,DbName,Name,Special,Ratio) VALUES(1,'Common','Common',0,4); -- 1/0.25
INSERT INTO Rarity(RarityId,DbName,Name,Special,Ratio) VALUES(2,'Short_Print','ShortPrint',0,0.0417);
INSERT INTO Rarity(RarityId,DbName,Name,Special,Ratio) VALUES(3,'Super_Short_Print','SuperShortPrint',0,0.0313);
INSERT INTO Rarity(RarityId,DbName,Name,Special,Ratio) VALUES(4,'Rare','Rare',1,1); -- 1/1
INSERT INTO Rarity(RarityId,DbName,Name,Special,Ratio) VALUES(5,'Super_Rare','SuperRare',1,0.1667); -- 1/6
INSERT INTO Rarity(RarityId,DbName,Name,Special,Ratio) VALUES(6,'Ultra_Rare','UltraRare',1,0.0833); -- 1/12
INSERT INTO Rarity(RarityId,DbName,Name,Special,Ratio) VALUES(7,'Ultimate_Rare','UltimateRare',1,0.0417); -- 1/24
INSERT INTO Rarity(RarityId,DbName,Name,Special,Ratio) VALUES(8,'Secret_Rare','SecretRare',1,0.0313); -- 1/32
INSERT INTO Rarity(RarityId,DbName,Name,Special,Ratio) VALUES(9,'Ghost_Rare','GhostRare',1,0.0156); -- 1/64
COMMIT;