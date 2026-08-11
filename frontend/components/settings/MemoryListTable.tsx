"use client";

import { useState } from "react";
import { useMemory } from "@/hooks/use-memory";
import { MemoryDto } from "@/types/memory";
import { format } from "date-fns";
import { BrainCircuit, Search, MoreVertical, Loader2, Sparkles, AlertCircle, Eye, PowerOff, Trash2 } from "lucide-react";
import { Input } from "@/components/ui/input";
import { Badge } from "@/components/ui/badge";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";

export function MemoryListTable() {
  const { useBusinessMemories, updateMemory, deleteMemory } = useMemory();
  const { data: memories, isLoading, error } = useBusinessMemories();
  const [searchQuery, setSearchQuery] = useState("");

  const filteredMemories = memories?.filter((m) =>
    m.title.toLowerCase().includes(searchQuery.toLowerCase()) ||
    m.content.toLowerCase().includes(searchQuery.toLowerCase()) ||
    m.memoryType.toLowerCase().includes(searchQuery.toLowerCase())
  );

  const getConfidenceBadge = (confidence: string) => {
    switch (confidence) {
      case "High": return <Badge variant="outline" className="text-emerald-600 border-emerald-500/30 bg-emerald-500/10">High</Badge>;
      case "Medium": return <Badge variant="outline" className="text-amber-600 border-amber-500/30 bg-amber-500/10">Medium</Badge>;
      case "Low": return <Badge variant="outline" className="text-rose-600 border-rose-500/30 bg-rose-500/10">Low</Badge>;
      default: return <Badge variant="outline">{confidence}</Badge>;
    }
  };

  if (isLoading) {
    return (
      <div className="flex h-64 items-center justify-center rounded-xl border border-border bg-card">
        <Loader2 className="h-8 w-8 animate-spin text-muted-foreground" />
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex h-64 flex-col items-center justify-center rounded-xl border border-red-500/20 bg-red-500/5 text-red-500">
        <AlertCircle className="h-8 w-8 mb-2" />
        <p className="text-sm font-medium">Failed to load business memory.</p>
      </div>
    );
  }

  if (!memories || memories.length === 0) {
    return (
      <div className="flex h-96 flex-col items-center justify-center rounded-xl border border-dashed border-border bg-card px-4 text-center">
        <div className="flex h-16 w-16 items-center justify-center rounded-full bg-indigo-500/10 text-indigo-500 mb-4">
          <Sparkles className="h-8 w-8" />
        </div>
        <h3 className="text-lg font-semibold text-foreground mb-1">BusinessOS AI is still learning</h3>
        <p className="text-sm text-muted-foreground max-w-sm">
          Use your business normally and BusinessOS AI will build useful context from important activity.
        </p>
      </div>
    );
  }

  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <div className="relative w-full max-w-sm">
          <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
          <Input
            type="search"
            placeholder="Search memory..."
            className="pl-9 bg-card"
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
          />
        </div>
      </div>

      <div className="rounded-xl border border-border overflow-hidden bg-card">
        <div className="overflow-x-auto">
          <table className="w-full text-sm text-left">
            <thead className="text-xs text-muted-foreground bg-muted/50 border-b border-border">
              <tr>
                <th className="px-4 py-3 font-medium">Memory</th>
                <th className="px-4 py-3 font-medium">Type</th>
                <th className="px-4 py-3 font-medium">Source</th>
                <th className="px-4 py-3 font-medium">Confidence</th>
                <th className="px-4 py-3 font-medium">Created</th>
                <th className="px-4 py-3 font-medium">Status</th>
                <th className="px-4 py-3 font-medium text-right">Actions</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-border">
              {filteredMemories?.map((memory) => (
                <tr key={memory.id} className="hover:bg-muted/30 transition-colors">
                  <td className="px-4 py-3 align-top">
                    <p className="font-medium text-foreground">{memory.title}</p>
                    <p className="text-xs text-muted-foreground line-clamp-1 mt-0.5" title={memory.content}>
                      {memory.content}
                    </p>
                  </td>
                  <td className="px-4 py-3 align-top">
                    <Badge variant="secondary" className="font-normal">{memory.memoryType.replace(/([A-Z])/g, ' $1').trim()}</Badge>
                  </td>
                  <td className="px-4 py-3 align-top text-muted-foreground">
                    {memory.sourceModule}
                  </td>
                  <td className="px-4 py-3 align-top">
                    {getConfidenceBadge(memory.confidence)}
                  </td>
                  <td className="px-4 py-3 align-top text-muted-foreground">
                    {format(new Date(memory.createdAt), "MMM d, yyyy")}
                  </td>
                  <td className="px-4 py-3 align-top">
                    {memory.isActive ? (
                      <span className="flex items-center gap-1.5 text-emerald-600 dark:text-emerald-400">
                        <span className="h-1.5 w-1.5 rounded-full bg-emerald-500"></span> Active
                      </span>
                    ) : (
                      <span className="flex items-center gap-1.5 text-muted-foreground">
                        <span className="h-1.5 w-1.5 rounded-full bg-slate-400"></span> Inactive
                      </span>
                    )}
                  </td>
                  <td className="px-4 py-3 align-top text-right">
                    <DropdownMenu>
                      <DropdownMenuTrigger className="p-1.5 rounded-md hover:bg-muted text-muted-foreground">
                        <MoreVertical className="h-4 w-4" />
                      </DropdownMenuTrigger>
                      <DropdownMenuContent align="end" className="w-40">
                        <DropdownMenuLabel>Actions</DropdownMenuLabel>
                        <DropdownMenuSeparator />
                        <DropdownMenuItem className="gap-2 cursor-pointer">
                          <Eye className="h-4 w-4" /> View Details
                        </DropdownMenuItem>
                        <DropdownMenuItem 
                          className="gap-2 cursor-pointer"
                          onClick={() => updateMemory({ id: memory.id, data: { isActive: !memory.isActive } })}
                        >
                          <PowerOff className="h-4 w-4" /> {memory.isActive ? 'Deactivate' : 'Activate'}
                        </DropdownMenuItem>
                        <DropdownMenuSeparator />
                        <DropdownMenuItem 
                          className="gap-2 text-red-600 focus:text-red-600 cursor-pointer"
                          onClick={() => deleteMemory(memory.id)}
                        >
                          <Trash2 className="h-4 w-4" /> Delete
                        </DropdownMenuItem>
                      </DropdownMenuContent>
                    </DropdownMenu>
                  </td>
                </tr>
              ))}
              {filteredMemories?.length === 0 && (
                <tr>
                  <td colSpan={7} className="px-4 py-8 text-center text-muted-foreground">
                    No memories found matching your search.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}
