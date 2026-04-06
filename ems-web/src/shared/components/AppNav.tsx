"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { useOrganizationContext } from "@/providers/OrganizationProvider";
import { cn } from "@/shared/utils/cn";

const mainLinks = [
  { href: "/", label: "Home" },
  { href: "/employees", label: "Employees" },
  { href: "/departments", label: "Departments" },
  { href: "/positions", label: "Positions" },
] as const;

function isActive(pathname: string, href: string): boolean {
  if (href === "/") return pathname === "/";
  return pathname === href || pathname.startsWith(`${href}/`);
}

export function AppNav() {
  const pathname = usePathname();
  const { needsSetup, currentOrganization } = useOrganizationContext();

  if (needsSetup) {
    return (
      <header className="border-b border-zinc-200 bg-white dark:border-zinc-800 dark:bg-zinc-950">
        <nav
          className="mx-auto flex max-w-6xl flex-wrap items-center gap-1 px-4 py-3 sm:gap-2 sm:px-6"
          aria-label="Main"
        >
          <Link
            href="/"
            className={cn(
              "rounded-lg px-3 py-2 text-sm font-medium transition-colors",
              pathname === "/"
                ? "bg-zinc-100 text-zinc-900 dark:bg-zinc-800 dark:text-zinc-50"
                : "text-zinc-600 hover:bg-zinc-50 hover:text-zinc-900 dark:text-zinc-400 dark:hover:bg-zinc-900 dark:hover:text-zinc-100",
            )}
          >
            Home
          </Link>
          <Link
            href="/setup"
            className={cn(
              "rounded-lg px-3 py-2 text-sm font-medium transition-colors",
              pathname === "/setup"
                ? "bg-zinc-100 text-zinc-900 dark:bg-zinc-800 dark:text-zinc-50"
                : "text-zinc-600 hover:bg-zinc-50 hover:text-zinc-900 dark:text-zinc-400 dark:hover:bg-zinc-900 dark:hover:text-zinc-100",
            )}
          >
            Create organization
          </Link>
        </nav>
      </header>
    );
  }

  return (
    <header className="border-b border-zinc-200 bg-white dark:border-zinc-800 dark:bg-zinc-950">
      <nav
        className="mx-auto flex max-w-6xl flex-wrap items-center gap-1 px-4 py-3 sm:gap-2 sm:px-6"
        aria-label="Main"
      >
        {currentOrganization ? (
          <span className="mr-2 hidden text-xs text-zinc-500 dark:text-zinc-400 sm:inline-block">
            {currentOrganization.name}
          </span>
        ) : null}
        {mainLinks.map(({ href, label }) => {
          const active = isActive(pathname, href);
          return (
            <Link
              key={href}
              href={href}
              className={cn(
                "rounded-lg px-3 py-2 text-sm font-medium transition-colors",
                active
                  ? "bg-zinc-100 text-zinc-900 dark:bg-zinc-800 dark:text-zinc-50"
                  : "text-zinc-600 hover:bg-zinc-50 hover:text-zinc-900 dark:text-zinc-400 dark:hover:bg-zinc-900 dark:hover:text-zinc-100",
              )}
            >
              {label}
            </Link>
          );
        })}
        <Link
          href="/organization/setup"
          className={cn(
            "rounded-lg px-3 py-2 text-sm font-medium transition-colors",
            pathname === "/organization/setup"
              ? "bg-zinc-100 text-zinc-900 dark:bg-zinc-800 dark:text-zinc-50"
              : "text-zinc-600 hover:bg-zinc-50 hover:text-zinc-900 dark:text-zinc-400 dark:hover:bg-zinc-900 dark:hover:text-zinc-100",
          )}
        >
          Organization
        </Link>
      </nav>
    </header>
  );
}
