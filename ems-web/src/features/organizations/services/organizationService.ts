import { httpClient } from "@/shared/api/http-client";
import type { CreateOrganizationRequest, Organization } from "../types/organization.types";

const PATH = "/api/Organizations";

export const organizationService = {
  getOrganizations: async (): Promise<Organization[]> => {
    const { data } = await httpClient.get<Organization[]>(PATH);
    return data;
  },

  createOrganization: async (body: CreateOrganizationRequest): Promise<Organization> => {
    const { data } = await httpClient.post<Organization>(PATH, body);
    return data;
  },
};
