using System.Numerics;
using FamilytreesLib;
namespace FamilyTreeApp;

using Microsoft.Data.Sqlite;
class Program
{
    static void Main(string[] args) //Mayr
    {
        StartProgram(); //Mayr
    }

    static void StartProgram() //Mayr
    {
        Console.WriteLine("Willkommen bei der Stammbaum-Software Familie Hinteregger");
        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine("");
        Console.WriteLine("Was möchten sie tun?");
        HauptMenu();
    }
    static FamilyTree Hinteregger = MakeCurrentHintereggerFamilyTree();
    static void HauptMenu() //Mayr
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
            DisplayFamilyTreeInfos(Hinteregger);
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

    static FamilyTree MakeCurrentHintereggerFamilyTree()
    {
        Person johann_hinteregger = new Person("Johann Hinteregger", 1880, true, 1951, true, 1);
        Person anna_hinteregger = new Person("Anna Hinteregger", 1885, true, 1958, false, 2);
        Person franz_hinteregger = new Person("Franz Hinteregger", 1912, true, 1983, true, 3);
        Person maria_hinteregger = new Person("Maria Hinteregger", 1916, true, 1990, false, 4);
        Person helene_hinteregger = new Person("Helene Hinteregger", 1947, true, null, false, 6);
        Person josef_hinteregger = new Person("Josef Hinteregger", 1943, true, 2005, true, 5);
        Person günther_hinteregger = new Person("Günther Hinteregger", 1970, true, null, true, 7);
        Person denise_hinteregger = new Person("Denise Hinteregger", 1990, true, null, false, 8);
        Person markus_hinteregger = new Person("Markus Hinteregger", 2010, false, null, true, 9);
        Person lisa_hinteregger = new Person("Lisa Hinteregger", 2012, false, null, false, 10);
        Person missgeburt = new Person("Simon Kumpitsch", 2008, true, null, false, 99);
        FamilyTree Hinteregger = new FamilyTree("Hinteregger");
        Hinteregger.AddPerson(johann_hinteregger);
        Hinteregger.AddPerson(anna_hinteregger);
        Hinteregger.AddPerson(franz_hinteregger);
        Hinteregger.AddPerson(maria_hinteregger);
        Hinteregger.AddPerson(helene_hinteregger);
        Hinteregger.AddPerson(josef_hinteregger);
        Hinteregger.AddPerson(günther_hinteregger);
        Hinteregger.AddPerson(denise_hinteregger);
        Hinteregger.AddPerson(lisa_hinteregger);
        Hinteregger.AddPerson(markus_hinteregger);
        Hinteregger.AddPerson(missgeburt);
        return Hinteregger;
    }


    static void SeeFamilyTree(FamilyTree familyTree) //Mayr
    {
        familyTree.DisplayFamilyTreeInfos();
    }

    static void DisplayFamilyTreeInfos(FamilyTree hinteregger)
    {
        foreach (Person p in hinteregger.Personen)
        {
            Console.WriteLine(p.ToString());
        }

        HauptMenu();
    }

    static int CheckWrongChoiceInputForMainMenu(int choice, int maxChoice, int minChoice) //Enter Taste tötet alles, Mayr
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


    static void EditFamilyTree() //Mayr
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("Was willst du ändern?");
        Console.WriteLine("---------------------------");
        Console.WriteLine("Drücke '1' um eine Person zu löschen");
        Console.WriteLine("Drücke '2' um eine Person hinzuzufügen");
        Console.WriteLine("Drücke '3' um zum Hauptmenu zurückzukehren");
        Console.WriteLine("Drücke '4' um das Program zu beenden");
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

        }
        else if (choice2 == 4)
        {

        }
    }

    static string MakeNewPersonNameMaker()
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

    static int? MakeNewPersonBirthdateMaker(string name)
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

    static bool MakeNewPersonMarriedMaker(string name)
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

    static int? MakeNewPersonIsAliveMaker(string name, int? birthyear)
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

    static bool MakeNewPersonIsMaleMaker(string name)
    {
        Console.WriteLine($"War {name} männlich? (ja oder nein)");
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
    static void MakeNewPerson()
    {
        Console.WriteLine("--------------------------");
        string name = MakeNewPersonNameMaker();
        int? birthyear = MakeNewPersonBirthdateMaker(name);
        bool married = MakeNewPersonMarriedMaker(name);
        int? deathyear = MakeNewPersonIsAliveMaker(name, birthyear);
        bool IsAMaleForProgram = MakeNewPersonIsMaleMaker(name);
        int newId = Hinteregger.MakeNewId();



        Console.WriteLine("--------------------------");
        Console.WriteLine($"{name} hat folgende Daten:");
        Console.WriteLine($"Geburtsjahr: {birthyear}");
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
        Console.WriteLine($"{name} hat die Id: {newId}");
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

        Person newPerson = new Person(name, birthyear, married, deathyear, IsAMaleForProgram, newId);
        Hinteregger.AddPerson(newPerson);

        HauptMenu();
    }
    static void DeletePersonFromFamilyTree()
    {
        Console.WriteLine("--------------------------");
        Console.WriteLine("Geben Sie den Namen der Person ein die sie löschen wollen:");
        Console.WriteLine("Um das löschen abzubrechen gebe 'nein' ein");
        string deletedPerson = Convert.ToString(Console.ReadLine());
        if (string.IsNullOrWhiteSpace(deletedPerson))
        {
            Console.WriteLine("Name kann nicht leer sein");
        }
        else if (deletedPerson == "nein")
        {
            HauptMenu();
        }

        foreach (Person p in Hinteregger.Personen)
        {
            if (p.getName() == deletedPerson)
            {
                Hinteregger.RemovePerson(p);
                Console.WriteLine($"{p.getName()} wurde aus dem Stammbaum gelöscht!");
                Console.WriteLine($"-----------------------------------");
                HauptMenu();
            }
        }

        Console.WriteLine($"{deletedPerson} wurde nicht gefunden");
        EditFamilyTree();
    }
    static void PrintFamilyTreeAsPdf() //Mayr
    {
        string one = "one";
        string two = "two";
        string test = one + two;
        Console.WriteLine(test);
    }

    static void EndProgram()
    {
        //does nothing
    }

    static bool DatabaseExists()
    {
        return File.Exists("datenbank.db");
    }

    static void RunDatabase() // Ins Programm einbauen Herr Mayr
    {
        DatabaseCreator.CreateDatabase();
        DataBaseInserter.InsertToDatabase();
    }
}


public static class DatabaseCreator //Kumpitsch
{
    public  static void CreateDatabase()
    {
        string connectionString = "Data Source=datenbank.db";
        string sqlFilePath = "sourceDatabank.sql";

        if (!File.Exists(sqlFilePath))
        {
            Console.WriteLine($"Vaterl Error: {sqlFilePath}");
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

            Console.WriteLine("Datenbank erfolgreich erstellt!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fehler beim Erstellen der Datenbank:");
            Console.WriteLine(ex.Message);
        }
    }
}

public static class DataBaseInserter //Kumpitsch
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

            Console.WriteLine("Daten wurden erfolgreich hinzugefügt");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fehler beim hinzufügen der Daten:");
            Console.WriteLine(ex.Message);
        }

    }
}
