using System.Numerics;
using FamilytreesLib;
namespace PaymentApp;
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
        if(DatabaseExists() == false) {
        Console.WriteLine("Drücke '5' um den Stammbaum zu erstellen (falls noch nicht vorhanden)");
        }
        int choicefirst = Convert.ToInt32(Console.ReadLine());
        int choicesecond = CheckWrongChoiceInputForMainMenu(choicefirst);
        if(choicesecond == 1)
        {
            DisplayFamilyTreeInfos(Hinteregger);
        } else if(choicesecond == 2)
        {
            EditFamilyTree();
        } else if(choicesecond == 3)
        {
            PrintFamilyTreeAsPdf();
        } else if(choicesecond == 4)
        {
            EndProgram();
        } else if(choicesecond == 5 || DatabaseExists() == false)
        {
            DatabaseCreator.CreateDatabase();
        }
    }

    static FamilyTree MakeCurrentHintereggerFamilyTree()
  {
    Person johann_hinteregger = new Person("Johann Hinteregger", 1880, true, 1951, true, null, null, 1);
    Person anna_hinteregger = new Person("Anna Hinteregger", 1885, true, 1958, false, null, null, 2);
    Person franz_hinteregger = new Person("Franz Hinteregger", 1912, true, 1983, true, johann_hinteregger, anna_hinteregger, 3);
    Person maria_hinteregger = new Person("Maria Hinteregger", 1916, true, 1990, false, null, null, 4);
    Person helene_hinteregger = new Person("Helene Hinteregger", 1947, true, null, false, null, null, 6);
    Person josef_hinteregger = new Person("Josef Hinteregger", 1943, true, 2005, true, franz_hinteregger, maria_hinteregger, 5);
    Person günther_hinteregger = new Person("Günther Hinteregger", 1970, true, null, true, josef_hinteregger, helene_hinteregger, 7);
    Person denise_hinteregger = new Person("Denise Hinteregger", 1990, true, null, false, null, null, 8);
    Person markus_hinteregger = new Person("Markus Hinteregger", 2010, false, null, true, günther_hinteregger, denise_hinteregger, 9);
    Person lisa_hinteregger = new Person("Lisa Hinteregger", 2012, false, null, false, günther_hinteregger, denise_hinteregger, 10);
    Person missgeburt =  new Person("Simon Kumpitsch", 2008, true, null, false, günther_hinteregger, lisa_hinteregger, 99);
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
        foreach(Person p in hinteregger.Personen)
        {
            Console.WriteLine(p.ToString());
        }

        HauptMenu();
    }

    static int CheckWrongChoiceInputForMainMenu(int choice) //Enter Taste tötet alles, Mayr
    {
        
        if(DatabaseExists() == false) {
        if(choice < 1 || choice > 5)
        {
          Console.WriteLine("Auswahl muss zwischen 1 und 5 sein");
          int correctChoice = Convert.ToInt32(Console.ReadLine());
          while(correctChoice < 1 || correctChoice > 5)
            {
                CheckWrongChoiceInputForMainMenu(correctChoice) ;
            }
            return correctChoice;
        }
        return choice;
        } else {
            if(choice < 1 || choice > 4)
            {
                Console.WriteLine("Auswahl muss zwischen 1 und 4 sein");
                int correctChoice = Convert.ToInt32(Console.ReadLine());
                while(correctChoice < 1 || correctChoice > 4)
            {
                CheckWrongChoiceInputForMainMenu(correctChoice) ;
            }
            return correctChoice;
            }
        return choice;
        }
        
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
        int choice2 = CheckWrongChoiceInputForMainMenu(choice);

        if(choice2 == 1)
        {
            DeletePersonFromFamilyTree();
        } else if (choice2 == 2)

        {
            
        } else if (choice2 == 3)
        {
            
        } else if (choice2 == 4)
        {
            
        }
    }


    static void DeletePersonFromFamilyTree()
    {
        Console.WriteLine("--------------------------");
        Console.WriteLine("Geben Sie den Namen der Person ein die sie löschen wollen:");
        Console.WriteLine("Um das löschen abzubrechen gebe 'nein' ein");
        string deletedPerson = Convert.ToString(Console.ReadLine());
        if(string.IsNullOrWhiteSpace(deletedPerson))
        {
            Console.WriteLine("Name kann nicht leer sein");
        } else if (deletedPerson == "nein")
        {
            HauptMenu();
        }

        foreach(Person p in Hinteregger.Personen)
        {
            if(p.getName() == deletedPerson)
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
        
    }

    static void EndProgram()
    {
        //does nothing
    }

    static bool DatabaseExists()
    {
        return File.Exists("datenbank.db");
    }   

}


public static class DatabaseCreator //Kumpitsch
{
    public static void CreateDatabase()
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

public static class AddAlreadyExistingInformation
{
    
}


   


