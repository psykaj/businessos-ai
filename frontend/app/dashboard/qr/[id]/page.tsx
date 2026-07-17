"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { qrService } from "@/lib/qr-service";
import { QRCodeDto } from "@/types/qr";
import { Skeleton } from "@/components/ui/skeleton";
import { toast } from "sonner";
import { Button, buttonVariants } from "@/components/ui/button";
import { ArrowLeft, Edit, Download, Trash, BarChart2 } from "lucide-react";
import Link from "next/link";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { QRPreview } from "@/components/qr/qr-preview";
import { toPng, toSvg } from "html-to-image";

export default function QRDetailsPage() {
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
        toast.error("Failed to load QR Code details");
        router.push("/dashboard/qr");
      } finally {
        setLoading(false);
      }
    };

    fetchQR();
  }, [id, router]);

  const handleDownload = async (format: "png" | "svg") => {
    if (!data) return;
    const node = document.getElementById("qr-preview-container");
    if (!node) {
      toast.error("Preview container not found");
      return;
    }
    
    try {
      let dataUrl;
      if (format === 'png') {
        dataUrl = await toPng(node, { quality: 1, pixelRatio: 2 });
      } else {
        dataUrl = await toSvg(node);
      }
      
      const link = document.createElement("a");
      link.href = dataUrl;
      link.download = `qrcode-${data.id}.${format}`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      toast.success(`Downloaded ${format.toUpperCase()} successfully`);
    } catch (error) {
      toast.error(`Failed to download ${format.toUpperCase()}`);
    }
  };

  const handleDelete = async () => {
    if (!data || !confirm("Are you sure you want to delete this QR code?")) return;
    try {
      await qrService.deleteQRCode(data.id);
      toast.success("QR Code deleted successfully");
      router.push("/dashboard/qr");
    } catch (error) {
      toast.error("Failed to delete QR Code");
    }
  };

  if (loading) {
    return (
      <div className="flex flex-col gap-6 max-w-6xl">
        <Skeleton className="h-8 w-1/3" />
        <Skeleton className="h-[400px] w-full mt-4" />
      </div>
    );
  }

  if (!data) return null;

  return (
    <div className="flex flex-col gap-6 max-w-6xl">
      <div className="flex items-center justify-between">
        <div>
          <Button variant="ghost" onClick={() => router.back()} className="-ml-4 mb-2 text-muted-foreground">
            <ArrowLeft className="mr-2 h-4 w-4" /> Back
          </Button>
          <div className="flex items-center gap-3">
            <h1 className="text-2xl font-semibold tracking-tight text-foreground">
              {data.name}
            </h1>
            <Badge variant={data.status === "Active" ? "default" : "secondary"}>{data.status}</Badge>
          </div>
          <p className="mt-1 text-sm text-muted-foreground">
            {data.description || "No description provided."}
          </p>
        </div>
        <div className="flex items-center gap-3">
          <Link href={`/dashboard/qr/edit/${data.id}`} className={buttonVariants({ variant: "outline" })}>
            <Edit className="mr-2 h-4 w-4" /> Edit
          </Link>
          <Button variant="destructive" onClick={handleDelete}>
            <Trash className="mr-2 h-4 w-4" /> Delete
          </Button>
        </div>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <Card className="col-span-1">
          <CardHeader>
            <CardTitle>QR Preview</CardTitle>
            <CardDescription>Scan to verify destination</CardDescription>
          </CardHeader>
          <CardContent className="flex flex-col items-center justify-center space-y-6">
            <div className="w-full flex items-center justify-center">
              <QRPreview 
                foregroundColor={data.foregroundColor}
                backgroundColor={data.backgroundColor}
                size={data.size}
                originalValue={data.originalValue}
                margin={data.margin}
                errorCorrectionLevel={data.errorCorrectionLevel}
                labelText={data.labelText}
                labelFont={data.labelFont}
              />
            </div>
            
            <div className="w-full space-y-2">
              <Button className="w-full" variant="outline" onClick={() => handleDownload("png")}>
                <Download className="mr-2 h-4 w-4" /> Download PNG
              </Button>
              <Button className="w-full" variant="outline" onClick={() => handleDownload("svg")}>
                <Download className="mr-2 h-4 w-4" /> Download SVG
              </Button>
            </div>
          </CardContent>
        </Card>

        <Card className="col-span-1 md:col-span-2">
          <CardHeader>
            <CardTitle>Details</CardTitle>
            <CardDescription>Configuration and metadata</CardDescription>
          </CardHeader>
          <CardContent className="space-y-6">
            <div className="grid grid-cols-2 gap-4 text-sm">
              <div>
                <p className="text-muted-foreground">Short Code</p>
                <p className="font-medium">{data.shortCode}</p>
              </div>
              <div>
                <p className="text-muted-foreground">Type</p>
                <p className="font-medium">{data.qrType}</p>
              </div>
              <div className="col-span-2">
                <p className="text-muted-foreground">Destination Value</p>
                <a href={data.qrType === "Website" ? data.originalValue : "#"} target="_blank" rel="noreferrer" className="font-medium text-primary hover:underline break-all">
                  {data.originalValue}
                </a>
              </div>
              <div>
                <p className="text-muted-foreground">Folder</p>
                <p className="font-medium">{data.folder || "Uncategorized"}</p>
              </div>
              <div>
                <p className="text-muted-foreground">Tags</p>
                <div className="flex gap-1 mt-1 flex-wrap">
                  {data.tags && data.tags.length > 0 ? (
                    data.tags.map(tag => (
                      <Badge key={tag} variant="secondary" className="text-xs">{tag}</Badge>
                    ))
                  ) : (
                    <span className="font-medium">-</span>
                  )}
                </div>
              </div>
              <div>
                <p className="text-muted-foreground">Colors</p>
                <div className="flex items-center gap-2 mt-1">
                  <div className="w-4 h-4 rounded-full border" style={{ backgroundColor: data.foregroundColor }} title="Foreground"></div>
                  <div className="w-4 h-4 rounded-full border" style={{ backgroundColor: data.backgroundColor }} title="Background"></div>
                </div>
              </div>
              <div>
                <p className="text-muted-foreground">Password Protected</p>
                <p className="font-medium">{data.passwordProtected ? "Yes" : "No"}</p>
              </div>
              <div>
                <p className="text-muted-foreground">Created</p>
                <p className="font-medium">{new Date(data.createdAt).toLocaleString()}</p>
              </div>
              <div>
                <p className="text-muted-foreground">Last Updated</p>
                <p className="font-medium">{new Date(data.updatedAt).toLocaleString()}</p>
              </div>
            </div>
          </CardContent>
        </Card>
      </div>

      <Card>
        <CardHeader>
          <CardTitle className="flex items-center">
            <BarChart2 className="mr-2 h-5 w-5" /> Analytics
          </CardTitle>
          <CardDescription>Total Scans: <span className="font-bold">{data.scanCount}</span></CardDescription>
        </CardHeader>
        <CardContent>
          <div className="h-64 flex items-center justify-center border border-dashed rounded-lg bg-muted/20">
            <p className="text-muted-foreground text-sm">Detailed analytics charts coming soon.</p>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
