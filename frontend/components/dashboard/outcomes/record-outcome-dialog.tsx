import React, { useState } from "react";
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogDescription, DialogFooter } from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { useCreateOutcome } from "@/hooks/use-outcomes";
import { CreateOutcomeDto } from "@/lib/outcomes-service";

interface RecordOutcomeDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  defaultSourceType?: string;
  defaultSourceId?: string;
}

export function RecordOutcomeDialog({ open, onOpenChange, defaultSourceType, defaultSourceId }: RecordOutcomeDialogProps) {
  const { mutate: createOutcome, isPending } = useCreateOutcome();
  const [formData, setFormData] = useState<Partial<CreateOutcomeDto>>({
    outcomeType: "RevenueRecovered",
    sourceType: defaultSourceType || "UserReported",
    sourceId: defaultSourceId || `manual-${Date.now()}`,
    confidence: "High", // User reported is taken as high confidence for the manual context
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    createOutcome(formData as CreateOutcomeDto, {
      onSuccess: () => {
        onOpenChange(false);
      }
    });
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[425px]">
        <DialogHeader>
          <DialogTitle>Record Business Outcome</DialogTitle>
          <DialogDescription>
            Manually log a business outcome that occurred outside of automated tracking.
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="space-y-2">
            <Label>Outcome Type</Label>
            <Select 
              value={formData.outcomeType} 
              onValueChange={(v) => setFormData({ ...formData, outcomeType: v || "RevenueRecovered" })}
            >
              <SelectTrigger>
                <SelectValue placeholder="Select type" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="RevenueRecovered">Revenue Recovered</SelectItem>
                <SelectItem value="RevenueIncrease">Revenue Generated</SelectItem>
                <SelectItem value="CostReduction">Cost Reduced</SelectItem>
                <SelectItem value="TimeSaved">Time Saved</SelectItem>
                <SelectItem value="CustomerRetained">Customer Retained</SelectItem>
                <SelectItem value="InvoicePaid">Invoice Paid</SelectItem>
              </SelectContent>
            </Select>
          </div>

          {(formData.outcomeType === "RevenueRecovered" || formData.outcomeType === "RevenueIncrease") && (
            <div className="space-y-2">
              <Label>Amount Impact (₹)</Label>
              <Input 
                type="number" 
                placeholder="e.g. 50000" 
                value={formData.revenueImpact || ""}
                onChange={(e) => setFormData({ ...formData, revenueImpact: Number(e.target.value) })}
                required
              />
            </div>
          )}

          {formData.outcomeType === "CostReduction" && (
            <div className="space-y-2">
              <Label>Cost Impact (₹)</Label>
              <Input 
                type="number" 
                placeholder="e.g. 10000" 
                value={formData.costImpact || ""}
                onChange={(e) => setFormData({ ...formData, costImpact: Number(e.target.value) })}
                required
              />
            </div>
          )}

          {formData.outcomeType === "TimeSaved" && (
            <div className="space-y-2">
              <Label>Time Saved (Minutes)</Label>
              <Input 
                type="number" 
                placeholder="e.g. 120" 
                value={formData.timeSavedMinutes || ""}
                onChange={(e) => setFormData({ ...formData, timeSavedMinutes: Number(e.target.value) })}
                required
              />
            </div>
          )}

          <div className="space-y-2">
            <Label>What happened? (Explanation)</Label>
            <Textarea 
              placeholder="e.g. Called the customer and they paid the outstanding invoice."
              value={formData.explanation || ""}
              onChange={(e) => setFormData({ ...formData, explanation: e.target.value })}
            />
          </div>

          <DialogFooter>
            <Button type="button" variant="outline" onClick={() => onOpenChange(false)}>
              Cancel
            </Button>
            <Button type="submit" disabled={isPending}>
              {isPending ? "Recording..." : "Record Outcome"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
