import { useQuery } from "@tanstack/react-query";
import { employeeService } from "../services/employeeService";
import { employeeKeys } from "../services/query-keys";

export function useEmployee(id: number | null) {
  return useQuery({
    queryKey: employeeKeys.detail(id ?? 0),
    queryFn: () => employeeService.getEmployeeById(id!),
    enabled: id != null && id > 0,
  });
}
