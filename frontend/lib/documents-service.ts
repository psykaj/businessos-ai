import apiClient from "./api-client";
import {
  DocumentDto,
  DocumentVersionDto,
  DocumentPreviewDto,
  FolderDto,
  FolderTreeDto,
  DocumentTemplateDto,
  ApprovalRequestDto,
  SignatureRequestDto,
  SignatureRecipientDto,
  SignatureAuditTrailDto,
  SharedDocumentDto,
  DocumentAuditEntryDto,
  DocumentSearchQuery
} from "@/types/document";

export const documentsService = {
  // ── Documents ─────────────────────────────────────────────────────────────
  async upload(file: File, folderId?: string, description?: string, tags?: string[]): Promise<DocumentDto> {
    const formData = new FormData();
    formData.append("file", file);
    if (folderId) formData.append("FolderId", folderId);
    if (description) formData.append("Description", description);
    if (tags && tags.length > 0) {
      tags.forEach((t) => formData.append("Tags", t));
    }

    const res = await apiClient.post<DocumentDto>("/api/documents/upload", formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
    return res.data;
  },

  async uploadVersion(documentId: string, file: File, changesSummary?: string): Promise<DocumentDto> {
    const formData = new FormData();
    formData.append("file", file);
    if (changesSummary) formData.append("changesSummary", changesSummary);

    const res = await apiClient.post<DocumentDto>(`/api/documents/${documentId}/versions`, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
    return res.data;
  },

  async getById(id: string): Promise<DocumentDto> {
    const res = await apiClient.get<DocumentDto>(`/api/documents/${id}`);
    return res.data;
  },

  async getPreview(id: string): Promise<DocumentPreviewDto> {
    const res = await apiClient.get<DocumentPreviewDto>(`/api/documents/${id}/preview`);
    return res.data;
  },

  async rename(id: string, newName: string): Promise<DocumentDto> {
    const res = await apiClient.put<DocumentDto>(`/api/documents/${id}/rename`, { newName });
    return res.data;
  },

  async move(id: string, targetFolderId: string | null): Promise<DocumentDto> {
    const res = await apiClient.put<DocumentDto>(`/api/documents/${id}/move`, { targetFolderId });
    return res.data;
  },

  async copy(id: string, targetFolderId?: string, newName?: string): Promise<DocumentDto> {
    const res = await apiClient.post<DocumentDto>(`/api/documents/${id}/copy`, { targetFolderId, newName });
    return res.data;
  },

  async softDelete(id: string): Promise<void> {
    await apiClient.delete(`/api/documents/${id}`);
  },

  async permanentDelete(id: string): Promise<void> {
    await apiClient.delete(`/api/documents/${id}/permanent`);
  },

  async restore(id: string): Promise<DocumentDto> {
    const res = await apiClient.post<DocumentDto>(`/api/documents/${id}/restore`);
    return res.data;
  },

  async getVersions(documentId: string): Promise<DocumentVersionDto[]> {
    const res = await apiClient.get<DocumentVersionDto[]>(`/api/documents/${documentId}/versions`);
    return res.data;
  },

  async revertVersion(documentId: string, versionId: string): Promise<DocumentDto> {
    const res = await apiClient.post<DocumentDto>(`/api/documents/${documentId}/versions/${versionId}/revert`);
    return res.data;
  },

  async search(params: DocumentSearchQuery): Promise<{ items: DocumentDto[]; totalCount: number }> {
    const res = await apiClient.get<{ items: DocumentDto[]; totalCount: number }>("/api/documents/search", { params });
    return res.data;
  },

  async updateTags(id: string, tags: string[]): Promise<DocumentDto> {
    const res = await apiClient.put<DocumentDto>(`/api/documents/${id}/tags`, { tags });
    return res.data;
  },

  async toggleFavorite(id: string): Promise<DocumentDto> {
    const res = await apiClient.post<DocumentDto>(`/api/documents/${id}/favorite`);
    return res.data;
  },

  async getFavorites(): Promise<DocumentDto[]> {
    const res = await apiClient.get<DocumentDto[]>("/api/documents/favorites");
    return res.data;
  },

  // ── Folders ───────────────────────────────────────────────────────────────
  async createFolder(name: string, parentFolderId?: string, description?: string, color?: string, icon?: string): Promise<FolderDto> {
    const res = await apiClient.post<FolderDto>("/api/folders", {
      name,
      parentFolderId,
      description,
      color,
      icon,
    });
    return res.data;
  },

  async getFolders(parentFolderId?: string): Promise<FolderDto[]> {
    const res = await apiClient.get<FolderDto[]>("/api/folders", { params: { parentFolderId } });
    return res.data;
  },

  async getFolderTree(): Promise<FolderTreeDto[]> {
    const res = await apiClient.get<FolderTreeDto[]>("/api/folders/tree");
    return res.data;
  },

  async getFolderById(id: string): Promise<FolderDto> {
    const res = await apiClient.get<FolderDto>(`/api/folders/${id}`);
    return res.data;
  },

  async updateFolder(id: string, name: string, description?: string, color?: string, icon?: string): Promise<FolderDto> {
    const res = await apiClient.put<FolderDto>(`/api/folders/${id}`, { name, description, color, icon });
    return res.data;
  },

  async moveFolder(id: string, targetParentFolderId: string | null): Promise<FolderDto> {
    const res = await apiClient.put<FolderDto>(`/api/folders/${id}/move`, { targetParentFolderId });
    return res.data;
  },

  async deleteFolder(id: string): Promise<void> {
    await apiClient.delete(`/api/folders/${id}`);
  },

  // ── Document Templates ───────────────────────────────────────────────────
  async createTemplate(name: string, category: string, content: string, description?: string, fieldsJson?: string): Promise<DocumentTemplateDto> {
    const res = await apiClient.post<DocumentTemplateDto>("/api/document-templates", {
      name,
      category,
      content,
      description,
      fieldsJson,
    });
    return res.data;
  },

  async getTemplates(category?: string): Promise<DocumentTemplateDto[]> {
    const res = await apiClient.get<DocumentTemplateDto[]>("/api/document-templates", { params: { category } });
    return res.data;
  },

  async getTemplateById(id: string): Promise<DocumentTemplateDto> {
    const res = await apiClient.get<DocumentTemplateDto>(`/api/document-templates/${id}`);
    return res.data;
  },

  async updateTemplate(id: string, data: Partial<DocumentTemplateDto>): Promise<DocumentTemplateDto> {
    const res = await apiClient.put<DocumentTemplateDto>(`/api/document-templates/${id}`, data);
    return res.data;
  },

  async deleteTemplate(id: string): Promise<void> {
    await apiClient.delete(`/api/document-templates/${id}`);
  },

  async renderTemplate(templateId: string, documentName: string, fieldValues: Record<string, string>, folderId?: string): Promise<DocumentDto> {
    const res = await apiClient.post<DocumentDto>(`/api/document-templates/${templateId}/render`, {
      documentName,
      fieldValues,
      folderId,
    });
    return res.data;
  },

  // ── Approvals ─────────────────────────────────────────────────────────────
  async createApproval(documentId: string, title: string, isSequential: boolean = true, dueDate?: string, autoEscalate: boolean = false, steps?: any[]): Promise<ApprovalRequestDto> {
    const res = await apiClient.post<ApprovalRequestDto>("/api/approvals", {
      documentId,
      title,
      isSequential,
      dueDate,
      autoEscalate,
      steps,
    });
    return res.data;
  },

  async getApprovalById(id: string): Promise<ApprovalRequestDto> {
    const res = await apiClient.get<ApprovalRequestDto>(`/api/approvals/${id}`);
    return res.data;
  },

  async getApprovalsByDocument(documentId: string): Promise<ApprovalRequestDto[]> {
    const res = await apiClient.get<ApprovalRequestDto[]>(`/api/approvals/document/${documentId}`);
    return res.data;
  },

  async getPendingApprovals(): Promise<ApprovalRequestDto[]> {
    const res = await apiClient.get<ApprovalRequestDto[]>("/api/approvals/pending");
    return res.data;
  },

  async approveStep(id: string, comments?: string): Promise<ApprovalRequestDto> {
    const res = await apiClient.post<ApprovalRequestDto>(`/api/approvals/${id}/approve`, { comments });
    return res.data;
  },

  async rejectStep(id: string, comments?: string): Promise<ApprovalRequestDto> {
    const res = await apiClient.post<ApprovalRequestDto>(`/api/approvals/${id}/reject`, { comments });
    return res.data;
  },

  async escalateApproval(id: string, escalatedToUserId: string, reason?: string): Promise<ApprovalRequestDto> {
    const res = await apiClient.post<ApprovalRequestDto>(`/api/approvals/${id}/escalate`, { escalatedToUserId, reason });
    return res.data;
  },

  // ── E-Signatures ──────────────────────────────────────────────────────────
  async createSignatureRequest(documentId: string, title: string, message?: string, expiresAt?: string, recipients?: any[]): Promise<SignatureRequestDto> {
    const res = await apiClient.post<SignatureRequestDto>("/api/esignatures", {
      documentId,
      title,
      message,
      expiresAt,
      recipients,
    });
    return res.data;
  },

  async getSignatureRequestById(id: string): Promise<SignatureRequestDto> {
    const res = await apiClient.get<SignatureRequestDto>(`/api/esignatures/${id}`);
    return res.data;
  },

  async getSignatureRequestsByDocument(documentId: string): Promise<SignatureRequestDto[]> {
    const res = await apiClient.get<SignatureRequestDto[]>(`/api/esignatures/document/${documentId}`);
    return res.data;
  },

  async cancelSignatureRequest(id: string, reason?: string): Promise<SignatureRequestDto> {
    const res = await apiClient.post<SignatureRequestDto>(`/api/esignatures/${id}/cancel?reason=${encodeURIComponent(reason || "")}`);
    return res.data;
  },

  async getSignatureAuditTrail(id: string): Promise<SignatureAuditTrailDto> {
    const res = await apiClient.get<SignatureAuditTrailDto>(`/api/esignatures/${id}/audit-trail`);
    return res.data;
  },

  async getRecipientByToken(token: string): Promise<SignatureRecipientDto> {
    const res = await apiClient.get<SignatureRecipientDto>(`/api/esignatures/public/sign/${token}`);
    return res.data;
  },

  async submitSignature(token: string, signatureData: string, accessCode?: string): Promise<SignatureRecipientDto> {
    const res = await apiClient.post<SignatureRecipientDto>(`/api/esignatures/public/sign/${token}`, {
      signatureData,
      accessCode,
    });
    return res.data;
  },

  // ── Shared Documents ──────────────────────────────────────────────────────
  async shareDocument(documentId: string, accessLevel: string, permissionType: string, sharedWithEmail?: string, passcode?: string, expirationDate?: string): Promise<SharedDocumentDto> {
    const res = await apiClient.post<SharedDocumentDto>("/api/documents/share", {
      documentId,
      accessLevel,
      permissionType,
      sharedWithEmail,
      passcode,
      expirationDate,
    });
    return res.data;
  },

  async getDocumentShares(documentId: string): Promise<SharedDocumentDto[]> {
    const res = await apiClient.get<SharedDocumentDto[]>(`/api/documents/${documentId}/shares`);
    return res.data;
  },

  async revokeShare(shareId: string): Promise<void> {
    await apiClient.delete(`/api/documents/shares/${shareId}`);
  },

  // ── Audit Logs ───────────────────────────────────────────────────────────
  async getDocumentAuditLogs(documentId: string, limit: number = 100): Promise<DocumentAuditEntryDto[]> {
    const res = await apiClient.get<DocumentAuditEntryDto[]>(`/api/documents/${documentId}/audit-logs`, { params: { limit } });
    return res.data;
  },

  async getOrganizationAuditLogs(entityType?: string, action?: string, page: number = 1, pageSize: number = 20): Promise<{ items: DocumentAuditEntryDto[]; totalCount: number }> {
    const res = await apiClient.get<{ items: DocumentAuditEntryDto[]; totalCount: number }>("/api/documents/audit-logs", {
      params: { entityType, action, page, pageSize },
    });
    return res.data;
  }
};
