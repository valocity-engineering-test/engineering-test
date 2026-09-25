# Code Review: `CodeToReview.cs`

**Reviewer:** Umashankar Pandey
**Verdict:** Request changes

---

## Summary

Thanks for putting this up. The shape of what you're building is clear, and separating
the person type from the thing that creates them is the right instinct. There's a fair
bit to work through before this is ready to merge, so I've grouped the comments by
severity rather than by line — the four blockers need attention before anything else,
and several of the remaining points will resolve themselves once those are fixed.

Two themes run through most of the findings, and they matter more than any individual
line:

1. **Several of the bugs are silent.** The name generator can only ever produce one
   name, and the truncation in `GetMarried` does nothing at all. Neither fails loudly —
   they just quietly produce wrong data. A handful of unit tests over `GetPeople` and
   `GetMarried` would have caught all four blockers, and I'd like to see them added
   alongside the fixes.
2. **Dates are doing a lot of work here without much precision.** There are three
   separate date approximations in the file plus a local/UTC mix. Given this is profile
   and age logic, that's the area I'd most want us to agree on before it ships.

Happy to pair on any of this if it's easier than a comment thread.

---

## Blockers

### 1. The file does not compile — `line 2`

```csharp
using System.Collegctions.Generic;
```

Typo in `Collections`. Worth checking whether CI actually builds this file, because
this should have been caught before review — if it isn't part of a project, that's a
gap in its own right.

### 2. `GetMarried` throws its own result away — `lines 71-74`

```csharp
if ((p.Name.Length + lastName).Length > 255)
{
    (p.Name + " " + lastName).Substring(0, 255);
}

return p.Name + " " + lastName;
```

The `Substring` result is never assigned or returned, so the truncation never happens
and the method returns the full-length string regardless. This is legal C# and compiles
without error, which is exactly what makes it dangerous — if there's a 255-character
column behind this, it will fail at the database rather than here.

### 3. The length check measures the wrong thing — `line 71`

`p.Name.Length + lastName` concatenates an `int` to a `string`. For `"Bob"` and
`"Smith"` it evaluates to `"3Smith"`, so the guard tests the length of a string that
never existed. It needs to measure the actual result:

```csharp
var marriedName = $"{p.Name} {lastName}";
return marriedName.Length > MaxNameLength
    ? marriedName[..MaxNameLength]
    : marriedName;
```

Could you also pull `255` out into a named constant? Its meaning — a column width, I
assume — isn't obvious at the call site.

### 4. The random name is never random — `line 44`

```csharp
if (random.Next(0, 1) == 0)
```

`Random.Next(int, int)` has an **exclusive** upper bound, so this returns `0` every
time and every person is called "Bob". "Betty" is unreachable. `random.Next(2)` fixes
it, though picking from an array of names would be better — adding a third name then
doesn't mean touching the control flow.

This one has a knock-on effect worth flagging: it makes `GetBobs` look like it works,
because everyone is a Bob.

---

## Major

### 5. A year is not 356 days — `lines 51, 64`

```csharp
new TimeSpan(random.Next(18, 85) * 356, 0, 0, 0)
```

`356` should be `365`, in both places. At the top of the range that's around eighteen
months of drift, which for age logic is the difference between an 84-year-old and an
85-year-old.

More broadly, I'd avoid day-multiplication for ages altogether — leap years make it
wrong even with the right constant. `DateTime.UtcNow.AddYears(-years)` says what you
mean and lets the framework handle the calendar.

Related: `random.Next(18, 85)` also excludes its upper bound, so nobody is ever 85. If
that's deliberate, a comment would help; if not, it's `Next(18, 86)`.

### 6. `GetBobs(olderThan30: true)` returns people *younger* than 30 — `line 64`

```csharp
x.DOB >= DateTime.Now.Subtract(new TimeSpan(30 * 356, 0, 0, 0))
```

A date of birth *greater than* thirty years ago means the person was born more recently
than that — so this returns everyone under 30. The comparison needs to be `<=`.

This is a good example of where an extracted, named helper carries its own correctness:
`x.IsOlderThan(30)` is hard to get backwards in a way that `x.DOB >= someDate` is not.

### 7. Exception handling discards the original failure — `lines 53-57`

```csharp
catch (Exception e)
{
    // Dont think this should ever happen
    throw new Exception("Something failed in user creation");
}
```

Three distinct problems:

- `e` is caught and dropped, so the original message and stack trace are gone. If this
  ever fires in production we'll have nothing to debug with. At minimum, pass it as the
  inner exception.
- Throwing bare `Exception` means callers can't catch this selectively without catching
  everything.
- The comment is honest, and I'd act on it: if this genuinely can't fail, the
  `try`/`catch` is noise. A `catch` that exists "just in case" tends to hide the very
  failure it was meant to surface.

My suggestion is to delete the block and let anything unexpected propagate.

### 8. `GetPeople` accumulates across calls and leaks internal state — `lines 23, 59`

`_people` is a field that's appended to and then returned directly. Two consequences:

- Calling `GetPeople(5)` twice returns ten people, not five. The name doesn't suggest
  that, and I doubt it's intended.
- Callers get the live `List<People>` and can add to or clear it behind the class's
  back.

I'd build and return a fresh list per call. If the field needs to stay, return it as
`IReadOnlyList<People>`. It should also be `readonly` — it's only ever assigned in the
constructor.

### 9. `GetMarried` has no null guards — `lines 67-71`

`p`, `p.Name` and `lastName` are all dereferenced unchecked. A null `lastName` throws
on `line 69` before reaching anything else. Since this is a public method, could you add
guards — or enable nullable reference types on the project so the compiler enforces it?

### 10. Test-data logic on a production path — `line 69`

```csharp
if (lastName.Contains("test"))
    return p.Name;
```

Silently returning a different result for names containing "test" is surprising, and
there's no comment explaining why. It's also a correctness problem: `Contains` matches
substrings, not whole words, so a legitimate surname like "Protester" takes this branch
and silently loses the last name.

If this exists for test fixtures, it belongs in the test project. If it's a real rule,
it needs a comment and a far more precise condition.

---

## Minor

### 11. `new Random()` inside the loop — `line 43`

Allocating a new instance per iteration is wasteful, and on .NET Framework it was an
outright bug — instances created within the same clock tick shared a seed and produced
identical sequences. Modern .NET seeds per instance, so it isn't broken here, but it's a
well-known trap and reviewers will keep flagging it. Hoist it out of the loop, or use
`Random.Shared`.

### 12. Local and UTC time are mixed — `lines 9, 51, 64`

`DateTimeOffset.UtcNow` on lines 9 and 51, `DateTime.Now` on line 64. Outside a UTC
timezone those differ, so the age boundary shifts depending on which path you came
through. Pick UTC and apply it consistently.

Related: the constructor on `line 13` takes a `DateTime` and assigns it to a
`DateTimeOffset` property. That conversion assumes the value is local time, which is
unlikely to be what the caller meant given it came from `UtcNow`. Taking a
`DateTimeOffset` parameter sidesteps the question entirely.

### 13. `Under16` is evaluated once, at type load — `line 9`

```csharp
private static readonly DateTimeOffset Under16 = DateTimeOffset.UtcNow.AddYears(-15);
```

In a long-running process this is fixed at startup and drifts further from "now" every
day it stays up. It should be computed per call — a static property or method rather
than a field.

The name is ambiguous too: it's `-15` years, so it yields a 15-year-old, whereas the
threshold for "under 16" would more naturally be `-16`. Worth clarifying which you
meant. Separately, defaulting an unspecified date of birth to a minor is a significant
behaviour to leave implicit — I'd rather the single-argument constructor didn't exist
than have it guess.

### 14. `GetBobs` is dead code — `line 62`

Private and never called, so it's unreachable and uncovered. Either wire it up or delete
it — right now it's carrying bug #6 somewhere nothing will catch it.

---

## Nits

- **`line 18` — `BirthingUnit`.** The name doesn't describe what the class does, and
  it's the kind of thing that reads badly outside the team. `PersonRepository` or
  `PersonFactory` would say what it is.
- **`line 7` — `People`.** The class models one person, so `Person`. The collection then
  reads naturally as `List<Person>`.
- **`line 35` — parameter `i`.** It's a count, not an index; `count` reads better, and
  the loop variable `j` would become `i`.
- **`lines 20-22`** — the doc comment says `MaxItemsToRetrieve`, which describes neither
  a list of people nor anything else here. Looks like a copy-paste leftover.
- **`lines 30-34`** — `<param>` names `j` but the parameter is `i`; `<returns>` says
  `List<object>` but the method returns `List<People>`; and the unescaped angle brackets
  will break the generated XML. Either fix it or drop it — a stale doc comment is worse
  than none.
- **`line 41`** — "Creates a dandon Name", typo for "random".
- **`lines 9-16`** use five-space indentation and close with `}}`; **`lines 44-49`** put
  braces on the same line while the rest of the file uses Allman. Running the formatter
  settles all of it.
- **`lines 10-11`** — `Name` and `DOB` are never reassigned after construction, so they
  could be get-only rather than `private set`.
- **`line 51`** does four things at once. Extracting the date-of-birth calculation into
  a named local would make it readable.

---

## What I liked

- Restricted setters on `Name` and `DOB` rather than public fields.
- The intent behind the XML doc comments — they just need to match the code.
- `GetBobs` correctly avoids duplicating the `Name == "Bob"` filter across both branches
  of the conditional.

---

## Suggested order of work

1. Fix the compile error (#1) and get the file into a project CI actually builds.
2. Add tests for `GetPeople` and `GetMarried`. They'll fail — that's the point.
3. Fix the blockers (#2, #3, #4), then the date handling (#5, #6).
4. Clean up exception handling and state management (#7, #8).
5. Naming and documentation last, once the behaviour is settled.
