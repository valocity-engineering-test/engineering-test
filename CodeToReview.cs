using System;
using System.Collegctions.Generic; //please fix the typo
using System.Linq;

namespace Utility.Valocity.ProfileHelper
{
    public class People // please consider a singular name for this class for eg. Person
    {
     private static readonly DateTimeOffset Under16 = DateTimeOffset.UtcNow.AddYears(-15);
     public string Name { get; private set; }
     public DateTimeOffset DOB { get; private set; }
     public People(string name) : this(name, Under16.Date) { }
     public People(string name, DateTime dob) {
         Name = name;
         DOB = dob;
     }}

    public class BirthingUnit
    {
        /// <summary>
        /// MaxItemsToRetrieve
        /// </summary>
        private List<People> _people;

        public BirthingUnit()
        {
            _people = new List<People>();
        }

        /// <summary>
        /// GetPeoples
        /// </summary>
        /// <param name="j"></param>
        /// <returns>List<object></returns>
        public List<People> GetPeople(int i)
        {
            for (int j = 0; j < i; j++)
            {
                try
                {
                    // Creates a dandon Name // please fix the typo from the description
                    string name = string.Empty;
                    var random = new Random();  // we can create this instance outside the for loop
                    if (random.Next(0, 1) == 0) { // you should replace the arguments from (0,1) to (0,2) as the upper bound is exclusive and you want to have equal chances for both names
                        name = "Bob";
                    }
                    else {
                        name = "Betty";
                    }
                    // Adds new people to the list
                    _people.Add(new People(name, DateTime.UtcNow.Subtract(new TimeSpan(random.Next(18, 85) * 356, 0, 0, 0))));
                }
                catch (Exception e)
                {
                    // Dont think this should ever happen
                    throw new Exception("Something failed in user creation"); // please fix the message or use Exception.Message
                }
            }
            return _people;
        }

        private IEnumerable<People> GetBobs(bool olderThan30) // please fix the method name to Something else which describes its functionality better
        {
            return olderThan30 ? _people.Where(x => x.Name == "Bob" && x.DOB >= DateTime.Now.Subtract(new TimeSpan(30 * 356, 0, 0, 0))) : _people.Where(x => x.Name == "Bob");
        }

        public string GetMarried(People p, string lastName)
        {
            if (lastName.Contains("test"))
                return p.Name;
            if ((p.Name.Length + lastName).Length > 255)
            {
                (p.Name + " " + lastName).Substring(0, 255); // please add missing return keyword here
            }

            return p.Name + " " + lastName;
        }
    }
}