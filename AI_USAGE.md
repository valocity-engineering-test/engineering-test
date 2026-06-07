# AI Usage Disclosure
## Exercise 1: Refer CodeReviewCommentsREADME.md 


Tool used: **Cursor** (AI-assisted coding chat)

## Exercise 2: Gilded Rose Refactoring Kata

### Where AI was used

- Understanding the kata rules and what "Conjured" items mean
- First pass at refactoring the original `UpdateQuality()` method
- Suggesting the Strategy + Factory pattern to split item logic into separate classes
- Writing and expanding unit tests
- Creating this disclosure document

### Why AI was used

- The original `UpdateQuality()` code was hard to follow — AI helped break down the rules faster
- Used it as a sounding board for a clean but simple design (not over-engineered)
- Helped double-check edge cases like quality staying between 0 and 50

### What I did myself / reviewed

- Ran `build.bat` and made sure all tests pass
- Kept the `Item` class unchanged (per exercise requirements)
- Chose a simple folder structure: `Updaters/` and `Helpers/`
- Reviewed each updater class to confirm the business rules are correct
- Made sure conjured items are detected with `StartsWith("Conjured")`

### Final design (my own words)

- **Strategy pattern** — each item type has its own updater class (`NormalItemUpdater`, `ConjuredItemUpdater`, etc.)
- **Factory** — `ItemUpdaterFactory` picks the right updater based on item name
- **Template method** — `DegradingItemUpdater` base class shared by normal and conjured items (only the degrade rate differs)
- **QualityHelper** — shared logic for keeping quality between 0 and 50

### What I can explain in interview

- How each item type behaves day by day
- Why normal and conjured share a base class
- How the factory decides which updater to use
- What tests cover and what could still be added


