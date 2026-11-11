# ---- Stage 1: build ----
# The SDK image has the compiler and tools. It's big, so we only use it to build.
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy only the project file first, then restore packages.
# Docker caches this layer, so rebuilds are fast unless dependencies change.
COPY MyApi/MyApi.csproj MyApi/
RUN dotnet restore MyApi/MyApi.csproj

# Now copy the rest of the API source and publish a release build.
COPY MyApi/ MyApi/
RUN dotnet publish MyApi/MyApi.csproj -c Release -o /app --no-restore

# ---- Stage 2: run ----
# The runtime image is much smaller: just what's needed to run the app.
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .

ARG GIT_SHA=dev
ENV GIT_SHA=$GIT_SHA

# Run as a non-root user (safer) and listen on port 8080.
USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["dotnet", "MyApi.dll"]