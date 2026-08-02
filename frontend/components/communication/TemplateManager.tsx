"use client";

import React, { useState } from "react";
import {
  FileText,
  Search,
  Plus,
  Edit2,
  Trash2,
  Zap,
  Tag,
  Copy,
  Check,
  Code2,
  SlidersHorizontal,
  FolderOpen,
  Sparkles,
  Loader2
} from "lucide-react";
import { useMessageTemplates, useCreateTemplate, useDeleteTemplate } from "@/hooks/use-communication";
import { MessageTemplateDto, CommunicationChannelType } from "@/lib/communication-service";
import { ChannelBadge } from "./ChannelBadge";
import { Button } from "@/components/ui/button";
import { toast } from "sonner";

const CATEGORIES = ["All", "General", "Sales & Upgrades", "Technical Support", "Logistics & Shipping", "Billing & Finance"];
const DYNAMIC_VARIABLES = ["{{customer.name}}", "{{customer.company}}", "{{customer.email}}", "{{agent.name}}", "{{ticket.id}}", "{{company.name}}"];

export function TemplateManager() {
  const [selectedCategory, setSelectedCategory] = useState("All");
  const [searchKeyword, setSearchKeyword] = useState("");
  const [showCreateModal, setShowCreateModal] = useState(false);

  // Form State
  const [title, setTitle] = useState("");
  const [category, setCategory] = useState("General");
  const [shortcutCode, setShortcutCode] = useState("");
  const [content, setContent] = useState("");
  const [channelType, setChannelType] = useState<CommunicationChannelType | "">("");
  const [copiedId, setCopiedId] = useState<string | null>(null);

  const { data: templates, isLoading } = useMessageTemplates(undefined, selectedCategory === "All" ? undefined : selectedCategory);
  const createMutation = useCreateTemplate();
  const deleteMutation = useDeleteTemplate();

  const handleInsertVariable = (variable: string) => {
    setContent(prev => `${prev} ${variable}`);
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!title || !content) {
      toast.error("Please provide both title and template message content.");
      return;
    }
    createMutation.mutate(
      {
        title,
        content,
        category,
        shortcutCode: shortcutCode || `/${title.toLowerCase().replace(/[^a-z0-9]/g, "-")}`,
        channelType: channelType || undefined,
        parametersJson: JSON.stringify(DYNAMIC_VARIABLES.filter(v => content.includes(v)))
      },
      {
        onSuccess: () => {
          setShowCreateModal(false);
          setTitle("");
          setContent("");
          setShortcutCode("");
        }
      }
    );
  };

  const handleCopyContent = (id: string, text: string) => {
    navigator.clipboard.writeText(text);
    setCopiedId(id);
    toast.success("Copied template text to clipboard!");
    setTimeout(() => setCopiedId(null), 2000);
  };

  const filtered = (templates || []).filter(t =>
    t.title.toLowerCase().includes(searchKeyword.toLowerCase()) ||
    t.content.toLowerCase().includes(searchKeyword.toLowerCase()) ||
    t.shortcutCode.toLowerCase().includes(searchKeyword.toLowerCase())
  );

  return (
    <div className="w-full flex flex-col gap-6 p-6 bg-card/50 border border-border/60 rounded-2xl shadow-sm backdrop-blur-xs">
      {/* Header & New Template button */}
      <div className="flex flex-col sm:flex-row items-start sm:items-center justify-between gap-4 pb-4 border-b border-border/60">
        <div className="flex items-center gap-3">
          <div className="p-3 bg-indigo-500/10 text-indigo-500 rounded-2xl flex items-center justify-center shadow-xs">
            <FileText className="w-6 h-6" />
          </div>
          <div>
            <h1 className="text-xl font-bold text-foreground">Omnichannel Message Templates</h1>
            <p className="text-xs text-muted-foreground">
              Save agent time and ensure brand tone consistency with variable-driven canned responses (&ldquo;&#123;&#123;customer.name&#125;&#125;&rdquo;).
            </p>
          </div>
        </div>

        <Button
          onClick={() => setShowCreateModal(true)}
          className="bg-primary hover:bg-primary/90 text-primary-foreground text-xs font-bold px-4 py-2 h-9 rounded-xl gap-1.5 shadow-sm"
        >
          <Plus className="w-4 h-4" />
          <span>Create Template</span>
        </Button>
      </div>

      {/* Filter Tabs & Search */}
      <div className="flex flex-col md:flex-row items-center justify-between gap-3">
        <div className="flex items-center gap-1.5 overflow-x-auto pb-1 w-full md:w-auto">
          {CATEGORIES.map(cat => {
            const active = selectedCategory === cat;
            return (
              <button
                key={cat}
                onClick={() => setSelectedCategory(cat)}
                className={`px-3 py-1.5 rounded-lg text-xs font-semibold transition-all whitespace-nowrap ${
                  active ? "bg-primary text-primary-foreground shadow-xs" : "bg-muted/60 text-muted-foreground hover:bg-muted"
                }`}
              >
                {cat}
              </button>
            );
          })}
        </div>

        <div className="relative w-full md:w-80">
          <Search className="w-4 h-4 text-muted-foreground absolute left-3 top-1/2 -translate-y-1/2 pointer-events-none" />
          <input
            type="text"
            value={searchKeyword}
            onChange={e => setSearchKeyword(e.target.value)}
            placeholder="Search template title, content or /shortcut..."
            className="w-full pl-9 pr-4 py-1.5 text-xs bg-background border border-border rounded-lg text-foreground focus:outline-hidden focus:ring-1 focus:ring-primary placeholder:text-muted-foreground"
          />
        </div>
      </div>

      {/* Templates Grid / List */}
      {isLoading ? (
        <div className="py-20 text-center text-muted-foreground"><Loader2 className="w-8 h-8 animate-spin mx-auto mb-2 text-primary" /></div>
      ) : filtered.length > 0 ? (
        <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-4">
          {filtered.map(t => (
            <div
              key={t.id}
              className="flex flex-col justify-between p-5 rounded-2xl bg-card border border-border/70 shadow-2xs hover:shadow-md transition-all group duration-200 space-y-3"
            >
              <div className="space-y-2">
                <div className="flex items-center justify-between gap-2">
                  <span className="text-2xs font-bold uppercase tracking-wider text-muted-foreground px-2 py-0.5 rounded bg-muted">
                    {t.category}
                  </span>
                  {t.channelType && <ChannelBadge type={t.channelType} size="sm" showLabel={false} />}
                </div>

                <h3 className="text-sm font-bold text-foreground group-hover:text-primary transition-colors truncate">
                  {t.title}
                </h3>
                <div className="inline-flex items-center gap-1 text-2xs font-mono font-semibold px-2 py-0.5 rounded bg-primary/10 text-primary">
                  <Zap className="w-3 h-3 text-amber-500" />
                  <span>{t.shortcutCode}</span>
                </div>

                <p className="text-xs text-muted-foreground line-clamp-4 font-sans bg-muted/30 p-3 rounded-xl border border-border/40 leading-relaxed">
                  {t.content}
                </p>
              </div>

              <div className="flex items-center justify-between pt-3 border-t border-border/40 text-2xs text-muted-foreground">
                <span className="font-medium">Used {t.usageCount} times this month</span>
                <div className="flex items-center gap-1">
                  <Button
                    variant="ghost"
                    size="icon"
                    onClick={() => handleCopyContent(t.id, t.content)}
                    className="w-7 h-7 hover:text-foreground"
                    title="Copy Text"
                  >
                    {copiedId === t.id ? <Check className="w-3.5 h-3.5 text-emerald-500" /> : <Copy className="w-3.5 h-3.5" />}
                  </Button>
                  <Button
                    variant="ghost"
                    size="icon"
                    onClick={() => deleteMutation.mutate(t.id)}
                    className="w-7 h-7 text-muted-foreground hover:text-rose-500"
                    title="Delete Template"
                  >
                    <Trash2 className="w-3.5 h-3.5" />
                  </Button>
                </div>
              </div>
            </div>
          ))}
        </div>
      ) : (
        <div className="py-20 text-center border border-dashed border-border rounded-2xl text-muted-foreground text-xs">
          No templates found matching your criteria. Click &ldquo;Create Template&rdquo; to define a new canned response!
        </div>
      )}

      {/* Creation Modal */}
      {showCreateModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-xs p-4">
          <form
            onSubmit={handleSubmit}
            className="w-full max-w-lg bg-card border border-border rounded-2xl p-6 shadow-2xl space-y-4 text-xs animate-in zoom-in-95 duration-150"
          >
            <h2 className="text-base font-bold text-foreground flex items-center gap-2">
              <Plus className="w-5 h-5 text-primary" />
              <span>New Canned Message Template</span>
            </h2>

            <div className="grid grid-cols-2 gap-3">
              <div className="space-y-1">
                <label className="font-bold text-foreground">Template Title *</label>
                <input
                  type="text"
                  required
                  value={title}
                  onChange={e => setTitle(e.target.value)}
                  placeholder="e.g. VIP Enterprise Quote Answer"
                  className="w-full bg-background border border-border rounded-lg p-2 text-foreground focus:outline-hidden focus:ring-1 focus:ring-primary"
                />
              </div>
              <div className="space-y-1">
                <label className="font-bold text-foreground">Category</label>
                <select
                  value={category}
                  onChange={e => setCategory(e.target.value)}
                  className="w-full bg-background border border-border rounded-lg p-2 text-foreground focus:outline-hidden"
                >
                  {CATEGORIES.filter(c => c !== "All").map(c => (
                    <option key={c} value={c}>
                      {c}
                    </option>
                  ))}
                </select>
              </div>
            </div>

            <div className="grid grid-cols-2 gap-3">
              <div className="space-y-1">
                <label className="font-bold text-foreground">Shortcut Command (optional)</label>
                <input
                  type="text"
                  value={shortcutCode}
                  onChange={e => setShortcutCode(e.target.value)}
                  placeholder="e.g. /enterprise-quote"
                  className="w-full bg-background border border-border rounded-lg p-2 text-foreground font-mono text-2xs"
                />
              </div>
              <div className="space-y-1">
                <label className="font-bold text-foreground">Target Channel (optional)</label>
                <select
                  value={channelType}
                  onChange={e => setChannelType(e.target.value as any)}
                  className="w-full bg-background border border-border rounded-lg p-2 text-foreground"
                >
                  <option value="">Any Channel</option>
                  <option value="WhatsApp">WhatsApp</option>
                  <option value="Email">Email</option>
                  <option value="LiveChat">Live Chat Widget</option>
                  <option value="SMS">SMS Text</option>
                </select>
              </div>
            </div>

            <div className="space-y-2">
              <div className="flex items-center justify-between font-bold text-foreground">
                <span>Message Content *</span>
                <span className="text-2xs font-normal text-muted-foreground">Click pills to insert dynamic fields:</span>
              </div>
              <div className="flex flex-wrap gap-1.5 mb-1">
                {DYNAMIC_VARIABLES.map(v => (
                  <button
                    key={v}
                    type="button"
                    onClick={() => handleInsertVariable(v)}
                    className="px-2 py-0.5 rounded bg-primary/10 hover:bg-primary/20 text-primary font-mono text-2xs font-bold transition-colors"
                  >
                    {v}
                  </button>
                ))}
              </div>
              <textarea
                required
                rows={5}
                value={content}
                onChange={e => setContent(e.target.value)}
                placeholder="Hi {{customer.name}}, thank you for contacting us regarding..."
                className="w-full bg-background border border-border rounded-xl p-3 text-foreground font-sans focus:outline-hidden focus:ring-1 focus:ring-primary text-xs resize-none leading-relaxed"
              />
            </div>

            <div className="flex items-center justify-end gap-2 pt-3 border-t border-border/60">
              <Button type="button" variant="ghost" onClick={() => setShowCreateModal(false)} className="text-xs">
                Cancel
              </Button>
              <Button type="submit" disabled={createMutation.isPending} className="bg-primary text-primary-foreground text-xs font-bold">
                {createMutation.isPending ? "Saving..." : "Save Template"}
              </Button>
            </div>
          </form>
        </div>
      )}
    </div>
  );
}
