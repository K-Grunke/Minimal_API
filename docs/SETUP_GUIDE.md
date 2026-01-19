# 🚀 Wymagania i konfiguracja - Lab03 Minimal API

**Uwaga: To projekt edukacyjny! Nie używaj w produkcji bez dodatkowych zabezpieczeń.** 🔒

## 📋 Wymagania systemowe

### Podstawowe
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (lub nowszy)
- [Git](https://git-scm.com/)
- IDE: [Visual Studio 2022](https://visualstudio.microsoft.com/) lub [VS Code](https://code.visualstudio.com/)

### Baza danych
1. **SQL Server 2022** (zalecane)
   - [SQL Server Express](https://www.microsoft.com/pl-pl/sql-server/sql-server-downloads)
   - Lub użyj Docker (patrz poniżej)

2. **Docker** (łatwiejsze)
   - [Docker Desktop](https://www.docker.com/products/docker-desktop)
   - Uruchom: `docker-compose up -d`

## 🛠️ Konfiguracja środowiska

### 1. Zmienne środowiskowe (opcjonalnie, ale zalecane)

    ```
    # Windows (PowerShell)
    $env:JWT_KEY="twoj_bardzo_tajny_klucz_min_32_znaki_1234567890"
    $env:ASPNETCORE_ENVIRONMENT="Development"

    # Linux/macOS
    export JWT_KEY="twoj_bardzo_tajny_klucz_min_32_znaki_1234567890"
    export ASPNETCORE_ENVIRONMENT="Development"
    ```

### 2. Konfiguracja bazy danych

Edytuj src/Api/appsettings.Development.json:

    ```
    {
        "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=Lab03DB;Trusted_Connection=True;TrustServerCertificate=True;"
        }
    }
    ```

### 3. Generowanie klucza JWT (jeśli nie ustawiłeś zmiennej)

    ```
    # PowerShell
    [System.Text.Encoding]::UTF8.GetString([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(64))
    # Bash
    openssl rand -base64 48
    ```

## 📦 Zależności NuGet

Projekt używa następujących pakietów (zdefiniowane w .csproj):

- Microsoft.EntityFrameworkCore.SqlServer - baza danych
- Microsoft.AspNetCore.Authentication.JwtBearer - JWT
- AutoMapper - mapowanie obiektów
- Serilog.AspNetCore - logowanie
- Swashbuckle.AspNetCore - Swagger/OpenAPI

## 🐳 Uruchomienie z Dockerem

**Opcja 1: Tylko baza danych**

    ```
    # Uruchom SQL Server
    docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourPassword123!" \
    -p 1433:1433 --name sqlserver \
    -d mcr.microsoft.com/mssql/server:2022-latest

    # Uruchom aplikację
    cd src/Api
    dotnet run
    ```

**Opcja 2: Całość w Dockerze**

    ```
    cd docker
    docker-compose up -d
    ```

## 🔧 Polecenia dotnet

**Podstawowe**

    ```
    dotnet restore          # Przywróć pakiety
    dotnet build           # Skompiluj
    dotnet run            # Uruchom
    dotnet watch run      # Uruchom z hot-reload
    ```

**Entity Framework**

    ```
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    dotnet ef migrations remove
    ```

**Testy**

    ```
    dotnet test           # Uruchom testy
    dotnet test --logger "console;verbosity=detailed"
    ```
        
## 🐛 Rozwiązywanie problemów

**"Cannot connect to SQL Server"**

1. Sprawdź czy SQL Server działa
2. Sprawdź connection string
3. Uruchom jako administrator (Windows)

**"JWT not working"**

1. Sprawdź czy JWT_KEY jest ustawiony
2. Klucz musi mieć min. 32 znaki
3. Restart aplikacji po zmianie zmiennych

**"Migration errors"**

    ```
    dotnet ef database drop --force
    dotnet ef migrations remove
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

## 📚 Przydatne linki

.NET 9 Documentation - https://learn.microsoft.com/pl-pl/dotnet/  
Entity Framework Core - https://learn.microsoft.com/pl-pl/ef/core/  
Minimal APIs Guide - https://learn.microsoft.com/pl-pl/aspnet/core/tutorials/min-web-api?view=aspnetcore-10.0&tabs=visual-studio  
JWT in .NET - https://learn.microsoft.com/en-us/aspnet/core/security/authentication/configure-jwt-bearer-authentication?view=aspnetcore-10.0  

**Uwaga: To projekt edukacyjny! Nie używaj w produkcji bez dodatkowych zabezpieczeń.** 🔒