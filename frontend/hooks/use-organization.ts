import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { organizationService } from "@/lib/organization-service";
import { UpdateOrganizationRequest } from "@/types/organization";

export function useOrganization(orgId: string | undefined) {
  return useQuery({
    queryKey: ["organization", orgId],
    queryFn: () => organizationService.getOrganization(orgId!),
    enabled: !!orgId,
  });
}

export function useUpdateOrganization() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ orgId, data }: { orgId: string; data: UpdateOrganizationRequest }) =>
      organizationService.updateOrganization(orgId, data),
    onSuccess: (_, { orgId }) => {
      queryClient.invalidateQueries({ queryKey: ["organization", orgId] });
    },
  });
}

export function useUploadLogo() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ orgId, file }: { orgId: string; file: File }) =>
      organizationService.uploadLogo(orgId, file),
    onSuccess: (_, { orgId }) => {
      queryClient.invalidateQueries({ queryKey: ["organization", orgId] });
    },
  });
}
