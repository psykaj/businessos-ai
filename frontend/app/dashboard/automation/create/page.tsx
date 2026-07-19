"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import { ArrowLeft, Plus, Save, Trash2, Zap } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { toast } from "sonner";
import { createAutomationRule } from "@/lib/api/automation";

export default function CreateAutomationPage() {
  const router = useRouter();
  const [name, setName] = useState("");
  const [trigger, setTrigger] = useState("");
  const [loading, setLoading] = useState(false);

  // Simplified state for conditions and actions for this V1 builder
  const [actionType, setActionType] = useState("");
  const [actionConfig, setActionConfig] = useState("");

  const handleSave = async () => {
    if (!name || !trigger || !actionType) {
      toast.error("Please fill in all required fields");
      return;
    }

    try {
      setLoading(true);
      await createAutomationRule({
        name,
        trigger,
        conditions: "[]", // Empty for now
        actions: JSON.stringify([{ type: actionType, config: actionConfig }]),
        isEnabled: true,
      });
      toast.success("Workflow created successfully");
      router.push("/dashboard/automation");
    } catch (error) {
      toast.error("Failed to create workflow");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="space-y-6 max-w-4xl mx-auto py-6">
      <div className="flex items-center gap-2 text-sm text-muted-foreground mb-2">
        <Link href="/dashboard/automation" className="hover:text-foreground flex items-center gap-1 transition-colors">
          <ArrowLeft className="h-4 w-4" />
          Back to Workflows
        </Link>
      </div>
      
      <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <div>
          <h1 className="text-2xl font-bold tracking-tight">Create Workflow</h1>
          <p className="text-muted-foreground">Define triggers and actions for your new automation.</p>
        </div>
        <Button onClick={handleSave} disabled={loading} className="gap-2">
          <Save className="h-4 w-4" />
          {loading ? "Saving..." : "Save Workflow"}
        </Button>
      </div>

      <div className="grid gap-6">
        <Card>
          <CardHeader>
            <CardTitle>Basic Details</CardTitle>
            <CardDescription>Name your workflow to easily identify it later.</CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="space-y-2">
              <Label htmlFor="name">Workflow Name</Label>
              <Input 
                id="name" 
                placeholder="e.g., Welcome New Customer" 
                value={name}
                onChange={(e) => setName(e.target.value)}
              />
            </div>
          </CardContent>
        </Card>

        <div className="relative">
          <div className="absolute left-6 top-0 bottom-0 w-0.5 bg-border -z-10" />
          
          <div className="space-y-6">
            {/* Trigger Section */}
            <Card className="relative ml-12">
              <div className="absolute -left-12 top-6 h-8 w-8 rounded-full bg-primary flex items-center justify-center border-4 border-background">
                <Zap className="h-4 w-4 text-primary-foreground" />
              </div>
              <CardHeader>
                <CardTitle>When this happens (Trigger)</CardTitle>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="space-y-2">
                  <Label>Select Event Trigger</Label>
                  <Select value={trigger} onValueChange={(val) => setTrigger(val || "")}>
                    <SelectTrigger>
                      <SelectValue placeholder="Choose a trigger event..." />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="Subscription.Created">Subscription Created</SelectItem>
                      <SelectItem value="Subscription.Expired">Subscription Expired</SelectItem>
                      <SelectItem value="QRCode.Scanned">QR Code Scanned</SelectItem>
                      <SelectItem value="Review.Submitted">New Review Submitted</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
              </CardContent>
            </Card>

            {/* Action Section */}
            <Card className="relative ml-12">
              <div className="absolute -left-12 top-6 h-8 w-8 rounded-full bg-secondary flex items-center justify-center border-4 border-background">
                <Plus className="h-4 w-4 text-secondary-foreground" />
              </div>
              <CardHeader>
                <CardTitle>Then do this (Action)</CardTitle>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="grid grid-cols-2 gap-4">
                  <div className="space-y-2">
                    <Label>Action Type</Label>
                    <Select value={actionType} onValueChange={(val) => setActionType(val || "")}>
                      <SelectTrigger>
                        <SelectValue placeholder="Choose action..." />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value="SendEmail">Send Email</SelectItem>
                        <SelectItem value="SendWhatsApp">Send WhatsApp Message</SelectItem>
                        <SelectItem value="SendNotification">Send App Notification</SelectItem>
                      </SelectContent>
                    </Select>
                  </div>
                  <div className="space-y-2">
                    <Label>Target / Template ID</Label>
                    <Input 
                      placeholder="e.g., Template ID or Message" 
                      value={actionConfig}
                      onChange={(e) => setActionConfig(e.target.value)}
                    />
                  </div>
                </div>
              </CardContent>
            </Card>
          </div>
        </div>
      </div>
    </div>
  );
}
