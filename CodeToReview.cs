using System;
using System.Collegctions.Generic; // Typo in "Collections" , Code won't compile, use intellisense in IDE
using System.Linq;

namespace Utility.Valocity.ProfileHelper
{
    public class People // This class represents one person , so we should use singular (person) for classname.
    {
     private static readonly DateTimeOffset Under16 = DateTimeOffset.UtcNow.AddYears(-15); // Variable name doesn't match the purpose, we named it under 16 but subtracted only 15
     public string Name { get; private set; }
     public DateTimeOffset DOB { get; private set; }
     public People(string name) : this(name, Under16.Date) { }
     public People(string name, DateTime dob) { // we should validate that name is not null or empty otherwise getMarried may later throw when we do p.Name.length.
         Name = name;
         DOB = dob;
     }}

    public class BirthingUnit
    {
        /// <summary>
        /// MaxItemsToRetrieve
        /// </summary>
        private List<People> _people; // Consider marking it readonly to prevent accidental reassignment.

        public BirthingUnit()
        {
            _people = new List<People>();
        }

        /// <summary>
        /// GetPeoples
        /// </summary>
        /// <param name="j"></param> // parameter name is wrong, use i rather than j for correct documentation
        /// <returns>List<object></returns> // return type is wrong here, correct one is List<people>
        public List<People> GetPeople(int i)
        {
            for (int j = 0; j < i; j++) // what would happen if i is -ve, consider putting safe-guards for negative values 
            {
                try
                {
                    // Creates a dandon Name (typo here, create a random name)
                    string name = string.Empty;
                    var random = new Random();
                    if (random.Next(0, 1) == 0) { // random.Next(min, max) includes the minimum but excludes the maximum , so Next(0,1) always returns 0 and betty branch is unreachable
                        name = "Bob";
                    }
                    else {
                        name = "Betty";
                    }
                    // Adds new people to the list
                    // an year is not alway 365 days, Leap year has 366 days. 
                    _people.Add(new People(name, DateTime.UtcNow.Subtract(new TimeSpan(random.Next(18, 85) * 356, 0, 0, 0))));
                }
                catch (Exception e)
                {
                    // Catching every exception and throwing a new exception loses the original stacktrace. we can remove this catch and catch at source directly.
                    throw new Exception("Something failed in user creation");
                }
            }
            // we are returning the mutable list here, Caller can modify values from this. consider returning a read-only collection.
            // we keep on adding to the existing list, so call getPeople(10) twice returns a list with 20 people. is accumlation intentional here ?
            return _people; 
        }

        private IEnumerable<People> GetBobs(bool olderThan30)
        {
            //
            return olderThan30 ? _people.Where(x => x.Name == "Bob" && x.DOB >= DateTime.Now.Subtract(new TimeSpan(30 * 356, 0, 0, 0))) : _people.Where(x => x.Name == "Bob");
        }


        //p and lastName can be null. we can have NRE here. add validation guards
        public string GetMarried(People p, string lastName)
        {
            if (lastName.Contains("test")) // any business rule which mandates ignoring last name containing "test". document the reason here. Also, do we need case senstitivity here.
                return p.Name;
            if ((p.Name.Length + lastName).Length > 255) // are we adding length (int) + string ? , this would create 3Goyal
            {
                (p.Name + " " + lastName).Substring(0, 255); // substring returns a new string. it doesn't modify the original string. the truncated result is not stored anyway and is discarded.
            }

            return p.Name + " " + lastName;
        }
    }
}
