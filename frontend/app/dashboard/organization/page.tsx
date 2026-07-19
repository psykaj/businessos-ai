"use client";

import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { toast } from "sonner";
import { useOrganization, useUpdateOrganization, useUploadLogo } from "@/hooks/use-organization";
import { useAuth } from "@/contexts/auth-context";

import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Building, Globe, MapPin, Loader2, Upload } from "lucide-react";

const orgSchema = z.object({
  name: z.string().min(2, "Name must be at least 2 characters."),
  slug: z.string().min(2, "Slug must be at least 2 characters.").regex(/^[a-z0-9-]+$/, "Only lowercase letters, numbers, and dashes are allowed."),
  industry: z.string().optional(),
  website: z.string().url("Must be a valid URL").optional().or(z.literal("")),
  address: z.string().optional(),
  timeZone: z.string().min(1, "Time zone is required"),
  language: z.string().min(1, "Language is required"),
  currency: z.string().min(1, "Currency is required"),
});

type OrgFormValues = z.infer<typeof orgSchema>;

export default function OrganizationPage() {
  const { user } = useAuth();
  const orgId = user?.organizationId;
  const { data: org, isLoading } = useOrganization(orgId);
  const updateOrg = useUpdateOrganization();
  const uploadLogo = useUploadLogo();
  const [logoPreview, setLogoPreview] = useState<string | null>(null);

  const form = useForm<OrgFormValues>({
    resolver: zodResolver(orgSchema),
    defaultValues: {
      name: "",
      slug: "",
      industry: "",
      website: "",
      address: "",
      timeZone: "UTC",
      language: "en-US",
      currency: "USD",
    },
  });

  useEffect(() => {
    if (org) {
      form.reset({
        name: org.name,
        slug: org.slug,
        industry: org.industry || "",
        website: org.website || "",
        address: org.address || "",
        timeZone: org.timeZone || "UTC",
        language: org.language || "en-US",
        currency: org.currency || "USD",
      });
    }
  }, [org, form]);

  const displayLogo = logoPreview || org?.logoUrl;

  const onSubmit = async (data: OrgFormValues) => {
    if (!orgId) return;
    try {
      await updateOrg.mutateAsync({ orgId, data });
      toast.success("Organization settings updated successfully.");
    } catch (error) {
      toast.error("Failed to update settings.");
    }
  };

  const handleLogoUpload = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file || !orgId) return;

    const reader = new FileReader();
    reader.onload = (e) => setLogoPreview(e.target?.result as string);
    reader.readAsDataURL(file);

    try {
      await uploadLogo.mutateAsync({ orgId, file });
      toast.success("Logo uploaded successfully.");
    } catch (error) {
      toast.error("Failed to upload logo.");
    }
  };

  if (isLoading) {
    return (
      <div className="flex h-[400px] items-center justify-center">
        <Loader2 className="h-8 w-8 animate-spin text-muted-foreground" />
      </div>
    );
  }

  return (
    <div className="mx-auto max-w-4xl space-y-6">
      <div>
        <h1 className="text-2xl font-bold tracking-tight text-foreground">Organization Settings</h1>
        <p className="mt-1 text-sm text-muted-foreground">Manage your organization&apos;s profile and global preferences.</p>
      </div>

      <div className="grid gap-6 md:grid-cols-3">
        <div className="md:col-span-1 space-y-6">
          <Card>
            <CardHeader>
              <CardTitle>Organization Logo</CardTitle>
              <CardDescription>Update your company logo.</CardDescription>
            </CardHeader>
            <CardContent className="flex flex-col items-center gap-4">
              <div className="relative flex h-32 w-32 items-center justify-center rounded-xl border border-dashed border-border bg-muted overflow-hidden">
                {displayLogo ? (
                  <img src={displayLogo} alt="Logo preview" className="h-full w-full object-cover" />
                ) : (
                  <Building className="h-10 w-10 text-muted-foreground/40" />
                )}
              </div>
              <Label htmlFor="logo-upload" className="cursor-pointer">
                <div className="flex items-center gap-2 rounded-md bg-secondary px-4 py-2 text-sm font-medium text-secondary-foreground hover:bg-secondary/80 transition-colors">
                  <Upload className="h-4 w-4" />
                  {uploadLogo.isPending ? "Uploading..." : "Upload Logo"}
                </div>
                <Input
                  id="logo-upload"
                  type="file"
                  accept="image/*"
                  className="hidden"
                  onChange={handleLogoUpload}
                  disabled={uploadLogo.isPending}
                />
              </Label>
            </CardContent>
          </Card>
        </div>

        <div className="md:col-span-2">
          <Card>
            <CardHeader>
              <CardTitle>General Information</CardTitle>
              <CardDescription>Basic details about your organization.</CardDescription>
            </CardHeader>
            <CardContent>
              <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
                <div className="grid gap-4 sm:grid-cols-2">
                  <div className="space-y-2">
                    <Label htmlFor="name">Organization Name</Label>
                    <Input id="name" {...form.register("name")} />
                    {form.formState.errors.name && (
                      <p className="text-sm text-destructive">{form.formState.errors.name.message}</p>
                    )}
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="slug">Workspace Slug</Label>
                    <Input id="slug" {...form.register("slug")} />
                    {form.formState.errors.slug && (
                      <p className="text-sm text-destructive">{form.formState.errors.slug.message}</p>
                    )}
                  </div>
                </div>

                <div className="grid gap-4 sm:grid-cols-2">
                  <div className="space-y-2">
                    <Label htmlFor="industry">Industry</Label>
                    <Input id="industry" {...form.register("industry")} />
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="website">Website</Label>
                    <div className="relative">
                      <Globe className="absolute left-3 top-2.5 h-4 w-4 text-muted-foreground" />
                      <Input id="website" className="pl-9" {...form.register("website")} />
                    </div>
                    {form.formState.errors.website && (
                      <p className="text-sm text-destructive">{form.formState.errors.website.message}</p>
                    )}
                  </div>
                </div>

                <div className="space-y-2">
                  <Label htmlFor="address">Address</Label>
                  <div className="relative">
                    <MapPin className="absolute left-3 top-2.5 h-4 w-4 text-muted-foreground" />
                    <Input id="address" className="pl-9" {...form.register("address")} />
                  </div>
                </div>

                <div className="grid gap-4 sm:grid-cols-3 border-t pt-6">
                  <div className="space-y-2">
                    <Label htmlFor="timeZone">Time Zone</Label>
                    <Select onValueChange={(val) => form.setValue("timeZone", val as string)} defaultValue={form.getValues("timeZone")}>
                      <SelectTrigger>
                        <SelectValue placeholder="Select Time Zone" />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value="UTC">UTC</SelectItem>
                        <SelectItem value="America/New_York">Eastern Time</SelectItem>
                        <SelectItem value="America/Los_Angeles">Pacific Time</SelectItem>
                        <SelectItem value="Europe/London">London</SelectItem>
                        <SelectItem value="Asia/Kolkata">India (IST)</SelectItem>
                      </SelectContent>
                    </Select>
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="language">Language</Label>
                    <Select onValueChange={(val) => form.setValue("language", val as string)} defaultValue={form.getValues("language")}>
                      <SelectTrigger>
                        <SelectValue placeholder="Select Language" />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value="en-US">English (US)</SelectItem>
                        <SelectItem value="en-GB">English (UK)</SelectItem>
                        <SelectItem value="es-ES">Spanish</SelectItem>
                        <SelectItem value="fr-FR">French</SelectItem>
                      </SelectContent>
                    </Select>
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="currency">Currency</Label>
                    <Select onValueChange={(val) => form.setValue("currency", val as string)} defaultValue={form.getValues("currency")}>
                      <SelectTrigger>
                        <SelectValue placeholder="Select Currency" />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value="USD">USD ($)</SelectItem>
                        <SelectItem value="EUR">EUR (€)</SelectItem>
                        <SelectItem value="GBP">GBP (£)</SelectItem>
                        <SelectItem value="INR">INR (₹)</SelectItem>
                      </SelectContent>
                    </Select>
                  </div>
                </div>

                <div className="flex justify-end gap-3 border-t pt-6">
                  <Button
                    type="button"
                    variant="outline"
                    onClick={() => form.reset()}
                    disabled={!form.formState.isDirty || updateOrg.isPending}
                  >
                    Reset
                  </Button>
                  <Button
                    type="submit"
                    disabled={!form.formState.isDirty || updateOrg.isPending}
                  >
                    {updateOrg.isPending && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                    Save Changes
                  </Button>
                </div>
              </form>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  );
}
