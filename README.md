# Todo API

## Description

This is a simple Todo List application built using ASP.NET Core, Entity Framework, and MySQL.

## Features

- Create, Read, Update, and Delete Todo Items.
- Each Todo item contains a Title and Description.

## How to Run

1. Clone the repository.
2. Add your MySQL connection string in `appsettings.json`.
3. Run the migrations to create the database.
4. Start the API using:
    ```bash
    dotnet run
    ```
5. Use Postman or another API client to test the endpoints.

## Tests

Unit tests are located in the `TodoApi.Tests` folder. Run them using:
```bash
dotnet test

Was it easy to complete the task using AI?
Yes, it was straightforward with detailed prompts for each step. The guidance helped structure the API properly.

How long did task take you to complete?
Approximately 2 hours.

Was the code ready to run after generation? What did you have to change to make it usable?
The code was mostly ready, but I had to change the connection string and switch the provider to SQL Server (SSMS) for convenience and better compatibility with my workflow.

Which challenges did you face during completion of the task?
Configuring EF Core for SQL Server and ensuring the connection string was correct for the SSMS setup.

Which specific prompts you learned as a good practice to complete the task?
"Provide step-by-step implementation with explanation and code snippets."
"Generate fully runnable code with configurations."