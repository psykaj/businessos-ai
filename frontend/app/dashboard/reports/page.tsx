"use client";

import { useState } from "react";
import { useReports, useGenerateReport } from "@/hooks/use-bi";
import { biService, Report } from "@/lib/bi-service";
import { 
  FileText, 
  Download, 
  Plus, 
  RefreshCw, 
  Eye, 
  Calendar, 
  Share2, 
  Clock, 
  CheckCircle2, 
  Filter, 
  FileSpreadsheet, 
  FileCheck 
} from "lucide-react";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogDescription, DialogFooter } from "@/components/ui/dialog";
import { toast } from "sonner";

export default function ReportsCenterPage() {
  const [selectedType, setSelectedType] = useState<string | undefined>(undefined);
  const [previewReport, setPreviewReport] = useState<Report | null>(null);
  const [isGenerateOpen, setIsGenerateOpen] = useState(false);
  const [reportName, setReportName] = useState("");
  const [reportType, setReportType] = useState("Executive");
  const [format, setFormat] = useState("PDF");

  const { data: reports = [], isLoading, refetch } = useReports(selectedType);
  const { mutate: generateReport, isPending: isGenerating } = useGenerateReport();

  const reportTypes = ["Executive", "Sales", "Marketing", "CRM", "Finance", "Workflow", "AIUsage"];

  const handleGenerate = () => {
    generateReport(
      { name: reportName, reportType, format },
      {
        onSuccess: () => {
          setIsGenerateOpen(false);
          setReportName("");
        }
      }
    );
  };

  const handleExport = async (id: string, name: string, exportFormat: string) => {
    try {
      toast.info(`Preparing ${exportFormat.toUpperCase()} download...`);
      const response = await biService.exportReport(id, exportFormat);
      const contentType = (response.headers["content-type"] as string) || "application/octet-stream";
      const blob = new Blob([response.data], { type: contentType });
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement("a");
      a.href = url;
      a.download = `${name.replace(/\s+/g, "_")}.${exportFormat.toLowerCase() === "excel" ? "xls" : exportFormat.toLowerCase()}`;
      document.body.appendChild(a);
      a.click();
      a.remove();
      window.URL.revokeObjectURL(url);
      toast.success("Download started successfully");
    } catch (err: any) {
      toast.error("Export failed", { description: err.message });
    }
  };

  return (
    <div className="flex flex-col gap-8 p-6 lg:p-8 max-w-[1600px] mx-auto min-h-screen">
      {/* Header Banner */}
      <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between border-b border-border/40 pb-6">
        <div>
          <div className="flex items-center gap-2.5">
            <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-primary/10 text-primary ring-1 ring-primary/20 shadow-inner">
              <FileText className="h-5 w-5" />
            </div>
            <div>
              <h1 className="text-2xl font-bold tracking-tight text-foreground sm:text-3xl">
                Enterprise Reports Center
              </h1>
              <p className="text-sm text-muted-foreground mt-0.5">
                Generate, preview, export and schedule automated business intelligence reports
              </p>
            </div>
          </div>
        </div>

        <div className="flex items-center gap-3">
          <Button
            onClick={() => setIsGenerateOpen(true)}
            className="gap-2 rounded-xl text-xs font-semibold shadow-md"
          >
            <Plus className="h-3.5 w-3.5" />
            Generate New Report
          </Button>
        </div>
      </div>

      {/* Filter Tabs */}
      <div className="flex items-center gap-2 flex-wrap">
        <button
          onClick={() => setSelectedType(undefined)}
          className={`rounded-xl px-4 py-2 text-xs font-semibold transition-all border ${
            !selectedType
              ? "bg-primary text-primary-foreground border-primary shadow-sm"
              : "bg-card/60 text-muted-foreground border-border/50 hover:bg-muted hover:text-foreground"
          }`}
        >
          All Reports
        </button>
        {reportTypes.map((type) => (
          <button
            key={type}
            onClick={() => setSelectedType(type)}
            className={`rounded-xl px-4 py-2 text-xs font-semibold transition-all border ${
              selectedType === type
                ? "bg-primary text-primary-foreground border-primary shadow-sm"
                : "bg-card/60 text-muted-foreground border-border/50 hover:bg-muted hover:text-foreground"
            }`}
          >
            {type}
          </button>
        ))}
      </div>

      {/* Reports Grid */}
      {isLoading ? (
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
          {[1, 2, 3].map((n) => (
            <div key={n} className="h-44 rounded-2xl border border-border/40 bg-card/30 animate-pulse" />
          ))}
        </div>
      ) : reports.length === 0 ? (
        <Card className="rounded-2xl border-border/40 bg-card/40 p-12 text-center">
          <FileSpreadsheet className="h-12 w-12 text-muted-foreground/50 mx-auto mb-3" />
          <h3 className="text-lg font-bold text-foreground">No Generated Reports Found</h3>
          <p className="text-sm text-muted-foreground max-w-md mx-auto mt-1 mb-4">
            Click "Generate New Report" to produce an on-demand PDF, Excel, or CSV report for your organization.
          </p>
          <Button onClick={() => setIsGenerateOpen(true)} size="sm" className="rounded-xl">
            Generate First Report
          </Button>
        </Card>
      ) : (
        <div className="grid gap-5 sm:grid-cols-2 lg:grid-cols-3">
          {reports.map((report) => (
            <Card key={report.id} className="rounded-2xl border-border/50 bg-card/60 backdrop-blur-xl shadow-sm transition-all hover:shadow-md hover:border-border flex flex-col justify-between">
              <CardHeader className="space-y-2">
                <div className="flex items-center justify-between">
                  <Badge variant="outline" className="rounded-lg text-[10px] font-semibold uppercase text-muted-foreground border-border/60">
                    {report.reportType} Report
                  </Badge>
                  <Badge className="bg-primary/10 text-primary border-primary/20 text-[10px]">
                    {report.format}
                  </Badge>
                </div>
                <CardTitle className="text-base font-bold text-foreground tracking-tight line-clamp-1">
                  {report.name}
                </CardTitle>
                <CardDescription className="text-xs flex items-center gap-1">
                  <Calendar className="h-3 w-3" />
                  Generated {new Date(report.generatedAt).toLocaleDateString("en-US", { month: "short", day: "numeric", year: "numeric" })}
                </CardDescription>
              </CardHeader>

              <CardContent className="pt-2">
                <div className="flex items-center gap-2 pt-3 border-t border-border/40">
                  <Button
                    variant="outline"
                    size="sm"
                    onClick={() => setPreviewReport(report)}
                    className="flex-1 gap-1.5 rounded-xl text-xs font-medium"
                  >
                    <Eye className="h-3.5 w-3.5" />
                    Preview
                  </Button>

                  <Button
                    variant="secondary"
                    size="sm"
                    onClick={() => handleExport(report.id, report.name, "pdf")}
                    className="gap-1 rounded-xl text-xs font-semibold"
                  >
                    <Download className="h-3.5 w-3.5" />
                    PDF
                  </Button>

                  <Button
                    variant="secondary"
                    size="sm"
                    onClick={() => handleExport(report.id, report.name, "excel")}
                    className="gap-1 rounded-xl text-xs font-semibold"
                  >
                    XLS
                  </Button>
                </div>
              </CardContent>
            </Card>
          ))}
        </div>
      )}

      {/* Generate Report Modal */}
      <Dialog open={isGenerateOpen} onOpenChange={setIsGenerateOpen}>
        <DialogContent className="sm:max-w-[425px] rounded-2xl border-border bg-card">
          <DialogHeader>
            <DialogTitle className="text-lg font-bold">Generate Enterprise Report</DialogTitle>
            <DialogDescription className="text-xs">
              Select your desired report type and output format.
            </DialogDescription>
          </DialogHeader>

          <div className="space-y-4 py-2">
            <div className="space-y-1.5">
              <label className="text-xs font-semibold text-foreground">Report Name</label>
              <input
                type="text"
                placeholder="e.g. Monthly Executive Performance Summary"
                value={reportName}
                onChange={(e) => setReportName(e.target.value)}
                className="w-full rounded-xl border border-border bg-background px-3 py-2 text-xs text-foreground focus:outline-none focus:ring-1 focus:ring-primary"
              />
            </div>

            <div className="space-y-1.5">
              <label className="text-xs font-semibold text-foreground">Report Category</label>
              <select
                value={reportType}
                onChange={(e) => setReportType(e.target.value)}
                className="w-full rounded-xl border border-border bg-background px-3 py-2 text-xs text-foreground focus:outline-none focus:ring-1 focus:ring-primary"
              >
                {reportTypes.map((t) => (
                  <option key={t} value={t}>{t} Report</option>
                ))}
              </select>
            </div>

            <div className="space-y-1.5">
              <label className="text-xs font-semibold text-foreground">Format</label>
              <div className="grid grid-cols-3 gap-2">
                {["PDF", "Excel", "CSV"].map((f) => (
                  <button
                    key={f}
                    type="button"
                    onClick={() => setFormat(f)}
                    className={`rounded-xl py-2 text-xs font-semibold border transition-all ${
                      format === f
                        ? "bg-primary text-primary-foreground border-primary"
                        : "bg-muted/40 text-muted-foreground border-border hover:bg-muted"
                    }`}
                  >
                    {f}
                  </button>
                ))}
              </div>
            </div>
          </div>

          <DialogFooter className="gap-2 sm:gap-0">
            <Button variant="outline" onClick={() => setIsGenerateOpen(false)} className="rounded-xl text-xs">
              Cancel
            </Button>
            <Button onClick={handleGenerate} disabled={isGenerating} className="rounded-xl text-xs font-semibold">
              {isGenerating ? "Generating..." : "Generate Report"}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      {/* Preview Modal */}
      {previewReport && (
        <Dialog open={!!previewReport} onOpenChange={() => setPreviewReport(null)}>
          <DialogContent className="sm:max-w-[700px] max-h-[85vh] overflow-y-auto rounded-2xl border-border bg-card">
            <DialogHeader>
              <DialogTitle className="text-lg font-bold">{previewReport.name}</DialogTitle>
              <DialogDescription className="text-xs">
                Generated at {new Date(previewReport.generatedAt).toLocaleString()} for {previewReport.reportType} Category
              </DialogDescription>
            </DialogHeader>

            <div className="space-y-4 py-4 border-t border-b border-border/40">
              <div className="rounded-xl border border-border/50 bg-muted/30 p-4 space-y-2">
                <h4 className="text-xs font-bold text-muted-foreground uppercase">Executive Summary Snapshot</h4>
                <p className="text-xs text-foreground leading-relaxed">
                  This on-demand {previewReport.reportType} report aggregates active customer growth metrics, pipeline volume, revenue trajectory, and operational workflow executions.
                </p>
              </div>

              <div className="grid grid-cols-2 sm:grid-cols-3 gap-3">
                <div className="rounded-xl border border-border/40 p-3 bg-card">
                  <span className="text-[10px] text-muted-foreground block">Format</span>
                  <span className="text-sm font-bold text-foreground">{previewReport.format}</span>
                </div>
                <div className="rounded-xl border border-border/40 p-3 bg-card">
                  <span className="text-[10px] text-muted-foreground block">Report ID</span>
                  <span className="text-xs font-mono font-bold text-foreground truncate block">{previewReport.id.slice(0, 8)}</span>
                </div>
                <div className="rounded-xl border border-border/40 p-3 bg-card">
                  <span className="text-[10px] text-muted-foreground block">Status</span>
                  <span className="text-xs font-bold text-emerald-500 flex items-center gap-1">
                    <CheckCircle2 className="h-3 w-3" /> Ready
                  </span>
                </div>
              </div>
            </div>

            <DialogFooter className="gap-2 sm:gap-0">
              <Button variant="outline" onClick={() => setPreviewReport(null)} className="rounded-xl text-xs">
                Close Preview
              </Button>
              <Button onClick={() => handleExport(previewReport.id, previewReport.name, "pdf")} className="rounded-xl text-xs font-semibold gap-1.5">
                <Download className="h-3.5 w-3.5" />
                Download PDF
              </Button>
            </DialogFooter>
          </DialogContent>
        </Dialog>
      )}
    </div>
  );
}
