# Code Review: Issue-by-Issue Comparison

## Issue #1: Typo in Using Statement ❌ → ✅

### Before (WRONG):
```csharp
using System.Collegctions.Generic;  // ← Typo!
```

### After (CORRECT):
```csharp
using System.Collections.Generic;   // ← Fixed
```

**Why it matters:** Code won't compile. Build fails immediately.

---

## Issue #2: Random.Next() Only Returns 0 ❌ → ✅

### Before (WRONG):
```csharp
var random = new Random();
if (random.Next(0, 1) == 0) {  // ← Only returns 0!
	name = "Bob";
}
else {
	name = "Betty";  // ← Never executes
}
```

**Result:** Always names people "Bob". Betty never appears.

### After (CORRECT):
```csharp
if (Random.Shared.Next(0, 2) == 0) {  // ← Returns 0 or 1
	name = "Bob";
}
else {
	name = "Betty";  // ← Now can execute
}
```

**Why:** `Random.Next(min, max)` returns values in `[min, max)` (exclusive of max).

---

## Issue #3: Missing Return Statement ❌ → ✅

### Before (WRONG):
```csharp
public string GetMarried(People p, string lastName)
{
	if ((p.Name.Length + lastName).Length > 255)
	{
		(p.Name + " " + lastName).Substring(0, 255);  // ← Missing 'return'!
	}

	return p.Name + " " + lastName;
}
```

**Problem:** Long names are not truncated. The substring is calculated but thrown away.

### After (CORRECT):
```csharp
public string GetMarried(People person, string lastName)
{
	string fullName = $"{person.Name} {lastName}";

	if (fullName.Length > MAX_NAME_LENGTH)
	{
		return fullName.Substring(0, MAX_NAME_LENGTH);  // ← Returns truncated string
	}

	return fullName;
}
```

---

## Issue #4: Wrong Days in Year (356 vs 365) ❌ → ✅

### Before (WRONG):
```csharp
// Line 9:
private static readonly DateTimeOffset Under16 = DateTimeOffset.UtcNow.AddYears(-15);

// Line 46:
DateTime.UtcNow.Subtract(new TimeSpan(random.Next(18, 85) * 356, 0, 0, 0))

// Line 60:
x.DOB >= DateTime.Now.Subtract(new TimeSpan(30 * 356, 0, 0, 0))
```

**Problem:** Uses 356 days instead of 365, causing ~4% age calculation error.

### After (CORRECT):
```csharp
// Best approach - use AddYears()
int ageInYears = Random.Shared.Next(MIN_AGE, MAX_AGE);
var dob = DateTime.UtcNow.AddYears(-ageInYears);

// For filtering:
var cutoffDate = DateTime.Now.AddYears(-CUTOFF_AGE);
return bobs.Where(x => x.DOB >= cutoffDate);
```

**Why:** Avoids hardcoded date math entirely. Let .NET handle leap years.

---

## Issue #5: Creating Random Inside Loop ❌ → ✅

### Before (WRONG):
```csharp
for (int j = 0; j < i; j++)
{
	var random = new Random();  // ← Created 1000x for 1000 items!
	if (random.Next(0, 2) == 0) { ... }
}
```

**Problems:**
- Creates new Random() instance every iteration (expensive)
- Poor randomness quality (seeded by system clock)
- Memory waste and GC pressure

### After (CORRECT):
```csharp
// Use .NET 6+ Random.Shared
for (int i = 0; i < count; i++)
{
	if (Random.Shared.Next(0, 2) == 0) { ... }
}

// Or use static field for older .NET:
private static readonly Random _random = new Random();

for (int i = 0; i < count; i++)
{
	if (_random.Next(0, 2) == 0) { ... }
}
```

---

## Issue #6: Poor Exception Handling ❌ → ✅

### Before (WRONG):
```csharp
catch (Exception e)  // ← Caught but not used
{
	// Dont think this should ever happen
	throw new Exception("Something failed in user creation");  // ← Loses original exception!
}
```

**Problem:** Stack trace and original exception information are lost.

### After (CORRECT):
```csharp
catch (Exception ex)
{
	throw new InvalidOperationException(
		$"Failed to create person at index {i} in GetPeople method",
		ex);  // ← Preserves original exception
}
```

**Why:** Using `new Exception(message, innerException)` preserves the full error context.

---

## Issue #7: Poor Naming Conventions ❌ → ✅

### Before (WRONG):
```csharp
public List<People> GetPeople(int i)  // ← What does 'i' mean?
{
	for (int j = 0; j < i; j++)  // ← Single-letter variable names
	{
		// ...
	}
}
```

### After (CORRECT):
```csharp
public List<People> GetPeople(int count)
{
	for (int i = 0; i < count; i++)
	{
		// ...
	}
}
```

**C# Naming Standards:**
- Parameters: PascalCase (`count`, not `i`)
- Loop variables: Can use `i`, `j` only for standard loops
- Descriptive names improve readability

---

## Issue #8: Invalid String Length Check ❌ → ✅

### Before (WRONG):
```csharp
if ((p.Name.Length + lastName).Length > 255)
	//  ↑ Can't add int + string!
{
	(p.Name + " " + lastName).Substring(0, 255);
}
```

**Problem:** `int + string` is invalid. Logic doesn't make sense.

### After (CORRECT):
```csharp
string fullName = $"{person.Name} {lastName}";

if (fullName.Length > MAX_NAME_LENGTH)  // ← Clear intent
{
	return fullName.Substring(0, MAX_NAME_LENGTH);
}
```

---

## Issue #9: Misleading Documentation ❌ → ✅

### Before (WRONG):
```csharp
/// <summary>
/// MaxItemsToRetrieve
/// </summary>
private List<People> _people;  // ← Doesn't match

/// <summary>
/// GetPeoples
/// </summary>
/// <param name="j"></param>  // ← Parameter is 'i', not 'j'
/// <returns>List<object></returns>  // ← Returns List<People>, not List<object>
public List<People> GetPeople(int i)
```

### After (CORRECT):
```csharp
/// <summary>
/// Generates a list of random people with ages between 18 and 85 years old.
/// </summary>
/// <param name="count">The number of people to generate.</param>
/// <returns>A list of randomly generated People objects.</returns>
public List<People> GetPeople(int count)
```

---

## Issue #10: Hardcoded Values ❌ → ✅

### Before (WRONG):
```csharp
if (random.Next(0, 2) == 0) {
	name = "Bob";      // ← Hardcoded
}
else {
	name = "Betty";    // ← Hardcoded
}

// ... later ...
random.Next(18, 85)    // ← Hardcoded age range
// ...
new TimeSpan(30 * 356, 0, 0, 0)  // ← Hardcoded cutoff
```

### After (CORRECT):
```csharp
private const string BOB = "Bob";
private const string BETTY = "Betty";
private const int MIN_AGE = 18;
private const int MAX_AGE = 85;
private const int CUTOFF_AGE = 30;

// ... then use ...
string name = Random.Shared.Next(0, 2) == 0 ? BOB : BETTY;
int ageInYears = Random.Shared.Next(MIN_AGE, MAX_AGE);
var cutoffDate = DateTime.Now.AddYears(-CUTOFF_AGE);
```

**Benefits:**
- One place to change values
- Self-documenting code
- Reusable across methods

---

## Issue #11: State Accumulation ❌ → ✅

### Before (WRONG):
```csharp
private List<People> _people;

public List<People> GetPeople(int i)
{
	for (int j = 0; j < i; j++)
	{
		_people.Add(new People(...));  // ← Appends to list
	}
	return _people;  // ← Returns ALL people ever added
}

// Usage:
var batch1 = unit.GetPeople(5);   // Returns 5
var batch2 = unit.GetPeople(5);   // Returns 10 (!) - unexpected
```

### After (CORRECT):
```csharp
public List<People> GetPeople(int count)
{
	var newPeople = new List<People>();  // ← New list for this batch

	for (int i = 0; i < count; i++)
	{
		var person = new People(...);
		newPeople.Add(person);
		_people.Add(person);  // ← Still track all, but return only new
	}
	return newPeople;  // ← Returns only newly created people
}

// Usage:
var batch1 = unit.GetPeople(5);   // Returns 5 ✅
var batch2 = unit.GetPeople(5);   // Returns 5 ✅
```

**Alternative:** Clear internal state if intended behavior is accumulation.

---

## Issue #12: Complex LINQ Expression ❌ → ✅

### Before (WRONG):
```csharp
private IEnumerable<People> GetBobs(bool olderThan30)
{
	return olderThan30 
		? _people.Where(x => x.Name == "Bob" && x.DOB >= DateTime.Now.Subtract(new TimeSpan(30 * 356, 0, 0, 0))) 
		: _people.Where(x => x.Name == "Bob");
}
```

**Problems:** Hard to read, duplicated logic, complex ternary.

### After (CORRECT):
```csharp
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
```

---

## Issue #13: Inconsistent Braces ❌ → ✅

### Before (WRONG):
```csharp
if (random.Next(0, 2) == 0) {  // ← Opening brace on same line
	name = "Bob";
}
else {
	name = "Betty";
}
```

### After (CORRECT):
```csharp
if (Random.Shared.Next(0, 2) == 0)
{  // ← Opening brace on new line (Allman style)
	name = "Bob";
}
else
{
	name = "Betty";
}
```

**Standard:** Follow EditorConfig or team standards for consistency.

---

## Issue #14 & #15: Typos ❌ → ✅

### Before (WRONG):
```csharp
// Line 40: Creates a dandon Name
// Line 48: Dont think this should ever happen
```

### After (CORRECT):
```csharp
// Line 40: Creates a random name
// Line 48: Don't think this should ever happen
```

---

## Summary of Changes

| Issue | Type | Impact | Fixed |
|-------|------|--------|-------|
| Typo: Collegctions | Syntax | 🔴 Build fails | ✅ |
| Random.Next(0,1) | Logic | 🔴 Wrong behavior | ✅ |
| Missing return | Logic | 🔴 Data loss | ✅ |
| 356 days | Logic | 🔴 Wrong calculations | ✅ |
| new Random() loop | Performance | 🟠 Slow | ✅ |
| Exception handling | Error handling | 🟠 Lost info | ✅ |
| Poor naming | Readability | 🟠 Unclear | ✅ |
| Invalid length check | Logic | 🟠 Wrong calc | ✅ |
| Wrong docs | Documentation | 🟠 Confusing | ✅ |
| Hardcoded values | Design | 🟡 Not maintainable | ✅ |
| State accumulation | Design | 🟡 Unexpected | ✅ |
| Complex LINQ | Readability | 🟡 Hard to test | ✅ |
| Inconsistent braces | Style | 🟢 Minor | ✅ |
| Typo: dandon | Spelling | 🟢 Minor | ✅ |
| Typo: Dont | Spelling | 🟢 Minor | ✅ |
