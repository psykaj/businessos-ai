"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import {
  Sparkles,
  History,
  Lightbulb,
  Activity,
  Settings,
  HelpCircle,
} from "lucide-react";
import { cn } from "@/lib/utils";

const navItems = [
  { name: "Copilot Chat", href: "/dashboard/copilot", icon: Sparkles },
  { name: "History", href: "/dashboard/copilot/history", icon: History },
  { name: "Recommendations", href: "/dashboard/copilot/recommendations", icon: Lightbulb },
  { name: "Activity Logs", href: "/dashboard/copilot/actions", icon: Activity },
  { name: "Settings", href: "/dashboard/copilot/settings", icon: Settings },
  { name: "Guide & Help", href: "/dashboard/copilot/help", icon: HelpCircle },
];

export function CopilotNav() {
  const pathname = usePathname();

  return (
    <div className="border-b border-border/50 bg-background/60 backdrop-blur-md px-6 py-3">
      <div className="flex flex-wrap items-center justify-between gap-4">
        <div className="flex items-center gap-3">
          <div className="flex h-9 w-9 items-center justify-center rounded-xl bg-gradient-to-tr from-indigo-600 via-purple-600 to-pink-500 text-white shadow-md shadow-indigo-500/20">
            <Sparkles className="h-5 w-5 animate-pulse" />
          </div>
          <div>
            <h1 className="text-lg font-bold tracking-tight text-foreground flex items-center gap-2">
              AI Business Copilot
              <span className="rounded-full bg-indigo-500/10 px-2.5 py-0.5 text-xs font-semibold text-indigo-500 border border-indigo-500/20">
                Enterprise
              </span>
            </h1>
            <p className="text-xs text-muted-foreground">
              Smart Workspace & Natural Language Command Center
            </p>
          </div>
        </div>

        <nav className="flex items-center gap-1 overflow-x-auto scrollbar-none py-1">
          {navItems.map((item) => {
            const Icon = item.icon;
            const isActive = pathname === item.href;
            return (
              <Link
                key={item.href}
                href={item.href}
                className={cn(
                  "flex items-center gap-2 rounded-lg px-3 py-1.5 text-xs font-medium transition-all duration-150 whitespace-nowrap",
                  isActive
                    ? "bg-indigo-600/10 text-indigo-600 dark:text-indigo-400 font-semibold shadow-sm"
                    : "text-muted-foreground hover:bg-muted hover:text-foreground"
                )}
              >
                <Icon className={cn("h-4 w-4", isActive ? "text-indigo-600 dark:text-indigo-400" : "text-muted-foreground")} />
                {item.name}
              </Link>
            );
          })}
        </nav>
      </div>
    </div>
  );
}
