"use client";

import { useEffect } from "react";
import { usePathname, useRouter } from "next/navigation";
import { getErrorMessage } from "@/shared/api/http-client";
import { Spinner } from "@/shared/components/Spinner";
import { useOrganizationContext } from "./OrganizationProvider";

const SETUP_PATH = "/setup";

export function OrganizationGate({ children }: { children: React.ReactNode }) {
  const pathname = usePathname();
  const router = useRouter();
  const { needsSetup, isLoading, isError, error } = useOrganizationContext();

  useEffect(() => {
    if (isLoading) return;

    if (needsSetup && pathname !== SETUP_PATH) {
      router.replace(SETUP_PATH);
      return;
    }

    if (!needsSetup && pathname === SETUP_PATH) {
      router.replace("/");
    }
  }, [needsSetup, isLoading, pathname, router]);

  if (isLoading) {
    return (
      <div className="flex min-h-[50vh] flex-col items-center justify-center gap-3 px-4">
        <Spinner />
        <p className="text-sm text-zinc-600 dark:text-zinc-400">Loading organization…</p>
      </div>
    );
  }

  if (isError) {
    return (
      <div
        className="mx-auto max-w-lg px-4 py-16"
        role="alert"
      >
        <div className="rounded-lg border border-red-200 bg-red-50 p-4 text-sm text-red-800 dark:border-red-900 dark:bg-red-950/50 dark:text-red-200">
          <p className="font-medium">Could not load organization</p>
          <p className="mt-2">{getErrorMessage(error)}</p>
          <p className="mt-3 text-zinc-600 dark:text-zinc-400">
            Check that EMS.API is running and <code className="rounded bg-red-100 px-1 dark:bg-red-900/50">NEXT_PUBLIC_API_BASE_URL</code> in{" "}
            <code className="rounded bg-red-100 px-1 dark:bg-red-900/50">.env.local</code> matches the API URL.
          </p>
        </div>
      </div>
    );
  }

  if (needsSetup && pathname !== SETUP_PATH) {
    return (
      <div className="flex min-h-[50vh] flex-col items-center justify-center gap-3 px-4">
        <Spinner />
        <p className="text-sm text-zinc-600 dark:text-zinc-400">Redirecting to setup…</p>
      </div>
    );
  }

  if (!needsSetup && pathname === SETUP_PATH) {
    return (
      <div className="flex min-h-[50vh] flex-col items-center justify-center gap-3 px-4">
        <Spinner />
        <p className="text-sm text-zinc-600 dark:text-zinc-400">Redirecting…</p>
      </div>
    );
  }

  return <>{children}</>;
}
