import { useQuery } from "@tanstack/react-query";
import { auditlogsService } from "@/lib/auditlogs-service";

export function useAuditLogs(
  orgId: string | undefined,
  params: {
    page?: number;
    pageSize?: number;
    search?: string;
    module?: string;
    startDate?: string;
    endDate?: string;
  }
) {
  return useQuery({
    queryKey: ["auditLogs", orgId, params],
    queryFn: () => auditlogsService.getAuditLogs(orgId!, params),
    enabled: !!orgId,
  });
}
