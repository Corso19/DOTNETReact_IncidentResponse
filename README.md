# Incident Response XDR Application

This project is an **Incident Detection & Response XDR (Extended Detection and Response) Platform** designed for academic, demo, and practical cybersecurity use cases. It features an ASP.NET Core backend with a React frontend, supporting event ingestion from Microsoft 365 services and providing real-time detection, recommendations, and observability.

## Features

- **Sensor Management:** Add, update, enable/disable, and monitor Microsoft 365 sensors (Email, Teams, SharePoint)
- **Event Ingestion:** Collect events (emails, messages) and process them for suspicious or anomalous activity
- **Incident Detection:** Detect security incidents (e.g., unusual email volume, suspicious attachments) and link them to events
- **Recommendations:** Automatically generate actionable security recommendations per incident
- **Attachments:** Store and serve attachments related to security events
- **Orchestrator:** Background job system for scheduling and automating event ingestion from enabled sensors
- **Metrics API:** Prometheus metrics endpoint for security monitoring and performance
- **Real-Time Updates:** Uses SignalR for instant notification of new incidents
- **Swagger UI:** API documentation and interactive testing
- **React Frontend:** User-friendly web dashboard for managing incidents, sensors, events, and recommendations

## Technology Stack

- **Backend:** ASP.NET Core 8.0, Entity Framework Core, SignalR, Quartz, Prometheus-net, Microsoft Graph SDK, Swashbuckle (Swagger)
- **Frontend:** React 18, Axios, Material UI (custom components)
- **Database:** Microsoft SQL Server / Azure SQL
- **Authentication:** Microsoft Identity (Graph Auth for sensors)
- **Containerization (optional):** Docker support available

## Project Structure

```
corso19-dotnetreact_incidentresponse/
├── IncidentResponseAPI/
│   ├── Controllers/
│   ├── Models/
│   ├── DTOs/
│   ├── Services/
│   ├── Orchestrators/
│   ├── Migrations/
│   ├── Helpers/
│   ├── Program.cs
│   └── appsettings.json
└── IncidentResponseFrontend/
    └── my-app/
        ├── src/
        │   ├── components/
        │   ├── incidents/
        │   ├── sensors/
        │   └── services/
        └── public/
```

## Setup & Running Locally

### Backend (ASP.NET Core)

1. **Set up SQL Server:**  
   Update the connection string in `appsettings.json` or set `DefaultConnection` as an environment variable.

2. **Database Migration:**
   ```bash
   dotnet ef database update --project IncidentResponseAPI/IncidentResponseAPI
   ```

3. **Run API:**
   ```bash
   cd IncidentResponseAPI/IncidentResponseAPI
   dotnet run
   ```

   - The API is available at `https://localhost:5175` (by default).
   - Swagger UI: `/swagger`
   - Prometheus metrics: `/metrics`
   - SignalR Hub: `/incidentHub`

### Frontend (React)

1. **Install dependencies:**
   ```bash
   cd IncidentResponseFrontend/my-app
   npm install
   ```

2. **Start the React App:**
   ```bash
   npm start
   ```
   - Available at `http://localhost:3001` (ensure CORS origin matches API settings)

## Usage

- Add sensors for Microsoft 365 Email, Teams, or SharePoint in the web UI.
  <img width="1899" height="321" alt="image" src="https://github.com/user-attachments/assets/d2fc8c28-0b93-498d-a199-940b690f176e" />
- Start the orchestrator to fetch and analyze events.
- Detected incidents appear in real-time on the dashboard.
  <img width="1845" height="308" alt="image" src="https://github.com/user-attachments/assets/190b6389-b904-40e9-bf4f-d2239f403420" />
- Review associated events, attachments, and recommendations.
  <img width="1805" height="288" alt="image" src="https://github.com/user-attachments/assets/94e6eaa3-c58a-4169-8189-9e7b119193e7" />
- Access `/api/metrics` for Prometheus-compatible monitoring.

## Development Notes

- API contracts are documented in Swagger UI.
- Microsoft 365 sensors require `TenantId`, `ApplicationId`, and `ClientSecret`.
- Metrics endpoint is Prometheus-ready.
- Real-time updates require a running SignalR connection.

## Acknowledgements

- Inspired by modern XDR and SOAR platforms.
- Uses [Microsoft Graph](https://docs.microsoft.com/en-us/graph/) for integration.
