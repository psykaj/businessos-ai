import { QrCode } from "lucide-react";

export default function QRCodesPage() {
  return (
    <div className="flex flex-col gap-6">
      <div>
        <h1 className="text-2xl font-semibold tracking-tight text-foreground">
          QR Codes
        </h1>
        <p className="mt-1 text-sm text-muted-foreground">
          Create, manage, and track your dynamic QR codes.
        </p>
      </div>

      <div className="flex h-96 flex-col items-center justify-center rounded-xl border border-dashed border-border bg-card">
        <QrCode className="h-12 w-12 text-muted-foreground/40" />
        <p className="mt-4 text-sm font-medium text-muted-foreground">
          QR Code management coming soon
        </p>
        <p className="mt-1 text-xs text-muted-foreground/70">
          Generate dynamic QR codes with built-in analytics
        </p>
      </div>
    </div>
  );
}
