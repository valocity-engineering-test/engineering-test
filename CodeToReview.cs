using System;
using System.Collegctions.Generic; // Review: Compile errors, Fix the typo System.Collegctions.Generic; -> using System.Collections.Generic
using System.Linq;

namespace Utility.Valocity.ProfileHelper // Review: Namespace should be Utility.Velocity.ProfileHelper, Fix the typo Utility.Valocity.ProfileHelper -> Utility.Velocity.ProfileHelper
{
    // Review: Class Alignment is off, Fix the class alignment
    public class People // Review: Class name should be singular, Fix the class name People -> Person
    {
        private static readonly DateTimeOffset Under16 = DateTimeOffset.UtcNow.AddYears(-15); // Review: Under16 variable is confusing, It should be DefaultDobForUnder16
        public string Name { get; private set; }
        public DateTimeOffset DOB { get; private set; } // Review: Variable name should be DateOfBirth, Fix the variable name DOB -> DateOfBirth
        public People(string name) : this(name, Under16.Date) { }
        public People(string name, DateTime dob) {
         Name = name; // Review: Throw an exception if name is null or empty, Fix ?? throw new ArgumentNullException(nameof(name));
            DOB = dob;
     }}

    // Review: This class breaks the SOLID Principles
    public class BirthingUnit // Review: Class name should be singular, Fix the class name BirthingUnit -> BirthUnit
    {
        /// <summary>
        /// MaxItemsToRetrieve // Review: Invalid comment, It should be something like "List of people in the birthing unit" or "List of people in the birth unit"
        /// </summary>
        private List<People> _people; // Review: Make IReadOnlyList, Variable name should be plural, Fix the variable name _people -> Peoples

        public BirthingUnit()
        {
            _people = new List<People>();
        }

        /// <summary>
        /// GetPeoples // Review: Summmary should be something like "Get a list of people in the birthing unit", Fix the summary GetPeoples -> GetPeople
        /// </summary>
        /// <param name="j"></param> // Reviw: Invalid Param name, it should be i, Fix the param name j -> i
        /// <returns>List<object></returns> // Review: Invalid return type, it should be List<People>, Fix the return type List<object> -> List<People>
        public List<People> GetPeople(int i)
        {
            for (int j = 0; j < i; j++)
            {
                try
                {
                    // Creates a dandon Name
                    string name = string.Empty;
                    var random = new Random(); // Review: Random should be static, Fix the Random instance to be static like "private static readonly Random random = new Random();"
                    if (random.Next(0, 1) == 0) // Review: This code always create Bob, Fix string name = _random.Next(2) == 0 ? "Bob" : "Betty";
                    { 
                        name = "Bob";
                    }
                    else {
                        name = "Betty"; // Review: Create Const for the name "Bob" and "Betty", Fix the magic string "Betty" to a constant variable like "const string BettyName = "Betty";"
                    }
                    // Adds new people to the list // Review: Remove comment, it is not necessary and does not add any value
                    _people.Add(new People(name, DateTime.UtcNow.Subtract(new TimeSpan(random.Next(18, 85) * 356, 0, 0, 0))));
                }
                catch (Exception e) // Review: Catching a general exception is not a good practice, It should be something like "catch (ArgumentException e)" or "catch (InvalidOperationException e)", Fix the catch block to catch specific exceptions
                { // Review: e varaible creates code smell. Log the exception e, Fix the catch block to log the exception e before throwing a new exception
                    // Dont think this should ever happen // Review: Remove the unused comment
                    throw new Exception("Something failed in user creation"); // Review: Invalid exception message, it should be something like "Failed to create a person" or Throw;
                }
            }
            return _people;
        }

        /// Review: This method is not use in code, Access modifier change to public to access it from outside the class, Fix the access modifier to public
        private IEnumerable<People> GetBobs(bool olderThan30)// Review: Create reuse method GetPeopleByName(string name, int? olderThanYears = null), This is not support for Betty , Fix the method to be more generic and reusable
        {
            // Review: Fix the string comparison to use StringComparison.OrdinalIgnoreCase
            // DOB comparison is wrong, it should be x.DOB <= DateTime.Now.AddYears(-30) to get people older than 30, Fix the DOB comparison in the LINQ query
            // DOB DateTime.UtcNow should be DateTime.Now, Fix the DateTime.UtcNow to DateTime.Now for consistency with the rest of the code
            return olderThan30 ? _people.Where(x => x.Name == "Bob" && x.DOB >= DateTime.Now.Subtract(new TimeSpan(30 * 356, 0, 0, 0))) : _people.Where(x => x.Name == "Bob");
        }

        public string GetMarried(People p, string lastName)
        {
            // Review: Add validation check if (p == null) throw new ArgumentNullException(nameof(p));
            if (lastName.Contains("test")) // Review: Add validation check for lastname "if (string.IsNullOrEmpty(lastName))" throw invalid argument exception, Fix the condition to check if lastName is null or empty
                return p.Name;
            if ((p.Name.Length + lastName).Length > 255) // Review: Invalid condition, it should be something like "if ((p.Name.Length + lastName.Length) > 255)", Fix the condition to check the length of both strings
            {
                (p.Name + " " + lastName).Substring(0, 255); // Review: 255 set as Const, Fix the magic number 255 to a constant variable like "const int MaxNameLength = 255;"
            }

            return p.Name + " " + lastName; // Fix the return statement to return the full name with a space in between, Fix the return statement to return $"{p.Name} {lastName}"
        }
    }
}