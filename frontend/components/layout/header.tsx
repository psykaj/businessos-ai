"use client";

import { Menu, Search, Bell } from "lucide-react";
import { Input } from "@/components/ui/input";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Badge } from "@/components/ui/badge";
import { ThemeToggle } from "@/components/layout/theme-toggle";

interface HeaderProps {
  onMenuClick: () => void;
}

export function Header({ onMenuClick }: HeaderProps) {
  return (
    <header className="sticky top-0 z-30 flex h-16 shrink-0 items-center gap-4 border-b border-border bg-background/95 px-4 backdrop-blur-sm supports-[backdrop-filter]:bg-background/80 sm:px-6">
      {/* Mobile menu trigger */}
      <button
        onClick={onMenuClick}
        className="flex h-9 w-9 items-center justify-center rounded-md text-muted-foreground hover:bg-muted hover:text-foreground lg:hidden"
        aria-label="Toggle sidebar"
      >
        <Menu className="h-5 w-5" />
      </button>

      {/* Mobile brand (visible when sidebar is hidden) */}
      <span className="text-sm font-semibold text-foreground lg:hidden">
        BusinessOS
        <span className="ml-1 text-xs font-medium text-muted-foreground">AI</span>
      </span>

      {/* Search */}
      <div className="relative hidden flex-1 sm:block sm:max-w-md">
        <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
        <Input
          type="search"
          placeholder="Search anything…"
          className="h-9 pl-9 text-sm"
        />
      </div>

      {/* Spacer */}
      <div className="flex-1 sm:hidden" />

      {/* Right section */}
      <div className="flex items-center gap-1 sm:gap-2">
        {/* Mobile search */}
        <button
          className="flex h-9 w-9 items-center justify-center rounded-md text-muted-foreground hover:bg-muted hover:text-foreground sm:hidden"
          aria-label="Search"
        >
          <Search className="h-4 w-4" />
        </button>

        {/* Theme toggle */}
        <ThemeToggle />

        {/* Notifications */}
        <button
          className="relative flex h-9 w-9 items-center justify-center rounded-md text-muted-foreground hover:bg-muted hover:text-foreground"
          aria-label="Notifications"
        >
          <Bell className="h-[18px] w-[18px]" />
          <Badge
            variant="destructive"
            className="absolute -right-0.5 -top-0.5 flex h-4 min-w-4 items-center justify-center rounded-full px-1 text-[10px] font-semibold"
          >
            3
          </Badge>
        </button>

        {/* Profile */}
        <Avatar className="h-8 w-8 cursor-pointer">
          <AvatarFallback className="bg-primary/10 text-xs font-medium text-primary">
            PY
          </AvatarFallback>
        </Avatar>
      </div>
    </header>
  );
}
