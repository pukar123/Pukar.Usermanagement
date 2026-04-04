@echo off
setlocal
cd /d "%~dp0"

echo Stopping EMS Docker stack (containers removed; named volumes kept)...
docker compose down
if errorlevel 1 (
  echo ERROR: docker compose down failed.
  exit /b 1
)

echo Done.
pause
