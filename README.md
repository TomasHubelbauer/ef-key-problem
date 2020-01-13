# EF IDs Atop Seeded IDs

This repository demonstrates that how EF Core will continue assigning
IDs in sequence after a database has been seeded with hardcoded IDs.

This used not to work but has since been implemented and is shown in
the current codebase of this repository.

This used to be a pain point for me and indeed this not working is a
nice addition, however I have since adopted a practice of instead of
hardcoding IDs in the seed using .NET object references so that the
change tracker is free to assign any IDs it likes to the entities,
because the relations are given by the references that get passed
around which are stable within the unit of work.

As an example of this, instead of:

```cs
dbContext.Users.Add(new User { Id = 1, Role = "admin", Name = "Tomas Hubelbauer" });
dbContext.Documents.Add(new Document { Id = 1, OwnerUserId = 1, Content = "empty doc" });
```

One would write this:

```cs
var tomasHubelbauerUser = new User { Role = "admin", Name = "Tomas Hubelbauer" };
var tomasHubelbauerEmptyDocument = new Document { OwnerUser = tomasHubelbauerUser, Content = "empty doc" };
dbContext.Users.Add(tomasHubelbauerUser);
dbContext.Documents.Add(tomasHubelbauerEmptyDocument);
```

There can even only be a single `Add` because the nagivation property
will make sure the related entity gets persisted while saving the one
which gets added explicitly.

This repository uses SQLite to demonstrate (because inlike LocalDB) it
is cross-platform and in-memory is not relational. It was set up this
way:

```sh
dotnet new console
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
# Set up the DB context and entity model classes
dotnet run
```

## To-Do
