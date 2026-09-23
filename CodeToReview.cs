using System;
using System.Collegctions.Generic;  //fix the namespace issue 
using System.Linq;

namespace Utility.Valocity.ProfileHelper  //namespace could be more structured like project=>folder name
{
    public class People
    {
     private static readonly DateTimeOffset Under16 = DateTimeOffset.UtcNow.AddYears(-15);  //create the variable for number -15 and assign the value at the time of use. instead if calculating every time
     public string Name { get; private set; }
     public DateTimeOffset DOB { get; private set; }
     public People(string name) : this(name, Under16.Date) { }
     public People(string name, DateTime dob) {
         Name = name;
         DOB = dob;
     }}

    public class BirthingUnit  // make it birthunit
    {
        /// <summary>
        /// MaxItemsToRetrieve
        /// </summary>
        private List<People> _people; //make it singular like list<person> person; 

        public BirthingUnit()
        {
            _people = new List<People>();
        }

        /// <summary>
        /// GetPeoples
        /// </summary>
        /// <param name="j"></param>
        /// <returns>List<object></returns>
        public List<People> GetPeople(int i) //same update person instead of people
        {
            for (int j = 0; j < i; j++)//for formatting purpose move this inside the try
            {
                try
                {
                    // Creates a dandon Name
                    string name = string.Empty;
                    var random = new Random();
                    if (random.Next(0, 1) == 0) { //use conditional operator to minimize the code ex. name=random.Next(0, 1) == 0?"BOB":"Betty"; Also check for random function it will always be true in this case becasue upper bound is 1. correct would random.next(0,2)
                        name = "Bob";
                    }
                    else {
                        name = "Betty";
                    }
                    // Adds new people to the list
                    _people.Add(new People(name, DateTime.UtcNow.Subtract(new TimeSpan(random.Next(18, 85) * 356, 0, 0, 0)))); //make the  timespan seperate veriable and explain it what it does. can we check if the given user is already present then dont add or think of use of othere data structure like dictionary instead of list
                }
                catch (Exception e)//use specific exception instead of generic
                {
                    // Dont think this should ever happen
                    throw new Exception("Something failed in user creation");
                }
            }
            return _people;
        }

        private IEnumerable<People> GetBobs(bool olderThan30)
        {
            return olderThan30 ? _people.Where(x => x.Name == "Bob" && x.DOB >= DateTime.Now.Subtract(new TimeSpan(30 * 356, 0, 0, 0))) : _people.Where(x => x.Name == "Bob");//check the condition when olderthan30 is true then it will return less tha 30 years person i.e younger 
        }

        public string GetMarried(People p, string lastName)
        {
            if (lastName.Contains("test")) // add null check 
                return p.Name;
            if ((p.Name.Length + lastName).Length > 255) //p.name.lenght is there any specific reason behind this code if not then substring inside this condition will be ignored
            {
                (p.Name + " " + lastName).Substring(0, 255);            }

            return p.Name + " " + lastName;
        }
    }
}
