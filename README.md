# EF key problem

Demonstrates a problem with EF Core where `Add`ing or `Attach`ing an entity with set `Id` (for a PK)
will make subsequent attempts to add new entities without given `Id` (for which the change tracker
should figure out the next ID and use that) fail.

Created for [my Stack Overflow question](https://stackoverflow.com/q/46018654/2715716)
