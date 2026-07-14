import { FileText } from "lucide-react";

export default function InvoicesPage() {
  return (
    <div className="flex flex-col gap-6">
      <div>
        <h1 className="text-2xl font-semibold tracking-tight text-foreground">
          Invoices
        </h1>
        <p className="mt-1 text-sm text-muted-foreground">
          Create, send, and track invoices for your business.
        </p>
      </div>

      <div className="flex h-96 flex-col items-center justify-center rounded-xl border border-dashed border-border bg-card">
        <FileText className="h-12 w-12 text-muted-foreground/40" />
        <p className="mt-4 text-sm font-medium text-muted-foreground">
          Invoice management coming soon
        </p>
        <p className="mt-1 text-xs text-muted-foreground/70">
          Professional invoicing with payment tracking
        </p>
      </div>
    </div>
  );
}
