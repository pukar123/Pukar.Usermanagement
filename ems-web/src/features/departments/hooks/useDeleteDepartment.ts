import { useMutation, useQueryClient } from "@tanstack/react-query";
import { departmentService } from "../services/departmentService";
import { departmentKeys } from "../services/query-keys";

export function useDeleteDepartment() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: number) => departmentService.deleteDepartment(id),
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey: departmentKeys.list() });
    },
  });
}
