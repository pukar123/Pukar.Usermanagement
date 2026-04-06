"use client";

import Link from "next/link";
import { useOrganizationContext } from "@/providers/OrganizationProvider";

const linkClass =
  "inline-flex items-center rounded-lg border border-zinc-300 bg-white px-4 py-2 text-sm font-medium text-zinc-900 shadow-sm transition hover:border-zinc-400 hover:bg-zinc-50 dark:border-zinc-600 dark:bg-zinc-900 dark:text-zinc-50 dark:hover:border-zinc-500 dark:hover:bg-zinc-800";

export function OrganizationSetupLinks() {
  const { needsSetup, isLoading, isError, currentOrganization } = useOrganizationContext();

  if (isLoading || isError) {
    return null;
  }

  if (needsSetup) {
    return (
      <p className="mt-4">
        <Link href="/setup" className={linkClass}>
          Create organization
        </Link>
        <span className="ml-3 text-sm text-zinc-500 dark:text-zinc-400">Required before managing employees and related data.</span>
      </p>
    );
  }

  if (!currentOrganization) {
    return null;
  }

  return (
    <p className="mt-4">
      <Link href="/organization/setup" className={linkClass}>
        Update organization
      </Link>
      <span className="ml-3 text-sm text-zinc-500 dark:text-zinc-400">
        Current: <span className="font-medium text-zinc-700 dark:text-zinc-300">{currentOrganization.name}</span>
      </span>
    </p>
  );
}
