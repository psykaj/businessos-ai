"use client";

import { usePathname } from "next/navigation";
import { Menu, Search, Bell, ChevronRight } from "lucide-react";
import { Input } from "@/components/ui/input";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Badge } from "@/components/ui/badge";
import { ThemeToggle } from "@/components/layout/theme-toggle";
import { GlobalSearch } from "@/components/layout/global-search";

interface HeaderProps {
  onMenuClick: () => void;
}

export function Header({ onMenuClick }: HeaderProps) {
  const pathname = usePathname();
  
  // Generate Breadcrumbs
  const segments = pathname.split('/').filter(Boolean);
  const pageTitle = segments.length > 0 
    ? segments[segments.length - 1].charAt(0).toUpperCase() + segments[segments.length - 1].slice(1).replace('-', ' ')
    : 'Dashboard';

  return (
    <header className="sticky top-0 z-30 flex h-16 shrink-0 items-center gap-4 border-b border-border/50 bg-background/95 px-4 backdrop-blur-xl supports-[backdrop-filter]:bg-background/80 sm:px-6 shadow-sm">
      {/* Mobile menu trigger */}
      <button
        onClick={onMenuClick}
        className="flex h-9 w-9 items-center justify-center rounded-md text-muted-foreground hover:bg-muted hover:text-foreground lg:hidden"
        aria-label="Toggle sidebar"
      >
        <Menu className="h-5 w-5" />
      </button>

      {/* Breadcrumbs (Desktop) */}
      <div className="hidden lg:flex items-center gap-2 text-sm font-medium">
        <span className="text-muted-foreground">App</span>
        <ChevronRight className="h-4 w-4 text-muted-foreground/50" />
        <span className="text-foreground capitalize">{pageTitle}</span>
      </div>

      {/* Mobile brand (visible when sidebar is hidden) */}
      <span className="text-base font-bold tracking-tight text-foreground lg:hidden">
        BusinessOS
        <span className="ml-1 text-xs font-semibold text-primary">AI</span>
      </span>

      {/* Spacer */}
      <div className="flex-1" />

      {/* Global Search (Desktop) */}
      <GlobalSearch />

      {/* Right section */}
      <div className="flex items-center gap-2 sm:gap-3">
        {/* Mobile search */}
        <button
          className="flex h-9 w-9 items-center justify-center rounded-full text-muted-foreground hover:bg-muted hover:text-foreground lg:hidden"
          aria-label="Search"
        >
          <Search className="h-4 w-4" />
        </button>

        {/* Theme toggle */}
        <ThemeToggle />

        {/* Notifications */}
        <button
          className="relative flex h-9 w-9 items-center justify-center rounded-full text-muted-foreground transition-colors hover:bg-muted hover:text-foreground"
          aria-label="Notifications"
        >
          <Bell className="h-[18px] w-[18px]" />
          <Badge
            variant="destructive"
            className="absolute -right-0.5 -top-0.5 flex h-4 min-w-4 items-center justify-center rounded-full px-1 text-[10px] font-semibold border-2 border-background"
          >
            3
          </Badge>
        </button>

        {/* Profile */}
        <div className="h-8 w-px bg-border mx-1 hidden sm:block"></div>
        <Avatar className="h-8 w-8 cursor-pointer ring-2 ring-transparent transition-all hover:ring-primary/20">
          <AvatarFallback className="bg-gradient-to-br from-primary to-primary/80 text-xs font-semibold text-primary-foreground">
            PY
          </AvatarFallback>
        </Avatar>
      </div>
    </header>
  );
}
