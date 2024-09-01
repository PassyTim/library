# Library Api

Api for library service.  Technologies used: ASP.NET Core 8., EF Core, SQL Server, AutoMapper, FluentValidation  


## Features

- CRUD functionality on Book and Author models
- Policy-based auth using refresh and access jst tokens
- Ability to take book from library and return it
- InMemory BooksRepository caching


# Run locally

First you need to have SQL Server installed and running

Clone the project

```bash
  git clone https://github.com/PassyTim/library.git
```

Go to the project directory

```bash
  cd library
```

Make appsettings.json file and put it into Library.Api directory
```bash
  {
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Kestrel": {
    "Endpoints": {
      "Http" : {
        "Url": "https://localhost:7212",
        "Protocols": "Http1AndHttp2"
      }
    }
  },
  "ImageBaseUrl": "https://localhost:7212/Uploads/",
  "JwtOptions" : {
    "ExpiresMin" : 15,
    "SecretKey" : "secretkeysecretkeysecretkeysecretkeysecretkeysecretkeysecretkeysecretkeysecretkey"
  }
  "ConnectionStrings": {
    "DefaultSQLConnection" : "YOUR_CONNECTION_STRING_TO_SQL_SERVER"
  }
}
```

Make sure you have .NET 8 installed ([Install .NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0))

```bash
  dotnet --version
```

Instal dependencies

```bash
  dotnet restore
  dotnet build
```
Run Api project

```bash
  dotnet run
```
Api will run on 7212 port
