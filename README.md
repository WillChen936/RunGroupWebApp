# RunGroupWebApp

An ASP.NET Core MVC web application for runners — browse and manage running **clubs** and **races** by city and state, with user accounts, profiles, and image uploads.

> **About this project:** This is a learning project built by following Teddy Smith's [RunGroop](https://github.com/teddysmithdev/RunGroop) ASP.NET Core MVC tutorial. I built it to learn the .NET MVC stack, Entity Framework Core, and ASP.NET Identity.

## Features

- **Clubs & Races** — full CRUD for running clubs and race events, organized by location (city / state).
- **Authentication & authorization** — registration, login, and role-based access using ASP.NET Identity with cookie authentication.
- **User profiles** — view and edit profile details, including profile images.
- **Image uploads** — photos stored and served via Cloudinary.
- **Dashboard** — a per-user dashboard listing the clubs and races they own.
- **Location detection** — IP-based geo-location to suggest nearby content.
- **Seed data** — seed command to populate default users, roles, and sample records.

## Tech Stack

- **Framework**: ASP.NET Core MVC (.NET 8)
- **ORM / Database**: Entity Framework Core 9 with Microsoft SQL Server
- **Identity**: ASP.NET Core Identity (cookie-based auth)
- **Image hosting**: Cloudinary (`CloudinaryDotNet`)
- **Patterns**: Repository pattern with dependency injection, in-memory caching, session

## Project Structure

```
RunGroupWebApp/
├── Controllers/   # MVC controllers
├── Views/         # Razor views
├── Models/        # Domain models (Club, Race, AppUser, ...)
├── ViewModels/    # View models for forms and pages
├── Data/          # ApplicationDbContext + Seed
├── Repositories/  # Data-access layer behind interfaces
├── Services/      # e.g. photo upload service
├── Helpers/       # Cloudinary settings, utilities
└── Program.cs     # App startup / DI configuration
```

## Getting Started

### Prerequisites

- .NET 8 SDK
- Microsoft SQL Server (LocalDB is fine for development)
- A Cloudinary account (for image uploads)

### Setup

1. Configure your connection string and Cloudinary credentials in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=...;Database=RunGroupWebApp;..."
     },
     "CloudinarySettings": {
       "CloudName": "...",
       "ApiKey": "...",
       "ApiSecret": "..."
     }
   }
   ```
2. Apply the database migrations:
   ```bash
   dotnet ef database update
   ```
3. (Optional) Seed default users, roles, and sample data:
   ```bash
   dotnet run seeddata
   ```
4. Run the app:
   ```bash
   dotnet run
   ```

## Acknowledgements

Tutorial and reference implementation by [Teddy Smith](https://github.com/teddysmithdev/RunGroop).
