# EMS Web (Next.js)

Browser client for **EMS.API**: feature-based folders, **React Query**, **Zustand**, **Tailwind**, and Axios.

**Full frontend documentation:** **[docs/FRONTEND.md](docs/FRONTEND.md)** (architecture, folders, env, Docker, API alignment, troubleshooting).

---

## Quick start

**Prerequisites:** Node.js **20.9+**, EMS.API running with CORS for `http://localhost:3000`.

```bash
cd ems-web
npm install
cp .env.example .env.local
```

Edit `.env.local`:

| Variable | Purpose |
|----------|---------|
| `NEXT_PUBLIC_API_BASE_URL` | API origin, no trailing slash (e.g. `http://localhost:5246`) |
| `NEXT_PUBLIC_DEFAULT_ORGANIZATION_ID` | Default org when creating employees |

```bash
npm run dev
```

Open **http://localhost:3000** → **Open employees**.

---

## Scripts

| Command | Description |
|---------|-------------|
| `npm run dev` | Development server (Turbopack) |
| `npm run build` | Production build |
| `npm run start` | Serve production build |
| `npm run lint` | ESLint |

---

## Docker

From the **solution root** (parent folder): `docker compose up -d --build`, or **`start-ems-docker.bat`** / **`stop-ems-docker.bat`**. Details: [docs/FRONTEND.md §7](docs/FRONTEND.md#7-docker-production-style-image).

---

## Related

| Doc | |
|-----|---|
| Frontend (detailed) | [docs/FRONTEND.md](docs/FRONTEND.md) |
| Backend & Compose | [../README.md](../README.md) |
