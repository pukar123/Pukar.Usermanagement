import { useMutation, useQueryClient } from "@tanstack/react-query";
import { organizationService } from "../services/organizationService";
import { organizationKeys } from "../services/query-keys";
import type { CreateOrganizationRequest } from "../types/organization.types";

export function useCreateOrganization() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateOrganizationRequest) => organizationService.createOrganization(data),
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey: organizationKeys.list() });
    },
  });
}
