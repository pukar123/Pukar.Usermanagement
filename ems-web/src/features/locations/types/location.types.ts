/** Aligns with EMS.Application.DTOs.Location.LocationResponseModel (camelCase JSON). */
export type Location = {
  id: number;
  organizationId: number;
  name: string;
  line1: string | null;
  line2: string | null;
  city: string | null;
  region: string | null;
  postalCode: string | null;
  country: string | null;
  isActive: boolean;
};
