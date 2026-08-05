import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import {
  MultiBranchService,
  CreateBranchPayload,
  AssignManagerPayload,
  CreateTransferPayload,
  BranchStatus,
  TransferStatus
} from "@/lib/multi-branch-service";
import { toast } from "sonner";

export const multiBranchKeys = {
  all: ["multi-branch"] as const,
  locations: () => [...multiBranchKeys.all, "locations"] as const,
  branches: () => [...multiBranchKeys.all, "branches"] as const,
  branchWarehouses: (branchId?: string) => [...multiBranchKeys.all, "warehouses", branchId ?? "all"] as const,
  transfers: (warehouseId?: string, status?: string) => [...multiBranchKeys.all, "transfers", warehouseId ?? "all", status ?? "all"] as const,
  regionalSummaries: () => [...multiBranchKeys.all, "regional-summaries"] as const,
  performances: (year: number, month: number) => [...multiBranchKeys.all, "performances", year, month] as const,
};

// ─── Queries ───────────────────────────────────────────────────────────────

export function useLocations() {
  return useQuery({
    queryKey: multiBranchKeys.locations(),
    queryFn: () => MultiBranchService.getLocations(),
    staleTime: 60_000,
  });
}

export function useBranches() {
  return useQuery({
    queryKey: multiBranchKeys.branches(),
    queryFn: () => MultiBranchService.getBranches(),
    staleTime: 30_000,
  });
}

export function useBranchWarehouses(branchId?: string) {
  return useQuery({
    queryKey: multiBranchKeys.branchWarehouses(branchId),
    queryFn: () => MultiBranchService.getBranchWarehouses(branchId),
    staleTime: 30_000,
  });
}

export function useTransfers(warehouseId?: string, status?: TransferStatus) {
  return useQuery({
    queryKey: multiBranchKeys.transfers(warehouseId, status),
    queryFn: () => MultiBranchService.getTransfers(warehouseId, status),
    staleTime: 15_000,
  });
}

export function useRegionalSummaries() {
  return useQuery({
    queryKey: multiBranchKeys.regionalSummaries(),
    queryFn: () => MultiBranchService.getRegionalSummaries(),
    staleTime: 60_000,
  });
}

export function useBranchPerformances(year: number = 2026, month: number = 8) {
  return useQuery({
    queryKey: multiBranchKeys.performances(year, month),
    queryFn: () => MultiBranchService.getBranchPerformances(year, month),
    staleTime: 30_000,
  });
}

// ─── Mutations ─────────────────────────────────────────────────────────────

export function useCreateLocation() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (payload: any) => MultiBranchService.createLocation(payload),
    onSuccess: (newLoc) => {
      queryClient.invalidateQueries({ queryKey: multiBranchKeys.locations() });
      toast.success(`Location '${newLoc.city}, ${newLoc.country}' created successfully!`);
    },
    onError: (error: any) => {
      toast.error(error.message || "Failed to create location.");
    },
  });
}

export function useCreateBranch() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (payload: CreateBranchPayload) => MultiBranchService.createBranch(payload),
    onSuccess: (newBranch) => {
      queryClient.invalidateQueries({ queryKey: multiBranchKeys.branches() });
      toast.success(`Branch '${newBranch.name}' deployed successfully!`);
    },
    onError: (error: any) => {
      toast.error(error.message || "Failed to create branch.");
    },
  });
}

export function useUpdateBranchStatus() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, status }: { id: string; status: BranchStatus }) =>
      MultiBranchService.updateBranchStatus(id, status),
    onSuccess: (updated) => {
      queryClient.invalidateQueries({ queryKey: multiBranchKeys.branches() });
      toast.success(`Branch '${updated.name}' status updated to ${updated.status}.`);
    },
    onError: (error: any) => {
      toast.error(error.message || "Failed to update branch status.");
    },
  });
}

export function useAssignBranchManager() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ branchId, payload }: { branchId: string; payload: AssignManagerPayload }) =>
      MultiBranchService.assignManager(branchId, payload),
    onSuccess: (updated) => {
      queryClient.invalidateQueries({ queryKey: multiBranchKeys.branches() });
      toast.success(`Manager '${updated.managerName}' assigned with transfer approval rights!`);
    },
    onError: (error: any) => {
      toast.error(error.message || "Failed to assign branch manager.");
    },
  });
}

export function useUpdateWorkingHours() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ branchId, workingHours, operationalSettings }: { branchId: string; workingHours: string; operationalSettings: string }) =>
      MultiBranchService.updateWorkingHours(branchId, workingHours, operationalSettings),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: multiBranchKeys.branches() });
      toast.success("Branch operational rules and working hours saved!");
    },
    onError: (error: any) => {
      toast.error(error.message || "Failed to save operational rules.");
    },
  });
}

export function useCreateBranchWarehouse() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (payload: any) => MultiBranchService.createBranchWarehouse(payload),
    onSuccess: (wh) => {
      queryClient.invalidateQueries({ queryKey: multiBranchKeys.branchWarehouses() });
      toast.success(`Warehouse facility '${wh.name}' registered into branch network!`);
    },
    onError: (error: any) => {
      toast.error(error.message || "Failed to register warehouse facility.");
    },
  });
}

export function useRequestTransfer() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (payload: CreateTransferPayload) => MultiBranchService.requestTransfer(payload),
    onSuccess: (trf) => {
      queryClient.invalidateQueries({ queryKey: multiBranchKeys.transfers() });
      toast.success(`Transfer Order #${trf.transferNumber} initiated successfully!`);
    },
    onError: (error: any) => {
      toast.error(error.message || "Failed to request inventory transfer.");
    },
  });
}

export function useApproveTransfer() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, isApproved, approvedByName, rejectionReason }: { id: string; isApproved: boolean; approvedByName: string; rejectionReason?: string }) =>
      MultiBranchService.approveTransfer(id, isApproved, approvedByName, rejectionReason),
    onSuccess: (trf) => {
      queryClient.invalidateQueries({ queryKey: multiBranchKeys.transfers() });
      toast.success(trf.status === "Approved" ? `Transfer #${trf.transferNumber} approved for shipment!` : `Transfer #${trf.transferNumber} rejected.`);
    },
    onError: (error: any) => {
      toast.error(error.message || "Failed to process transfer approval.");
    },
  });
}

export function useUpdateTransferTracking() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, status, trackingNotes }: { id: string; status: TransferStatus; trackingNotes: string }) =>
      MultiBranchService.updateTransferTracking(id, status, trackingNotes),
    onSuccess: (trf) => {
      queryClient.invalidateQueries({ queryKey: multiBranchKeys.transfers() });
      toast.success(`Transfer #${trf.transferNumber} tracking updated to ${trf.status}.`);
    },
    onError: (error: any) => {
      toast.error(error.message || "Failed to update tracking details.");
    },
  });
}

export function useReceiveTransfer() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, receiptNotes }: { id: string; receiptNotes?: string }) =>
      MultiBranchService.receiveTransfer(id, receiptNotes),
    onSuccess: (trf) => {
      queryClient.invalidateQueries({ queryKey: multiBranchKeys.transfers() });
      queryClient.invalidateQueries({ queryKey: multiBranchKeys.branchWarehouses() });
      toast.success(`Transfer #${trf.transferNumber} items received! Inventory valuation and item counts reconciled automatically.`);
    },
    onError: (error: any) => {
      toast.error(error.message || "Failed to receive transfer shipment.");
    },
  });
}
