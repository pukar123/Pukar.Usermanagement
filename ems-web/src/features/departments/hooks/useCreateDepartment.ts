import { useMutation, useQueryClient } from "@tanstack/react-query";
import { departmentService } from "../services/departmentService";
import { departmentKeys } from "../services/query-keys";
import type { CreateDepartmentRequest } from "../types/department.types";

export function useCreateDepartment() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateDepartmentRequest) => departmentService.createDepartment(data),
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey: departmentKeys.list() });
    },
  });
}
