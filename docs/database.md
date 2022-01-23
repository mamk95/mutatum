# Databases

You need a database to safely store the data Mutatum uses. Mutatum supports many different databases, making it easy to find one you already know or use. Not able to find the database you want? Just submit a feature request and I'll see what I can do!

Do you have no idea which database to choose? Here are some simple tips for the confused admin:
- Running **locally** on your computer: The simplest solution is probably SQLite or MariaDB.
- Running in **Azure**: Use the cheap and simple [Azure SQL Database](https://azure.microsoft.com/en-us/products/azure-sql/database/).
- Running in **Heroku**: Use the free/cheap [Heroku Postgres](https://www.heroku.com/postgres).

Need a simple tool to connect to your database? Check out the completely free and open source [DBeaver](https://dbeaver.io/) application. It supports all the databases listed below (and many more).

## MsSQL (Microsoft SQL)

| Version  | Tested  | Working? |
|---|---|---|
| ≥15 (≥2019) | ✔️ Automatically when released | ✔️ Probably |
| 15 (2019) | ✔️ 2022.01.22 | ✔️ Yes |
| 14 (2017) | No | ✔️ Probably |
| <14 | No | ❓ Unknown |

When you have your Microsoft SQL server ready, please connect to it and run one of the following SQL scripts:
- First-time Mutatum setup: Run [this script](../tree/master/src/Changelog/Data/Database/Scripts/MsSQL.sql ':ignore') to create the tables
- Upgrading Mutatum setup: Run [this script](../tree/master/src/Changelog/Data/Database/Scripts/MsSQL-idempotent.sql ':ignore') to create and update tables that are missing or outdated

Environment variable to enable Microsoft SQL database:
```
Database:Provider="MsSQL"
Database:MsSQL:ConnectionString="Server=tcp:localhost,1433;Initial Catalog=databaseName;Persist Security Info=False;User ID=root;Password=1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```



## Postgres

| Version  | Tested  | Working? |
|---|---|---|
| ≥13| ✔️ Automatically (delayed*) | ✔️ Probably |
| 13 | ✔️ 2022.01.22 | ✔️ Yes |
| 12 | No | ✔️ Probably |
| <12 | No | ❓ Unknown |

_(*) delayed: The hosted database provider is responsible for upgrading the database used for automatic testing. MySQL is upgraded within days of release. MariaDB and Postgres are usually upgraded within a year of a major release. MsSQL is usually ahead (yes, ahead) of new releases._

When you have your Postgres server ready, please connect to it and run one of the following SQL scripts:
- First-time Mutatum setup: Run [this script](../tree/master/src/Changelog/Data/Database/Scripts/Postgres.sql ':ignore') to create the tables
- Upgrading Mutatum setup: Run [this script](../tree/master/src/Changelog/Data/Database/Scripts/Postgres-idempotent.sql ':ignore') to create and update tables that are missing or outdated

Environment variable to enable Postgres database:
```
Database:Provider="Postgres"
Database:Postgres:ConnectionString="Host=myserver;port=123;Username=mylogin;Password=mypass;Database=mydatabase;"
```






## MySQL (Oracle)

| Version  | Tested  | Working? |
|---|---|---|
| ≥8.0| ✔️ Automatically when released | ✔️ Probably |
| 8.0 | ✔️ Yes, 2022.01.21 | ✔️ Yes |
| 5.7 | ✔️ Yes, 2022.01.21 | ❌ No |
| 5.6 | ✔️ Yes, 2022.01.21 | ❌ No |
| 5.5 | ✔️ Yes, 2022.01.21 | ❌ No |

When you have your MySQL server ready, please connect to it and run one of the following SQL scripts:
- First-time Mutatum setup: Run [this script](../tree/master/src/Changelog/Data/Database/Scripts/MySQL.sql ':ignore') to create the tables
- Upgrading Mutatum setup: Run [this script](../tree/master/src/Changelog/Data/Database/Scripts/MySQL-idempotent.sql ':ignore') to create and update tables that are missing or outdated

?> **Database warnings?**
<br/>When manually creating the tables with the provided SQL script, you might see warnings like this:
<br/>`Integer display width is deprecated and will be removed in a future release.`
<br/>These warnings can safely be ignored.

Environment variable to enable MySQL database:
```
Database:Provider="MySQL"
Database:MySQL:ConnectionString="server=localhost;port=3306;user=root;password=1234;database=name;"
```



## MariaDB

| Version  | Tested  | Working? |
|---|---|---|
| ≥10.4 | Automatically (delayed*) | ✔️ Probably |
| 10.4 | ✔️ 2022.01.22 | ✔️ Yes |
| 10.3 | No | ✔️ Probably |

_(*) delayed: The hosted database provider is responsible for upgrading the database used for automatic testing. MySQL is upgraded within days of release. MariaDB and Postgres are usually upgraded within a year of a major release. MsSQL is usually ahead (yes, ahead) of new releases._

When you have your MariaDB server ready, please connect to it and run one of the following SQL scripts:
- First-time Mutatum setup: Run [this script](../tree/master/src/Changelog/Data/Database/Scripts/MariaDB.sql ':ignore') to create the tables
- Upgrading Mutatum setup: Run [this script](../tree/master/src/Changelog/Data/Database/Scripts/MariaDB-idempotent.sql ':ignore') to create and update tables that are missing or outdated

?> **Database warnings?**
<br/>When manually creating the tables with the provided SQL script, you might see warnings like this:
<br/>`Name 'PK___EFMigrationsHistory' ignored for PRIMARY key.`
<br/>These warnings can safely be ignored.

Environment variable to enable MariaDB database:
```
Database:Provider="MariaDB"
Database:MariaDB:ConnectionString="server=localhost;port=3306;user=root;password=1234;database=name;"
```



## SQLite

| Version  | Tested  | Working? |
|---|---|---|
| ≥3.7.0 (2010) | Partially | ✔️ Probably |
| 3.36.0 (2021) | ✔️ Yes, 2022.01.23 | ✔️ Yes |
| ≥3.7.0 (2010) | Partially | ✔️ Probably |
| ≤3.6.23 (2010) | ❌ No | ❌ No |

!> **Careful where you store the database file!**
<br/>If you are running Mutatum in a Docker container, please make sure that your SQLite database is stored outside the container. If the SQLite database file is stored within the container, it will be removed when you update the Docker container.

You need a SQLite database file. Many tools allow you to create them. Here is one way to create an empty database using the `sqlite3` tool ([download sqlite3](https://sqlite.org/download.html)):
```bash
sqlite3 mutatum.sqlite "VACUUM;"
```

When you have your SQLite database file ready, please open it and run one of the following SQL scripts:
- First-time Mutatum setup: Run [this script](../tree/master/src/Changelog/Data/Database/Scripts/SQLite.sql ':ignore') to create the tables
- Upgrading Mutatum setup: Not as easy as other databases. Unlike other databases, SQLite doesn't include a procedural language. Because of this, there is no way to generate the if-then logic required by the idempotent migration scripts. You can try to create your own point-to-point migration script using [this guide](https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#idempotent-script-limitations), or ask me for help.

?> **No tables being created?**
<br/>Are the tables not being created when you run the script above? Your database tool (DBeaver, DBBrowser etc.) might be screwing with your script by automatically creating transactions for you, messing with the transactions within the script. Either turn off the transaction feature of your database tool, or remove `BEGIN TRANSACTION;` and `COMMIT;` from the SQL script. Then, start over completely by wiping the database and trying again.

Environment variable to enable SQLite database:
```
Database:Provider="SQLite"
Database:SQLite:ConnectionString="Filename=path/to/database/mutatum.sqlite"
```
If you are on Windows, remember to escape backslashes in the connection string file path (e.g. `Filename=C:\\Users\\username\\mutatum.sqlite`).



## In-memory
!> **You will lose data!**
<br/>In-memory database should only be used for demo purposes. The content of the database will be wiped every time the application is stopped.

The in-memory database is a great way to demonstrate what Mutatum can do without actually having to configure a database. You do not have to worry about database versions or configuration, you only have to worry about losing your data at <ins>every single reboot</ins>.

Environment variable to enable in-memory database:
```
Database:Provider="InMemory"
```



## Unsupported databases

Any database not listed above should be considered unsupported. Here is a list of some databases I want to support, but have not been able to integrate into Mutatum:

- **CosmosDB**: ASP.NET Identity does not work with Cosmos.
- **Redis** (for storage, not cache): Not supported by Entity Framework.
- **Azure Storage Table**: Not supported by Entity Framework.
- **Google Cloud Spanner**: The Entity Framework provider only supports EF Core 3.1 [(source)](https://github.com/googleapis/dotnet-spanner-entity-framework/issues/83#issuecomment-868408595). Once support for EF Core 6 is added, I would love to take a look at it.
- **Oracle Database**: I'm unable to find a open source Entity Framework provider with a permissive license. I'm also afraid to get sued by Oracle if I use the closed-source provider created by Oracle.

If you want Mutatum to support any other databases, you can either create a feature request or do it yourself and submit a pull request. Whatever option you choose, please note that it would be great if you could help me find a free or cheap way to host the database server. I need access to a database server for automatic testing before releases and for debugging purposes.



## General notes on database versions

Although I try my best to support new database versions and releases, it is hard to keep up. Specifically regarding Mutatum, here are some general rules to follow regarding if you should upgrade your database or not.

Note: Versioning of software is often done using SemVer. It basically boils down to the version number being _major.minor.patch_ (e.g. 1.18.3), where changes in __patch__ are bug fixes, changes in __minor__ is new functionality without breaking old functionality, and increments of __major__ are larger and possibly incompatible changes.

- Changes to __patch__: Upgrade database without worrying about Mutatum.
- Changes to __minor__: Upgrade database without worrying about Mutatum.
- Changes to __major__: Delay the database upgrade for a few months. You are free to contact me and request that I test the new version a few months after release.