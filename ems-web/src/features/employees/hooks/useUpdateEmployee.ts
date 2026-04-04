import { useMutation, useQueryClient } from "@tanstack/react-query";
import { employeeService } from "../services/employeeService";
import { employeeKeys } from "../services/query-keys";
import type { UpdateEmployeeRequest } from "../types/employee.types";

export function useUpdateEmployee() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: number; data: UpdateEmployeeRequest }) =>
      employeeService.updateEmployee(id, data),
    onSuccess: (_data, variables) => {
      void queryClient.invalidateQueries({ queryKey: employeeKeys.list() });
      void queryClient.invalidateQueries({ queryKey: employeeKeys.detail(variables.id) });
    },
  });
}
