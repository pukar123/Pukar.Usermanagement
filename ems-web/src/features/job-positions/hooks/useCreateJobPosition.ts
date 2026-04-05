import { useMutation, useQueryClient } from "@tanstack/react-query";
import { defaultOrganizationId } from "@/shared/config/public-env";
import { jobPositionService } from "../services/jobPositionService";
import { jobPositionKeys } from "../services/query-keys";
import type { CreateJobPositionRequest } from "../types/job-position.types";

export function useCreateJobPosition(organizationId: number = defaultOrganizationId) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateJobPositionRequest) => jobPositionService.createJobPosition(data),
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey: jobPositionKeys.list(organizationId) });
    },
  });
}
