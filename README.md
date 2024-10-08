# Library 

Welcome to the Library, a comprehensive service designed to manage a library system with seamless integration between the backend and frontend. This project utilizes ASP.NET Core 8 for the Web API and React for the frontend, offering a robust solution for managing books and authors.

You can find frontend project in this [repository](https://github.com/PassyTim/library_frontend)

## Features

- **CRUD Operations**: Create, read, update, and delete books and authors.
- **Book Management**:
    - Retrieve a list of all books.
    - Fetch specific books by ID or ISBN.
    - Add new books and update existing ones.
    - Remove books from the collection.
    - Allow users to borrow and return books.
    - Upload and store book cover images.
    - Notifications for due dates and impending expirations.

- **Author Management**:
    - List all authors.
    - Get author details by ID.
    - Add and update author information.
    - Remove authors from the database.
    - View all books associated with a specific author.

- **User Interface**:
    - Registration and authentication pages.
    - A book list page that indicates availability.
    - Detailed book information page with borrowing options.
    - Admin interface for managing book entries.
    - User-specific pages to view borrowed books.
    - Features pagination, search by book title, and author filtering.

- **Web API**:
    - Implemented policy-based authorization using JWT access and refresh tokens.
    - Repository and Unit of Work patterns for data management.
    - Global exception handling middleware.
    - Efficient pagination for book listings.
    - Unit tests for repositories and use cases using xUnit.
    - Image caching for enhanced performance (browser caching implemented).

## Technologies Used

- **Backend**:
    - ASP.NET Core 8
    - Entity Framework Core
    - MS SQL Server
    - AutoMapper
    - FluentValidation
    - JWT Authentication
    - xUnit for unit testing
    - Swagger for API documentation
    - Redis for caching
    - Docker for containerization (Docker Compose)

- **Frontend**:
    - React
    - Chakra UI

## Architecture and Design Patterns

This project is developed using the **Clean Architecture** approach to maintain a clear separation of concerns and ensure scalability and maintainability.

### Clean Architecture

- **Domain Layer**: Contains domain models. This layer is independent of any frameworks, UI, or external data sources.
- **Persistence Layer**: Contains the logic of working with external data storage. In this case SQL Server.
- **Application Layer**: Includes application services, exceptions, and contracts. It handles interactions between the domain and external systems.
- **Infrastructure Layer**: Implements external services such as token provider.
- **API Layer**: Manages Web API (ASP.NET Core backend) to interact with the application layer.

This layered approach ensures that changes in one part of the system do not affect other layers.

### Design Patterns

Several design patterns are used to improve the structure and maintainability of the project:

- **Repository Pattern**: To abstract database operations and separate business logic from data access logic.
- **Unit of Work Pattern**: Ensures that multiple repository changes are treated as a single transaction.
- **Dependency Injection (DI)**: Used for injecting dependencies throughout the application, promoting loose coupling.
- **Decorator Pattern**: Used to dynamically add additional behavior to objects without modifying their structure. In this project, the Decorator is applied to extend repository logic with caching.

These design principles and patterns help to create a clean, scalable, and maintainable codebase that is easy to extend and adapt to future requirements.

# Important
Application supports only 2 roles : "Admin" and "User" by default. A user with the Admin role can only be created via swagger. 

# Docker launch (backend + frontend)
## Prerequisites

1. **Docker**: Make sure Docker is installed on your machine. You can download it [here](https://www.docker.com/get-started).
2. **Docker Compose**: Docker Compose is included with Docker Desktop, but you can also install it separately by following the instructions [here](https://docs.docker.com/compose/install/).

## Cloning the Repository

First, clone the repository to your local machine:

```bash
git clone https://github.com/PassyTim/library.git
cd library
```

## Configuration
Add appsettings.json file to Library.Api:
```json
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
        "Url": "http://library.api:5000",
        "Protocols": "Http1AndHttp2"
      },
      "Https" : {
        "Url": "https://localhost:7212",
        "Protocols": "Http1AndHttp2"
      }
    }
  },
  "ImageBaseUrl": "http://localhost:5000/Uploads/",
  "JwtOptions" : {
    "ExpiresMin" : 15,
    "SecretKey" : "secretkeysecretkeysecretkeysecretkeysecretkeysecretkeysecretkeysecretkeysecretkey"
  },
  "ConnectionStrings": {
    "Redis": "library.cache:6379",
    "DefaultSQLConnection" : "Server=library.database,1433;Database=Library.API;User Id=sa;Password=Passw0rd;MultipleActiveResultSets=true;Integrated Security=false;TrustServerCertificate=true"
  }
}
```

## Running the Project with Docker Compose
Run the following command to start the containers

```bash
docker-compose up --build
```
This command will build and start all necessary containers, including the backend API, frontend, and database services.

### Initial Setup Time

- **Important**: The first time you run the containers, the setup process for the SQL Server database may take up to <ins>20 minutes</ins> due to initial configuration and seeding of the database. Please be patient and do not interrupt the process.

### Verifying the Containers
You can verify that the containers are running correctly by listing all active containers:

```bash
docker ps
```

You should see the following containers:

- library.api: ASP.NET Core backend service
- library.frontend: React frontend service
- library.database: SQL Server database
- library.cache: Redis cache

## Accessing the Application
- **Frontend:** Available at http://localhost:3000 (or the port specified in docker-compose.yml).
- **Backend API:** Available at http://localhost:5000 (or the port specified in docker-compose.yml).
- **Backend API Swagger** will be available at http://localhost:5000/swagger

## Stopping the Containers
To stop the containers, use the following command:
```bash
docker-compose down
```
This will stop and remove all running containers.

## Troubleshooting
1. **Database Initialization Timeout:**
 - If the SQL Server database takes too long to initialize, you may need to increase the startup timeout in the docker-compose.yml file.
2. **Ports Already in Use:**
 - If you encounter errors related to ports already being in use, ensure no other services are running on those ports, or modify the ports in the docker-compose.yml file.

# Run locally 
## Prerequisites
1. **SQL Server**
2. **Redis**
   
## Install Web Api project

#### Clone the project

```bash
  git clone https://github.com/PassyTim/library.git
```

#### Go to the project directory

```bash
  cd library
```

#### Make appsettings.json file and put it into Library.Api directory
```json
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
  },
  "ConnectionStrings": {
    "Redis": "YOUR_CONNECTION_STRING_TO_REDIS",
    "DefaultSQLConnection" : "YOUR_CONNECTION_STRING_TO_SQL_SERVER"
  }
}
```

> **_NOTE:_** Make sure you have .NET 8 installed ([Install .NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0))

```bash
  dotnet --version
```

#### Install dependencies

```bash
  dotnet restore
  dotnet build
```
#### Run Api project

```bash
  dotnet run
```

## Install Frontend project
#### Clone the project

```bash
  git clone https://github.com/PassyTim/library_frontend.git
```

#### Go to the project directory

```bash
  cd library_frontend
```

> **_NOTE:_** Make sure you have npm and node.js installed  ( [Install](https://nodejs.org/) )

```bash
  node -v
  npm -v
```

#### Install dependencies

```bash
  npm install
```
Run react App

```bash
  npm start
```
App will run on 3000 port

## Accessing the application
- **Frontend:** Available at http://localhost:3000/
- **Backend:** Available at https://localhost:7212/
- **Swagger:** Available at https://localhost:7212/swagger
