import { useMutation, useQueryClient } from "@tanstack/react-query";
import { jobPositionService } from "../services/jobPositionService";
import { jobPositionKeys } from "../services/query-keys";
import type { CreateJobPositionRequest } from "../types/job-position.types";

export function useCreateJobPosition(organizationId: number | null) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateJobPositionRequest) => jobPositionService.createJobPosition(data),
    onSuccess: () => {
      if (organizationId != null) {
        void queryClient.invalidateQueries({ queryKey: jobPositionKeys.list(organizationId) });
      }
    },
  });
}
