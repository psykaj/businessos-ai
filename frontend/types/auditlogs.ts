export interface AuditLog {
  id: string;
  organizationId: string;
  userId: string;
  userEmail: string;
  action: string;
  module: string;
  description: string;
  ipAddress?: string;
  userAgent?: string;
  timestamp: string;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}
