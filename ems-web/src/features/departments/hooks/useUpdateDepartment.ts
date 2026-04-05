import { useMutation, useQueryClient } from "@tanstack/react-query";
import { departmentService } from "../services/departmentService";
import { departmentKeys } from "../services/query-keys";
import type { UpdateDepartmentRequest } from "../types/department.types";

export function useUpdateDepartment() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, body }: { id: number; body: UpdateDepartmentRequest }) =>
      departmentService.updateDepartment(id, body),
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey: departmentKeys.list() });
    },
  });
}
