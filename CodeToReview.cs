using System;
using System.Collegctions.Generic; //RC: spell mistake
using System.Linq;

namespace Utility.Valocity.ProfileHelper
{
    public class People
    {
     private static readonly DateTimeOffset Under16 = DateTimeOffset.UtcNow.AddYears(-15);
     public string Name { get; private set; }
     public DateTimeOffset DOB { get; private set; }
     public People(string name) : this(name, Under16.Date) { }
     public People(string name, DateTime dob) {
         Name = name;
         DOB = dob;
     }
    }

    public class BirthingUnit
    {
        /// <summary>
        /// MaxItemsToRetrieve
        /// </summary>
        private List<People> _people; //RC: make variable name more descriptive, its list of people so it should be peopleList

        public BirthingUnit()
        {
            _people = new List<People>(); //RC: no need to initialize it again with list it can be simply -  new()
        }

        /// <summary>
        /// GetPeoples
        /// </summary>
        /// <param name="j"></param>
        /// <returns>List<object></returns>
        public List<People> GetPeople(int i) //RC: change method name to GetPeopleList or pluralize is better
        {
            for (int j = 0; j < i; j++)
            {
                try
                {
                    // Creates a dandon Name
                    string name = string.Empty;
                    var random = new Random();
                    if (random.Next(0, 1) == 0) { //RC: it will always selects Bob since random.Next doesn't include maxValule also instead of this logic to get random names, we could use enum to store some names with integer
                        name = "Bob";
                    }
                    else {
                        name = "Betty";
                    }
                    // Adds new people to the list
                    _people.Add(new People(name, DateTime.UtcNow.Subtract(new TimeSpan(random.Next(18, 85) * 356, 0, 0, 0))));
                }
                catch (Exception e) //RC: variable name should be ex instead of e.
                {
                    // Dont think this should ever happen
                    throw new Exception("Something failed in user creation");//RC: use string interpolation and format msg propely to include the exact error message.
                }
            }
            return _people;
        }

        private IEnumerable<People> GetBobs(bool olderThan30)
        {
            return olderThan30 ? _people.Where(x => x.Name == "Bob" && x.DOB >= DateTime.Now.Subtract(new TimeSpan(30 * 356, 0, 0, 0))) : _people.Where(x => x.Name == "Bob");
        }//RC: use Sring.ordinalIgnore when comparing name also we could simply add -30 years using AddYear Method.

        public string GetMarried(People p, string lastName)//RC: add method definition, method use is not clear, what exactly its used for
        {
            if (lastName.Contains("test"))
                return p.Name;
            if ((p.Name.Length + lastName).Length > 255) //RC: integer adding up with string
            {
                (p.Name + " " + lastName).Substring(0, 255);//RC: no variable is assigned here
            }

            return p.Name + " " + lastName;// RC: use string interpolation.
        }
    }
}