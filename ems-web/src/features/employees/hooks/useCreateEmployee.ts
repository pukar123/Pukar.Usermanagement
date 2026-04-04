import { useMutation, useQueryClient } from "@tanstack/react-query";
import { employeeService } from "../services/employeeService";
import { employeeKeys } from "../services/query-keys";
import type { CreateEmployeeRequest } from "../types/employee.types";

export function useCreateEmployee() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateEmployeeRequest) => employeeService.createEmployee(data),
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey: employeeKeys.list() });
    },
  });
}
