/** Aligns with EMS API organization DTOs (camelCase JSON). */
export type Organization = {
  id: number;
  name: string;
  code: string | null;
  isActive: boolean;
};

export type CreateOrganizationRequest = {
  name: string;
  code?: string | null;
  isActive: boolean;
};

export type UpdateOrganizationRequest = {
  name: string;
  code?: string | null;
  isActive: boolean;
};
