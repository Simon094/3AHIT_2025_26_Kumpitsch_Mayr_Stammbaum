namespace FamilytreesLib;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System.Linq;

public class FamilyTree
{
    /// <summary>
    /// "_person" is for saving a list of the persons on the familytree
    /// </summary>
    private List<Person> _personen = new List<Person>();
    /// <summary>
    /// "_name" is for saving the name as a string
    /// </summary>
    private string _name;

    /// <summary>
    /// Constructor for making the FamilyTree object
    /// </summary>
    /// <param name="name"></param> parameter "name" is for giving the name as a string
    public FamilyTree(string name)
    {
        _name = name;
    }

    /// <summary>
    /// Propertie "Personen" is for getting the list of persons in the familytree sorted by age
    /// </summary>
    public List<Person> Personen
    {
    get => _personen.OrderBy(p => p.Age()).ToList();
    }
    /// <summary>
    /// function "getName" is for giving out the name of a familytree as string
    /// </summary>
    /// <returns></returns> returns the name of the familytree as string
    public string getName()
    {
        return _name;
    }
    /// <summary>
    /// function "AddPerson" is for adding a person to the familytree (List)
    /// </summary>
    /// <param name="person"></param> parameter "person" is for giving the person as a person-object to add to the familytree
    public void AddPerson(Person person)
    {
        _personen.Add(person);
    }
    
    /// <summary>
    /// function "RemovePerson" is for removing a person from the familytree (funktioniert nicht)
    /// </summary>
    /// <param name="person"></param> parameter "person" is for giving the person as a person-object to remove from the familytree
    public void RemovePerson(Person person) {
       _personen.Remove(person);
    }

    /// <summary>
    /// function "DisplayFamilyTreeInfos" is for showing the infos for all persons in the familytree
    /// </summary>
   public void DisplayFamilyTreeInfos() {
    foreach(Person p in _personen) {
        Console.WriteLine(p.ToString());
    }
   } 

    /// <summary>
    /// Function "MakeNewId" is for the program to make a new id of + 1 to the person that is goint to be added
    /// </summary>
    /// <returns></returns> return the new id that a newly created person is going to get
   public int MakeNewId()
    {
        List<int> ids = new List<int>();
        foreach(Person i in _personen)
        {
          ids.Add(i.PersonID); 
        }

        int newId = ids.Last() + 1;
        return newId;
    }
   }
            
        