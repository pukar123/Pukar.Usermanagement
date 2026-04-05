"use client";

import { createContext, useContext, useMemo, type ReactNode } from "react";
import { useOrganizations } from "@/features/organizations/hooks";
import type { Organization } from "@/features/organizations/types/organization.types";

export type OrganizationContextValue = {
  currentOrganization: Organization | null;
  /** Resolved singleton org id for API payloads; null while loading, missing org, or error. */
  organizationId: number | null;
  needsSetup: boolean;
  isLoading: boolean;
  isError: boolean;
  error: unknown;
};

const OrganizationContext = createContext<OrganizationContextValue | null>(null);

export function OrganizationProvider({ children }: { children: ReactNode }) {
  const { data, isLoading, isError, error } = useOrganizations();

  const value = useMemo<OrganizationContextValue>(() => {
    const list = data ?? [];
    const currentOrganization = list[0] ?? null;
    const needsSetup = !isLoading && !isError && list.length === 0;

    return {
      currentOrganization,
      organizationId: currentOrganization?.id ?? null,
      needsSetup,
      isLoading,
      isError,
      error,
    };
  }, [data, isLoading, isError, error]);

  return <OrganizationContext.Provider value={value}>{children}</OrganizationContext.Provider>;
}

export function useOrganizationContext(): OrganizationContextValue {
  const ctx = useContext(OrganizationContext);
  if (!ctx) {
    throw new Error("useOrganizationContext must be used within OrganizationProvider");
  }
  return ctx;
}
