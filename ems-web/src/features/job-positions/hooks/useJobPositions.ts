import { useQuery } from "@tanstack/react-query";
import { defaultOrganizationId } from "@/shared/config/public-env";
import { jobPositionService } from "../services/jobPositionService";
import { jobPositionKeys } from "../services/query-keys";

export function useJobPositions(organizationId: number = defaultOrganizationId) {
  return useQuery({
    queryKey: jobPositionKeys.list(organizationId),
    queryFn: () => jobPositionService.getByOrganization(organizationId),
  });
}
