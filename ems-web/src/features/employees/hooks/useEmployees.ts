import { useQuery } from "@tanstack/react-query";
import { employeeService } from "../services/employeeService";
import { employeeKeys } from "../services/query-keys";

export function useEmployees() {
  return useQuery({
    queryKey: employeeKeys.list(),
    queryFn: () => employeeService.getEmployees(),
  });
}
