# KingAkademija2024

This project is a middleware REST API for fetching products from various sources (e.g., web services, databases, file systems, RSS feeds) with filtering capabilities. Currently, it supports fetching products from a REST API (https://dummyjson.com/products).

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [JetBrains Rider](https://www.jetbrains.com/rider/)
- [SQL Server or another database if needed for future data sources]

## Getting started

### Configuration and service setup

1. **Clone the repository**

   ```bash
   git clone https://github.com/Speeedyyyy/KingAkademija2024.git
   ```

2. **Configure the project**

    Open the project in Visual Studio 2022. Make sure the `appsettings.json` is configured properly. An example `appsettings.json` file:

    ```json
    {
        "Logging": {
            "LogLevel": {
                "Default": "Information",
                "Microsoft.AspNetCore": "Warning"
            }
        },
        "AllowedHosts": "*",
        "Jwt": {
            "Key": "your_secret_key_here_longer_than_32_characters"
        }
    }
    ```

3. **Install dependencies**

    Restore the necessary packages by building the solution in Visual Studio or by using the following command:

    ```bash
    dotnet restore
    ```

4. **Run the application**

    Start the application by pressing `F5` in Visual Studio or by using the following command:

    ```bash
    dotnet run
    ```

### Testing the application

You can use tools like Visual Studio, JetBrains Rider, Postman or SwaggerUI that opens upon running the application in development(default) enviroment to test the API endpoints.

#### .http file

`KingAkademija2024.http` file allows you to test the endpoints within Visual Studio/JetBrains Rider. If the application is started you can open the file and execute each endpoint one by one by pressing the green arrow next to the url. Additionaly you can edit the given filters to further test the application.

#### Swagger

Swagger is configured to provide interactive API documentation. Once the application is running, you can access the Swagger UI at: `https://localhost:44316/swagger/index.html`

## Future enhancements

- Add support for other data sources (e.g., databases, file systems, RSS feeds).
