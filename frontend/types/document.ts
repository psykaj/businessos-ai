export type DocumentStatus = "Active" | "Draft" | "InReview" | "PendingSignature" | "Signed" | "Archived";

export interface DocumentDto {
  id: string;
  organizationId: string;
  folderId?: string | null;
  name: string;
  description?: string | null;
  mimeType: string;
  fileExtension: string;
  fileSize: number;
  filePath: string;
  storageProvider: string;
  status: DocumentStatus;
  ownerId: string;
  currentVersionId?: string | null;
  versionCount: number;
  isFavorite: boolean;
  tags: string[];
  metadata?: string | null;
  createdAt: string;
  updatedAt: string;
}

export interface DocumentVersionDto {
  id: string;
  organizationId: string;
  documentId: string;
  versionNumber: number;
  filePath: string;
  storageProvider: string;
  fileSize: number;
  mimeType: string;
  changesSummary?: string | null;
  uploadedById?: string | null;
  uploadedByName?: string | null;
  createdAt: string;
}

export interface DocumentPreviewDto {
  documentId: string;
  name: string;
  mimeType: string;
  fileSize: number;
  previewUrl: string;
  downloadUrl: string;
  canPreviewInline: boolean;
}

export interface FolderDto {
  id: string;
  organizationId: string;
  parentFolderId?: string | null;
  name: string;
  description?: string | null;
  path: string;
  color?: string | null;
  icon?: string | null;
  subFolderCount: number;
  documentCount: number;
  createdAt: string;
  updatedAt: string;
}

export interface FolderTreeDto {
  id: string;
  organizationId: string;
  parentFolderId?: string | null;
  name: string;
  path: string;
  color?: string | null;
  icon?: string | null;
  children: FolderTreeDto[];
}

export interface DocumentTemplateDto {
  id: string;
  organizationId: string;
  name: string;
  description?: string | null;
  category: string;
  content: string;
  fieldsJson?: string | null;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface ApprovalStepDto {
  id: string;
  approvalRequestId: string;
  sequence: number;
  approverId?: string | null;
  approverEmail: string;
  approverName?: string | null;
  status: "Pending" | "Approved" | "Rejected" | "Skipped";
  comments?: string | null;
  actionDate?: string | null;
  isParallelGroup: boolean;
}

export interface ApprovalRequestDto {
  id: string;
  organizationId: string;
  documentId: string;
  documentName: string;
  title: string;
  status: "Pending" | "Approved" | "Rejected" | "Cancelled";
  currentStepSequence: number;
  isSequential: boolean;
  dueDate?: string | null;
  autoEscalate: boolean;
  escalatedToUserId?: string | null;
  requestedById: string;
  requestedByName?: string | null;
  steps: ApprovalStepDto[];
  createdAt: string;
  updatedAt: string;
}

export interface SignatureRecipientDto {
  id: string;
  signatureRequestId: string;
  signerName: string;
  signerEmail: string;
  signerUserId?: string | null;
  role: "Signer" | "Viewer" | "Approver" | "CC";
  signingOrder: number;
  status: "Pending" | "Sent" | "Viewed" | "Signed" | "Declined";
  viewedAt?: string | null;
  signedAt?: string | null;
  securityToken: string;
  accessCode?: string | null;
  ipAddress?: string | null;
}

export interface SignatureRequestDto {
  id: string;
  organizationId: string;
  documentId: string;
  documentName: string;
  title: string;
  message?: string | null;
  status: "Draft" | "Pending" | "Completed" | "Declined" | "Expired" | "Cancelled";
  expiresAt?: string | null;
  completedAt?: string | null;
  createdById: string;
  createdByName?: string | null;
  securityHash: string;
  signatureCertificateUrl?: string | null;
  recipients: SignatureRecipientDto[];
  createdAt: string;
  updatedAt: string;
}

export interface SignatureAuditTrailDto {
  signatureRequestId: string;
  title: string;
  documentName: string;
  securityHash: string;
  status: string;
  createdAt: string;
  completedAt?: string | null;
  auditLogRecipients: SignatureRecipientDto[];
}

export interface SharedDocumentDto {
  id: string;
  organizationId: string;
  documentId: string;
  documentName: string;
  sharedWithUserId?: string | null;
  sharedWithEmail?: string | null;
  accessLevel: "Read" | "Comment" | "Edit" | "Admin";
  permissionType: "InternalUser" | "PublicLink";
  publicShareToken?: string | null;
  shareUrl?: string | null;
  hasPasscode: boolean;
  expirationDate?: string | null;
  accessCount: number;
  isActive: boolean;
  sharedById: string;
  createdAt: string;
}

export interface DocumentAuditEntryDto {
  id: string;
  organizationId: string;
  documentId?: string | null;
  entityType: string;
  action: string;
  performedById?: string | null;
  performedByName?: string | null;
  ipAddress?: string | null;
  detailsJson?: string | null;
  timestamp: string;
}

export interface DocumentSearchQuery {
  query?: string;
  folderId?: string;
  status?: string;
  tag?: string;
  mimeType?: string;
  isFavorite?: boolean;
  includeDeleted?: boolean;
  page?: number;
  pageSize?: number;
  sortBy?: string;
  descending?: boolean;
}
