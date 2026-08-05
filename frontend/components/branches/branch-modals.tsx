"use client";

import React, { useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { BranchDto, LocationDto } from "@/lib/multi-branch-service";
import { useCreateBranch, useAssignBranchManager, useUpdateWorkingHours, useCreateLocation } from "@/hooks/use-multi-branch";
import { X, Building2, User, Clock, ShieldCheck, MapPin, Sparkles, CheckCircle2 } from "lucide-react";
import { cn } from "@/lib/utils";

// ─── Zod Schemas ─────────────────────────────────────────────────────────────

const createBranchSchema = z.object({
  name: z.string().min(3, "Branch name must be at least 3 characters").max(100),
  code: z.string().min(2, "Branch code required (e.g., BR-USA-SF01)"),
  locationId: z.string().optional(),
  contactEmail: z.string().email("Invalid corporate email").optional().or(z.literal("")),
  contactPhone: z.string().optional(),
  costCenterCode: z.string().optional(),
  isPrimary: z.boolean(),
});

const assignManagerSchema = z.object({
  managerName: z.string().min(2, "Manager full name required"),
  managerEmail: z.string().email("Valid corporate email required"),
  canApproveTransfers: z.boolean(),
  maxTransferApprovalLimit: z.number().min(0, "Approval limit cannot be negative"),
});

const createLocationSchema = z.object({
  region: z.string().min(2, "Region is required (e.g. EMEA, APAC, North America)"),
  country: z.string().min(2, "Country required"),
  stateProvince: z.string().min(2, "State/Province required"),
  city: z.string().min(2, "City required"),
  postalCode: z.string().min(2, "Postal code required"),
  addressLine: z.string().min(5, "Street address required"),
  timezone: z.string().min(2, "Timezone required (e.g. PST, EST, GMT)"),
});

// ─── 1. Create Branch Modal ──────────────────────────────────────────────────

export function CreateBranchModal({ isOpen, onClose, locations = [] }: { isOpen: boolean; onClose: () => void; locations?: LocationDto[] }) {
  const createBranchMutation = useCreateBranch();
  const { register, handleSubmit, reset, formState: { errors, isSubmitting } } = useForm<z.infer<typeof createBranchSchema>>({
    resolver: zodResolver(createBranchSchema),
    defaultValues: { isPrimary: false, code: `BR-${Math.floor(100 + Math.random() * 900)}` }
  });

  if (!isOpen) return null;

  const onSubmit = async (data: z.infer<typeof createBranchSchema>) => {
    await createBranchMutation.mutateAsync({
      name: data.name,
      code: data.code,
      locationId: data.locationId,
      contactEmail: data.contactEmail,
      contactPhone: data.contactPhone,
      costCenterCode: data.costCenterCode,
      isPrimary: data.isPrimary,
    });
    reset();
    onClose();
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm p-4 animate-in fade-in-0">
      <div className="relative w-full max-w-lg rounded-2xl bg-card border border-border shadow-2xl overflow-hidden p-6 space-y-5">
        <div className="flex items-center justify-between border-b border-border/50 pb-3">
          <div className="flex items-center gap-2 text-indigo-600 font-bold text-lg">
            <Building2 className="w-5 h-5" /> Deploy New Branch Hub
          </div>
          <button onClick={onClose} className="p-1 text-muted-foreground hover:text-foreground rounded-lg">
            <X className="w-5 h-5" />
          </button>
        </div>

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4 text-sm">
          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="font-semibold block mb-1">Branch Name *</label>
              <input {...register("name")} placeholder="e.g. Austin Enterprise Gateway" className="w-full px-3 py-2 rounded-xl border border-border bg-background text-foreground focus:ring-2 focus:ring-indigo-500" />
              {errors.name && <p className="text-red-500 text-xs mt-1">{errors.name.message}</p>}
            </div>
            <div>
              <label className="font-semibold block mb-1">Branch Code *</label>
              <input {...register("code")} placeholder="e.g. BR-USA-AUS01" className="w-full px-3 py-2 rounded-xl border border-border bg-background text-foreground font-mono focus:ring-2 focus:ring-indigo-500" />
              {errors.code && <p className="text-red-500 text-xs mt-1">{errors.code.message}</p>}
            </div>
          </div>

          <div>
            <label className="font-semibold block mb-1">Assigned Geographic Territory</label>
            <select {...register("locationId")} className="w-full px-3 py-2 rounded-xl border border-border bg-background text-foreground focus:ring-2 focus:ring-indigo-500">
              <option value="">-- Select Regional Territory --</option>
              {locations.map(loc => (
                <option key={loc.id} value={loc.id}>
                  {loc.city}, {loc.stateProvince} ({loc.region})
                </option>
              ))}
            </select>
          </div>

          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="font-semibold block mb-1">Contact Email</label>
              <input {...register("contactEmail")} placeholder="ops@businessos.ai" className="w-full px-3 py-2 rounded-xl border border-border bg-background text-foreground" />
              {errors.contactEmail && <p className="text-red-500 text-xs mt-1">{errors.contactEmail.message}</p>}
            </div>
            <div>
              <label className="font-semibold block mb-1">Cost Center Code</label>
              <input {...register("costCenterCode")} placeholder="e.g. CC-9045-NA" className="w-full px-3 py-2 rounded-xl border border-border bg-background text-foreground font-mono" />
            </div>
          </div>

          <div className="flex items-center gap-2 p-3 rounded-xl bg-accent/40 border border-border/40">
            <input type="checkbox" id="isPrimary" {...register("isPrimary")} className="w-4 h-4 rounded text-indigo-600 focus:ring-indigo-500" />
            <label htmlFor="isPrimary" className="font-medium cursor-pointer text-xs">
              Set as **Primary Corporate Headquarters** (Default routing destination for global operations)
            </label>
          </div>

          <div className="flex justify-end gap-3 pt-3 border-t border-border/50">
            <button type="button" onClick={onClose} className="px-4 py-2 rounded-xl border border-border text-xs font-semibold hover:bg-accent transition-colors">Cancel</button>
            <button type="submit" disabled={isSubmitting} className="px-5 py-2 rounded-xl bg-indigo-600 text-white font-semibold text-xs hover:bg-indigo-700 shadow-md transition-all">
              {isSubmitting ? "Deploying..." : "Deploy Branch Hub"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

// ─── 2. Assign Manager Modal ─────────────────────────────────────────────────

export function AssignManagerModal({ isOpen, onClose, branch }: { isOpen: boolean; onClose: () => void; branch: BranchDto | null }) {
  const assignMutation = useAssignBranchManager();
  const { register, handleSubmit, reset, formState: { errors, isSubmitting } } = useForm<z.infer<typeof assignManagerSchema>>({
    resolver: zodResolver(assignManagerSchema),
    defaultValues: {
      managerName: branch?.managerName || "",
      managerEmail: branch?.managerEmail || "",
      canApproveTransfers: branch?.canApproveTransfers !== undefined ? branch.canApproveTransfers : true,
      maxTransferApprovalLimit: branch?.maxTransferApprovalLimit || 25000,
    }
  });

  if (!isOpen || !branch) return null;

  const onSubmit = async (data: z.infer<typeof assignManagerSchema>) => {
    await assignMutation.mutateAsync({
      branchId: branch.id,
      payload: {
        userId: `usr-${Date.now()}`,
        managerName: data.managerName,
        managerEmail: data.managerEmail,
        canApproveTransfers: data.canApproveTransfers,
        maxTransferApprovalLimit: Number(data.maxTransferApprovalLimit),
      }
    });
    reset();
    onClose();
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm p-4 animate-in fade-in-0">
      <div className="relative w-full max-w-md rounded-2xl bg-card border border-border shadow-2xl overflow-hidden p-6 space-y-4">
        <div className="flex items-center justify-between border-b border-border/50 pb-3">
          <div className="flex items-center gap-2 text-indigo-600 font-bold text-base">
            <User className="w-5 h-5" /> Assign Branch Managing Executive
          </div>
          <button onClick={onClose} className="p-1 text-muted-foreground hover:text-foreground"><X className="w-5 h-5" /></button>
        </div>

        <p className="text-xs text-muted-foreground">
          Configure executive accountability and automated inventory transfer approval thresholds for **{branch.name}**.
        </p>

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4 text-sm">
          <div>
            <label className="font-semibold block mb-1">Executive Full Name & Title *</label>
            <input {...register("managerName")} placeholder="e.g. David Sterling, VP South" className="w-full px-3 py-2 rounded-xl border border-border bg-background text-foreground" />
            {errors.managerName && <p className="text-red-500 text-xs mt-1">{errors.managerName.message}</p>}
          </div>

          <div>
            <label className="font-semibold block mb-1">Corporate Email Address *</label>
            <input {...register("managerEmail")} placeholder="dsterling@businessos.ai" className="w-full px-3 py-2 rounded-xl border border-border bg-background text-foreground" />
            {errors.managerEmail && <p className="text-red-500 text-xs mt-1">{errors.managerEmail.message}</p>}
          </div>

          <div className="p-3.5 rounded-xl bg-indigo-500/5 border border-indigo-500/20 space-y-3">
            <div className="flex items-center gap-2 font-semibold text-indigo-600 text-xs">
              <ShieldCheck className="w-4 h-4" /> Automated Approval Authority
            </div>
            <div className="flex items-center gap-2 text-xs">
              <input type="checkbox" id="canApprove" {...register("canApproveTransfers")} className="w-4 h-4 rounded text-indigo-600" />
              <label htmlFor="canApprove" className="font-medium cursor-pointer">Grant stock transfer auto-approval authority</label>
            </div>
            <div>
              <label className="text-[11px] font-semibold block mb-1 text-muted-foreground">Max Financial Approval Limit (USD)</label>
              <input type="number" {...register("maxTransferApprovalLimit", { valueAsNumber: true })} className="w-full px-3 py-1.5 rounded-lg border border-border bg-background font-mono text-xs font-bold" />
              <p className="text-[10px] text-muted-foreground mt-1">Transfers below this value requested by this manager will be instantly auto-approved.</p>
            </div>
          </div>

          <div className="flex justify-end gap-3 pt-2">
            <button type="button" onClick={onClose} className="px-4 py-2 rounded-xl border border-border text-xs font-semibold">Cancel</button>
            <button type="submit" disabled={isSubmitting} className="px-5 py-2 rounded-xl bg-indigo-600 text-white font-semibold text-xs hover:bg-indigo-700 shadow-md">
              Save Assignment
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

// ─── 3. Working Hours & Rules Modal ──────────────────────────────────────────

export function WorkingHoursModal({ isOpen, onClose, branch }: { isOpen: boolean; onClose: () => void; branch: BranchDto | null }) {
  const updateHoursMutation = useUpdateWorkingHours();
  const [monFri, setMonFri] = useState("09:00-17:30");
  const [saturday, setSaturday] = useState("Closed");
  const [autoReorder, setAutoReorder] = useState(true);

  if (!isOpen || !branch) return null;

  const handleSave = async () => {
    const hoursJson = JSON.stringify({
      Monday: monFri, Tuesday: monFri, Wednesday: monFri, Thursday: monFri, Friday: monFri, Saturday: saturday, Sunday: "Closed"
    });
    const opsJson = JSON.stringify({
      AutoReorder: autoReorder, MaxTransferApprovalAmount: branch.maxTransferApprovalLimit || 25000, PosSyncEnabled: true
    });

    await updateHoursMutation.mutateAsync({
      branchId: branch.id,
      workingHours: hoursJson,
      operationalSettings: opsJson
    });
    onClose();
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm p-4 animate-in fade-in-0">
      <div className="relative w-full max-w-md rounded-2xl bg-card border border-border shadow-2xl overflow-hidden p-6 space-y-4">
        <div className="flex items-center justify-between border-b border-border/50 pb-3">
          <div className="flex items-center gap-2 text-indigo-600 font-bold text-base">
            <Clock className="w-5 h-5" /> Operational Hours & Rules
          </div>
          <button onClick={onClose} className="p-1 text-muted-foreground hover:text-foreground"><X className="w-5 h-5" /></button>
        </div>

        <p className="text-xs text-muted-foreground">
          Define SLA operating windows and automated reorder schedules for **{branch.name}**.
        </p>

        <div className="space-y-3 text-sm">
          <div>
            <label className="text-xs font-semibold block mb-1">Monday – Friday Operating Window</label>
            <input value={monFri} onChange={(e) => setMonFri(e.target.value)} className="w-full px-3 py-2 rounded-xl border border-border bg-background font-mono text-xs" />
          </div>
          <div>
            <label className="text-xs font-semibold block mb-1">Saturday Window</label>
            <input value={saturday} onChange={(e) => setSaturday(e.target.value)} placeholder="e.g. 09:00-13:00 or Closed" className="w-full px-3 py-2 rounded-xl border border-border bg-background font-mono text-xs" />
          </div>

          <div className="p-3.5 rounded-xl bg-accent/50 border border-border space-y-2">
            <span className="text-xs font-bold block flex items-center gap-1.5">
              <Sparkles className="w-4 h-4 text-amber-500" /> Smart Inventory Auto-Replenishment
            </span>
            <div className="flex items-center gap-2 text-xs">
              <input type="checkbox" checked={autoReorder} onChange={(e) => setAutoReorder(e.target.checked)} id="reorder" className="w-4 h-4 rounded text-indigo-600" />
              <label htmlFor="reorder" className="font-medium cursor-pointer">Enable AI automatic stock reordering when utilization drops below 65%</label>
            </div>
          </div>
        </div>

        <div className="flex justify-end gap-3 pt-3 border-t border-border/50">
          <button onClick={onClose} className="px-4 py-2 rounded-xl border border-border text-xs font-semibold">Cancel</button>
          <button onClick={handleSave} className="px-5 py-2 rounded-xl bg-indigo-600 text-white font-semibold text-xs hover:bg-indigo-700 shadow-md">
            Save Rules
          </button>
        </div>
      </div>
    </div>
  );
}

// ─── 4. Create Location Modal ────────────────────────────────────────────────

export function CreateLocationModal({ isOpen, onClose }: { isOpen: boolean; onClose: () => void }) {
  const createLocMutation = useCreateLocation();
  const { register, handleSubmit, reset, formState: { errors, isSubmitting } } = useForm<z.infer<typeof createLocationSchema>>({
    resolver: zodResolver(createLocationSchema),
    defaultValues: { region: "EMEA", timezone: "GMT (UTC+0)" }
  });

  if (!isOpen) return null;

  const onSubmit = async (data: z.infer<typeof createLocationSchema>) => {
    await createLocMutation.mutateAsync(data);
    reset();
    onClose();
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm p-4 animate-in fade-in-0">
      <div className="relative w-full max-w-lg rounded-2xl bg-card border border-border shadow-2xl overflow-hidden p-6 space-y-4">
        <div className="flex items-center justify-between border-b border-border/50 pb-3">
          <div className="flex items-center gap-2 text-indigo-600 font-bold text-lg">
            <MapPin className="w-5 h-5" /> Register Geographic Territory
          </div>
          <button onClick={onClose} className="p-1 text-muted-foreground hover:text-foreground"><X className="w-5 h-5" /></button>
        </div>

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-3 text-sm">
          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="font-semibold block mb-1 text-xs">Global Region *</label>
              <select {...register("region")} className="w-full px-3 py-2 rounded-xl border border-border bg-background">
                <option value="North America">North America</option>
                <option value="EMEA">EMEA (Europe, Middle East, Africa)</option>
                <option value="APAC">APAC (Asia-Pacific)</option>
                <option value="LATAM">LATAM (Latin America)</option>
              </select>
            </div>
            <div>
              <label className="font-semibold block mb-1 text-xs">Operational Timezone *</label>
              <input {...register("timezone")} placeholder="e.g. CET (UTC+1)" className="w-full px-3 py-2 rounded-xl border border-border bg-background text-xs font-mono" />
              {errors.timezone && <p className="text-red-500 text-[10px] mt-1">{errors.timezone.message}</p>}
            </div>
          </div>

          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="font-semibold block mb-1 text-xs">Country *</label>
              <input {...register("country")} placeholder="e.g. United Kingdom" className="w-full px-3 py-2 rounded-xl border border-border bg-background text-xs" />
              {errors.country && <p className="text-red-500 text-[10px] mt-1">{errors.country.message}</p>}
            </div>
            <div>
              <label className="font-semibold block mb-1 text-xs">State / Province / County *</label>
              <input {...register("stateProvince")} placeholder="e.g. Greater London" className="w-full px-3 py-2 rounded-xl border border-border bg-background text-xs" />
              {errors.stateProvince && <p className="text-red-500 text-[10px] mt-1">{errors.stateProvince.message}</p>}
            </div>
          </div>

          <div className="grid grid-cols-3 gap-3">
            <div className="col-span-2">
              <label className="font-semibold block mb-1 text-xs">City / Municipality *</label>
              <input {...register("city")} placeholder="e.g. London" className="w-full px-3 py-2 rounded-xl border border-border bg-background text-xs" />
            </div>
            <div>
              <label className="font-semibold block mb-1 text-xs">Postal Code *</label>
              <input {...register("postalCode")} placeholder="EC2M 7PP" className="w-full px-3 py-2 rounded-xl border border-border bg-background text-xs font-mono" />
            </div>
          </div>

          <div>
            <label className="font-semibold block mb-1 text-xs">Street Address Line *</label>
            <input {...register("addressLine")} placeholder="e.g. 10 Bishopsgate, Level 30" className="w-full px-3 py-2 rounded-xl border border-border bg-background text-xs" />
          </div>

          <div className="flex justify-end gap-3 pt-3 border-t border-border/50">
            <button type="button" onClick={onClose} className="px-4 py-2 rounded-xl border border-border text-xs font-semibold">Cancel</button>
            <button type="submit" disabled={isSubmitting} className="px-5 py-2 rounded-xl bg-indigo-600 text-white font-semibold text-xs hover:bg-indigo-700 shadow-md">
              Register Territory
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
