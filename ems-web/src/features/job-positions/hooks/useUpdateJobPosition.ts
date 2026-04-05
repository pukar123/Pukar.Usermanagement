import { useMutation, useQueryClient } from "@tanstack/react-query";
import { jobPositionService } from "../services/jobPositionService";
import { jobPositionKeys } from "../services/query-keys";
import type { UpdateJobPositionRequest } from "../types/job-position.types";

export function useUpdateJobPosition(organizationId: number | null) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, body }: { id: number; body: UpdateJobPositionRequest }) =>
      jobPositionService.updateJobPosition(id, body),
    onSuccess: () => {
      if (organizationId != null) {
        void queryClient.invalidateQueries({ queryKey: jobPositionKeys.list(organizationId) });
      }
    },
  });
}
