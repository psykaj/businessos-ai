"use client";

import { useEffect, useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { toast } from "sonner";
import { Save, Key, Phone, Building2, Link } from "lucide-react";

import { Card, CardContent, CardDescription, CardHeader, CardTitle, CardFooter } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Skeleton } from "@/components/ui/skeleton";
import { WhatsAppService, WhatsAppSettings } from "@/lib/whatsapp-service";

const settingsSchema = z.object({
  phoneNumberId: z.string().min(1, "Phone Number ID is required"),
  businessAccountId: z.string().min(1, "WhatsApp Business Account ID is required"),
  accessToken: z.string().min(1, "Access Token is required"),
  webhookVerifyToken: z.string().min(1, "Webhook Verify Token is required"),
});

type SettingsFormValues = z.infer<typeof settingsSchema>;

export default function WhatsAppSettingsPage() {
  const queryClient = useQueryClient();

  const { data: settings, isLoading } = useQuery({
    queryKey: ["whatsapp-settings"],
    queryFn: WhatsAppService.getSettings,
  });

  const mutation = useMutation({
    mutationFn: (data: WhatsAppSettings) => WhatsAppService.saveSettings(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["whatsapp-settings"] });
      toast.success("WhatsApp settings saved successfully");
    },
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    onError: (error: any) => {
      toast.error(error.response?.data?.message || "Failed to save settings");
    },
  });

  const form = useForm<SettingsFormValues>({
    resolver: zodResolver(settingsSchema),
    defaultValues: {
      phoneNumberId: "",
      businessAccountId: "",
      accessToken: "",
      webhookVerifyToken: "",
    },
  });

  useEffect(() => {
    if (settings) {
      form.reset({
        phoneNumberId: settings.phoneNumberId || "",
        businessAccountId: settings.businessAccountId || "",
        accessToken: settings.accessToken || "",
        webhookVerifyToken: settings.webhookVerifyToken || "",
      });
    }
  }, [settings, form]);

  const onSubmit = (data: SettingsFormValues) => {
    mutation.mutate(data);
  };

  if (isLoading) {
    return (
      <div className="space-y-6 max-w-2xl mx-auto">
        <Skeleton className="h-10 w-[250px]" />
        <Skeleton className="h-[400px] w-full" />
      </div>
    );
  }

  return (
    <div className="flex flex-col gap-6 max-w-2xl mx-auto">
      <div>
        <h1 className="text-3xl font-bold tracking-tight">WhatsApp Settings</h1>
        <p className="text-muted-foreground mt-1">Configure your WhatsApp Cloud API credentials.</p>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Meta App Configuration</CardTitle>
          <CardDescription>
            Enter the details from your Meta Developer Portal to connect the WhatsApp Business API.
          </CardDescription>
        </CardHeader>
        <form onSubmit={form.handleSubmit(onSubmit)}>
          <CardContent className="space-y-6">
            <div className="space-y-2">
              <Label htmlFor="phoneNumberId" className="flex items-center gap-2">
                <Phone className="h-4 w-4" /> Phone Number ID
              </Label>
              <Input id="phoneNumberId" placeholder="e.g. 102938475619283" {...form.register("phoneNumberId")} />
              {form.formState.errors.phoneNumberId && (
                <p className="text-xs text-destructive">{form.formState.errors.phoneNumberId.message}</p>
              )}
            </div>
            
            <div className="space-y-2">
              <Label htmlFor="businessAccountId" className="flex items-center gap-2">
                <Building2 className="h-4 w-4" /> WhatsApp Business Account ID
              </Label>
              <Input id="businessAccountId" placeholder="e.g. 192837465019283" {...form.register("businessAccountId")} />
              {form.formState.errors.businessAccountId && (
                <p className="text-xs text-destructive">{form.formState.errors.businessAccountId.message}</p>
              )}
            </div>

            <div className="space-y-2">
              <Label htmlFor="accessToken" className="flex items-center gap-2">
                <Key className="h-4 w-4" /> System User Access Token
              </Label>
              <Input id="accessToken" type="password" placeholder="EAAB..." {...form.register("accessToken")} />
              {form.formState.errors.accessToken && (
                <p className="text-xs text-destructive">{form.formState.errors.accessToken.message}</p>
              )}
              <p className="text-xs text-muted-foreground">Make sure to use a permanent token, not a temporary one.</p>
            </div>

            <div className="space-y-2">
              <Label htmlFor="webhookVerifyToken" className="flex items-center gap-2">
                <Link className="h-4 w-4" /> Webhook Verify Token
              </Label>
              <Input id="webhookVerifyToken" placeholder="A secure random string" {...form.register("webhookVerifyToken")} />
              {form.formState.errors.webhookVerifyToken && (
                <p className="text-xs text-destructive">{form.formState.errors.webhookVerifyToken.message}</p>
              )}
              <p className="text-xs text-muted-foreground">Use this token when configuring your webhook URL in the Meta portal.</p>
            </div>
          </CardContent>
          <CardFooter>
            <Button type="submit" disabled={mutation.isPending}>
              <Save className="mr-2 h-4 w-4" />
              {mutation.isPending ? "Saving..." : "Save Configuration"}
            </Button>
          </CardFooter>
        </form>
      </Card>
    </div>
  );
}
