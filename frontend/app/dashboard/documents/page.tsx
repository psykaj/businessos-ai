"use client";

import { useState } from "react";
import Link from "next/link";
import {
  FileText,
  FolderPlus,
  UploadCloud,
  Search,
  Grid,
  List as ListIcon,
  Star,
  Download,
  Share2,
  Trash2,
  Folder as FolderIcon,
  Clock,
  CheckCircle2,
  FileCheck2,
  MoreVertical,
  Plus,
  X,
  Lock,
  Tag as TagIcon,
  ChevronRight,
  Eye,
  RefreshCw,
  HardDrive
} from "lucide-react";
import {
  useDocuments,
  useFolders,
  useCreateFolder,
  useDeleteFolder,
  useUploadDocument,
  useToggleFavorite,
  useDeleteDocument
} from "@/hooks/use-documents";
import { DocumentDto, FolderDto } from "@/types/document";

export default function DocumentCenterPage() {
  const [currentFolderId, setCurrentFolderId] = useState<string | undefined>(undefined);
  const [folderBreadcrumbs, setFolderBreadcrumbs] = useState<{ id?: string; name: string }[]>([
    { name: "Root" }
  ]);
  const [viewMode, setViewMode] = useState<"grid" | "list">("grid");
  const [searchQuery, setSearchQuery] = useState("");
  const [selectedTag, setSelectedTag] = useState<string | undefined>(undefined);
  const [onlyFavorites, setOnlyFavorites] = useState(false);

  // Modals state
  const [isUploadOpen, setIsUploadOpen] = useState(false);
  const [isNewFolderOpen, setIsNewFolderOpen] = useState(false);
  const [newFolderName, setNewFolderName] = useState("");
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [fileDescription, setFileDescription] = useState("");
  const [fileTags, setFileTags] = useState("");
  const [uploadProgress, setUploadProgress] = useState<number | null>(null);

  // Data fetching
  const { data: documentsData, isLoading: docsLoading, refetch: refetchDocs } = useDocuments({
    folderId: currentFolderId,
    query: searchQuery || undefined,
    tag: selectedTag || undefined,
    isFavorite: onlyFavorites ? true : undefined,
  });

  const { data: folders, isLoading: foldersLoading } = useFolders(currentFolderId);

  // Mutations
  const createFolderMutation = useCreateFolder();
  const deleteFolderMutation = useDeleteFolder();
  const uploadDocMutation = useUploadDocument();
  const toggleFavoriteMutation = useToggleFavorite();
  const deleteDocMutation = useDeleteDocument();

  // Navigation handlers
  const handleOpenFolder = (folder: FolderDto) => {
    setCurrentFolderId(folder.id);
    setFolderBreadcrumbs((prev) => [...prev, { id: folder.id, name: folder.name }]);
  };

  const handleBreadcrumbClick = (index: number) => {
    const updated = folderBreadcrumbs.slice(0, index + 1);
    setFolderBreadcrumbs(updated);
    setCurrentFolderId(updated[updated.length - 1].id);
  };

  const handleCreateFolder = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newFolderName.trim()) return;
    await createFolderMutation.mutateAsync({
      name: newFolderName,
      parentFolderId: currentFolderId,
    });
    setNewFolderName("");
    setIsNewFolderOpen(false);
  };

  const handleUploadFile = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!selectedFile) return;

    setUploadProgress(20);
    const tagsArray = fileTags
      .split(",")
      .map((t) => t.trim())
      .filter((t) => t.length > 0);

    try {
      setUploadProgress(60);
      await uploadDocMutation.mutateAsync({
        file: selectedFile,
        folderId: currentFolderId,
        description: fileDescription,
        tags: tagsArray,
      });
      setUploadProgress(100);
      setTimeout(() => {
        setUploadProgress(null);
        setSelectedFile(null);
        setFileDescription("");
        setFileTags("");
        setIsUploadOpen(false);
      }, 500);
    } catch {
      setUploadProgress(null);
    }
  };

  const formatFileSize = (bytes: number) => {
    if (bytes === 0) return "0 B";
    const k = 1024;
    const sizes = ["B", "KB", "MB", "GB"];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(1)) + " " + sizes[i];
  };

  const documents = documentsData?.items || [];
  const totalCount = documentsData?.totalCount || 0;

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      {/* Top Header & Business Value Alert */}
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 bg-gradient-to-r from-slate-900 via-indigo-950 to-slate-900 p-6 rounded-2xl text-white shadow-xl">
        <div>
          <h1 className="text-2xl font-bold tracking-tight flex items-center gap-2">
            <FileText className="w-7 h-7 text-indigo-400" />
            Centralized Document Center
          </h1>
          <p className="text-slate-300 text-sm mt-1">
            Manage contracts, proposals, HR files, and e-signatures in one secure platform.
          </p>
        </div>
        <div className="flex items-center gap-3">
          <button
            onClick={() => setIsNewFolderOpen(true)}
            className="flex items-center gap-2 bg-slate-800 hover:bg-slate-700 text-slate-100 px-4 py-2.5 rounded-xl font-medium text-sm transition-all border border-slate-700 shadow-sm"
          >
            <FolderPlus className="w-4 h-4 text-indigo-400" />
            New Folder
          </button>
          <button
            onClick={() => setIsUploadOpen(true)}
            className="flex items-center gap-2 bg-indigo-600 hover:bg-indigo-500 text-white px-4 py-2.5 rounded-xl font-medium text-sm transition-all shadow-md shadow-indigo-600/30"
          >
            <UploadCloud className="w-4 h-4" />
            Upload File
          </button>
        </div>
      </div>

      {/* Overview Analytics Bar */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
        <div className="bg-white dark:bg-slate-900 p-4 rounded-xl border border-slate-200 dark:border-slate-800 shadow-sm flex items-center gap-4">
          <div className="p-3 bg-indigo-50 dark:bg-indigo-950/50 rounded-xl text-indigo-600 dark:text-indigo-400">
            <FileText className="w-6 h-6" />
          </div>
          <div>
            <div className="text-2xl font-bold text-slate-900 dark:text-white">{totalCount}</div>
            <div className="text-xs text-slate-500 font-medium">Total Documents</div>
          </div>
        </div>

        <div className="bg-white dark:bg-slate-900 p-4 rounded-xl border border-slate-200 dark:border-slate-800 shadow-sm flex items-center gap-4">
          <div className="p-3 bg-emerald-50 dark:bg-emerald-950/50 rounded-xl text-emerald-600 dark:text-emerald-400">
            <HardDrive className="w-6 h-6" />
          </div>
          <div>
            <div className="text-2xl font-bold text-slate-900 dark:text-white">
              {formatFileSize(documents.reduce((acc, d) => acc + (d.fileSize || 0), 0))}
            </div>
            <div className="text-xs text-slate-500 font-medium">Current Storage Used</div>
          </div>
        </div>

        <div className="bg-white dark:bg-slate-900 p-4 rounded-xl border border-slate-200 dark:border-slate-800 shadow-sm flex items-center gap-4">
          <div className="p-3 bg-amber-50 dark:bg-amber-950/50 rounded-xl text-amber-600 dark:text-amber-400">
            <Clock className="w-6 h-6" />
          </div>
          <div>
            <div className="text-2xl font-bold text-slate-900 dark:text-white">
              {documents.filter((d) => d.status === "InReview").length}
            </div>
            <div className="text-xs text-slate-500 font-medium">In Approval Review</div>
          </div>
        </div>

        <div className="bg-white dark:bg-slate-900 p-4 rounded-xl border border-slate-200 dark:border-slate-800 shadow-sm flex items-center gap-4">
          <div className="p-3 bg-purple-50 dark:bg-purple-950/50 rounded-xl text-purple-600 dark:text-purple-400">
            <FileCheck2 className="w-6 h-6" />
          </div>
          <div>
            <div className="text-2xl font-bold text-slate-900 dark:text-white">
              {documents.filter((d) => d.status === "Signed" || d.status === "PendingSignature").length}
            </div>
            <div className="text-xs text-slate-500 font-medium">Signature Workflows</div>
          </div>
        </div>
      </div>

      {/* Breadcrumb Navigation & Controls */}
      <div className="bg-white dark:bg-slate-900 p-4 rounded-xl border border-slate-200 dark:border-slate-800 shadow-sm flex flex-col md:flex-row md:items-center justify-between gap-4">
        {/* Breadcrumb */}
        <div className="flex items-center gap-1.5 overflow-x-auto text-sm text-slate-600 dark:text-slate-400 font-medium">
          {folderBreadcrumbs.map((crumb, idx) => (
            <div key={crumb.id || "root"} className="flex items-center gap-1.5 whitespace-nowrap">
              {idx > 0 && <ChevronRight className="w-4 h-4 text-slate-400" />}
              <button
                onClick={() => handleBreadcrumbClick(idx)}
                className={`hover:text-indigo-600 dark:hover:text-indigo-400 transition-colors ${
                  idx === folderBreadcrumbs.length - 1 ? "font-semibold text-slate-900 dark:text-white" : ""
                }`}
              >
                {crumb.name}
              </button>
            </div>
          ))}
        </div>

        {/* Search, Filter & View Controls */}
        <div className="flex items-center gap-3">
          <div className="relative flex-1 md:w-64">
            <Search className="w-4 h-4 absolute left-3 top-1/2 -translate-y-1/2 text-slate-400" />
            <input
              type="text"
              placeholder="Search files & tags..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              className="w-full pl-9 pr-4 py-2 bg-slate-50 dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 dark:text-white"
            />
          </div>

          <button
            onClick={() => setOnlyFavorites(!onlyFavorites)}
            className={`p-2 rounded-xl border transition-all ${
              onlyFavorites
                ? "bg-amber-50 border-amber-300 text-amber-600 dark:bg-amber-950/40 dark:border-amber-700"
                : "border-slate-200 dark:border-slate-700 text-slate-600 dark:text-slate-400 hover:bg-slate-50 dark:hover:bg-slate-800"
            }`}
            title="Toggle Favorites Only"
          >
            <Star className={`w-4 h-4 ${onlyFavorites ? "fill-amber-400 text-amber-400" : ""}`} />
          </button>

          <div className="flex items-center bg-slate-100 dark:bg-slate-800 p-1 rounded-xl border border-slate-200 dark:border-slate-700">
            <button
              onClick={() => setViewMode("grid")}
              className={`p-1.5 rounded-lg transition-all ${
                viewMode === "grid" ? "bg-white dark:bg-slate-700 shadow-sm text-indigo-600 dark:text-indigo-400" : "text-slate-500"
              }`}
            >
              <Grid className="w-4 h-4" />
            </button>
            <button
              onClick={() => setViewMode("list")}
              className={`p-1.5 rounded-lg transition-all ${
                viewMode === "list" ? "bg-white dark:bg-slate-700 shadow-sm text-indigo-600 dark:text-indigo-400" : "text-slate-500"
              }`}
            >
              <ListIcon className="w-4 h-4" />
            </button>
          </div>
        </div>
      </div>

      {/* Folders Section */}
      {folders && folders.length > 0 && (
        <div className="space-y-3">
          <h2 className="text-xs font-bold uppercase tracking-wider text-slate-500">Folders</h2>
          <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-6 gap-4">
            {folders.map((folder) => (
              <div
                key={folder.id}
                onClick={() => handleOpenFolder(folder)}
                className="group bg-white dark:bg-slate-900 p-4 rounded-xl border border-slate-200 dark:border-slate-800 hover:border-indigo-500 dark:hover:border-indigo-500 hover:shadow-md transition-all cursor-pointer relative"
              >
                <div className="flex items-center justify-between mb-2">
                  <div className="p-2.5 bg-indigo-50 dark:bg-indigo-950/50 rounded-xl text-indigo-600 dark:text-indigo-400">
                    <FolderIcon className="w-5 h-5 fill-indigo-100 dark:fill-indigo-950" />
                  </div>
                  <button
                    onClick={(e) => {
                      e.stopPropagation();
                      deleteFolderMutation.mutate(folder.id);
                    }}
                    className="opacity-0 group-hover:opacity-100 p-1 text-slate-400 hover:text-red-600 transition-opacity"
                    title="Delete Folder"
                  >
                    <Trash2 className="w-4 h-4" />
                  </button>
                </div>
                <div className="font-semibold text-sm text-slate-900 dark:text-white truncate">{folder.name}</div>
                <div className="text-xs text-slate-400 mt-1">
                  {folder.subFolderCount} folders · {folder.documentCount} files
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      {/* Documents Section */}
      <div className="space-y-3">
        <h2 className="text-xs font-bold uppercase tracking-wider text-slate-500">Documents ({documents.length})</h2>

        {docsLoading ? (
          <div className="p-12 text-center text-slate-500 flex flex-col items-center justify-center gap-2 bg-white dark:bg-slate-900 rounded-xl border border-slate-200 dark:border-slate-800">
            <RefreshCw className="w-6 h-6 animate-spin text-indigo-500" />
            <div>Loading documents...</div>
          </div>
        ) : documents.length === 0 ? (
          <div className="p-12 text-center bg-white dark:bg-slate-900 rounded-2xl border border-dashed border-slate-300 dark:border-slate-800 space-y-3">
            <div className="w-12 h-12 bg-indigo-50 dark:bg-indigo-950/50 text-indigo-600 rounded-full flex items-center justify-center mx-auto">
              <FileText className="w-6 h-6" />
            </div>
            <div className="text-lg font-semibold text-slate-900 dark:text-white">No documents found</div>
            <p className="text-sm text-slate-500 max-w-sm mx-auto">
              Upload contracts, proposals, or HR files to get started with seamless document management.
            </p>
            <button
              onClick={() => setIsUploadOpen(true)}
              className="inline-flex items-center gap-2 bg-indigo-600 text-white px-4 py-2 rounded-xl text-sm font-medium hover:bg-indigo-500 transition-all"
            >
              <UploadCloud className="w-4 h-4" /> Upload First Document
            </button>
          </div>
        ) : viewMode === "grid" ? (
          <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
            {documents.map((doc) => (
              <div
                key={doc.id}
                className="bg-white dark:bg-slate-900 rounded-xl border border-slate-200 dark:border-slate-800 p-4 hover:shadow-lg transition-all flex flex-col justify-between group"
              >
                <div>
                  <div className="flex items-start justify-between gap-2 mb-3">
                    <div className="p-2.5 bg-slate-100 dark:bg-slate-800 rounded-xl text-slate-700 dark:text-slate-300">
                      <FileText className="w-6 h-6 text-indigo-500" />
                    </div>
                    <div className="flex items-center gap-1">
                      <button
                        onClick={() => toggleFavoriteMutation.mutate(doc.id)}
                        className={`p-1.5 rounded-lg text-slate-400 hover:text-amber-500 ${
                          doc.isFavorite ? "text-amber-500 fill-amber-400" : ""
                        }`}
                      >
                        <Star className={`w-4 h-4 ${doc.isFavorite ? "fill-amber-400" : ""}`} />
                      </button>
                      <button
                        onClick={() => deleteDocMutation.mutate(doc.id)}
                        className="p-1.5 rounded-lg text-slate-400 hover:text-red-600 opacity-0 group-hover:opacity-100 transition-opacity"
                      >
                        <Trash2 className="w-4 h-4" />
                      </button>
                    </div>
                  </div>

                  <Link href={`/dashboard/documents/${doc.id}`} className="block group-hover:text-indigo-600 transition-colors">
                    <h3 className="font-semibold text-slate-900 dark:text-white truncate text-sm" title={doc.name}>
                      {doc.name}
                    </h3>
                  </Link>

                  <div className="text-xs text-slate-400 mt-1 flex items-center gap-2">
                    <span>{formatFileSize(doc.fileSize)}</span>
                    <span>•</span>
                    <span>v{doc.versionCount}</span>
                  </div>

                  {doc.tags && doc.tags.length > 0 && (
                    <div className="flex flex-wrap gap-1 mt-3">
                      {doc.tags.map((tag) => (
                        <span
                          key={tag}
                          className="text-[10px] bg-slate-100 dark:bg-slate-800 text-slate-600 dark:text-slate-300 px-2 py-0.5 rounded-md font-medium"
                        >
                          #{tag}
                        </span>
                      ))}
                    </div>
                  )}
                </div>

                <div className="mt-4 pt-3 border-t border-slate-100 dark:border-slate-800 flex items-center justify-between">
                  <span
                    className={`text-[11px] px-2 py-0.5 rounded-full font-medium ${
                      doc.status === "Active"
                        ? "bg-emerald-50 text-emerald-600 dark:bg-emerald-950/50 dark:text-emerald-400"
                        : doc.status === "InReview"
                        ? "bg-amber-50 text-amber-600 dark:bg-amber-950/50 dark:text-amber-400"
                        : doc.status === "Signed"
                        ? "bg-indigo-50 text-indigo-600 dark:bg-indigo-950/50 dark:text-indigo-400"
                        : "bg-slate-100 text-slate-600 dark:bg-slate-800 dark:text-slate-400"
                    }`}
                  >
                    {doc.status}
                  </span>

                  <Link
                    href={`/dashboard/documents/${doc.id}`}
                    className="text-xs text-indigo-600 dark:text-indigo-400 font-medium hover:underline flex items-center gap-1"
                  >
                    View <Eye className="w-3.5 h-3.5" />
                  </Link>
                </div>
              </div>
            ))}
          </div>
        ) : (
          /* List View */
          <div className="bg-white dark:bg-slate-900 rounded-xl border border-slate-200 dark:border-slate-800 overflow-hidden shadow-sm">
            <table className="w-full text-left border-collapse">
              <thead>
                <tr className="border-b border-slate-200 dark:border-slate-800 text-xs font-semibold text-slate-400 uppercase bg-slate-50 dark:bg-slate-800/50">
                  <th className="p-4">Name</th>
                  <th className="p-4">Size</th>
                  <th className="p-4">Version</th>
                  <th className="p-4">Status</th>
                  <th className="p-4 text-right">Actions</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-100 dark:divide-slate-800 text-sm">
                {documents.map((doc) => (
                  <tr key={doc.id} className="hover:bg-slate-50 dark:hover:bg-slate-800/50 transition-colors">
                    <td className="p-4">
                      <div className="flex items-center gap-3">
                        <FileText className="w-5 h-5 text-indigo-500 shrink-0" />
                        <div>
                          <Link href={`/dashboard/documents/${doc.id}`} className="font-semibold text-slate-900 dark:text-white hover:text-indigo-600">
                            {doc.name}
                          </Link>
                          {doc.description && <div className="text-xs text-slate-400">{doc.description}</div>}
                        </div>
                      </div>
                    </td>
                    <td className="p-4 text-slate-500 text-xs">{formatFileSize(doc.fileSize)}</td>
                    <td className="p-4 text-slate-500 text-xs">v{doc.versionCount}</td>
                    <td className="p-4">
                      <span className="text-xs px-2.5 py-1 rounded-full font-medium bg-slate-100 dark:bg-slate-800 text-slate-700 dark:text-slate-300">
                        {doc.status}
                      </span>
                    </td>
                    <td className="p-4 text-right">
                      <div className="flex items-center justify-end gap-2">
                        <button
                          onClick={() => toggleFavoriteMutation.mutate(doc.id)}
                          className={`p-1.5 rounded-lg text-slate-400 hover:text-amber-500 ${
                            doc.isFavorite ? "text-amber-500 fill-amber-400" : ""
                          }`}
                        >
                          <Star className={`w-4 h-4 ${doc.isFavorite ? "fill-amber-400" : ""}`} />
                        </button>
                        <Link href={`/dashboard/documents/${doc.id}`} className="p-1.5 text-slate-400 hover:text-indigo-600">
                          <Eye className="w-4 h-4" />
                        </Link>
                        <button onClick={() => deleteDocMutation.mutate(doc.id)} className="p-1.5 text-slate-400 hover:text-red-600">
                          <Trash2 className="w-4 h-4" />
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      {/* New Folder Modal */}
      {isNewFolderOpen && (
        <div className="fixed inset-0 bg-slate-900/60 backdrop-blur-sm z-50 flex items-center justify-center p-4">
          <div className="bg-white dark:bg-slate-900 rounded-2xl border border-slate-200 dark:border-slate-800 p-6 w-full max-w-md shadow-2xl space-y-4">
            <div className="flex items-center justify-between">
              <h3 className="text-lg font-bold text-slate-900 dark:text-white flex items-center gap-2">
                <FolderPlus className="w-5 h-5 text-indigo-500" /> Create New Folder
              </h3>
              <button onClick={() => setIsNewFolderOpen(false)} className="text-slate-400 hover:text-slate-600">
                <X className="w-5 h-5" />
              </button>
            </div>
            <form onSubmit={handleCreateFolder} className="space-y-4">
              <div>
                <label className="text-xs font-semibold text-slate-500 block mb-1">Folder Name</label>
                <input
                  type="text"
                  required
                  placeholder="e.g. Legal Contracts"
                  value={newFolderName}
                  onChange={(e) => setNewFolderName(e.target.value)}
                  className="w-full px-3.5 py-2.5 bg-slate-50 dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-xl text-sm dark:text-white"
                />
              </div>
              <div className="flex items-center justify-end gap-2 pt-2">
                <button
                  type="button"
                  onClick={() => setIsNewFolderOpen(false)}
                  className="px-4 py-2 rounded-xl text-sm font-medium text-slate-600 dark:text-slate-400 hover:bg-slate-100 dark:hover:bg-slate-800"
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  disabled={createFolderMutation.isPending}
                  className="px-4 py-2 bg-indigo-600 hover:bg-indigo-500 text-white rounded-xl text-sm font-medium"
                >
                  {createFolderMutation.isPending ? "Creating..." : "Create Folder"}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Upload File Modal */}
      {isUploadOpen && (
        <div className="fixed inset-0 bg-slate-900/60 backdrop-blur-sm z-50 flex items-center justify-center p-4">
          <div className="bg-white dark:bg-slate-900 rounded-2xl border border-slate-200 dark:border-slate-800 p-6 w-full max-w-lg shadow-2xl space-y-4">
            <div className="flex items-center justify-between">
              <h3 className="text-lg font-bold text-slate-900 dark:text-white flex items-center gap-2">
                <UploadCloud className="w-5 h-5 text-indigo-500" /> Upload Document
              </h3>
              <button onClick={() => setIsUploadOpen(false)} className="text-slate-400 hover:text-slate-600">
                <X className="w-5 h-5" />
              </button>
            </div>

            <form onSubmit={handleUploadFile} className="space-y-4">
              <div className="border-2 border-dashed border-slate-300 dark:border-slate-700 rounded-xl p-6 text-center hover:border-indigo-500 transition-colors cursor-pointer bg-slate-50 dark:bg-slate-800/50">
                <input
                  type="file"
                  required
                  onChange={(e) => setSelectedFile(e.target.files?.[0] || null)}
                  className="hidden"
                  id="file-upload-input"
                />
                <label htmlFor="file-upload-input" className="cursor-pointer space-y-2 block">
                  <UploadCloud className="w-8 h-8 text-indigo-500 mx-auto" />
                  <div className="text-sm font-medium text-slate-900 dark:text-white">
                    {selectedFile ? selectedFile.name : "Click or drag file here to upload"}
                  </div>
                  <div className="text-xs text-slate-400">PDF, DOCX, PNG, JPG, HTML up to 50MB</div>
                </label>
              </div>

              <div>
                <label className="text-xs font-semibold text-slate-500 block mb-1">Description (Optional)</label>
                <input
                  type="text"
                  placeholder="e.g. Q1 Master Services Agreement"
                  value={fileDescription}
                  onChange={(e) => setFileDescription(e.target.value)}
                  className="w-full px-3.5 py-2.5 bg-slate-50 dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-xl text-sm dark:text-white"
                />
              </div>

              <div>
                <label className="text-xs font-semibold text-slate-500 block mb-1">Tags (Comma-separated)</label>
                <input
                  type="text"
                  placeholder="e.g. Legal, Contract, ClientA"
                  value={fileTags}
                  onChange={(e) => setFileTags(e.target.value)}
                  className="w-full px-3.5 py-2.5 bg-slate-50 dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-xl text-sm dark:text-white"
                />
              </div>

              {uploadProgress !== null && (
                <div className="space-y-1">
                  <div className="flex justify-between text-xs text-slate-500">
                    <span>Uploading...</span>
                    <span>{uploadProgress}%</span>
                  </div>
                  <div className="w-full bg-slate-200 dark:bg-slate-700 h-2 rounded-full overflow-hidden">
                    <div className="bg-indigo-600 h-full transition-all duration-300" style={{ width: `${uploadProgress}%` }} />
                  </div>
                </div>
              )}

              <div className="flex items-center justify-end gap-2 pt-2">
                <button
                  type="button"
                  onClick={() => setIsUploadOpen(false)}
                  className="px-4 py-2 rounded-xl text-sm font-medium text-slate-600 dark:text-slate-400 hover:bg-slate-100 dark:hover:bg-slate-800"
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  disabled={!selectedFile || uploadDocMutation.isPending}
                  className="px-4 py-2 bg-indigo-600 hover:bg-indigo-500 text-white rounded-xl text-sm font-medium"
                >
                  {uploadDocMutation.isPending ? "Uploading..." : "Start Upload"}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
