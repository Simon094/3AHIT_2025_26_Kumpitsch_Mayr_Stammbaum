INSERT OR IGNORE INTO Person
(Id, Name, Birthyear, Deathdate, IsMarried, IsMale, FatherId, MotherId)
VALUES
(7, 'Günther Hinteregger', '1970', NULL, True, True, NULL, NULL)

INSERT OR IGNORE INTO FamilyTree
(Name)
VALUES
('Stammbaum der Familie Hinteregger!');