INSERT OR IGNORE INTO Person
(Id, Name, Birthyear, Deathyear, IsMarried, IsMale)
VALUES
(1, 'Johann Hinteregger', '1880', '1951', True, True);
(2, 'Anna Hinteregger', '1885', '1958', True, False);


INSERT OR IGNORE INTO FamilyTree
(Name)
VALUES
('Stammbaum der Familie Hinteregger!');