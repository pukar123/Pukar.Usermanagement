import { useMutation, useQueryClient } from "@tanstack/react-query";
import { defaultOrganizationId } from "@/shared/config/public-env";
import { jobPositionService } from "../services/jobPositionService";
import { jobPositionKeys } from "../services/query-keys";

export function useDeleteJobPosition(organizationId: number = defaultOrganizationId) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: number) => jobPositionService.deleteJobPosition(id),
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey: jobPositionKeys.list(organizationId) });
    },
  });
}
