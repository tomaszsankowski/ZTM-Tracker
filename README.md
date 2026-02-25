# ZTM Tracker

A full-stack web application for browsing **Gdańsk public transport (ZTM)** real-time data. Registered users can save their favourite bus/tram stops and see live estimated arrival times, delays, and vehicle positions on an interactive map. Built with **Vue 3 + Vite** on the frontend and **ASP.NET Core 8 Web API** with Clean Architecture on the backend.

<img width="1919" height="1031" alt="image" src="https://github.com/user-attachments/assets/c2a876b5-6f11-430e-8d21-85f5a650349e" />

## Features

| Feature                    | Description                                                                                              |
| :------------------------- | :------------------------------------------------------------------------------------------------------- |
| **User Authentication**    | Registration & login with bcrypt-hashed passwords and JWT Bearer tokens (access + refresh).              |
| **Personalised Stop List** | Full CRUD — add, view, rename, and remove favourite ZTM stops per user.                                  |
| **Live Departures**        | Real-time arrival estimates, delays, and route info fetched from the public ZTM API for each saved stop. |
| **Stop Search**            | Search all ZTM stops by name, description, or ID (backed by a cached `stops.json`).                      |
| **Interactive Map**        | Leaflet/OpenStreetMap visualisation of stop locations with popups showing live departures.               |
| **Delay Highlighting**     | Custom Vue directive (`v-delay-color`) that colours departure rows based on delay severity.              |
| **Swagger Docs**           | Full OpenAPI documentation with JWT authorisation support built-in.                                      |
| **Caching**                | The heavy `stops.json` file from ZTM is cached in-memory with a 24-hour expiration.                      |

## Tech Stack

### Backend

- **Framework:** ASP.NET Core 8 Web API (.NET 8)
- **Architecture:** Clean Architecture (Domain → Application → Infrastructure → API)
- **ORM:** Entity Framework Core 8 with SQLite
- **Auth:** JWT Bearer tokens (access + refresh) with bcrypt password hashing
- **CQRS:** MediatR for command/query separation
- **Caching:** `IMemoryCache` for ZTM stops data
- **API Docs:** Swashbuckle / Swagger with Bearer token support
- **Value Objects:** `Login`, `PasswordHash`, `RefreshToken`, `StopId`

### Frontend

- **Framework:** Vue 3 (Composition API, `<script setup>`)
- **Build Tool:** Vite
- **State Management:** Pinia
- **Routing:** Vue Router 4
- **HTTP Client:** Axios (with JWT interceptors)
- **Styling:** Tailwind CSS 4
- **Map:** Leaflet + vue-leaflet
- **Table:** vue-good-table-next
- **Date Formatting:** date-fns
- **Custom Directive:** `v-delay-color`
- **Custom Composable:** `useZtmData`
- **Custom Plugin:** `datePlugin`
- **Unit & Component Tests:** Vitest + Vue Test Utils
- **E2E Tests:** Nightwatch

## Project Structure

```
VueZtm/
├── VueZtmBackend/                          # ASP.NET Core 8 solution
│   ├── VueZtmBackend.Api/                  # HTTP layer (controllers, Program.cs)
│   │   ├── Controllers/
│   │   │   ├── AuthController.cs           # Register / Login / Refresh token
│   │   │   ├── UserStopsController.cs      # CRUD for user's saved stops
│   │   │   └── ZtmController.cs            # Proxy to ZTM public API (stops, delays)
│   │   └── Program.cs                      # App bootstrap & middleware pipeline
│   ├── VueZtmBackend.Application/          # Use-cases (CQRS with MediatR)
│   │   ├── DependencyInjection.cs          # MediatR DI registration
│   │   ├── Auth/
│   │   │   ├── Commands/                   # RegisterCommand, LoginCommand, RefreshTokenCommand
│   │   │   └── Handlers/                   # Corresponding handlers
│   │   ├── UserStops/
│   │   │   ├── Commands/                   # Create / Update / Delete stop commands
│   │   │   ├── Handlers/                   # Command & query handlers
│   │   │   └── Queries/                    # GetUserStops, GetUserStopsWithDelays
│   │   └── Common/
│   │       ├── Interfaces/                 # IJwtService, IPasswordHasher, IZtmApiService
│   │       └── Models/                     # ZtmStopDto, ZtmDelayDto
│   ├── VueZtmBackend.Domain/              # Domain entities & logic
│   │   ├── Entities/                       # User, UserStop
│   │   ├── ValueObjects/                   # Login, PasswordHash, RefreshToken, StopId
│   │   ├── Exceptions/                     # DomainException
│   │   └── Interfaces/                     # IUserRepository, IUserStopRepository
│   └── VueZtmBackend.Infrastructure/      # External concerns
│       ├── DependencyInjection.cs          # Infrastructure DI registration
│       ├── Persistence/
│       │   ├── AppDbContext.cs             # EF Core DbContext (SQLite)
│       │   ├── Configurations/             # UserConfiguration, UserStopConfiguration
│       │   └── Repositories/               # UserRepository, UserStopRepository
│       └── Services/                       # BcryptPasswordHasher, JwtService, ZtmApiService
│
└── VueZtmFrontend/                         # Vue 3 + Vite SPA
    ├── src/
    │   ├── api/
    │   │   └── axios.ts                    # Axios instance with JWT interceptors
    │   ├── assets/
    │   │   └── tailwind.css                # Tailwind CSS entry point
    │   ├── components/
    │   │   ├── __tests__/                  # Vitest component tests
    │   │   │   ├── DelaysTable.spec.ts
    │   │   │   └── NotificationToast.spec.ts
    │   │   ├── DelaysTable.vue             # Live departures table (multi-component)
    │   │   ├── NotificationToast.vue       # Global notification toast
    │   │   └── StopModal.vue               # Add/edit stop modal dialog
    │   ├── composables/
    │   │   ├── __tests__/
    │   │   │   └── useZtmData.spec.ts      # Vitest composable unit test
    │   │   └── useZtmData.ts               # Reusable composable for fetching ZTM data
    │   ├── directives/
    │   │   └── delayColor.ts               # v-delay-color custom directive
    │   ├── plugins/
    │   │   └── datePlugin.ts               # Custom Vue plugin for date formatting
    │   ├── router/
    │   │   └── index.ts                    # Route definitions & auth guards
    │   ├── stores/
    │   │   ├── auth.ts                     # Pinia store – JWT token & user state
    │   │   ├── notification.ts             # Pinia store – toast notifications
    │   │   └── stops.ts                    # Pinia store – user stops CRUD
    │   ├── views/
    │   │   ├── HomeView.vue                # Dashboard with saved stops & live data
    │   │   ├── AllStopsView.vue            # Searchable list of all ZTM stops
    │   │   ├── MapView.vue                 # Leaflet map with stops & departures
    │   │   ├── LoginView.vue               # Login form
    │   │   └── RegisterView.vue            # Registration form
    │   ├── App.vue                         # Root component with navbar
    │   └── main.ts                         # App entry point
    └── tests/
        └── e2e/
            └── login.test.js               # Nightwatch E2E test
```

## Prerequisites

- **.NET SDK** 8.0+
- **Node.js** 20.19+ or 22.12+
- **npm** (included with Node.js)

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/tomaszsankowski/ZTM-Tracker.git
cd ZTM-Tracker
```

### 2. Run the backend

```bash
cd VueZtmBackend
dotnet restore
dotnet run --project VueZtmBackend.Api
```

The API starts at `https://localhost:7078` (or the URL shown in the console).
Swagger UI is available at `https://localhost:7078/swagger`.

### 3. Run the frontend

```bash
cd VueZtmFrontend
npm install
npm run dev
```

The dev server starts at `http://localhost:5173`.

### 4. Use the app

1. Open `http://localhost:5173` in your browser.
2. Register a new account or log in.
3. Add your favourite ZTM stops and see live departure times.
4. Browse all stops or explore them on the interactive map.

## API Endpoints

### Auth

| Method | Endpoint             | Auth | Description                     |
| :----- | :------------------- | :--- | :------------------------------ |
| POST   | `/api/Auth/register` | —    | Create a new user account       |
| POST   | `/api/Auth/login`    | —    | Log in and receive JWT tokens   |
| POST   | `/api/Auth/refresh`  | —    | Refresh an expired access token |

### User Stops

| Method | Endpoint                     | Auth   | Description                       |
| :----- | :--------------------------- | :----- | :-------------------------------- |
| GET    | `/api/UserStops`             | Bearer | List saved stops                  |
| GET    | `/api/UserStops/with-delays` | Bearer | Saved stops + live ZTM delay data |
| POST   | `/api/UserStops`             | Bearer | Add a stop                        |
| PUT    | `/api/UserStops/{id}`        | Bearer | Update a stop                     |
| DELETE | `/api/UserStops/{id}`        | Bearer | Remove a stop                     |

### ZTM Data

| Method | Endpoint                       | Auth | Description                     |
| :----- | :----------------------------- | :--- | :------------------------------ |
| GET    | `/api/Ztm/stops`               | —    | All ZTM stops (cached)          |
| GET    | `/api/Ztm/stops/search?query=` | —    | Search stops by name/ID         |
| GET    | `/api/Ztm/delays/{stopId}`     | —    | Live delays for a specific stop |

## Running Tests

### Frontend unit & component tests

```bash
cd VueZtmFrontend
npm run test:unit
```

### Frontend E2E tests

```bash
cd VueZtmFrontend
npm run test:e2e
```

## External Data Sources

This application integrates with the **Gdańsk ZTM Open Data API**:

- **Stops list:** [`stops.json`](https://ckan.multimediagdansk.pl/dataset/c24aa637-3619-4dc2-a171-a23eec8f2172/resource/4c4025f0-01bf-41f7-a39f-d156d201b82b/download/stops.json)
- **Delays:** `http://ckan2.multimediagdansk.pl/delays?stopId={stopId}`
