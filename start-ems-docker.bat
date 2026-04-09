@echo off
setlocal EnableDelayedExpansion
cd /d "%~dp0"

echo ============================================
echo  EMS - Start Docker stack
echo ============================================
echo  Starts: SQL Server, MongoDB, Redis,
echo           Mongo Express, Next.js (ems-web)
echo.
echo  After this, start the API on the host:
echo    dotnet run --project EMS.API --launch-profile http
echo.
echo  Then open: http://localhost:3000
echo ============================================
echo.

docker compose up -d --build
if errorlevel 1 (
  echo.
  echo ERROR: docker compose failed. Is Docker Desktop running?
  exit /b 1
)

echo.
echo Done. Containers are up.
echo   Web UI:     http://localhost:3000
echo   Mongo UI:   http://localhost:8081
echo   SQL Server: localhost,1433  (sa / see docker-compose.yml)
echo.
pause
