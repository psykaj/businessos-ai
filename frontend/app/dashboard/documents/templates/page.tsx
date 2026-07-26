"use client";

import { useState } from "react";
import {
  LayoutTemplate,
  Plus,
  FileCode,
  Sparkles,
  Search,
  CheckCircle2,
  Trash2,
  X,
  Play,
  FileText
} from "lucide-react";
import {
  useDocumentTemplates,
  useCreateTemplate,
  useRenderTemplate
} from "@/hooks/use-documents";
import { DocumentTemplateDto } from "@/types/document";

export default function DocumentTemplatesPage() {
  const [selectedCategory, setSelectedCategory] = useState<string | undefined>(undefined);
  const [searchQuery, setSearchQuery] = useState("");

  // Modals state
  const [isNewTemplateOpen, setIsNewTemplateOpen] = useState(false);
  const [selectedTemplateForRender, setSelectedTemplateForRender] = useState<DocumentTemplateDto | null>(null);

  // New Template Form
  const [name, setName] = useState("");
  const [category, setCategory] = useState("Contract");
  const [description, setDescription] = useState("");
  const [content, setContent] = useState("MASTER SERVICES AGREEMENT\n\nThis agreement is made between {{CompanyName}} and {{ClientName}} on {{AgreementDate}} for total value of {{Amount}}.");

  // Render Form
  const [renderDocName, setRenderDocName] = useState("");
  const [fieldValues, setFieldValues] = useState<Record<string, string>>({});

  // Data fetching
  const { data: templates, isLoading } = useDocumentTemplates(selectedCategory);
  const createTemplateMutation = useCreateTemplate();
  const renderTemplateMutation = useRenderTemplate();

  const categories = ["All", "Contract", "NDA", "Proposal", "Invoice", "HR"];

  const handleCreateTemplate = async (e: React.FormEvent) => {
    e.preventDefault();
    await createTemplateMutation.mutateAsync({
      name,
      category,
      content,
      description,
    });
    setName("");
    setDescription("");
    setIsNewTemplateOpen(false);
  };

  const handleOpenRenderModal = (template: DocumentTemplateDto) => {
    setSelectedTemplateForRender(template);
    setRenderDocName(`${template.name} - ${new Date().toLocaleDateString()}`);

    // Extract placeholders like {{FieldName}}
    const regex = /\{\{([^}]+)\}\}/g;
    const matches = Array.from(template.content.matchAll(regex)).map((m) => m[1]);
    const initialFields: Record<string, string> = {};
    matches.forEach((field) => {
      initialFields[field] = "";
    });
    setFieldValues(initialFields);
  };

  const handleRenderSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!selectedTemplateForRender) return;

    await renderTemplateMutation.mutateAsync({
      templateId: selectedTemplateForRender.id,
      documentName: renderDocName,
      fieldValues,
    });
    setSelectedTemplateForRender(null);
  };

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      {/* Header */}
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 bg-gradient-to-r from-slate-900 via-indigo-950 to-slate-900 p-6 rounded-2xl text-white shadow-xl">
        <div>
          <h1 className="text-2xl font-bold tracking-tight flex items-center gap-2">
            <LayoutTemplate className="w-7 h-7 text-indigo-400" />
            Document Templates Library
          </h1>
          <p className="text-slate-300 text-sm mt-1">
            Standardize proposals, contracts, and HR documents with reusable variable templates.
          </p>
        </div>
        <button
          onClick={() => setIsNewTemplateOpen(true)}
          className="flex items-center gap-2 bg-indigo-600 hover:bg-indigo-500 text-white px-4 py-2.5 rounded-xl font-medium text-sm transition-all shadow-md shadow-indigo-600/30"
        >
          <Plus className="w-4 h-4" /> Create Template
        </button>
      </div>

      {/* Category Pills & Search */}
      <div className="flex flex-col sm:flex-row items-center justify-between gap-4">
        <div className="flex items-center gap-2 overflow-x-auto w-full sm:w-auto">
          {categories.map((cat) => (
            <button
              key={cat}
              onClick={() => setSelectedCategory(cat === "All" ? undefined : cat)}
              className={`px-3.5 py-1.5 rounded-xl text-xs font-semibold whitespace-nowrap transition-all ${
                (cat === "All" && !selectedCategory) || selectedCategory === cat
                  ? "bg-indigo-600 text-white shadow-sm"
                  : "bg-white dark:bg-slate-900 text-slate-600 dark:text-slate-400 border border-slate-200 dark:border-slate-800 hover:bg-slate-50"
              }`}
            >
              {cat}
            </button>
          ))}
        </div>

        <div className="relative w-full sm:w-64">
          <Search className="w-4 h-4 absolute left-3 top-1/2 -translate-y-1/2 text-slate-400" />
          <input
            type="text"
            placeholder="Search templates..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            className="w-full pl-9 pr-4 py-2 bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 dark:text-white"
          />
        </div>
      </div>

      {/* Templates Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {templates?.map((template) => (
          <div
            key={template.id}
            className="bg-white dark:bg-slate-900 rounded-2xl border border-slate-200 dark:border-slate-800 p-5 shadow-sm hover:shadow-md transition-all flex flex-col justify-between"
          >
            <div className="space-y-3">
              <div className="flex items-center justify-between">
                <span className="text-xs px-2.5 py-0.5 rounded-full font-semibold bg-indigo-50 text-indigo-600 dark:bg-indigo-950/50 dark:text-indigo-400">
                  {template.category}
                </span>
                <span className="text-xs text-slate-400">Active</span>
              </div>

              <h3 className="font-bold text-slate-900 dark:text-white text-base">{template.name}</h3>
              <p className="text-xs text-slate-500 line-clamp-2">{template.description || "Reusable template."}</p>

              <div className="bg-slate-50 dark:bg-slate-800/50 p-3 rounded-xl border border-slate-100 dark:border-slate-800 text-xs font-mono text-slate-600 dark:text-slate-400 line-clamp-3">
                {template.content}
              </div>
            </div>

            <div className="mt-4 pt-3 border-t border-slate-100 dark:border-slate-800 flex items-center justify-between">
              <button
                onClick={() => handleOpenRenderModal(template)}
                className="w-full flex items-center justify-center gap-2 bg-indigo-50 hover:bg-indigo-100 dark:bg-indigo-950/50 dark:hover:bg-indigo-900 text-indigo-600 dark:text-indigo-400 font-semibold text-xs py-2.5 rounded-xl transition-all"
              >
                <Play className="w-3.5 h-3.5" /> Generate Document
              </button>
            </div>
          </div>
        ))}
      </div>

      {/* Create Template Modal */}
      {isNewTemplateOpen && (
        <div className="fixed inset-0 bg-slate-900/60 backdrop-blur-sm z-50 flex items-center justify-center p-4">
          <div className="bg-white dark:bg-slate-900 rounded-2xl border border-slate-200 dark:border-slate-800 p-6 w-full max-w-xl shadow-2xl space-y-4 max-h-[90vh] overflow-y-auto">
            <div className="flex items-center justify-between">
              <h3 className="text-lg font-bold text-slate-900 dark:text-white flex items-center gap-2">
                <LayoutTemplate className="w-5 h-5 text-indigo-500" /> Create Document Template
              </h3>
              <button onClick={() => setIsNewTemplateOpen(false)} className="text-slate-400 hover:text-slate-600">
                <X className="w-5 h-5" />
              </button>
            </div>

            <form onSubmit={handleCreateTemplate} className="space-y-4">
              <div>
                <label className="text-xs font-semibold text-slate-500 block mb-1">Template Name</label>
                <input
                  type="text"
                  required
                  placeholder="e.g. Master Services Agreement"
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                  className="w-full px-3.5 py-2.5 bg-slate-50 dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-xl text-sm dark:text-white"
                />
              </div>

              <div>
                <label className="text-xs font-semibold text-slate-500 block mb-1">Category</label>
                <select
                  value={category}
                  onChange={(e) => setCategory(e.target.value)}
                  className="w-full px-3.5 py-2.5 bg-slate-50 dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-xl text-sm dark:text-white"
                >
                  <option value="Contract">Contract</option>
                  <option value="NDA">NDA</option>
                  <option value="Proposal">Proposal</option>
                  <option value="Invoice">Invoice</option>
                  <option value="HR">HR</option>
                </select>
              </div>

              <div>
                <label className="text-xs font-semibold text-slate-500 block mb-1">Description</label>
                <input
                  type="text"
                  placeholder="Short summary of template purpose"
                  value={description}
                  onChange={(e) => setDescription(e.target.value)}
                  className="w-full px-3.5 py-2.5 bg-slate-50 dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-xl text-sm dark:text-white"
                />
              </div>

              <div>
                <label className="text-xs font-semibold text-slate-500 block mb-1">
                  Template Content (Use {"{{Placeholder}}"} for variables)
                </label>
                <textarea
                  rows={6}
                  required
                  value={content}
                  onChange={(e) => setContent(e.target.value)}
                  className="w-full p-3 font-mono text-xs bg-slate-50 dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-xl dark:text-white"
                />
              </div>

              <div className="flex justify-end gap-2 pt-2">
                <button type="button" onClick={() => setIsNewTemplateOpen(false)} className="px-4 py-2 text-sm text-slate-600">
                  Cancel
                </button>
                <button type="submit" className="px-4 py-2 bg-indigo-600 text-white rounded-xl text-sm font-medium">
                  Save Template
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Render Template Modal */}
      {selectedTemplateForRender && (
        <div className="fixed inset-0 bg-slate-900/60 backdrop-blur-sm z-50 flex items-center justify-center p-4">
          <div className="bg-white dark:bg-slate-900 rounded-2xl border border-slate-200 dark:border-slate-800 p-6 w-full max-w-lg shadow-2xl space-y-4">
            <div className="flex items-center justify-between">
              <h3 className="text-lg font-bold text-slate-900 dark:text-white flex items-center gap-2">
                <Play className="w-5 h-5 text-indigo-500" /> Fill Variables & Generate
              </h3>
              <button onClick={() => setSelectedTemplateForRender(null)} className="text-slate-400 hover:text-slate-600">
                <X className="w-5 h-5" />
              </button>
            </div>

            <form onSubmit={handleRenderSubmit} className="space-y-4">
              <div>
                <label className="text-xs font-semibold text-slate-500 block mb-1">Generated Document Name</label>
                <input
                  type="text"
                  required
                  value={renderDocName}
                  onChange={(e) => setRenderDocName(e.target.value)}
                  className="w-full px-3.5 py-2.5 bg-slate-50 dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-xl text-sm dark:text-white"
                />
              </div>

              <div className="space-y-3 pt-2">
                <div className="text-xs font-bold text-slate-500 uppercase tracking-wider">Template Variables</div>
                {Object.keys(fieldValues).length === 0 ? (
                  <div className="text-xs text-slate-400 italic">No variables found in template.</div>
                ) : (
                  Object.keys(fieldValues).map((key) => (
                    <div key={key}>
                      <label className="text-xs font-medium text-slate-600 dark:text-slate-300 block mb-1">{key}</label>
                      <input
                        type="text"
                        required
                        placeholder={`Enter ${key}`}
                        value={fieldValues[key]}
                        onChange={(e) => setFieldValues({ ...fieldValues, [key]: e.target.value })}
                        className="w-full px-3.5 py-2 bg-slate-50 dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-xl text-sm dark:text-white"
                      />
                    </div>
                  ))
                )}
              </div>

              <div className="flex justify-end gap-2 pt-3">
                <button type="button" onClick={() => setSelectedTemplateForRender(null)} className="px-4 py-2 text-sm text-slate-600">
                  Cancel
                </button>
                <button type="submit" disabled={renderTemplateMutation.isPending} className="px-4 py-2 bg-indigo-600 text-white rounded-xl text-sm font-medium">
                  {renderTemplateMutation.isPending ? "Generating..." : "Generate & Save Document"}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
