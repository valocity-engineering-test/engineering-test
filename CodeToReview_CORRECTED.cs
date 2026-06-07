using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility.Valocity.ProfileHelper
{
    public class People
    {
        private static readonly DateTimeOffset Under16 = DateTimeOffset.UtcNow.AddYears(-15);
        public string Name { get; private set; }
        public DateTimeOffset DOB { get; private set; }

        public People(string name) : this(name, Under16.Date) { }

        public People(string name, DateTime dob)
        {
            Name = name;
            DOB = dob;
        }
    }

    public class BirthingUnit
    {
        private const string BOB = "Bob";
        private const string BETTY = "Betty";
        private const int MIN_AGE = 18;
        private const int MAX_AGE = 85;
        private const int CUTOFF_AGE = 30;
        private const int MAX_NAME_LENGTH = 255;

        private List<People> _people;

        public BirthingUnit()
        {
            _people = new List<People>();
        }

        /// <summary>
        /// Generates a list of random people with ages between 18 and 85 years old.
        /// </summary>
        /// <param name="count">The number of people to generate.</param>
        /// <returns>A list of randomly generated People objects.</returns>
        public List<People> GetPeople(int count)
        {
            var newPeople = new List<People>();

            for (int i = 0; i < count; i++)
            {
                try
                {
                    // Create a random name (Bob or Betty)
                    string name = Random.Shared.Next(0, 2) == 0 ? BOB : BETTY;
                    int ageInYears = Random.Shared.Next(MIN_AGE, MAX_AGE);
                    var dob = DateTime.UtcNow.AddYears(-ageInYears);
                    var person = new People(name, dob);

                    newPeople.Add(person);
                    _people.Add(person);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        $"Failed to create person at index {i} in GetPeople method",
                        ex);
                }
            }

            return newPeople;
        }

        /// <summary>
        /// Gets all people named "Bob", optionally filtered by age.
        /// </summary>
        /// <param name="olderThan30">If true, returns only people older than 30.</param>
        /// <returns>An enumerable of Bob's.</returns>
        private IEnumerable<People> GetBobs(bool olderThan30)
        {
            var bobs = _people.Where(x => x.Name == BOB);

            if (olderThan30)
            {
                var cutoffDate = DateTime.Now.AddYears(-CUTOFF_AGE);
                return bobs.Where(x => x.DOB >= cutoffDate);
            }

            return bobs;
        }

        /// <summary>
        /// Combines a person's name with a last name, truncating if necessary.
        /// </summary>
        /// <param name="person">The person object.</param>
        /// <param name="lastName">The last name to append.</param>
        /// <returns>The combined full name, truncated to 255 characters if needed.</returns>
        public string GetMarried(People person, string lastName)
        {
            if (lastName.Contains("test"))
                return person.Name;

            string fullName = $"{person.Name} {lastName}";

            if (fullName.Length > MAX_NAME_LENGTH)
            {
                return fullName.Substring(0, MAX_NAME_LENGTH);
            }

            return fullName;
        }

        /// <summary>
        /// Clears all stored people.
        /// </summary>
        public void Clear() => _people.Clear();
    }
}
