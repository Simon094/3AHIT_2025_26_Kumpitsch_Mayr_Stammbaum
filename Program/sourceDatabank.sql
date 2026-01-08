CREATE TABLE IF NOT EXISTS Person (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Birthyear TEXT NULL,
    Deathyear TEXT NULL,
    IsMarried BOOLEAN NOT NULL,
    IsMale BOOLEAN NOT NULL,
    UNIQUE (Name, IsMarried, IsMale)
);

CREATE TABLE IF NOT EXISTS ParentChild (
    ChildId INTEGER NOT NULL,
    ParentId INTEGER NOT NULL,
    PRIMARY KEY (ChildId, ParentId),
    FOREIGN KEY (ChildId) REFERENCES Person(Id),
    FOREIGN KEY (ParentId) REFERENCES Person(Id)
);

