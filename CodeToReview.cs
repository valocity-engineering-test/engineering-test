using System;
///namespace was wrong
using System.Collections.Generic;
using System.Linq;

namespace Utility.Valocity.ProfileHelper
{
   
    public class Person
    {
        //Hardcoded values reduce maintainability. If the maximum name length needs to be changed in the future, it would require modifying the code in multiple places. By defining a constant for the maximum name length, we can easily update it in one place if needed, improving maintainability and readability.
        private const int MaxNameLength = 255;
        private const int MinimumAge = 18;
        private const int MaximumAge = 85;
        //The intent is unclear. subtracting 15 years actually means the person is approximately 15 years old. so there will be slight change in the code.
        //private static readonly DateTimeOffset Under16 = DateTimeOffset.UtcNow.AddYears(-15);
        private static readonly DateTimeOffset reqminDOB = DateTimeOffset.UtcNow.AddYears(-15);
        public string Name { get; private set; }
     public DateTimeOffset DOB { get; private set; }
     public Person(string name) : this(name, reqminDOB.Date) { }
        //DOB is DateTimeOffset, but constructor accepts DateTime. This is not a good design. I have changed the constructor to accept DateTimeOffset instead of DateTime.
        public Person(string name, DateTimeOffset dob) {
            //Missing Input Validation for name
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));
            Name = name;
         DOB = dob;
     }}

    public class BirthingUnit
    {
        /// <summary>
        /// MaxItemsToRetrieve
        /// </summary>

        /// Class names should generally represent a single entity and use singular nouns.
        ///People represents one person, not a collection.
        /////The reference itself should not change after initialization. so added readonly keyword to the list.
        private readonly List<Person> _person = new();

        public BirthingUnit()
        {
            _person = new List<Person>();
        }

        /// <summary>
        /// GetPeoples
        /// </summary>
        /// <param name="j"></param>
        /// <returns>List<object></returns>
        /// int i was descriptive so used as int count instead of j. Also, the method name should be GetPerson instead of GetPeoples as it returns a list of Person objects.
        public List<Person> GetPerson(int count)
        {
            for (int j = 0; j < count; j++)
            {
                try
                {
                    // Creates a dandon Name
                    string name = string.Empty;
                   // var random = new Random();
                   //Creating Random repeatedly inside a loop can generate duplicate sequences because of identical seeds. To avoid this, we can create a single instance of Random and reuse it throughout the loop.
                   private static readonly Random _random = new();
                    ///(random.Next(0, 1) will always return 0, so it will always assign "Bob" to name. To get a random number between 0 and 1, we should use random.Next(0, 2) instead.
                     if (random.Next(0, 2) == 0) {
                        name = "Bob";
                    }
                    else {
                        name = "Betty";
                    }
                    // Adds new people to the list
                    _person.Add(new Person(name, DateTime.UtcNow.Subtract(new TimeSpan(random.Next(MinimumAge, MaximumAge) * 365, 0, 0, 0))));
                }
                catch (Exception e)
                {
    // Dont think this should ever happen
    //to capture the exception added paremeter e to the catch block and rethrowing it with a more descriptive message. This way, if an exception does occur, we will have more context about where and why it happened.
    throw new Exception("Something failed in user creation",e);
                }
            }
            return _person;
        }

        private IEnumerable<Person> GetBobs(bool olderThan30)
        {
    //incorrect year calender 356. it should be 365.
    //Current logic returns younger people instead of older people.
    //Standardize on UTC time to avoid issues with time zones and daylight saving time changes. This way, the code will be more consistent and less prone to errors related to date and time calculations.
    return olderThan30 ? _person.Where(x => x.Name == "Bob" && x.DOB <= DateTime.UtcNow.AddYears(-30)) : _person.Where(x => x.Name == "Bob");
        }

        public string GetMarried(Person p, string lastName)
        {
    //lastName may be null. Added input validation to check if lastName is null or whitespace and throw an appropriate exception if it is. This ensures that the method behaves predictably and provides clear feedback when invalid input is provided.
            if (string.IsNullOrWhiteSpace(lastName))
             throw new ArgumentException("Last name cannot be null or whitespace.", nameof(lastName));
            if (lastName.Contains("test"))
                return p.Name;
            if ((p.Name.Length + lastName).Length > MaxNameLength)
            {
               return (p.Name + " " + lastName).Substring(0, MaxNameLength);
            }

            return p.Name + " " + lastName;
        }
    }
}