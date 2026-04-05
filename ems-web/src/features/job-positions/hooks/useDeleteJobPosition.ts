import { useMutation, useQueryClient } from "@tanstack/react-query";
import { jobPositionService } from "../services/jobPositionService";
import { jobPositionKeys } from "../services/query-keys";

export function useDeleteJobPosition(organizationId: number | null) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: number) => jobPositionService.deleteJobPosition(id),
    onSuccess: () => {
      if (organizationId != null) {
        void queryClient.invalidateQueries({ queryKey: jobPositionKeys.list(organizationId) });
      }
    },
  });
}
