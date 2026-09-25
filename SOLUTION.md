# Solution Notes

Submission for the Valocity engineering test. Two exercises:

- **Exercise 1 — Code review:** [`CODE_REVIEW.md`](CODE_REVIEW.md)
- **Exercise 2 — Gilded Rose kata:** `src/GildedRose.Console/Updaters/` and
  `src/GildedRose.Tests/`

---

## How to run

```
build.bat
```

Restores, cleans, builds in Release and runs the tests. 42 tests, all passing.
Built against .NET 8; also verified on the .NET 10 SDK via roll-forward.

---

## Exercise 2: approach

### The order of work mattered more than the design

The task says to add conjured items "as long as everything still works correctly", but
the repository contained no definition of *correctly* — `TestAssemblyTests.cs` asserted
`true` and nothing else. So the refactor had no safety net and no way to demonstrate
that behaviour was preserved.

I worked in three commits, deliberately kept separate so the history shows the reasoning
rather than just the outcome:

| Commit | What it does | Tests |
|---|---|---|
| `b6f3d0c` | Characterisation tests against the **untouched** legacy code | 31 added, all green |
| `5d3707a` | Refactor to per-category updaters, **no behaviour change** | same 31, unmodified, still green |
| `d1c6aa1` | Add conjured items | 8 added — red before, green after |

The middle commit is the one that carries the argument: 78 lines of nested conditionals
were replaced with delegation, and the test file was not touched. That is what makes the
refactor safe rather than merely plausible.

### Characterisation tests describe the legacy, not the ideal

They pin what the code *did*, including its quirks. Two were worth calling out
explicitly, because both are decisions rather than accidents:

**The 50 cap is applied per increment, not as an invariant.** A backstage pass at
quality 48 with 4 days remaining reaches 50, not 51 — the legacy code re-checked
`Quality < 50` before each individual increment. My implementation clamps once at the
end, which is behaviourally identical here because these categories only ever increase.
I verified this against every boundary case rather than assuming it.

**Sulfuras sits at 80 because nothing touches it.** The "never more than 50" rule is a
constraint on *increases*, not on the type. This is why `LegendaryItemUpdater`
deliberately does **not** derive from `ItemUpdater` — a legendary item is not an item
that ages by zero, it is outside the ageing cycle entirely, and routing it through the
shared clamp would silently crush its quality from 80 to 50.

### Why the strategy pattern

The rules vary along exactly one axis — the category of the item — and that set is
explicitly open for extension, since conjured items are the proof. So each category owns
its rule behind `IItemUpdater`, and `UpdateQuality` is reduced to resolving an updater
and delegating.

The constraint that `Item` cannot be modified is what rules out the otherwise-obvious
alternative of polymorphic item subclasses. Behaviour therefore lives in collaborators
that act *on* an item rather than in the item itself. That turns out to be the better
design anyway — the ageing rules are a property of the business, not of the data.

`ItemUpdater` holds the shared cycle: recalculate quality, hold it within range, advance
the sell-in. A new category only has to state how its own quality changes.

I kept `UpdateQuality` and `Items` as instance members. The brief permits making them
static, but static would be a testability regression here for no benefit — the tests
construct a `Program` per case and rely on that isolation.

### Expiry is evaluated before the sell-in is decremented

The legacy code decrements `SellIn` in the middle of the method, so its first-half
thresholds read against the pre-decrement value and its second half against the
post-decrement one. I evaluate `expired` once, up front, as `SellIn <= 0` — which is
equivalent to the legacy's post-decrement `SellIn < 0` — and keep it that way for the
whole update.

The benefit is that the backstage thresholds can be written as the rules are actually
stated: `<= 5` and `<= 10`, matching "5 days or less" and "10 days or less" from the
brief. Written against the post-decrement value they would have to read `< 5` and `< 10`,
which invites exactly the off-by-one that the boundary tests at 11/10/6/5 exist to catch.

---

## Assumptions and trade-offs

**"Conjured" is a category, matched on the prefix.** The inventory contains only
`"Conjured Mana Cake"`, so exact-name matching would have satisfied the letter of the
brief. But the brief describes a *supplier of conjured items* and phrases the rule as a
category, so a supplier shipping a second line is the expected case, not an exotic one.
I match on the `"Conjured "` prefix — including the trailing space, so that names merely
sharing the stem, such as `"Conjuration Robes"`, stay out. Two tests guard that boundary
in both directions.

This is the one place I extended beyond the literal requirement, so it is the decision
I'd most expect to be challenged. The trade-off is a small risk of over-matching a name
like `"Conjured by Allison"` against the much likelier cost of every new conjured
product silently ageing at the wrong rate. If the team prefers the conservative reading,
it is a one-line change in `ItemUpdaterResolver` and the tests state the expectation
explicitly either way.

**Name matching is ordinal and case sensitive.** This mirrors the `!=` comparisons in
the legacy code. Loosening it would be a behaviour change, and it belongs in its own
commit if it's wanted.

**Item names are still strings in two places.** `ItemUpdaterResolver` and the test
constants both hold them. Deduplicating would mean the test project depending on a
production constant for its own expected values, which weakens the test — it would then
pass even if the production name were wrong. The duplication is deliberate.

**`Main` is unchanged.** It still prints only `OMGHAI!` and never displays the
inventory, which makes the console app a poor demonstration of the feature. Fixing that
is outside what was asked, and the tests are the better evidence, so I left it alone
rather than widen the diff. Worth a follow-up if the console output matters to anyone.

**`Item` and `Items` are untouched**, as required. Verified in the refactor commit before
committing.

---

## Use of generative AI

Per the brief, here is where AI was used and why.

I used Claude Code throughout, primarily as a pair-programming partner. Concretely:

- **Analysing the legacy method.** I worked through the nested conditionals with it to
  enumerate every branch before writing the characterisation tests, including the
  interaction between the mid-method `SellIn` decrement and the backstage thresholds.
  The boundary cases in the test suite came out of that analysis.
- **Generating the test suite and updater classes** from a design I specified — the
  strategy shape, the decision to keep `LegendaryItemUpdater` off the base class, and
  the pre-decrement expiry check were my calls, made before any code was written.
- **Drafting the code review and these notes**, which I directed, reviewed and edited.

What I did not delegate: the sequencing (tests before refactor, refactor before
feature), the design decisions recorded above, and the prefix-matching judgement call.

Two points where I checked the output rather than taking it:

- The claim that clamping once at the end is equivalent to the legacy's per-increment
  cap. I verified it against the boundary cases (48/49/50 quality at 4 and 10 days)
  rather than accepting the reasoning, because it is exactly the kind of equivalence
  that sounds right and isn't.
- The `new Random()` finding in the code review. The commonly cited version of that bug —
  instances sharing a time-based seed — has not applied since .NET Core 3.0, so I
  corrected the draft to describe it accurately as a historical trap and a wasteful
  allocation, rather than repeating the folklore as a live defect.

I'm happy to walk through any of this in the technical discussion.
