using System.Diagnostics.Contracts;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using Microsoft.VisualBasic;
using Microsoft.Win32;
namespace FamilytreesLib;

public class Person // 50/50 von Beiden geschrieben und kommentiert
{
    /// <summary>
    /// "_name" is for saving the name as a string
    /// </summary>/
    private string _name;
    /// <summary>
    /// "_birthyear" is for saving the birthyear as an int
    /// </summary>
    private int? _birthyear;
    /// <summary>
    /// "_deathyear" is for saving the deathdate as DateTime
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
    /// "_isMale" is a bool to see if a person is a male or not
    /// </summary>
    private bool _isMale;
    /// <summary>
    /// "_personID" is a integer for assigning a ID to a person wich is the primary key for the databank (automatic assignment)
    /// </summary>
    private int _personID;
    /// <summary>
    /// "PersonID" is a getter setter property to set the ID of a person to a specific ID
    /// </summary>
    public int PersonID
    {
        get => _personID;
        set => _personID = value;
    }
    /// <summary>
    /// The Constructor for the class Person which "defines" the attributes for the person
    /// </summary>
    /// <param name="name"> The name of the person </param>
    /// <param name="birthyear"> The birthyear of the person </param>
    /// <param name="married"> The married status of the person </param>
    /// <param name="deathyear"> The deathyear of the person </param>
    /// <param name="isMale"> Checks if the person is male or not </param>
    /// <param name="personID"></param>
    public Person(string name, int? birthyear, bool married, int? deathyear, bool isMale, int personID)
    {
        _name = name;
        _birthyear = birthyear;
        _married = married;
        DateTime today = DateTime.Today;
        _deathyear = deathyear;
        _isMale = isMale;
        _personID = personID;
        if (this.IsAdult() == true)
        {
            _child = false;
        }
        else
        {
            _child = true;
        }
    }

    /// <summary>
    /// Adds a deathyear to the person
    /// </summary>
    /// <param name="year"> The year of the death </param>
    public void addDeathdate(int year)
    {
        _deathyear = year;
    }

    /// <summary>
    /// "Married" is for getting and setting the marriage status
    /// </summary>
    public bool Married
    {
        get => _married;
        set => _married = value;
    }

    /// <summary>
    /// "IsMale" is for getting and setting the gender
    /// </summary>
    public bool IsMale
    {
        get => _isMale;
        set => _isMale = value;
    }

    //// <summary>
    /// "Name" is for getting and setting the name of the person
    /// </summary>
    public string Name
    {
        get => _name;
        set => _name = value;
    }

    /// <summary>
    /// Changes the marriage status of a person, if its not known, then its throwing an exception
    /// </summary>
    /// <exception cref="Exception"> The exception with a specific message </exception>
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

    /// <summary>
    /// Method for returning the age of a person
    /// </summary>
    /// <returns> Returns the calculaded age </returns>
    public int Age()
    {
        if (_deathyear == null)
        {
            return Convert.ToInt32(DateTime.Today.Year) - Convert.ToInt32(_birthyear);
        }
        else
        {
            return Convert.ToInt32(_deathyear) - Convert.ToInt32(_birthyear);
        }
    }

    /// <summary>
    /// Method for getting the name
    /// </summary>
    /// <returns> Returns the name </returns>
    public string getName()
    {
        return _name;
    }

    /// <summary>
    /// Property for getting and setting the birthyear
    /// </summary>
    public int? Birthyear
    {
        get => _birthyear;
        set => _birthyear = value;
    }

    /// <summary>
    /// Property for getting and setting the deathyear
    /// </summary>
    public int? Deathyear
    {
        get => _deathyear;
    }

    /// <summary>
    /// Method for getting the marriagestatus 
    /// </summary>
    /// <returns> Returns the marriedstatus </returns>
    public bool getMariageStatus()
    {
        return _married;
    }

    /// <summary>
    /// Method for getting the gender
    /// </summary>
    /// <returns> Returns the gender (male or female) </returns>
    public string getGender()
    {
        return _isMale ? "Mann" : "Frau";
    }
    /// <summary>
    /// Method for cheking if a person is an adult or not
    /// </summary>
    /// <returns> Returns a boolean if person is adult or not </returns>
    public bool IsAdult()
    {
        if (Convert.ToInt32(DateTime.Today.Year) - _birthyear > 18)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Method for returning the ID of a person
    /// </summary>
    /// <returns> Retruns the ID of a person </returns>
    public int getPersonID()
    {
        return _personID;
    }

    /// <summary>
    /// ToString for showing the information of a person
    /// </summary>
    /// <returns> Returns a tostring with the different informations </returns>
    public override string ToString()
    {
        string gender;
        if (_isMale == true)
        {
            gender = "männlich";
        }
        else
        {
            gender = "weiblich";
        }

        string aliveOrNot;

        if (_deathyear == null)
        {
            aliveOrNot = "ist";
        }
        else
        {
            aliveOrNot = "war";
        }

        string ageAlive;

        if (_deathyear == null)
        {
            ageAlive = $"ist {this.Age()} Jahre Alt";
        }
        else
        {
            ageAlive = $"ist {this.Age()} Jahre alt geworden";
        }

        return $"{_name} {aliveOrNot} {gender}und {ageAlive}";
    }
}
