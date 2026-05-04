# Copilot Instructions

## Build & Test

```powershell
# Full build + test pipeline
.\build.bat

# Or individual steps:
dotnet restore
dotnet build --configuration Release
dotnet test --configuration Release --verbosity normal

# Run a single test by name
dotnet test --filter "FullyQualifiedName~TestTheTruth"
```

## Architecture

This repo contains two exercises:

- **Exercise 1 — Code Review**: Review `CodeToReview.cs` at the repo root. No build required; the task is to provide feedback as if it were a PR.
- **Exercise 2 — Gilded Rose Kata**: Refactor `src/GildedRose.Console/Program.cs` to support "Conjured" items.
  - `GildedRose.Tests` (xUnit) references `GildedRose.Console` and is where test coverage should be added.
  - The console app serves as the entry point and hosts all domain logic.

## Key Constraints (Gilded Rose Kata)

- **Do NOT modify the `Item` class or the `Items` property** in `Program.cs` — this is a hard kata rule.
- `UpdateQuality()` and the `Items` property may be made `static` if desired.
- Quality is bounded: never negative, never above 50 (except "Sulfuras" which is always 80 and never changes).
- Item names are used as identifiers — exact string matching drives all special-case logic.

## Conventions

- Target framework: **net8.0**; nullable and implicit usings are enabled in both projects.
- Tests use **xUnit** (`[Fact]`, `[Theory]`). The test project namespace is `GildedRose.Tests`.
- The single existing test (`TestTheTruth`) is a placeholder — real coverage for `UpdateQuality` should be added.
