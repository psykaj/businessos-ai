import { Badge } from "@/components/ui/badge";
import { cn } from "@/lib/utils";

interface StatusBadgeProps {
  status: string | boolean;
  className?: string;
}

export function StatusBadge({ status, className }: StatusBadgeProps) {
  let label = String(status);
  let variantClass = "bg-muted text-muted-foreground";

  if (typeof status === "boolean") {
    label = status ? "Active" : "Inactive";
  }

  const normalizedStatus = label.toLowerCase();

  switch (normalizedStatus) {
    case "active":
    case "accepted":
    case "verified":
    case "published":
    case "completed":
      variantClass = "bg-green-100 text-green-800 dark:bg-green-900/30 dark:text-green-400";
      break;
    case "pending":
    case "draft":
      variantClass = "bg-yellow-100 text-yellow-800 dark:bg-yellow-900/30 dark:text-yellow-400";
      break;
    case "expired":
    case "revoked":
    case "inactive":
    case "failed":
      variantClass = "bg-red-100 text-red-800 dark:bg-red-900/30 dark:text-red-400";
      break;
    default:
      variantClass = "bg-muted text-muted-foreground";
      break;
  }

  return (
    <Badge variant="outline" className={cn("font-medium border-0", variantClass, className)}>
      {label}
    </Badge>
  );
}
