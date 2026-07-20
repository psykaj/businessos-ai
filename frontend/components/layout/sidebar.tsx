"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import {
  LayoutDashboard,
  QrCode,
  Star,
  FileText,
  Users,
  BarChart3,
  MessageCircle,
  Bot,
  Settings,
  X,
  LogOut,
  CreditCard,
  Crown,
  Receipt,
  ArrowUpCircle,
  Sparkles,
  ShieldAlert,
  KeyRound,
  FileClock,
  Building,
  Palette,
  Globe,
  LayoutTemplate,
  Paintbrush,
  Search,
  Image as ImageIcon,
  Mail,
  Bell,
  Zap,
  Briefcase,
  Target,
  Building2,
  UserCircle,
  Kanban,
  CheckSquare,
  CalendarDays,
  Activity,
  Tags
} from "lucide-react";
import { cn } from "@/lib/utils";
import { useAuth } from "@/contexts/auth-context";
import { Button } from "@/components/ui/button";

const navGroups = [
  {
    title: "Overview",
    items: [
      { label: "Dashboard", href: "/dashboard", icon: LayoutDashboard },
      { label: "Analytics", href: "/dashboard/analytics", icon: BarChart3 },
      { label: "Customers", href: "/dashboard/customers", icon: Users },
    ]
  },
  {
    title: "CRM & Sales",
    items: [
      { label: "CRM Dashboard", href: "/dashboard/crm", icon: Briefcase },
      { label: "Leads", href: "/dashboard/leads", icon: Target },
      { label: "Contacts", href: "/dashboard/contacts", icon: UserCircle },
      { label: "Companies", href: "/dashboard/companies", icon: Building2 },
      { label: "Sales Pipeline", href: "/dashboard/deals", icon: Kanban },
      { label: "Tasks", href: "/dashboard/tasks", icon: CheckSquare },
      { label: "Calendar", href: "/dashboard/calendar", icon: CalendarDays },
      { label: "Activities", href: "/dashboard/activities", icon: Activity },
      { label: "Tags", href: "/dashboard/tags", icon: Tags },
    ]
  },
  {
    title: "Apps & Tools",
    items: [
      { label: "QR Codes", href: "/dashboard/qr", icon: QrCode },
      { label: "Reviews", href: "/dashboard/reviews", icon: Star },
      { label: "AI Assistant", href: "/dashboard/ai", icon: Bot },
    ]
  },
  {
    title: "Communication",
    items: [
      { label: "Email Center", href: "/dashboard/email", icon: Mail },
      { label: "WhatsApp", href: "/dashboard/whatsapp", icon: MessageCircle },
      { label: "Notifications", href: "/dashboard/notifications", icon: Bell },
    ]
  },
  {
    title: "Automation",
    items: [
      { label: "Workflows", href: "/dashboard/automation", icon: Zap },
    ]
  },
  {
    title: "Organization",
    items: [
      { label: "Team", href: "/dashboard/team", icon: Users },
      { label: "Roles & Permissions", href: "/dashboard/roles", icon: ShieldAlert },
      { label: "API Keys", href: "/dashboard/api-keys", icon: KeyRound },
      { label: "Audit Logs", href: "/dashboard/audit-logs", icon: FileClock },
    ]
  },
  {
    title: "White Label",
    items: [
      { label: "Branding", href: "/dashboard/branding", icon: Palette },
      { label: "Custom Domains", href: "/dashboard/domains", icon: Globe },
      { label: "Landing Pages", href: "/dashboard/landing-pages", icon: LayoutTemplate },
      { label: "Themes", href: "/dashboard/themes", icon: Paintbrush },
      { label: "SEO", href: "/dashboard/seo", icon: Search },
      { label: "Media Library", href: "/dashboard/media", icon: ImageIcon },
    ]
  },
  {
    title: "Billing & Settings",
    items: [
      { label: "Billing", href: "/dashboard/billing", icon: CreditCard },
      { label: "Subscription", href: "/dashboard/subscription", icon: Crown },
      { label: "Invoices", href: "/dashboard/invoices", icon: FileText },
      { label: "Payments", href: "/dashboard/payments", icon: Receipt },
      { label: "Pricing", href: "/pricing", icon: ArrowUpCircle },
      { label: "Org Settings", href: "/dashboard/organization", icon: Building },
      { label: "Profile", href: "/dashboard/profile", icon: Settings },
    ]
  }
];

interface SidebarProps {
  open: boolean;
  onClose: () => void;
}

export function Sidebar({ open, onClose }: SidebarProps) {
  const pathname = usePathname();
  const { user, logout } = useAuth();

  const isActive = (href: string) => {
    if (href === "/dashboard") {
      return pathname === "/dashboard";
    }
    return pathname.startsWith(href);
  };

  const handleLogout = async () => {
    onClose();
    await logout();
  };

  const initials = user?.fullName
    ? user.fullName
        .split(" ")
        .map((n) => n[0])
        .join("")
        .toUpperCase()
        .slice(0, 2)
    : "?";

  return (
    <>
      {/* Mobile overlay */}
      {open && (
        <div
          className="fixed inset-0 z-40 bg-black/50 backdrop-blur-sm lg:hidden"
          onClick={onClose}
          aria-hidden="true"
        />
      )}

      {/* Sidebar panel */}
      <aside
        className={cn(
          "fixed inset-y-0 left-0 z-50 flex w-72 flex-col border-r border-border bg-background/95 backdrop-blur-xl transition-transform duration-300 ease-in-out lg:static lg:translate-x-0 shadow-2xl lg:shadow-none",
          open ? "translate-x-0" : "-translate-x-full"
        )}
      >
        {/* Brand header */}
        <div className="flex h-16 shrink-0 items-center justify-between border-b border-border/50 px-6">
          <Link href="/dashboard" className="flex items-center gap-2.5 transition-opacity hover:opacity-80">
            <div className="flex h-9 w-9 items-center justify-center rounded-xl bg-gradient-to-br from-primary to-primary/80 shadow-lg shadow-primary/20">
              <span className="text-sm font-bold text-primary-foreground">B</span>
            </div>
            <span className="text-lg font-bold tracking-tight text-foreground">
              BusinessOS
              <span className="ml-1 text-xs font-semibold text-primary">AI</span>
            </span>
          </Link>
          <button
            onClick={onClose}
            className="flex h-8 w-8 items-center justify-center rounded-md text-muted-foreground hover:bg-muted hover:text-foreground lg:hidden"
            aria-label="Close sidebar"
          >
            <X className="h-4 w-4" />
          </button>
        </div>

        {/* Navigation */}
        <nav className="flex-1 overflow-y-auto px-4 py-6 scrollbar-hide">
          <div className="flex flex-col gap-8">
            {navGroups.map((group, groupIdx) => (
              <div key={groupIdx}>
                <h4 className="mb-2 px-2 text-xs font-semibold uppercase tracking-wider text-muted-foreground/70">
                  {group.title}
                </h4>
                <ul className="flex flex-col gap-1">
                  {group.items.map((item) => {
                    const Icon = item.icon;
                    const active = isActive(item.href);

                    return (
                      <li key={item.href}>
                        <Link
                          href={item.href}
                          onClick={onClose}
                          className={cn(
                            "group flex items-center gap-3 rounded-xl px-3 py-2.5 text-sm font-medium transition-all duration-200",
                            active
                              ? "bg-primary/10 text-primary shadow-sm"
                              : "text-muted-foreground hover:bg-muted hover:text-foreground"
                          )}
                        >
                          <Icon
                            className={cn(
                              "h-[18px] w-[18px] shrink-0 transition-colors duration-200",
                              active
                                ? "text-primary"
                                : "text-muted-foreground group-hover:text-foreground"
                            )}
                          />
                          {item.label}
                        </Link>
                      </li>
                    );
                  })}
                </ul>
              </div>
            ))}
          </div>
        </nav>

        {/* Upgrade Banner */}
        <div className="px-4 py-4">
          <div className="relative overflow-hidden rounded-2xl bg-gradient-to-br from-primary/10 to-primary/5 border border-primary/20 p-4 shadow-sm">
            <div className="absolute -right-4 -top-4 h-24 w-24 rounded-full bg-primary/10 blur-2xl"></div>
            <div className="relative z-10 flex flex-col gap-3">
              <div className="flex items-center gap-2">
                <Sparkles className="h-5 w-5 text-primary" />
                <h5 className="font-semibold text-foreground text-sm">Upgrade to Pro</h5>
              </div>
              <p className="text-xs text-muted-foreground">
                Unlock unlimited QR codes, advanced analytics, and premium AI credits.
              </p>
              <Link 
                href="/pricing" 
                onClick={onClose}
                className="inline-flex items-center justify-center whitespace-nowrap rounded-lg text-sm font-medium transition-colors w-full bg-primary hover:bg-primary/90 text-primary-foreground shadow-md h-9 px-4 py-2"
              >
                Upgrade Now
              </Link>
            </div>
          </div>
        </div>

        {/* Footer — user info + logout */}
        <div className="border-t border-border/50 p-4">
          <div className="flex items-center gap-3 rounded-xl p-2 transition-colors hover:bg-muted/50">
            <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-full bg-gradient-to-br from-primary/20 to-primary/10 text-xs font-bold text-primary ring-1 ring-primary/20">
              {initials}
            </div>
            <div className="flex min-w-0 flex-1 flex-col">
              <span className="truncate text-sm font-semibold text-foreground">
                {user?.fullName ?? "Loading…"}
              </span>
              <span className="truncate text-xs font-medium text-muted-foreground">
                {user?.email ?? ""}
              </span>
            </div>
            <button
              onClick={handleLogout}
              className="flex h-9 w-9 shrink-0 items-center justify-center rounded-lg text-muted-foreground transition-all hover:bg-destructive/10 hover:text-destructive"
              aria-label="Sign out"
              title="Sign out"
            >
              <LogOut className="h-[18px] w-[18px]" />
            </button>
          </div>
        </div>
      </aside>
    </>
  );
}
