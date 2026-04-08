# Pukar.Usermanagement: standalone repo and submodule

The branch `pukar-um-standalone` contains only the history of the `Pukar.Usermanagement/` folder (created with `git subtree split -P Pukar.Usermanagement -b pukar-um-standalone`).

## 1. Create the GitHub repository

Create an **empty** repository named e.g. `Pukar.Usermanagement` (no README/license if you want a clean first push).

## 2. Push the standalone branch

From this repository root (`EmployeeManagementSystem`):

```powershell
.\scripts\Publish-PukarStandaloneRepo.ps1 -RemoteUrl 'https://github.com/YOUR_USER/Pukar.Usermanagement.git'
```

## 3. Replace the in-tree folder with a submodule

**Only after step 2 succeeds:**

```powershell
.\scripts\Complete-PukarSubmodule.ps1 -RemoteUrl 'https://github.com/YOUR_USER/Pukar.Usermanagement.git'
```

## 4. Clone EMS elsewhere

```bash
git clone --recurse-submodules https://github.com/YOUR_USER/EmployeeManagementSystem.git
```

Existing clones:

```bash
git submodule update --init --recursive
```

## CI

Configure your pipeline to fetch submodules (e.g. `actions/checkout` with `submodules: recursive` on GitHub Actions).
