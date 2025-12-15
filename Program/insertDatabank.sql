INSERT OR IGNORE INTO Person
(Id, Name, Birthyear, Deathyear, IsMarried, IsMale, FatherId, MotherId)
VALUES
(1, 'Johann Hinteregger', '1880', '1951', True, True, NULL, NULL)
()

INSERT OR IGNORE INTO FamilyTree
(Name)
VALUES
('Stammbaum der Familie Hinteregger!');