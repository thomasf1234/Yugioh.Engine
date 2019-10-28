BEGIN;
INSERT INTO Rarity(RarityId,Name,Special,Ratio) VALUES(1,'Common',0,4); -- 1/0.25
INSERT INTO Rarity(RarityId,Name,Special,Ratio) VALUES(2,'ShortPrint',0,0.0417);
INSERT INTO Rarity(RarityId,Name,Special,Ratio) VALUES(3,'SuperShortPrint',0,0.0313);
INSERT INTO Rarity(RarityId,Name,Special,Ratio) VALUES(4,'Rare',1,1); -- 1/1
INSERT INTO Rarity(RarityId,Name,Special,Ratio) VALUES(5,'SuperRare',1,0.1667); -- 1/6
INSERT INTO Rarity(RarityId,Name,Special,Ratio) VALUES(6,'UltraRare',1,0.0833); -- 1/12
INSERT INTO Rarity(RarityId,Name,Special,Ratio) VALUES(7,'UltimateRare',1,0.0417); -- 1/24
INSERT INTO Rarity(RarityId,Name,Special,Ratio) VALUES(8,'SecretRare',1,0.0313); -- 1/32
INSERT INTO Rarity(RarityId,Name,Special,Ratio) VALUES(9,'GhostRare',1,0.0156); -- 1/64
COMMIT;