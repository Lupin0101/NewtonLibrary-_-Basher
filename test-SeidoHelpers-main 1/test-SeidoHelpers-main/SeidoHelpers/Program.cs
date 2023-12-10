using System.Collections.Generic;
using Helpers;

namespace SeidoHelpers;

class Program
{
    static void Main(string[] args)
    {
        #region csSeedGenerator Usage Examples
        Console.WriteLine("csSeedGenerator Usage Examples");

        //Create a generator, inherited from .NET Random
        var rnd = new csSeedGenerator();

        Console.WriteLine("Random Names");
        Console.WriteLine($"Firstname: {rnd.FirstName}");
        Console.WriteLine($"Lastname: {rnd.LastName}");
        Console.WriteLine($"Fullname: {rnd.FullName}");
        Console.WriteLine($"Petname: {rnd.PetName}");

        Console.WriteLine("\nRandom Address");
        var _country = rnd.Country;
        Console.WriteLine($"Streetname: {rnd.StreetAddress(_country)}");
        Console.WriteLine($"City: {rnd.City(_country)}");
        Console.WriteLine($"Zip code: {rnd.ZipCode}");
        Console.WriteLine($"Country: {_country}");

        Console.WriteLine("\nRandom Email and Phone number");
        Console.WriteLine($"Email: {rnd.Email()}");
        Console.WriteLine($"Email for specific name: {rnd.Email("John", "Smith")}");
        Console.WriteLine($"Phone number: {rnd.PhoneNr}");

        Console.WriteLine("\nRandom Quote");
        var _quote = rnd.Quote;
        Console.WriteLine($"Famous Quote: {_quote.Quote}");
        Console.WriteLine($"Author: {_quote.Author}");

        Console.WriteLine("\nRandom Music group and album names");
        Console.WriteLine($"Music group name: {rnd.MusicBand}");
        Console.WriteLine($"Music album name: {rnd.MusicAlbum}");

        Console.WriteLine("\nDateAndTime and Bool");
        Console.WriteLine($"This Year: {rnd.DateAndTime()}");
        Console.WriteLine($"Between Years: {rnd.DateAndTime(2000, 2020)}");
        Console.WriteLine($"True or False: {rnd.Bool}");

        Console.WriteLine("\nFrom String, Enum and List");

        Console.WriteLine($"From String: {rnd.FromString("Quick brown fox", " ")}");
        Console.WriteLine($"From Enum {nameof(enGreetings)}: {rnd.FromEnum<enGreetings>()}");

        var f = "Cloudy, Stormy, Rainy, Sunny, Windy";
        List<csWeather> _forecast = new List<csWeather>
        {
            new csWeather{ Temp = rnd.NextDecimal(100, 300), Visibility = rnd.FromString(f)},
            new csWeather{ Temp = rnd.NextDecimal(100, 300), Visibility = rnd.FromString(f)},
            new csWeather{ Temp = rnd.NextDecimal(100, 300), Visibility = rnd.FromString(f)}
        };
        Console.WriteLine($"From List {nameof(csWeather)} : {rnd.FromList(_forecast)}");

        Console.WriteLine("\nGenerating a randomly seeded list");
        var _persons = rnd.ToList<csPerson>(5);
        foreach (var item in _persons)
        {
            Console.WriteLine(item);
        }


        Console.WriteLine("\nGenerating a list of unique, randomly seeded, items");
        int _tryNrItems = 1000;
        var _pets = rnd.ToListUnique<csPet>(_tryNrItems);
        Console.WriteLine($"Try to generate {_tryNrItems} unique {nameof(csPet)}");
        Console.WriteLine($"{_pets.Count} unique {nameof(csPet)} could be created");
        foreach (var item in _pets)
        {
            Console.WriteLine(item);
        }


        Console.WriteLine("\nPicking unique items from a List");

        var _picklist = "Morning, Evening, Morning, Afternoon, Afternoon".Split(", ");
        var _uniquePicks = rnd.PickFromListUnique<string>(_tryNrItems, _picklist.ToList());
        Console.WriteLine($"Try to pick {_tryNrItems} unique items from {nameof(_picklist)}");
        Console.WriteLine($"{_uniquePicks.Count} unique items could be picked");
        foreach (var item in _uniquePicks)
        {
            Console.WriteLine(item);
        }


        var _AnotherPicklist = rnd.ToList<csPet>(10000);
        var _AnotherUniquePicks = rnd.PickFromListUnique<csPet>(_tryNrItems, _AnotherPicklist);
        Console.WriteLine($"\nTry to pick {_tryNrItems} unique items from {nameof(_AnotherPicklist)}");
        Console.WriteLine($"{_AnotherUniquePicks.Count} unique items could be picked");
        foreach (var item in _AnotherUniquePicks)
        {
            Console.WriteLine(item);
        }

        #endregion

        #region csConsoleInput Usage Example
        bool _continue = true;
        do
        {
            Console.WriteLine("\n\ncsConsoleInput Usage Example");

            int _intanswer;
            if (!csConsoleInput.TryReadInt32("Enter an integer", -1, 101, out _intanswer))
            {
                _continue = false;
                break;
            }
            Console.WriteLine($"You entered {_intanswer}");

            string _stringanswer = null;
            if (_continue &&
                !csConsoleInput.TryReadString("Enter a string", out _stringanswer))
            {
                _continue = false;
                break;
            }
            Console.WriteLine($"You entered {_stringanswer}");
            
            DateTime _dtanswer = default;
            if (_continue &&
                !csConsoleInput.TryReadDateTime("Enter a date and time", out _dtanswer))
            {
                _continue = false;
                break;
            }
            Console.WriteLine($"You entered {_dtanswer}");
            
        } while (_continue);
        #endregion

        Console.WriteLine("\n\nSeidoHelpers Quit");
        Console.ReadKey();

    }
}

public enum enGreetings { Hello, Goodbye, GoodMorning, GoodEvening }
public class csWeather
{
    public decimal Temp { get; set; }
    public string Visibility { get; set; }
    public override string ToString() => $"{Visibility} {Temp} degC";
}

public class csPerson : ISeed<csPerson>
{
    public string FullName { get; set; }
    public DateTime Birthday { get; set; }
    public override string ToString() => $"{FullName} is born on {Birthday:d}";

    #region ISeed implementation to use csSeeGenerator to create random lists
    public bool Seeded { get; set; } = false;
    public csPerson Seed(csSeedGenerator rnd)
    {
        FullName = rnd.FullName;
        Birthday = rnd.DateAndTime(1970, 2010);
        return this;
    }
    #endregion
}

public class csPet : ISeed<csPet>, IEquatable<csPet>
{
    public string PetName { get; set; }
    public override string ToString() => $"{PetName}";

    #region ISeed implementation to use csSeeGenerator to create random lists
    public bool Seeded { get; set; } = false;
    public csPet Seed(csSeedGenerator rnd)
    {
        PetName = rnd.PetName;
        return this;
    }
    #endregion

    #region implementing IEquatable
    public bool Equals(csPet other) => (other != null) ? (PetName) == (other.PetName) : false;

    public override bool Equals(object obj) => Equals(obj as csPet);
    public override int GetHashCode() => (PetName).GetHashCode();
    #endregion

}

