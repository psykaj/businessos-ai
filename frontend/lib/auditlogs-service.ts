import apiClient from "./api-client";
import { AuditLog, PagedResult } from "@/types/auditlogs";

export const auditlogsService = {
  getAuditLogs: async (
    orgId: string,
    params: {
      page?: number;
      pageSize?: number;
      search?: string;
      module?: string;
      startDate?: string;
      endDate?: string;
    }
  ): Promise<PagedResult<AuditLog>> => {
    const response = await apiClient.get<PagedResult<AuditLog>>(
      `/api/v1/organizations/${orgId}/auditlogs`,
      { params }
    );
    return response.data;
  },
};
