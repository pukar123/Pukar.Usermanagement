import { useQuery } from "@tanstack/react-query";
import { departmentService } from "../services/departmentService";
import { departmentKeys } from "../services/query-keys";

export function useDepartments() {
  return useQuery({
    queryKey: departmentKeys.list(),
    queryFn: () => departmentService.getDepartments(),
  });
}
