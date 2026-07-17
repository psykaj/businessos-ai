"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { QRForm } from "@/components/qr/qr-form";
import { qrService } from "@/lib/qr-service";
import { QRCodeDto } from "@/types/qr";
import { Skeleton } from "@/components/ui/skeleton";
import { toast } from "sonner";
import { Button } from "@/components/ui/button";
import { ArrowLeft } from "lucide-react";

export default function EditQRCodePage() {
  const params = useParams();
  const router = useRouter();
  const id = params.id as string;

  const [data, setData] = useState<QRCodeDto | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!id) return;
    
    const fetchQR = async () => {
      try {
        const qr = await qrService.getQRCode(id);
        setData(qr);
      } catch (error) {
        toast.error("Failed to load QR Code for editing");
        router.push("/dashboard/qr");
      } finally {
        setLoading(false);
      }
    };

    fetchQR();
  }, [id, router]);

  if (loading) {
    return (
      <div className="flex flex-col gap-6 max-w-6xl">
        <Skeleton className="h-8 w-1/3" />
        <Skeleton className="h-[600px] w-full mt-4" />
      </div>
    );
  }

  if (!data) return null;

  return (
    <div className="flex flex-col gap-6 max-w-6xl">
      <div>
        <Button variant="ghost" onClick={() => router.back()} className="-ml-4 mb-2 text-muted-foreground">
          <ArrowLeft className="mr-2 h-4 w-4" /> Back
        </Button>
        <h1 className="text-2xl font-semibold tracking-tight text-foreground">
          Edit QR Code
        </h1>
        <p className="mt-1 text-sm text-muted-foreground">
          Update configuration and destination for {data.name}.
        </p>
      </div>

      <div className="mt-4">
        <QRForm initialData={data} isEditing />
      </div>
    </div>
  );
}
