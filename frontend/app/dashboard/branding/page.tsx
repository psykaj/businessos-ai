"use client";

import { useEffect, useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { toast } from "sonner";
import { Paintbrush, LayoutTemplate, Type, Link as LinkIcon, Upload, Trash2, Mail, Building, Palette, ImageIcon } from "lucide-react";

import { Card, CardContent, CardDescription, CardHeader, CardTitle, CardFooter } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Skeleton } from "@/components/ui/skeleton";

import { BrandingService, BrandDto, UpdateBrandDto } from "@/lib/branding-service";

const brandingSchema = z.object({
  companyName: z.string().min(2, "Company name must be at least 2 characters"),
  supportEmail: z.string().email("Please enter a valid email address").optional().or(z.literal("")),
  primaryColor: z.string().regex(/^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$/, "Valid hex code required"),
  secondaryColor: z.string().regex(/^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$/, "Valid hex code required"),
  accentColor: z.string().regex(/^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$/, "Valid hex code required"),
  fontFamily: z.string().min(1, "Please select a font family"),
  footerText: z.string().max(500, "Footer text too long").optional().or(z.literal("")),
});

type BrandingFormValues = z.infer<typeof brandingSchema>;

const FONT_OPTIONS = [
  { value: "Inter, sans-serif", label: "Inter (Modern & Clean)" },
  { value: "Roboto, sans-serif", label: "Roboto (Tech & Minimal)" },
  { value: "Outfit, sans-serif", label: "Outfit (Geometric & Friendly)" },
  { value: "Playfair Display, serif", label: "Playfair (Elegant & Serif)" },
  { value: "Space Grotesk, sans-serif", label: "Space Grotesk (Tech & Bold)" },
];

export default function BrandingPage() {
  const queryClient = useQueryClient();
  const [logoUploading, setLogoUploading] = useState(false);
  const [faviconUploading, setFaviconUploading] = useState(false);

  const { data: brand, isLoading, isError } = useQuery({
    queryKey: ["brand"],
    queryFn: BrandingService.getBrand,
  });

  const updateMutation = useMutation({
    mutationFn: (data: UpdateBrandDto) => BrandingService.updateBrand(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["brand"] });
      toast.success("Branding updated successfully");
    },
    onError: (error: Error | { response?: { data?: { message?: string } } }) => {
      const err = error as { response?: { data?: { message?: string } } };
      toast.error(err.response?.data?.message || "Failed to update branding");
    },
  });

  const form = useForm<BrandingFormValues>({
    resolver: zodResolver(brandingSchema),
    defaultValues: {
      companyName: "",
      supportEmail: "",
      primaryColor: "#000000",
      secondaryColor: "#ffffff",
      accentColor: "#0ea5e9",
      fontFamily: "Inter, sans-serif",
      footerText: "",
    },
  });

  useEffect(() => {
    if (brand) {
      form.reset({
        companyName: brand.companyName || "",
        supportEmail: brand.supportEmail || "",
        primaryColor: brand.primaryColor || "#000000",
        secondaryColor: brand.secondaryColor || "#ffffff",
        accentColor: brand.accentColor || "#0ea5e9",
        fontFamily: brand.fontFamily || "Inter, sans-serif",
        footerText: brand.footerText || "",
      });
    }
  }, [brand, form]);

  const watchAllFields = form.watch();

  const handleLogoUpload = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;
    setLogoUploading(true);
    const toastId = toast.loading("Uploading logo...");
    try {
      await BrandingService.uploadLogo(file);
      queryClient.invalidateQueries({ queryKey: ["brand"] });
      toast.success("Logo updated successfully", { id: toastId });
    } catch (err: unknown) {
      const error = err as { response?: { data?: { message?: string } } };
      toast.error(error.response?.data?.message || "Failed to upload logo", { id: toastId });
    } finally {
      setLogoUploading(false);
      e.target.value = "";
    }
  };

  const handleFaviconUpload = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;
    setFaviconUploading(true);
    const toastId = toast.loading("Uploading favicon...");
    try {
      await BrandingService.uploadFavicon(file);
      queryClient.invalidateQueries({ queryKey: ["brand"] });
      toast.success("Favicon updated successfully", { id: toastId });
    } catch (err: unknown) {
      const error = err as { response?: { data?: { message?: string } } };
      toast.error(error.response?.data?.message || "Failed to upload favicon", { id: toastId });
    } finally {
      setFaviconUploading(false);
      e.target.value = "";
    }
  };

  const onSubmit = (values: BrandingFormValues) => {
    updateMutation.mutate(values);
  };

  if (isLoading) {
    return (
      <div className="space-y-6">
        <Skeleton className="h-10 w-48" />
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          <Skeleton className="h-[600px] lg:col-span-2" />
          <Skeleton className="h-[600px]" />
        </div>
      </div>
    );
  }

  if (isError) {
    return (
      <div className="p-8 text-center text-destructive">
        <p>Failed to load branding settings. Please try again later.</p>
      </div>
    );
  }

  return (
    <div className="flex flex-col gap-6 max-w-[1400px] mx-auto">
      <div>
        <h1 className="text-3xl font-bold tracking-tight">Branding</h1>
        <p className="text-muted-foreground mt-1">Customize how your platform looks to your customers.</p>
      </div>

      <div className="grid grid-cols-1 xl:grid-cols-3 gap-6">
        {/* Editor Form */}
        <div className="xl:col-span-2 flex flex-col gap-6">
          <form id="branding-form" onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
            <Card>
              <CardHeader>
                <CardTitle className="flex items-center gap-2 text-lg">
                  <Building className="h-5 w-5 text-primary" />
                  General Information
                </CardTitle>
                <CardDescription>Basic details about your company.</CardDescription>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div className="space-y-2">
                    <Label htmlFor="companyName">Company Name <span className="text-destructive">*</span></Label>
                    <Input id="companyName" {...form.register("companyName")} />
                    {form.formState.errors.companyName && (
                      <p className="text-xs text-destructive">{form.formState.errors.companyName.message}</p>
                    )}
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="supportEmail">Support Email</Label>
                    <div className="relative">
                      <Mail className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
                      <Input id="supportEmail" type="email" className="pl-9" {...form.register("supportEmail")} placeholder="support@company.com" />
                    </div>
                    {form.formState.errors.supportEmail && (
                      <p className="text-xs text-destructive">{form.formState.errors.supportEmail.message}</p>
                    )}
                  </div>
                </div>
              </CardContent>
            </Card>

            <Card>
              <CardHeader>
                <CardTitle className="flex items-center gap-2 text-lg">
                  <Palette className="h-5 w-5 text-primary" />
                  Brand Colors & Typography
                </CardTitle>
                <CardDescription>Choose colors and fonts that match your brand identity.</CardDescription>
              </CardHeader>
              <CardContent className="space-y-6">
                <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                  <div className="space-y-2">
                    <Label>Primary Color</Label>
                    <div className="flex gap-2">
                      <Input type="color" className="w-12 h-10 p-1 cursor-pointer" {...form.register("primaryColor")} />
                      <Input {...form.register("primaryColor")} />
                    </div>
                    {form.formState.errors.primaryColor && (
                      <p className="text-xs text-destructive">{form.formState.errors.primaryColor.message}</p>
                    )}
                  </div>
                  <div className="space-y-2">
                    <Label>Secondary Color</Label>
                    <div className="flex gap-2">
                      <Input type="color" className="w-12 h-10 p-1 cursor-pointer" {...form.register("secondaryColor")} />
                      <Input {...form.register("secondaryColor")} />
                    </div>
                    {form.formState.errors.secondaryColor && (
                      <p className="text-xs text-destructive">{form.formState.errors.secondaryColor.message}</p>
                    )}
                  </div>
                  <div className="space-y-2">
                    <Label>Accent Color</Label>
                    <div className="flex gap-2">
                      <Input type="color" className="w-12 h-10 p-1 cursor-pointer" {...form.register("accentColor")} />
                      <Input {...form.register("accentColor")} />
                    </div>
                    {form.formState.errors.accentColor && (
                      <p className="text-xs text-destructive">{form.formState.errors.accentColor.message}</p>
                    )}
                  </div>
                </div>

                <div className="space-y-2">
                  <Label>Primary Font Family</Label>
                  <Select
                    value={form.watch("fontFamily")}
                    onValueChange={(v) => form.setValue("fontFamily", v || "", { shouldValidate: true })}
                  >
                    <SelectTrigger>
                      <SelectValue placeholder="Select a font" />
                    </SelectTrigger>
                    <SelectContent>
                      {FONT_OPTIONS.map((f) => (
                        <SelectItem key={f.value} value={f.value}>
                          <span style={{ fontFamily: f.value }}>{f.label}</span>
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>
              </CardContent>
            </Card>

            <Card>
              <CardHeader>
                <CardTitle className="flex items-center gap-2 text-lg">
                  <LayoutTemplate className="h-5 w-5 text-primary" />
                  Footer
                </CardTitle>
                <CardDescription>Customize the footer on your public pages.</CardDescription>
              </CardHeader>
              <CardContent>
                <div className="space-y-2">
                  <Label htmlFor="footerText">Footer Text</Label>
                  <Textarea
                    id="footerText"
                    {...form.register("footerText")}
                    placeholder={`© ${new Date().getFullYear()} Your Company. All rights reserved.`}
                    rows={3}
                  />
                  {form.formState.errors.footerText && (
                    <p className="text-xs text-destructive">{form.formState.errors.footerText.message}</p>
                  )}
                </div>
              </CardContent>
            </Card>
          </form>

          {/* Media uploads are separate from the main form since they upload immediately */}
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2 text-lg">
                <ImageIcon className="h-5 w-5 text-primary" />
                Media Assets
              </CardTitle>
              <CardDescription>Upload your brand logo and favicon.</CardDescription>
            </CardHeader>
            <CardContent className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div className="space-y-3">
                <Label>Company Logo</Label>
                <div className="flex items-center gap-4 border rounded-lg p-4 bg-muted/20">
                  <div className="h-16 w-16 bg-white border rounded-md flex items-center justify-center p-2 overflow-hidden shadow-sm">
                    {brand?.logoUrl ? (
                      /* eslint-disable-next-line @next/next/no-img-element */
                      <img src={brand.logoUrl} alt="Logo" className="max-w-full max-h-full object-contain" />
                    ) : (
                      <ImageIcon className="h-6 w-6 text-muted-foreground/40" />
                    )}
                  </div>
                  <div className="flex-1">
                    <p className="text-xs text-muted-foreground mb-2">Recommended: 512x512px (PNG, SVG)</p>
                    <input type="file" id="logo-upload" className="hidden" accept="image/*" onChange={handleLogoUpload} disabled={logoUploading} />
                    <Button variant="outline" size="sm" onClick={() => document.getElementById("logo-upload")?.click()} disabled={logoUploading}>
                      <Upload className="mr-2 h-3.5 w-3.5" />
                      {logoUploading ? "Uploading..." : "Upload Logo"}
                    </Button>
                  </div>
                </div>
              </div>

              <div className="space-y-3">
                <Label>Favicon</Label>
                <div className="flex items-center gap-4 border rounded-lg p-4 bg-muted/20">
                  <div className="h-12 w-12 bg-white border rounded-md flex items-center justify-center p-2 overflow-hidden shadow-sm">
                    {brand?.faviconUrl ? (
                      /* eslint-disable-next-line @next/next/no-img-element */
                      <img src={brand.faviconUrl} alt="Favicon" className="max-w-full max-h-full object-contain" />
                    ) : (
                      <ImageIcon className="h-5 w-5 text-muted-foreground/40" />
                    )}
                  </div>
                  <div className="flex-1">
                    <p className="text-xs text-muted-foreground mb-2">Recommended: 32x32px (ICO, PNG)</p>
                    <input type="file" id="favicon-upload" className="hidden" accept="image/*" onChange={handleFaviconUpload} disabled={faviconUploading} />
                    <Button variant="outline" size="sm" onClick={() => document.getElementById("favicon-upload")?.click()} disabled={faviconUploading}>
                      <Upload className="mr-2 h-3.5 w-3.5" />
                      {faviconUploading ? "Uploading..." : "Upload Favicon"}
                    </Button>
                  </div>
                </div>
              </div>
            </CardContent>
          </Card>

          <div className="flex items-center justify-end gap-4 py-4">
            <Button
              type="button"
              variant="ghost"
              onClick={() => {
                if (brand) {
                  form.reset({
                    companyName: brand.companyName || "",
                    supportEmail: brand.supportEmail || "",
                    primaryColor: brand.primaryColor || "#000000",
                    secondaryColor: brand.secondaryColor || "#ffffff",
                    accentColor: brand.accentColor || "#0ea5e9",
                    fontFamily: brand.fontFamily || "Inter, sans-serif",
                    footerText: brand.footerText || "",
                  });
                }
              }}
              disabled={!form.formState.isDirty || updateMutation.isPending}
            >
              Reset Changes
            </Button>
            <Button type="submit" form="branding-form" disabled={!form.formState.isDirty || updateMutation.isPending}>
              {updateMutation.isPending ? "Saving..." : "Save Branding"}
            </Button>
          </div>
        </div>

        {/* Live Preview */}
        <div className="xl:col-span-1">
          <div className="sticky top-6">
            <h3 className="text-lg font-medium mb-4 flex items-center gap-2">
              <LayoutTemplate className="h-4 w-4" />
              Live Preview
            </h3>
            
            {/* Preview Box */}
            <div 
              className="border rounded-xl shadow-lg overflow-hidden flex flex-col bg-background transition-all duration-300 h-[600px]"
              style={{ fontFamily: watchAllFields.fontFamily }}
            >
              {/* Header Preview */}
              <div 
                className="h-14 border-b px-4 flex items-center justify-between"
                style={{ backgroundColor: watchAllFields.secondaryColor }}
              >
                <div className="flex items-center gap-2">
                  {brand?.logoUrl ? (
                    /* eslint-disable-next-line @next/next/no-img-element */
                    <img src={brand.logoUrl} alt="Logo" className="h-6 w-auto" />
                  ) : (
                    <div className="h-6 w-6 rounded bg-primary/20 flex items-center justify-center font-bold text-xs" style={{ color: watchAllFields.primaryColor }}>
                      {watchAllFields.companyName ? watchAllFields.companyName.charAt(0).toUpperCase() : "B"}
                    </div>
                  )}
                  <span className="font-semibold text-sm" style={{ color: watchAllFields.primaryColor }}>
                    {watchAllFields.companyName || "Your Company"}
                  </span>
                </div>
                <div className="flex gap-3">
                  <div className="h-2 w-12 rounded bg-muted/60"></div>
                  <div className="h-2 w-12 rounded bg-muted/60"></div>
                </div>
              </div>

              {/* Body Preview */}
              <div className="flex-1 p-6 flex flex-col items-center justify-center text-center gap-4 bg-gradient-to-b from-transparent to-muted/20">
                <div 
                  className="inline-flex items-center justify-center h-16 w-16 rounded-2xl shadow-sm mb-2"
                  style={{ backgroundColor: watchAllFields.primaryColor, color: "#fff" }}
                >
                  <Palette className="h-8 w-8" />
                </div>
                <h2 className="text-2xl font-bold tracking-tight" style={{ color: watchAllFields.primaryColor }}>
                  Welcome to {watchAllFields.companyName || "Your Company"}
                </h2>
                <p className="text-sm max-w-[250px] mx-auto text-muted-foreground">
                  This is how your brand colors and typography will appear to your users.
                </p>
                <Button 
                  className="mt-4 hover:opacity-90 transition-opacity rounded-full px-6 shadow-md"
                  style={{ backgroundColor: watchAllFields.accentColor, color: "#fff" }}
                >
                  Call to Action
                </Button>

                {watchAllFields.supportEmail && (
                  <p className="text-xs text-muted-foreground mt-8">
                    Contact us at: <span style={{ color: watchAllFields.accentColor }}>{watchAllFields.supportEmail}</span>
                  </p>
                )}
              </div>

              {/* Footer Preview */}
              <div 
                className="h-12 border-t flex items-center justify-center px-4"
                style={{ backgroundColor: watchAllFields.secondaryColor }}
              >
                <p className="text-[10px] text-muted-foreground">
                  {watchAllFields.footerText || `© ${new Date().getFullYear()} ${watchAllFields.companyName || "Your Company"}. All rights reserved.`}
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
