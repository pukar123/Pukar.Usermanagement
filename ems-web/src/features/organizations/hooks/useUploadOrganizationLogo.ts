import { useMutation, useQueryClient } from "@tanstack/react-query";
import { organizationService } from "../services/organizationService";
import { organizationKeys } from "../services/query-keys";

export function useUploadOrganizationLogo(organizationId: number | null) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (file: File) => {
      if (organizationId == null || organizationId <= 0) {
        return Promise.reject(new Error("Organization is not loaded."));
      }
      return organizationService.uploadOrganizationLogo(organizationId, file);
    },
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey: organizationKeys.list() });
    },
  });
}
