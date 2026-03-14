# Book Collection App

*(This README is intentionally rough so you can improve it with GitHub Copilot CLI)*

A C# console app for managing books you have or want to read.
It can add, remove, and list books. Also mark them as read.

---

## Current Features

* Reads books from a JSON file (our database)
* Includes a `LinearEquationSolver` example for solving linear systems up to 5 unknowns
* Input checking is weak in some areas
* Some tests exist but probably not enough

---

## Files

* `Program.cs` - Main CLI entry point
* `Models/Book.cs` - Book model class
* `Services/BookCollection.cs` - BookCollection class with data logic
* `Services/LinearEquationSolver.cs` - Gaussian-elimination solver with a `SolutionVector` property
* `data.json` - Sample book data
* `Tests/BookCollectionTests.cs` - xUnit tests
* `Tests/LinearEquationSolverTests.cs` - xUnit tests for the solver

---

## Running the App

```bash
dotnet run -- list
dotnet run -- add
dotnet run -- find
dotnet run -- remove
dotnet run -- help
```

## Running Tests

```bash
cd Tests
dotnet test
```

---

## Notes

* Not production-ready (obviously)
* Some code could be improved
* Could add more commands later
