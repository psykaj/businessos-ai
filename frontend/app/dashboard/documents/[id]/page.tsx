"use client";

import { useState } from "react";
import { useParams, useRouter } from "next/navigation";
import Link from "next/link";
import {
  ArrowLeft,
  FileText,
  Download,
  Share2,
  Trash2,
  RefreshCw,
  Clock,
  History,
  ShieldCheck,
  Eye,
  Edit2,
  Upload,
  RotateCcw,
  CheckCircle2,
  ExternalLink,
  Tag as TagIcon
} from "lucide-react";
import {
  useDocument,
  useDocumentPreview,
  useDocumentVersions,
  useDocumentAuditLogs,
  useUploadVersion,
  useRevertVersion,
  useRenameDocument,
  useDeleteDocument
} from "@/hooks/use-documents";

export default function DocumentDetailPage() {
  const params = useParams();
  const router = useRouter();
  const id = params?.id as string;

  const [activeTab, setActiveTab] = useState<"preview" | "versions" | "audit">("preview");

  // Modals state
  const [isUploadVersionOpen, setIsUploadVersionOpen] = useState(false);
  const [isRenameOpen, setIsRenameOpen] = useState(false);
  const [newName, setNewName] = useState("");
  const [versionFile, setVersionFile] = useState<File | null>(null);
  const [changesSummary, setChangesSummary] = useState("");

  // Data fetching
  const { data: document, isLoading: docLoading } = useDocument(id);
  const { data: preview } = useDocumentPreview(id);
  const { data: versions } = useDocumentVersions(id);
  const { data: auditLogs } = useDocumentAuditLogs(id);

  // Mutations
  const uploadVersionMutation = useUploadVersion();
  const revertVersionMutation = useRevertVersion();
  const renameDocMutation = useRenameDocument();
  const deleteDocMutation = useDeleteDocument();

  const handleUploadVersionSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!versionFile || !id) return;
    await uploadVersionMutation.mutateAsync({
      documentId: id,
      file: versionFile,
      changesSummary,
    });
    setVersionFile(null);
    setChangesSummary("");
    setIsUploadVersionOpen(false);
  };

  const handleRenameSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newName.trim() || !id) return;
    await renameDocMutation.mutateAsync({ id, newName });
    setIsRenameOpen(false);
  };

  const handleDelete = async () => {
    if (!confirm("Are you sure you want to soft delete this document?")) return;
    await deleteDocMutation.mutateAsync(id);
    router.push("/dashboard/documents");
  };

  if (docLoading) {
    return (
      <div className="p-12 text-center text-slate-500 flex flex-col items-center justify-center gap-2 min-h-[60vh]">
        <RefreshCw className="w-8 h-8 animate-spin text-indigo-500" />
        <div>Loading document details...</div>
      </div>
    );
  }

  if (!document) {
    return (
      <div className="p-12 text-center max-w-md mx-auto space-y-4">
        <FileText className="w-12 h-12 text-slate-400 mx-auto" />
        <div className="text-xl font-bold text-slate-900 dark:text-white">Document Not Found</div>
        <p className="text-sm text-slate-500">The requested document could not be located or may have been deleted.</p>
        <Link href="/dashboard/documents" className="inline-block bg-indigo-600 text-white px-4 py-2 rounded-xl text-sm font-medium">
          Return to Document Center
        </Link>
      </div>
    );
  }

  const formatFileSize = (bytes: number) => {
    if (bytes === 0) return "0 B";
    const k = 1024;
    const sizes = ["B", "KB", "MB", "GB"];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(1)) + " " + sizes[i];
  };

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      {/* Top Bar Navigation */}
      <div className="flex items-center justify-between gap-4">
        <Link
          href="/dashboard/documents"
          className="inline-flex items-center gap-2 text-sm text-slate-600 dark:text-slate-400 hover:text-slate-900 dark:hover:text-white font-medium"
        >
          <ArrowLeft className="w-4 h-4" /> Back to Documents
        </Link>

        <div className="flex items-center gap-2">
          <button
            onClick={() => setIsUploadVersionOpen(true)}
            className="flex items-center gap-2 bg-slate-100 hover:bg-slate-200 dark:bg-slate-800 dark:hover:bg-slate-700 text-slate-700 dark:text-slate-200 px-3.5 py-2 rounded-xl text-sm font-medium transition-all"
          >
            <Upload className="w-4 h-4 text-indigo-500" /> New Version
          </button>
          <a
            href={`/api/documents/${document.id}/download`}
            target="_blank"
            rel="noreferrer"
            className="flex items-center gap-2 bg-indigo-600 hover:bg-indigo-500 text-white px-3.5 py-2 rounded-xl text-sm font-medium transition-all shadow-md shadow-indigo-600/20"
          >
            <Download className="w-4 h-4" /> Download File
          </a>
          <button
            onClick={handleDelete}
            className="p-2 text-slate-400 hover:text-red-600 bg-slate-100 dark:bg-slate-800 rounded-xl transition-colors"
            title="Delete Document"
          >
            <Trash2 className="w-4 h-4" />
          </button>
        </div>
      </div>

      {/* Main Document Header Card */}
      <div className="bg-white dark:bg-slate-900 rounded-2xl border border-slate-200 dark:border-slate-800 p-6 shadow-sm space-y-4">
        <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 border-b border-slate-100 dark:border-slate-800 pb-4">
          <div className="flex items-start gap-4">
            <div className="p-3 bg-indigo-50 dark:bg-indigo-950/50 text-indigo-600 dark:text-indigo-400 rounded-2xl">
              <FileText className="w-8 h-8" />
            </div>
            <div>
              <div className="flex items-center gap-3">
                <h1 className="text-xl font-bold text-slate-900 dark:text-white">{document.name}</h1>
                <button
                  onClick={() => {
                    setNewName(document.name);
                    setIsRenameOpen(true);
                  }}
                  className="text-slate-400 hover:text-indigo-600"
                >
                  <Edit2 className="w-4 h-4" />
                </button>
              </div>
              <p className="text-xs text-slate-400 mt-1">{document.description || "No description provided."}</p>
            </div>
          </div>

          <div className="flex items-center gap-2">
            <span className="text-xs px-3 py-1 rounded-full font-semibold bg-indigo-50 text-indigo-600 dark:bg-indigo-950 dark:text-indigo-400">
              {document.status}
            </span>
            <span className="text-xs px-3 py-1 rounded-full font-semibold bg-slate-100 text-slate-600 dark:bg-slate-800 dark:text-slate-400">
              v{document.versionCount}
            </span>
          </div>
        </div>

        {/* Metadata Details Row */}
        <div className="grid grid-cols-2 sm:grid-cols-4 gap-4 text-xs">
          <div>
            <span className="text-slate-400 block font-medium">File Size</span>
            <span className="font-semibold text-slate-700 dark:text-slate-200">{formatFileSize(document.fileSize)}</span>
          </div>
          <div>
            <span className="text-slate-400 block font-medium">Storage Provider</span>
            <span className="font-semibold text-slate-700 dark:text-slate-200">{document.storageProvider}</span>
          </div>
          <div>
            <span className="text-slate-400 block font-medium">Created Date</span>
            <span className="font-semibold text-slate-700 dark:text-slate-200">{new Date(document.createdAt).toLocaleDateString()}</span>
          </div>
          <div>
            <span className="text-slate-400 block font-medium">Last Modified</span>
            <span className="font-semibold text-slate-700 dark:text-slate-200">{new Date(document.updatedAt).toLocaleDateString()}</span>
          </div>
        </div>

        {document.tags && document.tags.length > 0 && (
          <div className="flex items-center gap-2 pt-2 border-t border-slate-100 dark:border-slate-800">
            <TagIcon className="w-3.5 h-3.5 text-slate-400" />
            <div className="flex flex-wrap gap-1.5">
              {document.tags.map((t) => (
                <span key={t} className="text-[11px] bg-slate-100 dark:bg-slate-800 text-slate-600 dark:text-slate-300 px-2.5 py-0.5 rounded-md font-medium">
                  #{t}
                </span>
              ))}
            </div>
          </div>
        )}
      </div>

      {/* Tabs Navigation */}
      <div className="flex items-center border-b border-slate-200 dark:border-slate-800 gap-6">
        <button
          onClick={() => setActiveTab("preview")}
          className={`pb-3 text-sm font-semibold flex items-center gap-2 border-b-2 transition-colors ${
            activeTab === "preview"
              ? "border-indigo-600 text-indigo-600 dark:text-indigo-400"
              : "border-transparent text-slate-500 hover:text-slate-800 dark:hover:text-slate-300"
          }`}
        >
          <Eye className="w-4 h-4" /> Preview
        </button>
        <button
          onClick={() => setActiveTab("versions")}
          className={`pb-3 text-sm font-semibold flex items-center gap-2 border-b-2 transition-colors ${
            activeTab === "versions"
              ? "border-indigo-600 text-indigo-600 dark:text-indigo-400"
              : "border-transparent text-slate-500 hover:text-slate-800 dark:hover:text-slate-300"
          }`}
        >
          <History className="w-4 h-4" /> Version History ({versions?.length || 1})
        </button>
        <button
          onClick={() => setActiveTab("audit")}
          className={`pb-3 text-sm font-semibold flex items-center gap-2 border-b-2 transition-colors ${
            activeTab === "audit"
              ? "border-indigo-600 text-indigo-600 dark:text-indigo-400"
              : "border-transparent text-slate-500 hover:text-slate-800 dark:hover:text-slate-300"
          }`}
        >
          <ShieldCheck className="w-4 h-4" /> Audit Log ({auditLogs?.length || 0})
        </button>
      </div>

      {/* Tab 1: Preview */}
      {activeTab === "preview" && (
        <div className="bg-white dark:bg-slate-900 rounded-2xl border border-slate-200 dark:border-slate-800 p-6 min-h-[400px] flex flex-col items-center justify-center text-center">
          {preview?.canPreviewInline && preview?.previewUrl ? (
            <iframe src={preview.previewUrl} className="w-full h-[600px] rounded-xl border border-slate-200 dark:border-slate-800" title="Document Preview" />
          ) : (
            <div className="space-y-4 max-w-sm">
              <div className="w-16 h-16 bg-indigo-50 dark:bg-indigo-950/50 text-indigo-600 rounded-full flex items-center justify-center mx-auto">
                <FileText className="w-8 h-8" />
              </div>
              <div className="text-lg font-bold text-slate-900 dark:text-white">Document Preview Ready</div>
              <p className="text-xs text-slate-500">
                Click below to open the secure preview or download the file directly to your device.
              </p>
              <div className="flex items-center justify-center gap-3">
                <a
                  href={`/api/documents/${document.id}/download`}
                  target="_blank"
                  rel="noreferrer"
                  className="inline-flex items-center gap-2 bg-indigo-600 text-white px-4 py-2 rounded-xl text-sm font-medium hover:bg-indigo-500 transition-all"
                >
                  <Download className="w-4 h-4" /> Download File
                </a>
              </div>
            </div>
          )}
        </div>
      )}

      {/* Tab 2: Version History */}
      {activeTab === "versions" && (
        <div className="bg-white dark:bg-slate-900 rounded-2xl border border-slate-200 dark:border-slate-800 p-6 space-y-4">
          <h3 className="font-bold text-slate-900 dark:text-white text-sm">Historical Versions</h3>
          <div className="divide-y divide-slate-100 dark:divide-slate-800">
            {versions?.map((ver) => (
              <div key={ver.id} className="py-3 flex items-center justify-between gap-4">
                <div className="flex items-center gap-3">
                  <div className="p-2 bg-slate-100 dark:bg-slate-800 rounded-xl text-slate-700 dark:text-slate-300 font-bold text-xs">
                    v{ver.versionNumber}
                  </div>
                  <div>
                    <div className="text-sm font-semibold text-slate-900 dark:text-white">
                      {ver.changesSummary || `Version ${ver.versionNumber}`}
                    </div>
                    <div className="text-xs text-slate-400">
                      Uploaded by {ver.uploadedByName || "User"} on {new Date(ver.createdAt).toLocaleString()} · {formatFileSize(ver.fileSize)}
                    </div>
                  </div>
                </div>

                <div className="flex items-center gap-2">
                  {document.currentVersionId === ver.id ? (
                    <span className="text-xs px-2.5 py-1 bg-emerald-50 text-emerald-600 dark:bg-emerald-950/50 dark:text-emerald-400 rounded-full font-medium flex items-center gap-1">
                      <CheckCircle2 className="w-3.5 h-3.5" /> Current Active
                    </span>
                  ) : (
                    <button
                      onClick={() => revertVersionMutation.mutate({ documentId: document.id, versionId: ver.id })}
                      className="text-xs bg-slate-100 hover:bg-slate-200 dark:bg-slate-800 dark:hover:bg-slate-700 text-slate-700 dark:text-slate-200 px-3 py-1.5 rounded-lg font-medium transition-colors flex items-center gap-1.5"
                    >
                      <RotateCcw className="w-3.5 h-3.5 text-indigo-500" /> Revert
                    </button>
                  )}
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      {/* Tab 3: Audit Trail */}
      {activeTab === "audit" && (
        <div className="bg-white dark:bg-slate-900 rounded-2xl border border-slate-200 dark:border-slate-800 p-6 space-y-4">
          <h3 className="font-bold text-slate-900 dark:text-white text-sm">Activity Audit Trail</h3>
          <div className="space-y-3">
            {auditLogs?.map((log) => (
              <div key={log.id} className="p-3 bg-slate-50 dark:bg-slate-800/50 rounded-xl border border-slate-100 dark:border-slate-800 flex items-center justify-between text-xs">
                <div>
                  <span className="font-semibold text-indigo-600 dark:text-indigo-400">{log.action}</span> by{" "}
                  <span className="font-medium text-slate-800 dark:text-slate-200">{log.performedByName || "System User"}</span>
                </div>
                <div className="text-slate-400">{new Date(log.timestamp).toLocaleString()}</div>
              </div>
            ))}
          </div>
        </div>
      )}

      {/* Upload New Version Modal */}
      {isUploadVersionOpen && (
        <div className="fixed inset-0 bg-slate-900/60 backdrop-blur-sm z-50 flex items-center justify-center p-4">
          <div className="bg-white dark:bg-slate-900 rounded-2xl border border-slate-200 dark:border-slate-800 p-6 w-full max-w-md shadow-2xl space-y-4">
            <h3 className="text-lg font-bold text-slate-900 dark:text-white">Upload New Version</h3>
            <form onSubmit={handleUploadVersionSubmit} className="space-y-4">
              <input
                type="file"
                required
                onChange={(e) => setVersionFile(e.target.files?.[0] || null)}
                className="w-full text-sm text-slate-500 file:mr-4 file:py-2 file:px-4 file:rounded-xl file:border-0 file:text-sm file:font-semibold file:bg-indigo-50 file:text-indigo-700 hover:file:bg-indigo-100"
              />
              <input
                type="text"
                placeholder="Changes summary (e.g. Updated Section 3.2)"
                value={changesSummary}
                onChange={(e) => setChangesSummary(e.target.value)}
                className="w-full px-3 py-2 bg-slate-50 dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-xl text-sm dark:text-white"
              />
              <div className="flex justify-end gap-2">
                <button type="button" onClick={() => setIsUploadVersionOpen(false)} className="px-4 py-2 text-sm text-slate-600">
                  Cancel
                </button>
                <button type="submit" className="px-4 py-2 bg-indigo-600 text-white text-sm font-medium rounded-xl">
                  Upload Version
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
