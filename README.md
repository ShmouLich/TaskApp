# TaskApp - Task Management Application

Blazor Server aplikace pro management tasků.

Pro autentizaci využívá ASP.NET Core Identity a jako databázi SQLite.

## Funkce

- Autentizace uživatele (Login/Register)
- Task management s prioritou, statusem, checklisty, komentáři i soubory
- Filtrování tasků (podle přiřazeného řešitele, tvůrce, statusu, prošvihlých termínů)

## Zprovoznění

Před spuštěním je potřeba mít:

- [.NET 9 SDK]

### 1. Clone repozitáře

```bash
git clone git@github.com:ShmouLich/TaskApp.git
cd TaskApp
```

### 2. spuštění db (migrace a seedování)

```bash
cd TaskApp
dotnet restore
dotnet ef database update
```

### 3. spuštění aplikace

```bash
dotnet run
```

Aplikace bude dostupná na adrese `http://localhost:5150`.

## Přihlášení

Pro některou funkcionalitu která zaznamenává autora (např. přidání komentáře) je potřeba být přihlášený. 

Před přihlášením je potřeba zaregistrovat nového uživatele.


## Použité technologie

- **ASP.NET Core 9** - Web framework
- **Blazor Server** - UI framework
- **Entity Framework Core** - ORM
- **ASP.NET Core Identity** - Autentizace
- **SQLite** - databáze
- **Bootstrap** - CSS framework
- **Bogus** - seedování
