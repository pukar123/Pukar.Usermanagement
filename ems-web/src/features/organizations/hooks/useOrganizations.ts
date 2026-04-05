import { useQuery } from "@tanstack/react-query";
import { organizationService } from "../services/organizationService";
import { organizationKeys } from "../services/query-keys";

export function useOrganizations() {
  return useQuery({
    queryKey: organizationKeys.list(),
    queryFn: () => organizationService.getOrganizations(),
  });
}
