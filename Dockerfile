# syntax=docker/dockerfile:1

# ---- Stage 1: build the React web UI ----
FROM node:22-alpine AS webui
WORKDIR /webui
COPY job-trackr-webui/package*.json ./
RUN npm ci
COPY job-trackr-webui/ ./
# Production build uses relative API URLs (.env.development is ignored by `vite build`).
RUN npm run build

# ---- Stage 2: build & publish the .NET API ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS api
WORKDIR /src
COPY job-trackr-api/*.csproj ./job-trackr-api/
RUN dotnet restore job-trackr-api/CachesJobTrackerApi.csproj
COPY job-trackr-api/ ./job-trackr-api/
RUN dotnet publish job-trackr-api/CachesJobTrackerApi.csproj -c Release -o /app/publish

# ---- Stage 3: runtime (one image serving API + SPA) ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=api /app/publish ./
# React build output -> wwwroot, served by ASP.NET static files.
COPY --from=webui /webui/dist ./wwwroot

# Run HTTP only inside the container; persist setup config + resume files on a mounted volume.
ENV ASPNETCORE_URLS=http://+:8080
ENV Setup__ConfigPath=/data/setup.json
ENV ResumeStorage__BasePath=/data/resumes

EXPOSE 8080
ENTRYPOINT ["dotnet", "CachesJobTrackerApi.dll"]
