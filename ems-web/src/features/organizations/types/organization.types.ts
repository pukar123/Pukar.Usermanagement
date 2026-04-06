/** Aligns with EMS API organization DTOs (camelCase JSON). */
export type Organization = {
  id: number;
  name: string;
  code: string | null;
  isActive: boolean;
  description: string | null;
  motto: string | null;
  /** Web path under the API host (static files), not a full URL. */
  logoRelativePath: string | null;
};

export type CreateOrganizationRequest = {
  name: string;
  code?: string | null;
  isActive: boolean;
  description?: string | null;
  motto?: string | null;
};

export type UpdateOrganizationRequest = {
  name: string;
  code?: string | null;
  isActive: boolean;
  description?: string | null;
  motto?: string | null;
};
