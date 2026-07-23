"use client";

import { useState } from "react";
import { useReports } from "@/hooks/use-bi";
import { biService } from "@/lib/bi-service";
import { 
  Download, 
  FileSpreadsheet, 
  FileText, 
  Clock, 
  CheckCircle2, 
  HardDriveDownload, 
  Calendar, 
  Sparkles, 
  ArrowUpRight 
} from "lucide-react";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { toast } from "sonner";

export default function ExportCenterPage() {
  const { data: reports = [], isLoading } = useReports();
  const [downloadingId, setDownloadingId] = useState<string | null>(null);

  const handleExport = async (id: string, name: string, format: string) => {
    try {
      setDownloadingId(id);
      toast.info(`Preparing ${format.toUpperCase()} export file...`);
      const response = await biService.exportReport(id, format);
      const contentType = (response.headers["content-type"] as string) || "application/octet-stream";
      const blob = new Blob([response.data], { type: contentType });
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement("a");
      a.href = url;
      a.download = `${name.replace(/\s+/g, "_")}.${format.toLowerCase() === "excel" ? "xls" : format.toLowerCase()}`;
      document.body.appendChild(a);
      a.click();
      a.remove();
      window.URL.revokeObjectURL(url);
      toast.success("Download finished successfully");
    } catch (err: any) {
      toast.error("Download failed", { description: err.message });
    } finally {
      setDownloadingId(null);
    }
  };

  return (
    <div className="flex flex-col gap-8 p-6 lg:p-8 max-w-[1600px] mx-auto min-h-screen">
      {/* Header Banner */}
      <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between border-b border-border/40 pb-6">
        <div>
          <div className="flex items-center gap-2.5">
            <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-primary/10 text-primary ring-1 ring-primary/20 shadow-inner">
              <HardDriveDownload className="h-5 w-5" />
            </div>
            <div>
              <h1 className="text-2xl font-bold tracking-tight text-foreground sm:text-3xl">
                Enterprise Export Hub
              </h1>
              <p className="text-sm text-muted-foreground mt-0.5">
                Download formatted executive PDF briefs, Excel spreadsheets & raw CSV streams
              </p>
            </div>
          </div>
        </div>
      </div>

      {/* Quick Export Cards */}
      <div className="grid gap-4 sm:grid-cols-3">
        <Card className="rounded-2xl border-border/50 bg-card/60 backdrop-blur-xl hover:border-border transition-all">
          <CardHeader className="space-y-1">
            <Badge className="w-fit bg-red-500/10 text-red-400 border-red-500/20 text-[10px]">PDF Layout</Badge>
            <CardTitle className="text-base font-bold">Executive PDF Report</CardTitle>
            <CardDescription className="text-xs">Formatted executive brief for board meetings and stakeholder updates</CardDescription>
          </CardHeader>
          <CardContent>
            <Button
              onClick={() => reports.length > 0 && handleExport(reports[0].id, reports[0].name, "pdf")}
              disabled={reports.length === 0}
              size="sm"
              className="w-full rounded-xl text-xs font-semibold gap-2"
            >
              <Download className="h-3.5 w-3.5" />
              Download PDF Brief
            </Button>
          </CardContent>
        </Card>

        <Card className="rounded-2xl border-border/50 bg-card/60 backdrop-blur-xl hover:border-border transition-all">
          <CardHeader className="space-y-1">
            <Badge className="w-fit bg-emerald-500/10 text-emerald-400 border-emerald-500/20 text-[10px]">Excel Workbook</Badge>
            <CardTitle className="text-base font-bold">Financial Excel Spreadsheet</CardTitle>
            <CardDescription className="text-xs">Full multi-column spreadsheet layout compatible with Excel & Google Sheets</CardDescription>
          </CardHeader>
          <CardContent>
            <Button
              onClick={() => reports.length > 0 && handleExport(reports[0].id, reports[0].name, "excel")}
              disabled={reports.length === 0}
              variant="secondary"
              size="sm"
              className="w-full rounded-xl text-xs font-semibold gap-2"
            >
              <FileSpreadsheet className="h-3.5 w-3.5" />
              Download XLS Workbook
            </Button>
          </CardContent>
        </Card>

        <Card className="rounded-2xl border-border/50 bg-card/60 backdrop-blur-xl hover:border-border transition-all">
          <CardHeader className="space-y-1">
            <Badge className="w-fit bg-blue-500/10 text-blue-400 border-blue-500/20 text-[10px]">Raw CSV</Badge>
            <CardTitle className="text-base font-bold">Operational Data Stream</CardTitle>
            <CardDescription className="text-xs">Raw CSV dataset export for downstream data pipelines and external tools</CardDescription>
          </CardHeader>
          <CardContent>
            <Button
              onClick={() => reports.length > 0 && handleExport(reports[0].id, reports[0].name, "csv")}
              disabled={reports.length === 0}
              variant="outline"
              size="sm"
              className="w-full rounded-xl text-xs font-semibold gap-2"
            >
              <FileText className="h-3.5 w-3.5" />
              Download CSV Stream
            </Button>
          </CardContent>
        </Card>
      </div>

      {/* History Table */}
      <Card className="rounded-2xl border-border/50 bg-card/60 backdrop-blur-xl">
        <CardHeader>
          <CardTitle className="text-base font-bold">Generated Exports Log</CardTitle>
          <CardDescription>History of generated reports and multi-tenant export links</CardDescription>
        </CardHeader>
        <CardContent>
          {isLoading ? (
            <div className="space-y-2">
              {[1, 2, 3].map((n) => (
                <div key={n} className="h-12 bg-muted/20 animate-pulse rounded-xl" />
              ))}
            </div>
          ) : reports.length === 0 ? (
            <div className="py-8 text-center text-xs text-muted-foreground">
              No exports recorded yet. Generate a report from the Reports Center to download formats.
            </div>
          ) : (
            <div className="overflow-x-auto">
              <table className="w-full text-left text-xs">
                <thead>
                  <tr className="border-b border-border/40 text-muted-foreground">
                    <th className="pb-3 font-semibold">Report Name</th>
                    <th className="pb-3 font-semibold">Type</th>
                    <th className="pb-3 font-semibold">Generated Date</th>
                    <th className="pb-3 font-semibold text-right">Actions</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-border/30">
                  {reports.map((r) => (
                    <tr key={r.id} className="hover:bg-muted/20 transition-colors">
                      <td className="py-3 font-semibold text-foreground">{r.name}</td>
                      <td className="py-3">
                        <Badge variant="outline" className="text-[10px] uppercase">
                          {r.reportType}
                        </Badge>
                      </td>
                      <td className="py-3 text-muted-foreground">
                        {new Date(r.generatedAt).toLocaleString()}
                      </td>
                      <td className="py-3 text-right">
                        <div className="flex items-center justify-end gap-1.5">
                          <button
                            onClick={() => handleExport(r.id, r.name, "pdf")}
                            className="rounded-lg px-2 py-1 bg-primary/10 text-primary hover:bg-primary/20 transition-colors text-[10px] font-bold"
                          >
                            PDF
                          </button>
                          <button
                            onClick={() => handleExport(r.id, r.name, "excel")}
                            className="rounded-lg px-2 py-1 bg-emerald-500/10 text-emerald-400 hover:bg-emerald-500/20 transition-colors text-[10px] font-bold"
                          >
                            XLS
                          </button>
                          <button
                            onClick={() => handleExport(r.id, r.name, "csv")}
                            className="rounded-lg px-2 py-1 bg-blue-500/10 text-blue-400 hover:bg-blue-500/20 transition-colors text-[10px] font-bold"
                          >
                            CSV
                          </button>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
