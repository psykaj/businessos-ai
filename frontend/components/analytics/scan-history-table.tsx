"use client";

import { ScanHistory, PagedResult } from "@/types/analytics";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Search, ChevronLeft, ChevronRight } from "lucide-react";
import { format, parseISO } from "date-fns";
import { Skeleton } from "@/components/ui/skeleton";
import { Badge } from "@/components/ui/badge";

interface ScanHistoryTableProps {
  data?: PagedResult<ScanHistory>;
  isLoading: boolean;
  search: string;
  onSearchChange: (val: string) => void;
  page: number;
  onPageChange: (page: number) => void;
}

export function ScanHistoryTable({
  data,
  isLoading,
  search,
  onSearchChange,
  page,
  onPageChange,
}: ScanHistoryTableProps) {
  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <h3 className="text-lg font-semibold tracking-tight">Scan History</h3>
        <div className="relative w-72">
          <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
          <Input
            placeholder="Search by QR name, device, country..."
            className="pl-9 bg-background"
            value={search}
            onChange={(e) => onSearchChange(e.target.value)}
          />
        </div>
      </div>

      <div className="rounded-md border bg-card overflow-hidden">
        <Table>
          <TableHeader className="bg-muted/50">
            <TableRow>
              <TableHead>Date & Time</TableHead>
              <TableHead>QR Code</TableHead>
              <TableHead>Location</TableHead>
              <TableHead>Device / OS</TableHead>
              <TableHead>Browser</TableHead>
              <TableHead>UTM Campaign</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {isLoading ? (
              Array.from({ length: 5 }).map((_, i) => (
                <TableRow key={i}>
                  <TableCell><Skeleton className="h-4 w-24" /></TableCell>
                  <TableCell><Skeleton className="h-4 w-32" /></TableCell>
                  <TableCell><Skeleton className="h-4 w-20" /></TableCell>
                  <TableCell><Skeleton className="h-4 w-28" /></TableCell>
                  <TableCell><Skeleton className="h-4 w-20" /></TableCell>
                  <TableCell><Skeleton className="h-4 w-16" /></TableCell>
                </TableRow>
              ))
            ) : data && data.items.length > 0 ? (
              data.items.map((scan) => (
                <TableRow key={scan.id}>
                  <TableCell className="font-medium whitespace-nowrap">
                    {format(parseISO(scan.scanDateTime), "MMM d, yyyy HH:mm")}
                  </TableCell>
                  <TableCell>{scan.qrCodeName}</TableCell>
                  <TableCell>
                    {scan.city && scan.country ? `${scan.city}, ${scan.country}` : scan.country || "Unknown"}
                  </TableCell>
                  <TableCell>
                    <div className="flex flex-col">
                      <span>{scan.deviceType || "Unknown"}</span>
                      <span className="text-xs text-muted-foreground">{scan.operatingSystem}</span>
                    </div>
                  </TableCell>
                  <TableCell>{scan.browser || "Unknown"}</TableCell>
                  <TableCell>
                    {scan.utmCampaign ? (
                      <Badge variant="secondary" className="font-normal">{scan.utmCampaign}</Badge>
                    ) : (
                      <span className="text-muted-foreground">-</span>
                    )}
                  </TableCell>
                </TableRow>
              ))
            ) : (
              <TableRow>
                <TableCell colSpan={6} className="h-24 text-center text-muted-foreground">
                  No scan records found.
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </div>

      {/* Pagination */}
      {data && data.totalPages > 1 && (
        <div className="flex items-center justify-between px-2">
          <div className="text-sm text-muted-foreground">
            Showing {(page - 1) * data.pageSize + 1} to{" "}
            {Math.min(page * data.pageSize, data.totalItems)} of {data.totalItems} entries
          </div>
          <div className="flex items-center space-x-2">
            <Button
              variant="outline"
              size="sm"
              onClick={() => onPageChange(page - 1)}
              disabled={page <= 1}
            >
              <ChevronLeft className="h-4 w-4 mr-1" /> Previous
            </Button>
            <div className="text-sm font-medium">
              Page {page} of {data.totalPages}
            </div>
            <Button
              variant="outline"
              size="sm"
              onClick={() => onPageChange(page + 1)}
              disabled={page >= data.totalPages}
            >
              Next <ChevronRight className="h-4 w-4 ml-1" />
            </Button>
          </div>
        </div>
      )}
    </div>
  );
}
