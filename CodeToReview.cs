using System;
using System.Collections.Generic; //correct the namespace, it seems to be typo error.
using System.Linq;

namespace Utility.Valocity.ProfileHelper
{
    public class People
    {
     private static readonly DateTimeOffset Under16 = DateTimeOffset.UtcNow.AddYears(-15); //this variable is used to default add person as Under16
     public string Name { get; private set; }
     public DateTimeOffset DOB { get; private set; } //can be as DateOfBirth for readability
     public People(string name) : this(name, Under16.Date) { } //can directly store the Date value in Under16 variable as its readonly values, .Date not reuqired. 
     public People(string name, DateTime dob) {
         Name = name;
         DOB = dob;
     }}

    public class BirthingUnit // this class is used to add people on the basis of name and birth date so can have class name as BirthUnit
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
        /// AddPeople
        /// </summary>
        /// <param name="name">name of person</param>
        /// <param name="dateOfBirth">Date of Birth</param>
        /// <returns>true/false to confirm if person is added on not</returns>
    public bool AddPeople(string name, DateTime? dateOfBirth=null) // name and Date of Birth of is optional
        {
        try{
            if(!dateOfBirth.HasValue)
            {
                var random = new Random();
                dateOfBirth = DateTime.UtcNow.Subtract(new TimeSpan(random.Next(18, 85) * 356, 0, 0, 0));
            }
            _people.Add(new People(name, dateOfBirth.Value));
         }
          catch (Exception e)
            {
                //Log the exception in logs.
                return false;            
            }
         return true;            
        }
        
        /// <summary>
        /// GetPeoples
        /// </summary>
        /// <param name="j"></param>
        /// <returns>List<object></returns>
        //now we can fetch people direct from the _people object so not required this method
        public List<People> GetPeople(int i) //this method is used for adding person and also fetching the people. We can have separate method for each. //AddPerson and GetPeople. 
        {
            for (int j = 0; j < i; j++)
            {
                try
                {
                    // Creates a dandon Name
                    string name = string.Empty;
                    var random = new Random();
                    if (random.Next(0, 1) == 0) {
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
                    throw new Exception("Something failed in user creation");
                }
            }
            return _people;
        }
        //it seems, this method is specifically used to fetch person that has name ="Bob" with filter age above 30 or not. 
        //it is private so we can access only in same class. 
        private IEnumerable<People> GetBobs(bool olderThan30)
        {
            //return olderThan30 ? _people.Where(x => x.Name == "Bob" && x.DOB >= DateTime.Now.Subtract(new TimeSpan(30 * 356, 0, 0, 0))) : _people.Where(x => x.Name == "Bob");
            //have use Addyears for the readability. 
            return olderThan30 ? _people.Where(x => x.Name == "Bob" && x.DOB >= DateTime.Now.AddYears(-30)) : _people.Where(x => x.Name == "Bob");
        }
         /// <summary>
        /// GetPersons
        /// </summary>
        /// <param name="name">name of person</param>
        /// <param name="olderThan30">if person must be older than 30 or not</param>
        /// <returns>List<object></returns> 
        private IEnumerable<People> GetPersons(string name,bool olderThan30)
        {
            return olderThan30 ? _people.Where(x => x.Name == name && x.DOB >= DateTime.Now.AddYears(-30)) : _people.Where(x => x.Name == name);
        }
        //i am not sure what are we doing in this method. 
        public string GetMarried(People p, string lastName)
        {
            if (lastName.Contains("test"))
                return p.Name;
            if ((p.Name.Length + lastName).Length > 255)
            {
                (p.Name + " " + lastName).Substring(0, 255);// no use of this code. it might be assigned to some variable. 
            }

            return p.Name + " " + lastName;
        }
    }
}
