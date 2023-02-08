# PatientManagerService

## Getting Started

You can run the project and then browse to the [homepage](http://localhost:57678/) and you can view the [API swagger documentation](http://localhost:57678/swagger/index.html)

This project will use IISExpress and SQLExpress. These can be changed, for example it will run with SqlServer (defualt) or SQLite. To toggle between them, change `options.UseSqlServer(connectionString));` to `options.UseSqlite(connectionString));` in the `PatientManagerService.Infrastructure.StartupSetup` file. Also remember to replace the `DefaultConnection` with `SqliteConnection` in the `Your.ProjectName.Web.Program` file, which points to your Database Server

Log files are written to the Web project root directory.  Patient upload files are saved to Resources\Uploads

## The Core Project

The Core project is the center of the design, and all other project dependencies should point toward it. The Core project includes things like:

- Entities
- Aggregates
- Domain Events
- DTOs
- Interfaces
- Specifications

## The SharedKernel Project

It contains types that would be shared between multiple projects or solutions.

## The Infrastructure Project

Most dependencies on external resources should be implemented in classes defined in the Infrastructure project. These classes should implement interfaces defined in Core. 

The Infrastructure project depends on `Microsoft.EntityFrameworkCore.SqlServer` and `Autofac`. The former is used because it's built into the default ASP.NET Core templates and is the least common denominator of data access. If desired, it can easily be replaced with a lighter-weight ORM like Dapper. Autofac (formerly StructureMap) is used to allow wireup of dependencies to take place closest to where the implementations reside. 

## The Web Project

The entry point of the application is the ASP.NET Core web project. This is actually a console application, with a `public static void Main` method in `Program.cs`. It currently uses the default MVC organization (Controllers and Views folders) as well as most of the default ASP.NET Core project template code. This includes its configuration system, which uses the default `appsettings.json` file plus environment variables, and is configured in `Startup.cs`. The project delegates to the `Infrastructure` project to wire up its services using Autofac.

## The Test Projects

Test projects are organized based on the kind of test (unit, functional, integration, performance, etc.). In terms of dependencies, there are three things to note:

- [xunit](https://www.nuget.org/packages/xunit) 

- [Moq](https://www.nuget.org/packages/Moq/) Moq can be used as a mocking framework for white box behavior-based tests. 

- [Microsoft.AspNetCore.TestHost](https://www.nuget.org/packages/Microsoft.AspNetCore.TestHost) I'm using TestHost to test my web project using its full stack. Using TestHost, I make actual HttpClient requests without going over the wire (so no firewall or port configuration issues). Tests run in memory and are very fast, and requests exercise the full MVC stack, including routing, model binding, model validation, filters, etc.

# Patterns Used

This solution template has code built in to support a few common patterns, especially Domain Driven Design patterns. 