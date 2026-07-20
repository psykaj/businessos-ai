"use client";

import { useState, useRef, useEffect } from "react";
import { Search, Loader2 } from "lucide-react";
import { Input } from "@/components/ui/input";
import { useQuery } from "@tanstack/react-query";
import { crmService } from "@/lib/crm-service";
import Link from "next/link";

export function GlobalSearch() {
  const [query, setQuery] = useState("");
  const [isOpen, setIsOpen] = useState(false);
  const containerRef = useRef<HTMLDivElement>(null);

  // Close dropdown on outside click
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (containerRef.current && !containerRef.current.contains(event.target as Node)) {
        setIsOpen(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  const { data: results, isLoading } = useQuery({
    queryKey: ['crm-search', query],
    queryFn: () => crmService.search(query),
    enabled: query.length > 1,
  });

  const getUrlForEntity = (type: string, id: string) => {
    switch (type.toLowerCase()) {
      case 'lead': return `/dashboard/leads/${id}`;
      case 'contact': return `/dashboard/contacts/${id}`;
      case 'company': return `/dashboard/companies/${id}`;
      case 'deal': return `/dashboard/deals/${id}`;
      default: return '#';
    }
  };

  return (
    <div ref={containerRef} className="relative hidden lg:flex max-w-sm w-full mx-4 items-center z-50">
      <Search className="absolute left-3 h-4 w-4 text-muted-foreground" />
      <Input
        type="search"
        placeholder="Search across your workspace..."
        className="h-9 w-full rounded-full bg-muted/50 pl-9 pr-4 text-sm focus-visible:ring-1 focus-visible:bg-background transition-all"
        value={query}
        onChange={(e) => {
          setQuery(e.target.value);
          setIsOpen(true);
        }}
        onFocus={() => {
          if (query.length > 1) setIsOpen(true);
        }}
      />
      
      {/* Dropdown Results */}
      {isOpen && query.length > 1 && (
        <div className="absolute top-12 left-0 w-full bg-background border rounded-lg shadow-lg overflow-hidden max-h-96 overflow-y-auto">
          {isLoading ? (
            <div className="flex items-center justify-center p-4 text-muted-foreground">
              <Loader2 className="h-5 w-5 animate-spin mr-2" />
              <span className="text-sm">Searching...</span>
            </div>
          ) : results && results.length > 0 ? (
            <div className="py-2">
              <div className="px-3 pb-2 text-xs font-semibold text-muted-foreground uppercase tracking-wider">
                CRM Results
              </div>
              {results.map((result) => (
                <Link
                  key={`${result.entityType}-${result.entityId}`}
                  href={getUrlForEntity(result.entityType, result.entityId)}
                  onClick={() => setIsOpen(false)}
                  className="flex flex-col px-4 py-2 hover:bg-muted transition-colors"
                >
                  <div className="flex items-center justify-between">
                    <span className="text-sm font-medium">{result.title}</span>
                    <span className="text-[10px] bg-primary/10 text-primary px-1.5 py-0.5 rounded uppercase font-semibold">
                      {result.entityType}
                    </span>
                  </div>
                  <span className="text-xs text-muted-foreground line-clamp-1">{result.description}</span>
                </Link>
              ))}
            </div>
          ) : (
            <div className="p-4 text-center text-sm text-muted-foreground">
              No results found for &quot;{query}&quot;.
            </div>
          )}
        </div>
      )}
    </div>
  );
}
