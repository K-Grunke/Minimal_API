# 🚀 Lab03 - Minimal API .NET 9.0

Hej! 👋 To mój projekt edukacyjny stworzony w ramach zajęć/laboratoriów. Jest to REST API zbudowane na .NET 9 Minimal API z pełną autentykacją, autoryzacją i zarządzaniem zadaniami.

## 📖 O projekcie

To backendowy system do zarządzania użytkownikami i zadaniami z:
- 🔐 **Autentykacja JWT** (logowanie/rejestracja)
- 👥 **Role-based authorization** (Admin/Manager/User)
- ✅ **CRUD** dla użytkowników, zadań i ról
- 📊 **Raporty** (podstawowe)
- 📝 **Automatyczna dokumentacja** (Swagger/OpenAPI)

## 🎯 Funkcjonalności

### 👤 Użytkownicy
- Rejestracja z walidacją
- Logowanie z JWT tokenem
- Profil użytkownika
- Przypisywanie ról (tylko Admin)

### 📝 Zadania
- Tworzenie zadań
- Przeglądanie swoich zadań
- Aktualizacja i usuwanie
- Filtrowanie i wyszukiwanie

### 🛡️ Role i uprawnienia
- **Admin** - pełny dostęp
- **Manager** - przeglądanie użytkowników i zadań
- **User** - tylko swoje dane

## 🚀 Szybki start

### Wymagania
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/pl-pl/sql-server/sql-server-downloads) (lokalnie lub Docker)
- [Git](https://git-scm.com/)

### 1. Klonowanie repozytorium
    ```
    git clone https://github.com/K-Grunke/Minimal_API.git
    cd Minimal_API
    cd Lab03_MinimalAPI
    ```
### 2. Konfiguracja
    ```
    # Edytuj connection string w appsettings.Development.json w razie potrzeby
    ```
### 3. Uruchomienie
    ```
    cd src/Api
    dotnet restore
    dotnet run
    ```
📡 API będzie dostępne pod lokalhostem

## 🔑 Domyślne konto

**Po uruchomieniu bazy danych:**

- Login: admin
- Hasło: Password123
- Rola: Admin

## 📚 Endpointy

**Publiczne:**
    
    ```
    POST /api/v1/users/register  #Rejestracja  
    POST /api/v1/users/login     #Logowanie (zwraca JWT)  
    GET  /api/v1/hello/{name}      # Testowy endpoint  
    GET  /api/v1/health            # Health check  
    ```

**Chronione (wymagają JWT):**

    ```
    GET    /api/v1/users           # Lista użytkowników
    GET    /api/v1/users/{id}      # Szczegóły użytkownika
    PUT    /api/v1/users/{id}      # Aktualizacja użytkownika

    GET    /api/v1/tasks           # Lista zadań (z filtrami)
    POST   /api/v1/tasks           # Utworzenie zadania
    GET    /api/v1/tasks/{id}      # Szczegóły zadania
    PUT    /api/v1/tasks/{id}      # Aktualizacja zadania
    DELETE /api/v1/tasks/{id}      # Usunięcie zadania

    POST   /api/v1/roles           # Utworzenie roli (Admin only)
    GET    /api/v1/roles           # Lista ról
    POST   /api/v1/roles/assign    # Przypisanie roli (Admin only)
    ```

**UWAGA: ten podgląd może nie być aktualny - to będą endpointy po przyszłych rozbudowach i modyfikacjach**

## 🛠️ Technologie

- .NET 9.0 - Platforma
- Entity Framework Core - ORM
- JWT Bearer - Autentykacja
- AutoMapper - Mapowanie obiektów
- Serilog - Logowanie
- Swagger/OpenAPI - Dokumentacja
- SQL Server - Baza danych

## 📁 Struktura projektu

Lab03_MinimalAPI/  
│  
├── 📁 src/  
│   └── 📁 Api/  
│       ├── 📁 Domain/                    # Modele domenowe  
│       │  
│       ├── 📁 Endpoints/                 # Endpointy Minimal API  
│       │  
│       ├── 📁 Infrastructure/  
│       │   ├── 📁 Data/  
│       │   │  
│       │   ├── 📁 DTOs/                 # Data Transfer Objects  
│       │   │  
│       │   ├── 📁 Mapping/              # AutoMapper profile  
│       │   │  
│       │   ├── 📁 Middleware/  
│       │   │  
│       │   └── 📁 Services/             # (do rozbudowy) Serwisy biznesowe  
│       │  
│       ├── 📁 Properties/  
│       │  
│       └── 📄 Program.cs                # Główny plik Minimal API  
│  
├── 📁 tests/                            # Testy jednostkowe  
│  
├── 📁 docs/                             # Dokumentacja  
│  
└── 📁 scripts/                          # Skrypty pomocnicze  

## 🧪 Testowanie API

1. Użyj Swagger UI

Otwórz w przeglądarce lokal hosta

2. Użyj curl
    
    ```
    # Logowanie
    curl -X POST https://localhost:5001/api/v1/users/login \
    -H "Content-Type: application/json" \
    -d '{"username":"admin","password":"Password123"}'
    ```

    ```# Pobieranie zadań (z tokenem)
    curl -X GET https://localhost:5001/api/v1/tasks \
    -H "Authorization: Bearer TWÓJ_TOKEN_JWT"
    ```

## 🤝 Chcesz pomóc w rozwoju?

**Super!** 🎉

- Forkuj repozytorium
- Stwórz branch (git checkout -b feature/nowa-funkcjonalnosc)
- Commit changes (git commit -m 'Add some amazing feature')
- Push (git push origin feature/nowa-funkcjonalnosc)
- Otwórz Pull Request
- Masz pomysł na nową funkcjonalność? Napisz issue! 💡

## 📞 Kontakt & Dyskusja

Chcesz o coś zapytać? Podyskutować o kodzie? Masz sugestie?

- 📧 Email: konrad.grunke@gmail.com
- 💬 Linkedin: https://www.linkedin.com/in/konrad-grunke/
- 🐛 Issues w repo

## 🎓 Cel edukacyjny

Projekt powstał w celu nauki:

- Minimal API w .NET 9
- Autentykacji i autoryzacji JWT
- Entity Framework Core
- Clean Architecture
- Dokumentacji API

⭐ **Jeśli projekt Ci się podoba, daj gwiazdkę!** ⭐  
**Pamiętaj: To projekt studencki - może nie być perfekcyjny** ❤️