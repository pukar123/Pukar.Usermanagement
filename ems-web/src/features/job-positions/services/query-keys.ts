import { defaultOrganizationId } from "@/shared/config/public-env";

export const jobPositionKeys = {
  all: ["jobPositions"] as const,
  list: (organizationId: number = defaultOrganizationId) =>
    [...jobPositionKeys.all, "list", organizationId] as const,
};
