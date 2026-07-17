"use client";

import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger } from "@/components/ui/dropdown-menu";
import { Button, buttonVariants } from "@/components/ui/button";
import { MoreHorizontal, Eye, Edit, Trash, Download } from "lucide-react";
import { QRCodeDto } from "@/types/qr";
import { qrService } from "@/lib/qr-service";
import Link from "next/link";
import { useState } from "react";
import { toast } from "sonner";
import { Badge } from "@/components/ui/badge";
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from "@/components/ui/alert-dialog";

interface QRTableProps {
  data: QRCodeDto[];
  onRefresh: () => void;
}

export function QRTable({ data, onRefresh }: QRTableProps) {
  const [deleteId, setDeleteId] = useState<string | null>(null);

  const handleDelete = async () => {
    if (!deleteId) return;
    try {
      await qrService.deleteQRCode(deleteId);
      toast.success("QR Code deleted successfully");
      onRefresh();
    } catch (error) {
      toast.error("Failed to delete QR Code");
    } finally {
      setDeleteId(null);
    }
  };

  const handleDownload = async (id: string, format: "png" | "svg") => {
    try {
      await qrService.downloadQRImage(id, format);
      toast.success(`Downloaded ${format.toUpperCase()} successfully`);
    } catch (error) {
      toast.error(`Failed to download ${format.toUpperCase()}`);
    }
  };

  if (!data || data.length === 0) {
    return (
      <div className="flex flex-col items-center justify-center p-12 border border-dashed rounded-lg bg-card">
        <p className="text-muted-foreground mb-4">No QR Codes found.</p>
        <Link href="/dashboard/qr/create" className={buttonVariants()}>
          Create your first QR Code
        </Link>
      </div>
    );
  }

  return (
    <>
      <div className="rounded-md border bg-card overflow-hidden">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead className="w-[80px]">Preview</TableHead>
              <TableHead>Name</TableHead>
              <TableHead>Type</TableHead>
              <TableHead>Status</TableHead>
              <TableHead>Scans</TableHead>
              <TableHead className="hidden md:table-cell">Created</TableHead>
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data.map((qr) => (
              <TableRow key={qr.id}>
                <TableCell>
                  <div className="w-10 h-10 border rounded overflow-hidden bg-white">
                    <img src={`/api/qrcodes/${qr.id}/image?format=png`} alt={qr.name} className="w-full h-full object-cover" />
                  </div>
                </TableCell>
                <TableCell className="font-medium">
                  {qr.name}
                  {qr.folder && <span className="block text-xs text-muted-foreground mt-1">📁 {qr.folder}</span>}
                </TableCell>
                <TableCell>
                  <Badge variant="outline">{qr.qrType}</Badge>
                </TableCell>
                <TableCell>
                  <Badge variant={qr.status === "Active" ? "default" : "secondary"}>
                    {qr.status}
                  </Badge>
                </TableCell>
                <TableCell>{qr.scanCount}</TableCell>
                <TableCell className="hidden md:table-cell text-muted-foreground">
                  {new Date(qr.createdAt).toLocaleDateString()}
                </TableCell>
                <TableCell className="text-right">
                  <DropdownMenu>
                    <DropdownMenuTrigger className={buttonVariants({ variant: "ghost", className: "h-8 w-8 p-0" })}>
                      <span className="sr-only">Open menu</span>
                      <MoreHorizontal className="h-4 w-4" />
                    </DropdownMenuTrigger>
                    <DropdownMenuContent align="end">
                      <DropdownMenuItem>
                        <Link href={`/dashboard/qr/${qr.id}`} className="cursor-pointer flex items-center w-full">
                          <Eye className="mr-2 h-4 w-4" /> View Details
                        </Link>
                      </DropdownMenuItem>
                      <DropdownMenuItem>
                        <Link href={`/dashboard/qr/edit/${qr.id}`} className="cursor-pointer flex items-center w-full">
                          <Edit className="mr-2 h-4 w-4" /> Edit
                        </Link>
                      </DropdownMenuItem>
                      <DropdownMenuItem onClick={() => handleDownload(qr.id, "png")} className="cursor-pointer flex items-center">
                        <Download className="mr-2 h-4 w-4" /> Download PNG
                      </DropdownMenuItem>
                      <DropdownMenuItem onClick={() => handleDownload(qr.id, "svg")} className="cursor-pointer flex items-center">
                        <Download className="mr-2 h-4 w-4" /> Download SVG
                      </DropdownMenuItem>
                      <DropdownMenuItem onClick={() => setDeleteId(qr.id)} className="cursor-pointer text-destructive focus:text-destructive flex items-center">
                        <Trash className="mr-2 h-4 w-4" /> Delete
                      </DropdownMenuItem>
                    </DropdownMenuContent>
                  </DropdownMenu>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>

      <AlertDialog open={!!deleteId} onOpenChange={() => setDeleteId(null)}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Are you absolutely sure?</AlertDialogTitle>
            <AlertDialogDescription>
              This action cannot be undone. This will permanently delete your QR Code.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancel</AlertDialogCancel>
            <AlertDialogAction onClick={handleDelete} className="bg-destructive text-destructive-foreground hover:bg-destructive/90">
              Delete
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </>
  );
}
