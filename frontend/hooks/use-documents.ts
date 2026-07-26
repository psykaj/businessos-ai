import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { documentsService } from "@/lib/documents-service";
import { DocumentSearchQuery } from "@/types/document";

export function useDocuments(params: DocumentSearchQuery) {
  return useQuery({
    queryKey: ["documents", params],
    queryFn: () => documentsService.search(params),
  });
}

export function useDocument(id: string) {
  return useQuery({
    queryKey: ["document", id],
    queryFn: () => documentsService.getById(id),
    enabled: !!id,
  });
}

export function useDocumentPreview(id: string) {
  return useQuery({
    queryKey: ["document-preview", id],
    queryFn: () => documentsService.getPreview(id),
    enabled: !!id,
  });
}

export function useDocumentVersions(documentId: string) {
  return useQuery({
    queryKey: ["document-versions", documentId],
    queryFn: () => documentsService.getVersions(documentId),
    enabled: !!documentId,
  });
}

export function useFavoriteDocuments() {
  return useQuery({
    queryKey: ["document-favorites"],
    queryFn: () => documentsService.getFavorites(),
  });
}

export function useUploadDocument() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ file, folderId, description, tags }: { file: File; folderId?: string; description?: string; tags?: string[] }) =>
      documentsService.upload(file, folderId, description, tags),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["documents"] });
      queryClient.invalidateQueries({ queryKey: ["folders"] });
    },
  });
}

export function useUploadVersion() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ documentId, file, changesSummary }: { documentId: string; file: File; changesSummary?: string }) =>
      documentsService.uploadVersion(documentId, file, changesSummary),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ["document", variables.documentId] });
      queryClient.invalidateQueries({ queryKey: ["document-versions", variables.documentId] });
      queryClient.invalidateQueries({ queryKey: ["documents"] });
    },
  });
}

export function useRenameDocument() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, newName }: { id: string; newName: string }) => documentsService.rename(id, newName),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ["document", variables.id] });
      queryClient.invalidateQueries({ queryKey: ["documents"] });
    },
  });
}

export function useMoveDocument() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, targetFolderId }: { id: string; targetFolderId: string | null }) =>
      documentsService.move(id, targetFolderId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["documents"] });
      queryClient.invalidateQueries({ queryKey: ["folders"] });
    },
  });
}

export function useCopyDocument() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, targetFolderId, newName }: { id: string; targetFolderId?: string; newName?: string }) =>
      documentsService.copy(id, targetFolderId, newName),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["documents"] });
    },
  });
}

export function useDeleteDocument() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => documentsService.softDelete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["documents"] });
    },
  });
}

export function useRestoreDocument() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => documentsService.restore(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["documents"] });
    },
  });
}

export function useRevertVersion() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ documentId, versionId }: { documentId: string; versionId: string }) =>
      documentsService.revertVersion(documentId, versionId),
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ["document", variables.documentId] });
      queryClient.invalidateQueries({ queryKey: ["document-versions", variables.documentId] });
    },
  });
}

export function useToggleFavorite() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => documentsService.toggleFavorite(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["documents"] });
      queryClient.invalidateQueries({ queryKey: ["document-favorites"] });
    },
  });
}

// ── Folders Hooks ─────────────────────────────────────────────────────────
export function useFolders(parentFolderId?: string) {
  return useQuery({
    queryKey: ["folders", parentFolderId],
    queryFn: () => documentsService.getFolders(parentFolderId),
  });
}

export function useFolderTree() {
  return useQuery({
    queryKey: ["folder-tree"],
    queryFn: () => documentsService.getFolderTree(),
  });
}

export function useCreateFolder() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ name, parentFolderId, description, color, icon }: { name: string; parentFolderId?: string; description?: string; color?: string; icon?: string }) =>
      documentsService.createFolder(name, parentFolderId, description, color, icon),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["folders"] });
      queryClient.invalidateQueries({ queryKey: ["folder-tree"] });
    },
  });
}

export function useDeleteFolder() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => documentsService.deleteFolder(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["folders"] });
      queryClient.invalidateQueries({ queryKey: ["folder-tree"] });
    },
  });
}

// ── Templates Hooks ───────────────────────────────────────────────────────
export function useDocumentTemplates(category?: string) {
  return useQuery({
    queryKey: ["document-templates", category],
    queryFn: () => documentsService.getTemplates(category),
  });
}

export function useCreateTemplate() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ name, category, content, description, fieldsJson }: { name: string; category: string; content: string; description?: string; fieldsJson?: string }) =>
      documentsService.createTemplate(name, category, content, description, fieldsJson),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["document-templates"] });
    },
  });
}

export function useRenderTemplate() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ templateId, documentName, fieldValues, folderId }: { templateId: string; documentName: string; fieldValues: Record<string, string>; folderId?: string }) =>
      documentsService.renderTemplate(templateId, documentName, fieldValues, folderId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["documents"] });
    },
  });
}

// ── Approvals Hooks ───────────────────────────────────────────────────────
export function usePendingApprovals() {
  return useQuery({
    queryKey: ["approvals-pending"],
    queryFn: () => documentsService.getPendingApprovals(),
  });
}

export function useCreateApproval() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ documentId, title, isSequential, dueDate, autoEscalate, steps }: { documentId: string; title: string; isSequential?: boolean; dueDate?: string; autoEscalate?: boolean; steps?: any[] }) =>
      documentsService.createApproval(documentId, title, isSequential, dueDate, autoEscalate, steps),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["approvals-pending"] });
      queryClient.invalidateQueries({ queryKey: ["documents"] });
    },
  });
}

export function useApproveStep() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, comments }: { id: string; comments?: string }) => documentsService.approveStep(id, comments),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["approvals-pending"] });
      queryClient.invalidateQueries({ queryKey: ["documents"] });
    },
  });
}

export function useRejectStep() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, comments }: { id: string; comments?: string }) => documentsService.rejectStep(id, comments),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["approvals-pending"] });
      queryClient.invalidateQueries({ queryKey: ["documents"] });
    },
  });
}

// ── Signatures Hooks ──────────────────────────────────────────────────────
export function useCreateSignatureRequest() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ documentId, title, message, expiresAt, recipients }: { documentId: string; title: string; message?: string; expiresAt?: string; recipients?: any[] }) =>
      documentsService.createSignatureRequest(documentId, title, message, expiresAt, recipients),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["documents"] });
    },
  });
}

export function useSignatureAuditTrail(id: string) {
  return useQuery({
    queryKey: ["signature-audit-trail", id],
    queryFn: () => documentsService.getSignatureAuditTrail(id),
    enabled: !!id,
  });
}

// ── Audit Logs Hook ───────────────────────────────────────────────────────
export function useDocumentAuditLogs(documentId: string) {
  return useQuery({
    queryKey: ["document-audit-logs", documentId],
    queryFn: () => documentsService.getDocumentAuditLogs(documentId),
    enabled: !!documentId,
  });
}
