## LOGICAL Review:

1. GetMarried - Never return truncated value. Return value like below:
return (p.Name + " " + lastName).Substring(0, 255);

2. GetMarried - If condition does not require p.Name.Length (Line 72). p.Name is fine.

3. GetMarried - lastName can be null, so check null or empty using string.IsNullOrEmpty.

4. GetMarried - "test" is static content and it is in lower case, so first check null, then convert it to lower (lastName.ToLower()) and also trim the value.
Condition something like:

if (!string.IsNullOrEmpty(lastName) && lastName.ToLower().Trim().Contains("test"))

GetMarried - final method should be like below:

public string GetMarried(People p, string lastName)
{
    if (!string.IsNullOrEmpty(lastName) && lastName.ToLower().Trim().Contains("test"))
        return p.Name;

    if ((p.Name + lastName).Length > 255)
    {
        return (p.Name + " " + lastName).Substring(0, 255);
    }

    return p.Name + " " + lastName;
}

5. GetBobs - Comparison is reverse. Looking for olderThan30, so condition should be like:
x.DOB <= DateTime.Now.Subtract(new TimeSpan(30 * 365, 0, 0, 0))
NOT
x.DOB >= DateTime.Now.Subtract(new TimeSpan(30 * 365, 0, 0, 0))

6. People Constructor - DateTime type is not correct. It should be DateTimeOffset instead of DateTime.

Correct one:
public People(string name, DateTimeOffset dob)
{
    Name = name;
    DOB = dob;
}

7. GetPeople - It returns a List of People but _people is not cleared. If we call GetPeople twice, it will append records to the same list for the entire lifetime.

So before loop, use:
_people.Clear();

Here is a better implementation logic. Instead of initializing the object in constructor, create it inside GetPeople:

	// initialize object when GetPeople is invoked instead of constructor
	public List<People> GetPeople(int i)
	{
		var _people = new List<People>();

		for (int j = 0; j < i; j++)
		{
			// existing logic
		}

		return _people;
	}

_people pass in method:

	private IEnumerable<People> GetBobs(bool olderThan30, List<People> _people)
	{
		return olderThan30
			? _people.Where(x => x.Name == "Bob" && x.DOB <= DateTime.Now.Subtract(new TimeSpan(30 * 365, 0, 0, 0)))
			: _people.Where(x => x.Name == "Bob");
	}


8. Under16 is a static value and it never gets changed. It will initialize only once when the class is first invoked.

Remove line:
	private static readonly DateTimeOffset Under16 = DateTimeOffset.UtcNow.AddYears(-15);

Change below:
Before:
	public People(string name) : this(name, Under16.Date) { }
After:
	public People(string name) : this(name, DateTimeOffset.UtcNow.AddYears(-15)) { }

9. GetPeople - Try-catch block is used but exception stack trace is lost.

10. We should use throw; and in caller method we should log or handle it.

11. Or we can log it in the current method.

## NON-LOGICAL:

1. Method name should be GetPeoples instead of GetPeople, as comments mention GetPeoples.

2. Correct comment:
   /// <returns>List<object></returns> should be
   /// <returns>List<People></returns>

3. Maintain consistency between DateTime.UtcNow and DateTime.Now. Prefer using DateTime.UtcNow everywhere.

4. Static content is used in many places. Better to use constants to avoid duplication.

5. This code is tightly coupled, so use Repository pattern for creating Person.

6. Correct namespace System.Collegctions.Generic -->System.Collections.Generic

### Suggested Design:

1. Person Class - Separate entity

2. IPersonFactory (Object Creation)

   * Create logic in one place:
   * CreateRandom(int minAge = 18, int maxAge = 85)
   * CreateUnder16(string name)

3. IPersonRepository (Storage and retrieval logic)

   * Add
   * AddRange
   * GetAll

4. PersonService (Business rules)

   * Inject IPersonFactory and IPersonRepository via DI
   * Implement business logic here
