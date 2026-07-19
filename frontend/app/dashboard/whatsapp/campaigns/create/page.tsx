"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { toast } from "sonner";
import { Send, Users, LayoutTemplate, Clock } from "lucide-react";

import { Card, CardContent, CardDescription, CardHeader, CardTitle, CardFooter } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { WhatsAppService } from "@/lib/whatsapp-service";
import Link from "next/link";

const campaignSchema = z.object({
  name: z.string().min(2, "Campaign name is required"),
  templateId: z.string().min(1, "Please select a template"),
});

type CampaignFormValues = z.infer<typeof campaignSchema>;

export default function CreateCampaignPage() {
  const router = useRouter();
  const queryClient = useQueryClient();

  const { data: templates, isLoading: isLoadingTemplates } = useQuery({
    queryKey: ["whatsapp-templates"],
    queryFn: WhatsAppService.getTemplates,
  });

  const createMutation = useMutation({
    mutationFn: (data: Partial<Parameters<typeof WhatsAppService.createCampaign>[0]>) => 
      WhatsAppService.createCampaign({ ...data, type: "WhatsApp", status: "Running", totalMessages: 2 }), // Mock total
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["campaigns"] });
      toast.success("Campaign launched successfully!");
      router.push("/dashboard/whatsapp");
    },
    onError: (error: any) => {
      toast.error(error.response?.data?.message || "Failed to create campaign");
    },
  });

  const form = useForm<CampaignFormValues>({
    resolver: zodResolver(campaignSchema),
    defaultValues: {
      name: "",
      templateId: "",
    },
  });

  const onSubmit = (data: CampaignFormValues) => {
    createMutation.mutate(data);
  };

  return (
    <div className="flex flex-col gap-6 max-w-3xl mx-auto">
      <div>
        <h1 className="text-3xl font-bold tracking-tight">Create Campaign</h1>
        <p className="text-muted-foreground mt-1">Send a bulk WhatsApp message to your contacts.</p>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Campaign Details</CardTitle>
          <CardDescription>Setup your campaign settings and select a template.</CardDescription>
        </CardHeader>
        <form onSubmit={form.handleSubmit(onSubmit)}>
          <CardContent className="space-y-6">
            <div className="space-y-2">
              <Label htmlFor="name">Campaign Name</Label>
              <Input id="name" placeholder="e.g. Summer Sale 2026" {...form.register("name")} />
              {form.formState.errors.name && (
                <p className="text-xs text-destructive">{form.formState.errors.name.message}</p>
              )}
            </div>

            <div className="space-y-2">
              <Label htmlFor="templateId">Message Template</Label>
              <Select 
                onValueChange={(val) => form.setValue("templateId", val || "")} 
                defaultValue={form.getValues("templateId") || ""}
              >
                <SelectTrigger>
                  <SelectValue placeholder="Select an approved template" />
                </SelectTrigger>
                <SelectContent>
                  {isLoadingTemplates ? (
                    <SelectItem value="loading" disabled>Loading templates...</SelectItem>
                  ) : templates?.filter(t => t.status === "APPROVED").length === 0 ? (
                    <SelectItem value="none" disabled>No approved templates found</SelectItem>
                  ) : (
                    templates?.filter(t => t.status === "APPROVED").map(t => (
                      <SelectItem key={t.id} value={t.id}>{t.name} ({t.language})</SelectItem>
                    ))
                  )}
                </SelectContent>
              </Select>
              {form.formState.errors.templateId && (
                <p className="text-xs text-destructive">{form.formState.errors.templateId.message}</p>
              )}
              <div className="text-xs text-muted-foreground mt-1">
                Only approved templates can be used. <Link href="/dashboard/whatsapp/templates" className="text-primary hover:underline">Manage templates</Link>
              </div>
            </div>

            <div className="rounded-lg border p-4 bg-muted/20">
              <div className="flex items-center gap-3 mb-2">
                <Users className="h-5 w-5 text-muted-foreground" />
                <h4 className="font-medium">Recipients</h4>
              </div>
              <p className="text-sm text-muted-foreground">
                For this demo, the campaign will be sent to 2 mock contacts in your list.
              </p>
            </div>
          </CardContent>
          <CardFooter className="flex justify-between">
            <Button type="button" variant="outline" onClick={() => router.back()}>Cancel</Button>
            <Button type="submit" disabled={createMutation.isPending}>
              <Send className="mr-2 h-4 w-4" />
              {createMutation.isPending ? "Launching..." : "Launch Campaign"}
            </Button>
          </CardFooter>
        </form>
      </Card>
    </div>
  );
}
