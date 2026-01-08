using System.Numerics;
using FamilytreesLib;
namespace FamilyTreeApp;

using Microsoft.Data.Sqlite;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
class Program //Fast alle made by Mayr
{
    static string connectionString = "Data Source=datenbank.db";
    static void Main(string[] args) //fast alles made by Mayr
    {
        QuestPDF.Settings.License = LicenseType.Community;
        RunDatabase(); //Kumpitsch
        StartProgram();
    }

    static void StartProgram() //made by Mayr
    {
        Console.WriteLine("Willkommen bei der Stammbaum-Software Familie Hinteregger");
        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine("");
        Console.WriteLine("Was möchten sie tun?");
        HauptMenu();
    }

    static void HauptMenu() //made by Mayr
    {

        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine("Drücke '1' um den Stammbaum zu sehen");
        Console.WriteLine("Drücke '2' um den Stammbaum zu bearbeiten");
        Console.WriteLine("Drücke '3' um den Stammbaum als PDF zu bekommen");
        Console.WriteLine("Drücke '4' um den Stammbaum zu verlassen");
        int choicefirst = Convert.ToInt32(Console.ReadLine());
        int choicesecond = CheckWrongChoiceInputForMainMenu(choicefirst, 4, 1);
        if (choicesecond == 1)
        {
            DisplayFamilyTreeInfos(); //bearbeitet
        }
        else if (choicesecond == 2)
        {
            EditFamilyTree();
        }
        else if (choicesecond == 3)
        {
            PrintFamilyTreeAsPdf();
        }
        else if (choicesecond == 4)
        {
            EndProgram();
        }
    }

    static void SeeFamilyTree(FamilyTree familyTree) //made by Mayr
    {
        familyTree.DisplayFamilyTreeInfos();
    }

    static void DisplayFamilyTreeInfos() //made by mayr
    {
        var personen = LadeAllePersonenAusDb()
            .OrderBy(p => p.Birthyear ?? int.MaxValue)
            .ToList();

        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine("Stammbaum aller Personen (alt → neu):");
        Console.WriteLine("-------------------------------------------------");

        foreach (Person p in personen)
        {
            string gender = p.IsMale ? "Männlich" : "Weiblich";
            string married = p.Married ? "Ja" : "Nein";
            string deathYear = p.Deathyear.HasValue ? p.Deathyear.Value.ToString() : "-";

            Console.WriteLine($"Name: {p.Name}");
            Console.WriteLine($"Geburtsjahr: {p.Birthyear}");
            Console.WriteLine($"Sterbejahr: {deathYear}");
            if (married == "Ja")
            {
                Console.WriteLine($"Verheiratet: {married} zu {p.Spouse}");
            }
            else
            {
                Console.WriteLine($"Verheiratet: {married}");
            }
            Console.WriteLine($"Geschlecht: {gender}");
            Console.WriteLine("-------------------------------------------------");
        }

        HauptMenu();
    }

    static int CheckWrongChoiceInputForMainMenu(int choice, int maxChoice, int minChoice) //made by Mayr
    {
        if (choice < minChoice || choice > maxChoice)
        {
            Console.WriteLine($"Auswahl muss zwischen {minChoice} und {maxChoice} sein");
            int correctChoice = Convert.ToInt32(Console.ReadLine());
            while (correctChoice < minChoice || correctChoice > maxChoice)
            {
                CheckWrongChoiceInputForMainMenu(correctChoice, maxChoice, minChoice);
            }
            return correctChoice;
        }
        return choice;
    }


    static void EditFamilyTree() //made by Mayr
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("Was willst du ändern?");
        Console.WriteLine("---------------------------");
        Console.WriteLine("Drücke '1' um eine Person zu löschen");
        Console.WriteLine("Drücke '2' um eine Person hinzuzufügen");
        Console.WriteLine("Drücke '3' um zum Hauptmenu zurückzukehren");
        Console.WriteLine("Drücke '4' um zum Hauptmenu zurückzukehren");
        int choice = Convert.ToInt32(Console.ReadLine());
        int choice2 = CheckWrongChoiceInputForMainMenu(choice, 4, 1);

        if (choice2 == 1)
        {
            DeletePersonFromFamilyTree();
        }
        else if (choice2 == 2)
        {
            MakeNewPerson();
        }
        else if (choice2 == 3)
        {
            EditPerson();
        }
        else if (choice2 == 4)
        {
            HauptMenu();
        }
    }

    static bool CheckStringEmptyOrSpace(string checking)
    {
        if (string.IsNullOrWhiteSpace(checking))
        {
            return false;
        }
        else if (string.IsNullOrEmpty(checking))
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    static void UpdatePersonField(
    SqliteConnection connection,
    string name,
    string field,
    object value)
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText = $@"
        UPDATE Person
        SET {field} = $value
        WHERE Name = $name;
    ";

        cmd.Parameters.AddWithValue("$value", value ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$name", name);

        cmd.ExecuteNonQuery();
    }


    static void EditPerson()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("Person bearbeitem");
        Console.WriteLine("---------------------------");
        Console.WriteLine("Wie heißt die Person die sie bearbeiten wollen?");
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        bool boolean = false;
        bool exists = false;
        int i = 0;
        string nameOfPersonThatIsBeeingChanged = "";
        while (boolean == false || exists == false)
        {
            nameOfPersonThatIsBeeingChanged = Convert.ToString(Console.ReadLine());
            boolean = CheckStringEmptyOrSpace(nameOfPersonThatIsBeeingChanged);
            exists = PersonExistiertInDb(connection, nameOfPersonThatIsBeeingChanged);
            if (boolean == false)
            {
                Console.WriteLine("Zeile darf nicht leer sein!");
            }
            if (exists == false)
            {
                Console.WriteLine("Person existiert nicht");
            }
            i++;
            if (i > 2)
            {
                Console.WriteLine("Tippe 'exit' um ins Hauptmenü zurückzukehren");
            }
            if (i > 2 && nameOfPersonThatIsBeeingChanged.ToLower() == "exit")
            {
                HauptMenu();
                return;
            }

            Console.WriteLine("---------------------------");
            Console.WriteLine($"Was möchtest du bei {nameOfPersonThatIsBeeingChanged} ändern?");
            Console.WriteLine("1 - Name");
            Console.WriteLine("2 - Geburtsjahr");
            Console.WriteLine("3 - Sterbejahr");
            Console.WriteLine("4 - Ehestatus");
            Console.WriteLine("6 - Geschlecht");
            Console.WriteLine("7 - Abbrechen");
            int choice = Convert.ToInt32(Console.ReadLine());
            int choice2 = CheckWrongChoiceInputForMainMenu(choice, 6, 1);

            switch (choice2)
            {
                case 1:
                    Console.WriteLine("Was wollen Sie als neuen Namen?");
                    string newName = Console.ReadLine();
                    UpdatePersonField(connection, nameOfPersonThatIsBeeingChanged, "Name", newName);
                    break;

                case 2:
                    Console.WriteLine("Was wollen Sie als neues Geburtsjahr?");
                    int? newBirthYear = Convert.ToInt32(Console.ReadLine());
                    UpdatePersonField(connection, nameOfPersonThatIsBeeingChanged, "Birthyear", newBirthYear);
                    break;

                case 3:
                    int? newDeath = MakeNewPersonIsAliveMaker(nameOfPersonThatIsBeeingChanged, null);
                    UpdatePersonField(connection, nameOfPersonThatIsBeeingChanged, "Deathyear", newDeath);
                    break;

                case 4:
                    bool married = MakeNewPersonMarriedMaker(nameOfPersonThatIsBeeingChanged);
                    UpdatePersonField(connection, nameOfPersonThatIsBeeingChanged, "IsMarried", married);
                    break;

                case 5:
                    bool isMale = MakeNewPersonIsMaleMaker(nameOfPersonThatIsBeeingChanged);
                    UpdatePersonField(connection, nameOfPersonThatIsBeeingChanged, "IsMale", isMale);
                    break;
                case 6:
                    HauptMenu();
                    return;
            }
        }



    }

    static string MakeNewPersonNameMaker() //made by Mayr
    {
        Console.WriteLine("Geben Sie den Namen der Person ein die sie erstellen wollen (Vorname + Nachname):");
        string name = Convert.ToString(Console.ReadLine());

        for (int i = 0; i < 10; i++)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name kann nicht leer sein");
                name = Convert.ToString(Console.ReadLine());
                i--;
            }
            else if (name.Contains(" "))
            {
                break;
            }
            else
            {
                Console.WriteLine("Zwischen Name und Nachname muss ein Lerrzeichen sein!");
                name = Convert.ToString(Console.ReadLine());
                i--;
            }
        }
        return name;
    }

    static int? MakeNewPersonBirthdateMaker(string name) //made by Mayr
    {
        Console.WriteLine($"Geben Sie bitte das Geburtsjahr von {name} ein:");
        int? birthyear = Convert.ToInt32(Console.ReadLine());
        for (int i = 0; i < 10; i++)
        {
            if (birthyear == null)
            {
                Console.WriteLine("Geburtsjahr kann nicht leer sein");
                birthyear = Convert.ToInt32(Console.ReadLine());
                i--;
            }
            else if (birthyear > Convert.ToInt32(DateTime.Today.Year))
            {
                Console.WriteLine("Geburtsjahr darf nicht höher sein als das aktuelle Jahr!");
                birthyear = Convert.ToInt32(Console.ReadLine());
                i--;
            }
            else if (birthyear < 1800)
            {
                Console.WriteLine("Geburtsjahr muss aktueller sein (oder kontaktieren Sie einen Admin)!");
                birthyear = Convert.ToInt32(Console.ReadLine());
                i--;
            }
            else
            {
                break;
            }
        }

        return birthyear;
    }

    static bool MakeNewPersonMarriedMaker(string name) //made by Mayr
    {
        bool married = false;
        Console.WriteLine($"Ist {name} verheiratet? (Ja oder Nein)");
        string marriedString = Convert.ToString(Console.ReadLine());
        for (int i = 0; i < 10; i++)
        {
            if (string.IsNullOrWhiteSpace(marriedString))
            {
                Console.WriteLine("Ehestatus kann nicht leer sein");
                marriedString = Convert.ToString(Console.ReadLine());
                i--;
            }
            else if (marriedString.ToLower() == "ja")
            {
                married = true;
            }
            else if (marriedString.ToLower() == "nein")
            {
                married = false;
            }
            else if (marriedString.ToLower() != "ja" || marriedString.ToLower() != "nein")
            {
                Console.WriteLine("Ehestatus kann nur ja oder nein sein");
                marriedString = Convert.ToString(Console.ReadLine());
                i--;
            }
            else
            {
                break;
            }
        }
        return married;
    }

    static int? MakeNewPersonIsAliveMaker(string name, int? birthyear) //made by Mayr
    {
        Console.WriteLine($"Ist {name} noch am Leben? (Ja oder Nein)");
        int? deathyear = null;
#pragma warning restore format
        string deathString = Convert.ToString(Console.ReadLine());
        for (int i = 0; i < 10; i++)
        {
            if (string.IsNullOrWhiteSpace(deathString))
            {
                Console.WriteLine("Lebensstatus kann nicht leer sein");
                deathString = Convert.ToString(Console.ReadLine());
                i--;
            }
            else if (deathString.ToLower() == "ja")
            {
                deathyear = null;
            }
            else if (deathString.ToLower() == "nein")
            {
                Console.WriteLine("Wann ist die Person verstorben?");
                deathyear = Convert.ToInt32(Console.ReadLine());
                for (int index = 1; index < 2; index++)
                {
                    if (deathyear < birthyear)
                    {
                        Console.WriteLine("Todesjahr kann nicht älter sein als das Geburtsjahr!");
                        deathString = Convert.ToString(Console.ReadLine());
                        index--;
                    }
                    else if (deathyear > Convert.ToInt32(DateTime.Now.Year))
                    {
                        Console.WriteLine("Todesjahr kann nicht jünger sein als das aktuelle Jahr!");
                        deathString = Convert.ToString(Console.ReadLine());
                        index--;
                    }
                    else if (deathyear == null)
                    {
                        Console.WriteLine("Todesjahr kann nicht leer sein!");
                        deathString = Convert.ToString(Console.ReadLine());
                        index--;
                    }
                    else
                    {
                        break;
                    }
                }
                break;
            }
            else if (deathString.ToLower() != "ja" || deathString.ToLower() != "nein")
            {
                Console.WriteLine("Todesstatus kann nur ja oder nein sein");
                deathString = Convert.ToString(Console.ReadLine());
                i--;
            }
            else
            {
                break;
            }
        }

        return deathyear;
    }

    static bool MakeNewPersonIsMaleMaker(string name) //made by Mayr
    {
        Console.WriteLine($"Ist {name} männlich? (ja oder nein)");
        bool IsAMaleForProgram = true;
        string isMaleString = Convert.ToString(Console.ReadLine());
        while (string.IsNullOrWhiteSpace(isMaleString) || (isMaleString.ToLower() != "ja" && isMaleString.ToLower() != "nein"))
        {
            Console.WriteLine("Geschlecht kann nicht leer sein und kann nur ja oder nein sein!");
            isMaleString = Convert.ToString(Console.ReadLine());
        }

        if (isMaleString.ToLower() == "ja")
        {
            IsAMaleForProgram = true;
        }
        else if (isMaleString.ToLower() == "nein")
        {
            IsAMaleForProgram = false;
        }
        return IsAMaleForProgram;
    }
    static void MakeNewPerson() //made by Mayr
    {

        Console.WriteLine("--------------------------");
        string name = MakeNewPersonNameMaker();
        int? birthyear = MakeNewPersonBirthdateMaker(name);
        bool married = MakeNewPersonMarriedMaker(name);
        int? deathyear = MakeNewPersonIsAliveMaker(name, birthyear);
        bool IsAMaleForProgram = MakeNewPersonIsMaleMaker(name);



        Console.WriteLine("--------------------------");
        Console.WriteLine($"{name} hat folgende Daten:");
        Console.WriteLine($"Geburtsjahr: {birthyear}");

        if (IsAMaleForProgram == true)
        {
            Console.WriteLine($"{name} ist männlich");
        }
        else
        {
            Console.WriteLine($"{name} ist weiblich");
        }
        if (married == true)
        {
            Console.WriteLine($"{name} ist verheiratet");
        }
        else
        {
            Console.WriteLine($"{name} ist nicht verheiratet");
        }
        if (deathyear != null)
        {
            Console.WriteLine($"Sterbejahr ist {deathyear}");
        }
        Console.WriteLine("Sind die eingegebenen Daten korrekt?");
        string mirFälltKeinNameMehrEin = Convert.ToString(Console.ReadLine());
        while (string.IsNullOrWhiteSpace(mirFälltKeinNameMehrEin))
        {
            Console.WriteLine("Eingabe kann nicht null sein");
            mirFälltKeinNameMehrEin = Convert.ToString(Console.ReadLine());
        }
        while (mirFälltKeinNameMehrEin.ToLower() != "ja" && mirFälltKeinNameMehrEin.ToLower() != "nein")
        {
            Console.WriteLine("Eingabe kann nur ja oder nein sein");
            mirFälltKeinNameMehrEin = Convert.ToString(Console.ReadLine());
        }

        while (mirFälltKeinNameMehrEin.ToLower() == "nein")
        {
            Console.WriteLine("Was ist falsch?");
            Console.WriteLine("Gebe '1' ein um den Namen zu ändern");
            Console.WriteLine("Gebe '2' ein um das Geburtsjahr zu ändern");
            Console.WriteLine("Gebe '3' ein um den Ehestatus zu ändern");
            Console.WriteLine("Gebe '4' ein um das Sterbejahr zu ändern oder hinzuzufügen");
            Console.WriteLine("Gebe '5' ein um das Geschlecht zu ändern");
            int choice = Convert.ToInt32(Console.ReadLine());
            int secondChoice = CheckWrongChoiceInputForMainMenu(choice, 5, 1);
            switch (secondChoice)
            {
                case 1:
                    name = MakeNewPersonNameMaker();
                    break;
                case 2:
                    birthyear = MakeNewPersonBirthdateMaker(name);
                    break;
                case 3:
                    married = MakeNewPersonMarriedMaker(name);
                    break;
                case 4:
                    deathyear = MakeNewPersonIsAliveMaker(name, birthyear);
                    break;
                case 5:
                    IsAMaleForProgram = MakeNewPersonIsMaleMaker(name);
                    break;
            }

            Console.WriteLine("Sind die eingegebenen Daten korrekt?");
            mirFälltKeinNameMehrEin = Convert.ToString(Console.ReadLine());
            while (string.IsNullOrWhiteSpace(mirFälltKeinNameMehrEin))
            {
                Console.WriteLine("Eingabe kann nicht null sein");
                mirFälltKeinNameMehrEin = Convert.ToString(Console.ReadLine());
            }
            while (mirFälltKeinNameMehrEin.ToLower() != "ja" && mirFälltKeinNameMehrEin.ToLower() != "nein")
            {
                Console.WriteLine("Eingabe kann nur ja oder nein sein");
                mirFälltKeinNameMehrEin = Convert.ToString(Console.ReadLine());
            }
        }
        Console.WriteLine("--------------------------");


        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        PersonZuDbHinzufügen(connection, name, birthyear, married, deathyear, IsAMaleForProgram);
        connection.Dispose();

        HauptMenu();
    }

    static void PersonZuDbHinzufügen(SqliteConnection connection, string name, int? birthyear, bool married, int? deathyear, bool IsMale) //made by Mayr
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText =
        @"
        INSERT INTO Person (Name, Birthyear, Deathyear, IsMarried, IsMale)
        VALUES ($name, $birthyear, $deathyear, $married, $IsMale);
        ";

        cmd.Parameters.AddWithValue("$name", name);
        cmd.Parameters.AddWithValue(
        "$birthyear",
        birthyear.HasValue ? birthyear.Value : DBNull.Value
        );
        cmd.Parameters.AddWithValue(
        "$deathyear",
        deathyear.HasValue ? deathyear.Value : DBNull.Value
        );
        cmd.Parameters.AddWithValue("$married", married);
        cmd.Parameters.AddWithValue("$IsMale", IsMale);

        cmd.ExecuteNonQuery();
    }

    static void PersonAusDbLöschenNachName(SqliteConnection connection, string name) //made by Mayr
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText =
        @"
        DELETE FROM Person
        WHERE Name = $name;
        ";

        cmd.Parameters.AddWithValue("$name", name);

        int rows = cmd.ExecuteNonQuery();

        if (rows == 0)
            Console.WriteLine("Keine Person mit diesem Namen in der Datenbank gefunden.");
        else
            Console.WriteLine($"{rows} Person(en) aus der Datenbank gelöscht.");
    }

    static bool PersonExistiertInDb(SqliteConnection connection, string name) //made by Mayr
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText =
        @"
    SELECT COUNT(*) 
    FROM Person
    WHERE Name = $name;
    ";

        cmd.Parameters.AddWithValue("$name", name);

        long count = (long)cmd.ExecuteScalar();
        return count > 0;
    }

    static void PersonAusDbLöschenNachName1(SqliteConnection connection, string name) //made by Mayr
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText =
        @"
    DELETE FROM Person
    WHERE Name = $name;
    ";

        cmd.Parameters.AddWithValue("$name", name);

        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine($"{rows} Person(en) aus der Datenbank gelöscht.");
    }
    static void DeletePersonFromFamilyTree() //made by Mayr
    {
        Console.WriteLine("--------------------------");
        Console.WriteLine("Geben Sie den Namen der Person ein die Sie löschen wollen:");
        Console.WriteLine("Abbrechen mit 'nein'");

        string name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Name darf nicht leer sein");
            return;
        }

        if (name.ToLower() == "nein")
        {
            HauptMenu();
            return;
        }

        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        if (!PersonExistiertInDb(connection, name))
        {
            Console.WriteLine("Diese Person existiert nicht in der Datenbank.");
            return;
        }

        Console.WriteLine($"Soll {name} wirklich gelöscht werden? (ja/nein)");
        string confirm = Console.ReadLine()?.ToLower();

        if (confirm == "ja")
        {
            PersonAusDbLöschenNachName1(connection, name);
        }
        else
        {
            Console.WriteLine("Löschen abgebrochen.");
        }

        HauptMenu();
    }
    static void PrintFamilyTreeAsPdf() //made by Mayr
    {
        ExportiereDbAlsPdfQuestPdf();
        HauptMenu();

    }


    static void SetSpouse(string name1, string name2) //made by Mayr
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        int id1 = GetPersonIdByName(connection, name1);
        int id2 = GetPersonIdByName(connection, name2);

        if (id1 == -1 || id2 == -1)
        {
            Console.WriteLine("Eine oder beide Personen existieren nicht.");
            return;
        }

        using var transaction = connection.BeginTransaction();
        try
        {
            var cmd = connection.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = @"
            UPDATE Person SET SpouseId = @id2, IsMarried = 1 WHERE Id = @id1;
            UPDATE Person SET SpouseId = @id1, IsMarried = 1 WHERE Id = @id2;";

            cmd.Parameters.AddWithValue("@id1", id1);
            cmd.Parameters.AddWithValue("@id2", id2);

            cmd.ExecuteNonQuery();
            transaction.Commit();

            Console.WriteLine($"{name1} und {name2} sind jetzt verheiratet.");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine("Fehler beim Heiraten: " + ex.Message);
        }
    }

    static int GetPersonIdByName(SqliteConnection connection, string name) //made by Mayr
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT Id FROM Person WHERE Name = @name";
        cmd.Parameters.AddWithValue("@name", name);

        var result = cmd.ExecuteScalar();

        return result != null ? Convert.ToInt32(result) : -1;
    }
    static List<Person> LadeAllePersonenAusDb() //made by Mayr
    {
        var personen = new List<Person>();

        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
        SELECT Id, Name, Birthyear, Deathyear, IsMarried, IsMale 
        FROM Person
        ORDER BY Birthyear ASC;"; // ASC = aufsteigend (alt → neu)

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            string name = reader.GetString(1);
            int? birthyear = reader.IsDBNull(2) ? null : reader.GetInt32(2);
            int? deathyear = reader.IsDBNull(3) ? null : reader.GetInt32(3);
            bool married = reader.GetBoolean(4);
            bool isMale = reader.GetBoolean(5);
            int id = reader.GetInt32(0);

            var person = new Person(name, birthyear, married, deathyear, isMale, null, id);
            personen.Add(person);
        }

        return personen;
    }

    static string GeneriereHtmlFürPdf(List<Person> personen) //nicht mehr nötig wegen Umstellung auf QuestPDF - made by Mayr
    {
        var html = @"
    <html>
    <head>
        <style>
            body { font-family: Arial; }
            table { width: 100%; border-collapse: collapse; }
            th, td { border: 1px solid black; padding: 5px; text-align: left; }
            th { background-color: #f2f2f2; }
        </style>
    </head>
    <body>
        <h1>Stammbaum aller Personen</h1>
        <table>
            <tr>
                <th>Name</th>
                <th>Geburtsjahr</th>
                <th>Sterbejahr</th>
                <th>Verheiratet</th>
                <th>Geschlecht</th>
            </tr>";

        foreach (var p in personen)
        {
            string gender = p.IsMale ? "Männlich" : "Weiblich";
            string deathYear = p.Deathyear.HasValue ? p.Deathyear.Value.ToString() : "-";
            string married = p.Married ? "Ja" : "Nein";

            html += $@"
            <tr>
                <td>{p.Name}</td>
                <td>{p.Birthyear}</td>
                <td>{deathYear}</td>
                <td>{married}</td>
                <td>{gender}</td>
            </tr>";
        }

        html += @"
        </table>
    </body>
    </html>";

        return html;
    }

    static void ExportiereDbAlsPdfQuestPdf() //made by Mayr mit ein bisschen aushilfe von ChatGPT
    {
        var personen = LadeAllePersonenAusDb();

        var pdfPath = "Stammbaum_Hinteregger.pdf";

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text("Stammbaum aller Personen")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                page.Content()
                    .Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Name").SemiBold();
                            header.Cell().Text("Geburtsjahr").SemiBold();
                            header.Cell().Text("Sterbejahr").SemiBold();
                            header.Cell().Text("Verheiratet").SemiBold();
                            header.Cell().Text("Geschlecht").SemiBold();
                            header.Cell().Text("Eltern").SemiBold();
                        });

                        foreach (var p in personen)
                        {
                            var parents = GetParentsOfChild(p.PersonID);
                            var parentText = parents.Count > 0
                                ? string.Join(" & ", parents)
                                : "-";
                            table.Cell().Text(p.Name);
                            table.Cell().Text(p.Birthyear?.ToString() ?? "-");
                            table.Cell().Text(p.Deathyear?.ToString() ?? "-");
                            table.Cell().Text(p.Married ? "Ja" : "Nein");
                            table.Cell().Text(p.IsMale ? "Männlich" : "Weiblich");
                            table.Cell().Text(parentText);
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(txt =>
                    {
                        txt.Span("Erstellt am ");
                        txt.Span(DateTime.Now.ToString("dd.MM.yyyy"));
                    });
            });
        })
        .GeneratePdf(pdfPath);

        Console.WriteLine($"PDF erfolgreich erstellt: {pdfPath}");
    }

    static void EndProgram() //made by Mayr
    {
        //does nothing
    }

    static bool DatabaseExists() //made by Kumpitsch
    {
        return File.Exists("datenbank.db");
    }

    static void RunDatabase() //made by Kumpitsch
    {

        DatabaseCreator.CreateDatabase();
        DataBaseInserter.InsertToDatabase();
    }

    static void AddParentToChild(SqliteConnection connection, int parentId, int childId)
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
        INSERT INTO ParentChild (ChildId, ParentId)
        VALUES ($childId, $parentId);
    ";

        cmd.Parameters.AddWithValue("$childId", childId);
        cmd.Parameters.AddWithValue("$parentId", parentId);

        cmd.ExecuteNonQuery();
    }

    static List<string> GetParentsOfChild(int childId)
    {
        var parents = new List<string>();

        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
        SELECT p.Name
        FROM ParentChild pc
        JOIN Person p ON p.Id = pc.ParentId
        WHERE pc.ChildId = $childId;
    ";

        cmd.Parameters.AddWithValue("$childId", childId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            parents.Add(reader.GetString(0));
        }

        return parents;
    }


}


public static class DatabaseCreator //made by Kumpitsch
{
    public static void CreateDatabase()
    {
        string connectionString = "Data Source=datenbank.db";
        string sqlFilePath = "sourceDatabank.sql";

        if (!File.Exists(sqlFilePath))
        {
            return;
        }

        string sql = File.ReadAllText(sqlFilePath);

        try
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = sql;
            command.ExecuteNonQuery();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

public static class DataBaseInserter //made by Kumpitsch
{
    public static void InsertToDatabase()
    {
        string connectionString = "Data Source=datenbank.db";
        string sqlFilePath = "insertDatabank.sql";

        string sql = File.ReadAllText(sqlFilePath);

        try
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = sql;
            command.ExecuteNonQuery();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

}
