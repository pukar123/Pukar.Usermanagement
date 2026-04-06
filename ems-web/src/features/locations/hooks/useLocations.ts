import { useQuery } from "@tanstack/react-query";
import { locationService } from "../services/locationService";
import { locationKeys } from "../services/query-keys";

export function useLocations() {
  return useQuery({
    queryKey: locationKeys.list(),
    queryFn: () => locationService.getLocations(),
  });
}
