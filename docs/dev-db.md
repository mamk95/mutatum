# Database changes and migrations

!> **Not a developer?**
<br/>This document is only for developers who are changing parts of the Mutatum code related to the database. If you just want to use Mutatum and are looking for ways to connect it to your favorite database, [click here](database.md).

## Prerequisites

To create migration files you need the `dotnet-ef` tools installed. You only need to do this once:

```bash
dotnet tool install --global dotnet-ef
```

If your `dotnet-ef` is old you can update it:

```bash
dotnet tool update --global dotnet-ef
```

## After you have changed a database file

Note 1: you have to have an actual database connection for each environment, or else the migration tool will not work (yes, I know it sucks). I'll be happy to help you out with some temporary databases or resources to where you can create your own databases for free - just contact me on GitHub. You can set the database connection strings in appsettings.json, but remember to remove them before committing your code.

Note 2: You do not need to change the `Database:Provider` environment/appsetting variable between each migration command - if you look closely at the end of each command you can see it is set automatically.

Note 3: Although _in-memory_ can be used as a database (see [Getting started - Databases](database.md)), it does not need a migration. Therefore, it is not a part of the command list below.

```bash
cd src/Changelog

#MySQL
dotnet ef migrations add DescriptiveNameOfMigration --context MySqlDbContext --output-dir Data/Database/Migrations/MySQL -- --Database:Provider MySQL

dotnet ef migrations script --context MySqlDbContext --output Data/Database/Scripts/MySQL.sql -- --Database:Provider MySQL

dotnet ef migrations script --context MySqlDbContext --idempotent --output Data/Database/Scripts/MySQL-idempotent.sql -- --Database:Provider MySQL

#MariaDB
dotnet ef migrations add DescriptiveNameOfMigration --context MariaDbContext --output-dir Data/Database/Migrations/MariaDB -- --Database:Provider MariaDB

dotnet ef migrations script --context MariaDbContext --output Data/Database/Scripts/MariaDB.sql -- --Database:Provider MariaDB

dotnet ef migrations script --context MariaDbContext --idempotent --output Data/Database/Scripts/MariaDB-idempotent.sql -- --Database:Provider MariaDB

#MsSQL (Microsoft SQL)
dotnet ef migrations add DescriptiveNameOfMigration --context MsSqlDbContext --output-dir Data/Database/Migrations/MsSQL -- --Database:Provider MsSQL

dotnet ef migrations script --context MsSqlDbContext --output Data/Database/Scripts/MsSQL.sql -- --Database:Provider MsSQL

dotnet ef migrations script --context MsSqlDbContext --idempotent --output Data/Database/Scripts/MsSQL-idempotent.sql -- --Database:Provider MsSQL

#Postgres
dotnet ef migrations add Test --context PostgresDbContext --output-dir Data/Database/Migrations/Postgres -- --Database:Provider Postgres

dotnet ef migrations script --context PostgresDbContext --output Data/Database/Scripts/Postgres.sql -- --Database:Provider Postgres

dotnet ef migrations script --context PostgresDbContext --idempotent --output Data/Database/Scripts/Postgres-idempotent.sql -- --Database:Provider Postgres

#SQLite
dotnet ef migrations add DescriptiveNameOfMigration --context SQLiteDbContext --output-dir Data/Database/Migrations/SQLite -- --Database:Provider SQLite

dotnet ef migrations script --context SQLiteDbContext --output Data/Database/Scripts/SQLite.sql -- --Database:Provider SQLite
```

If you did something wrong and want to remove a migration:
```bash
dotnet ef migrations remove --context MySqlDbContext -- --Database:Provider MySQL

dotnet ef migrations remove --context MariaDbContext -- --Database:Provider MariaDB

dotnet ef migrations remove --context MsSqlDbContext -- --Database:Provider MsSQL

dotnet ef migrations remove --context PostgresDbContext -- --Database:Provider Postgres

dotnet ef migrations remove --context SQLiteDbContext -- --Database:Provider SQLite
```

