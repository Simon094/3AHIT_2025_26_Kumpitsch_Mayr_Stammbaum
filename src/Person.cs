using System.Diagnostics.Contracts;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using Microsoft.VisualBasic;
using Microsoft.Win32;
namespace FamilytreesLib;

public class Person
{
    /// <summary>
    /// "_name" is for saving the name as a string
    /// </summary>/
    private string _name;
    /// <summary>
    /// "_birthyear" is for saving the birthyear as an int
    /// </summary>
    private int _birthyear;
    /// <summary>
    /// "_deathdate" is for saving the deathdate as DateTime
    /// </summary>
    private int? _deathyear;
    /// <summary>
    /// "_married" is a bool to say if the person is married
    /// </summary>
    private bool _married;
    /// <summary>
    /// "_child" is a bool to see if the person is over 18 or not
    /// </summary>/
    private bool _child;
    /// <summary>
    /// "_job" is for saving the job
    /// </summary>
    private bool _isMale;
    private List<Person> _children = new List<Person>();
    private Person? _father;
    private Person? _mother;
    private int _personID;
    private int? _fatherID;
    private int? _motherID;
    private int _generation;

    public int PersonID
  {
    get => _personID;
    set => _personID = value;
  }

  public int? FatherID
  {
    get => _fatherID;
    set => _fatherID = value;
  }

  public int? MotherID
  {
    get => _motherID;
    set => _motherID = value;
  }

    public int Generation
  {
    get => _generation;
    set => _generation = value;
  }
    public Person(string name, int birthyear, bool married, int? deathyear,bool isMale, Person? father, Person? mother, int personID)
    {
        _name = name;
        _birthyear = birthyear;
        _married = married;
        DateTime today = DateTime.Today;
        _deathyear = deathyear;
        _isMale = isMale;
        _father = father;
        _mother = mother;
        _personID = personID;
        if (this.IsAdult() == true)
        {
            _child = false;
        }
        else
        {
            _child = true;
        }

        _generation = CalculateGeneration();
    }

    /// <summary>
    /// Function "CalculateGeneration" is for calculating which generation the person is in
    /// </summary>
    /// <returns></returns>
    private int CalculateGeneration()
{
    // wenn keine Eltern bekannt → älteste Generation
    if (_father == null && _mother == null)
        return 1;

    int fatherGen = _father?.Generation ?? 5;
    int motherGen = _mother?.Generation ?? 5;

    int parentGen = Math.Max(fatherGen, motherGen);

    // Kind ist immer 1 Generation jünger
    return parentGen + 1;
}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="child"></param>
    public void addChild(Person child)
    {
        _children.Add(child);
    }

    /// <summary>
    /// Adds a deathyear to the person
    /// </summary>
    /// <param name="year"></param>
    public void addDeathdate(int year)
    {
        _deathyear = year;
    }

    /// <summary>
    /// Changes the marriage status of a person, if its not known, then its 
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void changeMarriageStatus()
    {
        if (_married == false)
        {
            _married = true;
        }
        else if (_married == true)
        {
            _married = false;
        }
        else
        {
            throw new Exception("Beziehungstatus nicht bekannt");
        }
    }

    public int Age()
  {
    if(_deathyear == null)
    {
      return Convert.ToInt32(DateTime.Today.Year) - _birthyear;
    } else
    {
      return Convert.ToInt32(_deathyear) - _birthyear;
    }
  }

    public string getName()
    {
        return _name;
    }

    public int Birthyear
    {
        get => _birthyear;
        set => _birthyear = value;
    }

    public int? Deathyear
    {
        get => _deathyear;
    }

    public bool getMariageStatus()
    {
        return _married;
    }

    public string getGender()
    {
        return _isMale ? "Mann" : "Frau";
    }
    public bool IsAdult()
    {
        if(Convert.ToInt32(DateTime.Today.Year) - _birthyear > 18)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public Person Father
    {
        get => _father;
        set => _father = value;
    }

    public Person Mother
    {
        get => _mother;
        set => _mother = value;
    }

    public int getPersonID()
    {
        return _personID;
    }

    public int? getFatherID
    {
        get => _fatherID;
    }

    public int? getMotherID
    {
        get => _motherID;
    }

    public override string  ToString()
    {
        string gender;
        if(_isMale == true)
        {
            gender = "männlich";
        } else
        {
            gender = "weiblich";
        }

        string aliveOrNot;

        if(_deathyear == null)
        {
            aliveOrNot = "ist";
        } else
        {
            aliveOrNot = "war";
        }

        string ageAlive;

        if(_deathyear == null)
        {
            ageAlive = $"ist {this.Age()} Jahre Alt";
        } else
        {
            ageAlive = $"ist {this.Age()} Jahre alt geworden";
        }
        return $"{_name} {aliveOrNot} {gender} und {ageAlive}";
    }
}
