"use client";

import { useEffect, useState } from "react";
import { Button, buttonVariants } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Plus, Search, Filter } from "lucide-react";
import Link from "next/link";
import { qrService } from "@/lib/qr-service";
import { QRCodeDto } from "@/types/qr";
import { QRTable } from "@/components/qr/qr-table";
import { Skeleton } from "@/components/ui/skeleton";
import { toast } from "sonner";

export default function QRCodesPage() {
  const [data, setData] = useState<QRCodeDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState("");

  const fetchData = async () => {
    try {
      setLoading(true);
      const result = await qrService.getQRCodes({ search });
      setData(result.items);
    } catch (error) {
      toast.error("Failed to load QR Codes");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    // Debounce search slightly
    const delayDebounceFn = setTimeout(() => {
      fetchData();
    }, 300);

    return () => clearTimeout(delayDebounceFn);
  }, [search]);

  return (
    <div className="flex flex-col gap-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-semibold tracking-tight text-foreground">
            QR Codes
          </h1>
          <p className="mt-1 text-sm text-muted-foreground">
            Create, manage, and track your dynamic QR codes.
          </p>
        </div>
        <Link href="/dashboard/qr/create" className={buttonVariants()}>
          <Plus className="mr-2 h-4 w-4" /> Create QR Code
        </Link>
      </div>

      <div className="flex items-center gap-4">
        <div className="relative flex-1 max-w-sm">
          <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
          <Input
            placeholder="Search QR Codes..."
            className="pl-9"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />
        </div>
        <Button variant="outline" className="hidden sm:flex">
          <Filter className="mr-2 h-4 w-4" /> Filters
        </Button>
      </div>

      {loading ? (
        <div className="space-y-4">
          <Skeleton className="h-10 w-full" />
          <Skeleton className="h-16 w-full" />
          <Skeleton className="h-16 w-full" />
          <Skeleton className="h-16 w-full" />
        </div>
      ) : (
        <QRTable data={data} onRefresh={fetchData} />
      )}
    </div>
  );
}
