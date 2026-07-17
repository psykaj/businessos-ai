import { QRForm } from "@/components/qr/qr-form";

export default function CreateQRCodePage() {
  return (
    <div className="flex flex-col gap-6 max-w-6xl">
      <div>
        <h1 className="text-2xl font-semibold tracking-tight text-foreground">
          Create QR Code
        </h1>
        <p className="mt-1 text-sm text-muted-foreground">
          Design and configure your new dynamic QR code.
        </p>
      </div>

      <div className="mt-4">
        <QRForm />
      </div>
    </div>
  );
}
