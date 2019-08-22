# EF key problem

Demonstrates a problem with EF Core where `Add`ing or `Attach`ing an entity with set `Id` (for a PK)
will make subsequent attempts to add new entities without given `Id` (for which the change tracker
should figure out the next ID and use that) fail.

I asked for the EF change tracked to be smarted about this and assign IDs that follow in sequence
atop of what was already assigned explicitly in the seed, but the issue didn't go anywhere:

https://github.com/aspnet/EntityFrameworkCore/issues/9683

I have since adopted a practice of instead of hardcoding IDs in the seed using .NET object references
so that the change tracker is free to assign any IDs it likes to the entities, because the relations
are given by the references that get passed around which are stable within the unit of work.

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

There can even only be a single `Add` because the nagivation property will make sure the related entity
gets persisted while saving the one which gets added explicitly.
