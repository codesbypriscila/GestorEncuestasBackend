# Survey Manager · Backend

REST API for a survey platform: users sign up and answer surveys, and administrators create surveys, see statistics and export reports.

This is the backend. The React frontend lives in [GestorEncuestasFrontend](https://github.com/codesbypriscila/GestorEncuestasFrontend).

## What it does

- **Auth**: register and log in. Login returns a **JWT** token, and protected endpoints check the user's role (administrator or user).
- **Surveys**: create, update, delete and list surveys, with a description, visibility, and creation and expiry dates.
- **Questions**: add questions to a survey and list or delete them.
- **Answers**: users answer the questions of a survey. Each new answer sends an email notification to the administrator.
- **Statistics**: results per survey, ready to be shown in charts.
- **Reports**: export a survey's results as **PDF** or **Excel**. The format is picked with a small factory (`ReporteFactory`).

## Tech stack

- **C# / .NET 8** with **ASP.NET Core Web API**
- **Entity Framework Core** + **SQL Server**
- **JWT** authentication
- **Swagger** to explore and test the API
- **iText 7** (PDF) and **EPPlus** (Excel) for reports
- **MailKit** for email notifications

## Main endpoints

| Route | Purpose |
| --- | --- |
| `api/User` | Register and log in |
| `api/encuestas` | Surveys |
| `api/preguntas` | Questions |
| `api/respuestas` | Answers |
| `api/reportes` | Statistics and PDF / Excel reports |

## Project structure

```
API.Entities      Entities: Usuario, Encuesta, Pregunta, Respuesta, Reporte
API.Data          DbContext
API.Services      Services, DTOs, report generators and email
API.Presentation  Controllers and app setup (Program.cs)
GestorEncuestas.sql  Script to create the database
```

## Running it locally

1. Install the [.NET 8 SDK](https://dotnet.microsoft.com/download) and SQL Server.
2. Create the database by running `GestorEncuestas.sql`.
3. In `API.Presentation/appsettings.json`, set the connection string, the `Jwt` settings and the `EmailSettings`.
4. Run the API:
   ```bash
   dotnet run --project API.Presentation
   ```
5. Open `http://localhost:5260/swagger` to try the endpoints.

The API allows requests from `http://localhost:3000`, where the frontend runs.
