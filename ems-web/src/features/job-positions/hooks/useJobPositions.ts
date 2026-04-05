import { useQuery } from "@tanstack/react-query";
import { jobPositionService } from "../services/jobPositionService";
import { jobPositionKeys } from "../services/query-keys";

export function useJobPositions(organizationId: number | null) {
  return useQuery({
    queryKey: jobPositionKeys.list(organizationId ?? 0),
    queryFn: () => jobPositionService.getByOrganization(organizationId!),
    enabled: organizationId != null && organizationId > 0,
  });
}
