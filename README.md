# JobTrackr

JobTrackr is a self-hosted tool for tracking job applications. It provides a Kanban board for moving applications through their stages, along with resume storage, contacts, follow-up reminders, and a small analytics view. It runs as a single Docker image and connects to a PostgreSQL database that you provide.

## Features

- Kanban board with drag-and-drop between status columns
- Create, edit, and delete applications, including salary, work mode, source, and tags
- Search applications by company or role
- Analytics view: pipeline breakdown, interview rate, and grouping by source and work mode
- Application detail pages with follow-up reminders and links to a resume and a contact
- Reminders page grouped into overdue and upcoming
- Resume upload (PDF/DOC/DOCX) with inline PDF preview, download, and delete
- Contacts management
- Email and password authentication (ASP.NET Core Identity)
- First-run setup wizard for database configuration and admin account creation
- Dark mode and a responsive, installable (PWA) interface

## Tech stack

| Layer | Technology |
|---|---|
| API | .NET 10, ASP.NET Core, C# |
| Data | Entity Framework Core, PostgreSQL (Npgsql) |
| Authentication | ASP.NET Core Identity (bearer tokens) |
| Web UI | React, Vite, Tailwind CSS, React Router, axios |
| Drag and drop | dnd-kit |
| Packaging | Docker (multi-stage build) |

## Architecture

The repository contains two projects that are packaged into a single container:

```
job-trackr-api/     .NET Web API; also serves the built web UI
job-trackr-webui/   React application (Vite)
Dockerfile          builds the UI, publishes the API, serves both
```

The Docker build compiles the web UI and copies it into the API's `wwwroot`. The API serves both the static UI and the API endpoints from the same origin.

PostgreSQL runs separately. The database connection is supplied through the setup wizard, which writes the connection string to `setup.json` on a mounted volume. Uploaded resumes are stored on disk under the same volume; the database stores only their relative paths.

## Getting started (Docker)

Requires Docker and a reachable PostgreSQL server.

```bash
docker build -t jobtrackr .
docker run -d -p 8080:8080 -v jobtrackr-data:/data --name jobtrackr jobtrackr
```

Open http://localhost:8080. On first run, the setup wizard will:

1. Ask for the Postgres host, port, database, username, and password.
2. Create the admin account.
3. Apply migrations, save the connection, and return to the login screen.

Keep the `/data` volume mounted; it stores the saved connection and uploaded resumes.

### docker-compose

```yaml
services:
  jobtrackr:
    image: jobtrackr        # or: build: .
    ports:
      - "8080:8080"
    volumes:
      - jobtrackr-data:/data
    restart: unless-stopped

volumes:
  jobtrackr-data:
```

## Local development

Run the API and the Vite dev server separately.

API (`job-trackr-api/`):

```bash
dotnet run
```

Without a configured connection the API starts in setup mode. To skip the wizard during development, set a connection string with user-secrets:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=jobtrackr;Username=postgres;Password=postgres"
```

Web UI (`job-trackr-webui/`):

```bash
npm install
npm run dev
```

The dev server runs on http://localhost:5173 and reads the API URL from `VITE_API_URL` (see `.env.development`).

## Configuration

Configured through environment variables; the Docker image sets defaults.

| Variable | Default | Purpose |
|---|---|---|
| `ASPNETCORE_URLS` | `http://+:8080` | Listen address |
| `Setup__ConfigPath` | `/data/setup.json` | Location of the saved database connection |
| `ResumeStorage__BasePath` | `/data/resumes` | Location of uploaded resumes |
| `VITE_API_URL` | empty (same origin) | API base URL for the web UI (development) |

## Resetting the database connection

To clear the saved connection and re-run the setup wizard (the database itself is not affected):

```bash
docker exec jobtrackr rm -f /data/setup.json
docker restart jobtrackr
```

## Donations

If you find JobTrackr useful, you can support development through PayPal at [paypal.me/cachecc](https://paypal.me/cachecc).

## License

MIT. See [LICENSE](LICENSE).
