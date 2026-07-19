"use client";

import { useEffect } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { toast } from "sonner";
import { Search, Globe, Image as ImageIcon, Sparkles } from "lucide-react";

import { Card, CardContent, CardDescription, CardHeader, CardTitle, CardFooter } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Skeleton } from "@/components/ui/skeleton";

import { SEOService, UpdateSEODto } from "@/lib/seo-service";
import { DomainsService } from "@/lib/domains-service";

const seoSchema = z.object({
  metaTitle: z.string().max(60, "Title should be under 60 characters").optional().or(z.literal("")),
  metaDescription: z.string().max(160, "Description should be under 160 characters").optional().or(z.literal("")),
  keywords: z.string().optional().or(z.literal("")),
  canonicalUrl: z.string().url("Must be a valid URL").optional().or(z.literal("")),
  openGraphTitle: z.string().max(60).optional().or(z.literal("")),
  openGraphDescription: z.string().max(200).optional().or(z.literal("")),
  openGraphImage: z.string().url("Must be a valid URL").optional().or(z.literal("")),
  twitterCard: z.string(),
  robots: z.string(),
});

type SEOFormValues = z.infer<typeof seoSchema>;

export default function SEOPage() {
  const queryClient = useQueryClient();

  const { data: seo, isLoading, isError } = useQuery({
    queryKey: ["seo"],
    queryFn: SEOService.getSEO,
  });

  const { data: domains } = useQuery({
    queryKey: ["domains"],
    queryFn: DomainsService.getDomains,
  });

  const primaryDomain = domains?.find((d) => d.isPrimary)?.domain || "yourdomain.com";

  const updateMutation = useMutation({
    mutationFn: (data: UpdateSEODto) => SEOService.updateSEO(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["seo"] });
      toast.success("SEO settings updated successfully");
    },
    onError: (error: Error | { response?: { data?: { message?: string } } }) => {
      const err = error as { response?: { data?: { message?: string } } };
      toast.error(err.response?.data?.message || "Failed to update SEO");
    },
  });

  const form = useForm<SEOFormValues>({
    resolver: zodResolver(seoSchema),
    defaultValues: {
      metaTitle: "",
      metaDescription: "",
      keywords: "",
      canonicalUrl: "",
      openGraphTitle: "",
      openGraphDescription: "",
      openGraphImage: "",
      twitterCard: "summary_large_image",
      robots: "index, follow",
    },
  });

  useEffect(() => {
    if (seo) {
      form.reset({
        metaTitle: seo.metaTitle || "",
        metaDescription: seo.metaDescription || "",
        keywords: seo.keywords || "",
        canonicalUrl: seo.canonicalUrl || "",
        openGraphTitle: seo.openGraphTitle || "",
        openGraphDescription: seo.openGraphDescription || "",
        openGraphImage: seo.openGraphImage || "",
        twitterCard: seo.twitterCard || "summary_large_image",
        robots: seo.robots || "index, follow",
      });
    }
  }, [seo, form]);

  const watchAllFields = form.watch();

  const onSubmit = (values: SEOFormValues) => {
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
        <p>Failed to load SEO settings.</p>
      </div>
    );
  }

  return (
    <div className="flex flex-col gap-6 max-w-[1400px] mx-auto">
      <div>
        <h1 className="text-3xl font-bold tracking-tight flex items-center gap-2">
          SEO Management
        </h1>
        <p className="text-muted-foreground mt-1">Optimize your public pages for search engines and social sharing.</p>
      </div>

      <div className="grid grid-cols-1 xl:grid-cols-3 gap-6">
        {/* Editor Form */}
        <div className="xl:col-span-2 flex flex-col gap-6">
          <form id="seo-form" onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
            <Card>
              <CardHeader>
                <CardTitle className="flex items-center gap-2 text-lg">
                  <Search className="h-5 w-5 text-primary" />
                  General SEO
                </CardTitle>
                <CardDescription>Configure how your site appears in search engine results.</CardDescription>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="space-y-2">
                  <div className="flex justify-between">
                    <Label htmlFor="metaTitle">Meta Title</Label>
                    <span className="text-xs text-muted-foreground">{watchAllFields.metaTitle?.length || 0} / 60</span>
                  </div>
                  <Input id="metaTitle" {...form.register("metaTitle")} placeholder="e.g. Acme Corp | Best SaaS Platform" />
                  {form.formState.errors.metaTitle && (
                    <p className="text-xs text-destructive">{form.formState.errors.metaTitle.message}</p>
                  )}
                </div>

                <div className="space-y-2">
                  <div className="flex justify-between">
                    <Label htmlFor="metaDescription">Meta Description</Label>
                    <span className="text-xs text-muted-foreground">{watchAllFields.metaDescription?.length || 0} / 160</span>
                  </div>
                  <Textarea id="metaDescription" {...form.register("metaDescription")} placeholder="Briefly describe your company and its value proposition..." rows={3} />
                  {form.formState.errors.metaDescription && (
                    <p className="text-xs text-destructive">{form.formState.errors.metaDescription.message}</p>
                  )}
                </div>

                <div className="space-y-2">
                  <Label htmlFor="keywords">Keywords (Comma separated)</Label>
                  <Input id="keywords" {...form.register("keywords")} placeholder="saas, software, b2b, technology" />
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div className="space-y-2">
                    <Label htmlFor="canonicalUrl">Canonical URL</Label>
                    <Input id="canonicalUrl" {...form.register("canonicalUrl")} placeholder={`https://${primaryDomain}`} />
                    {form.formState.errors.canonicalUrl && (
                      <p className="text-xs text-destructive">{form.formState.errors.canonicalUrl.message}</p>
                    )}
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="robots">Robots Directives</Label>
                    <Select
                      value={form.watch("robots")}
                      onValueChange={(v) => form.setValue("robots", v || "", { shouldValidate: true })}
                    >
                      <SelectTrigger>
                        <SelectValue />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value="index, follow">Index, Follow (Recommended)</SelectItem>
                        <SelectItem value="noindex, nofollow">No Index, No Follow</SelectItem>
                        <SelectItem value="index, nofollow">Index, No Follow</SelectItem>
                        <SelectItem value="noindex, follow">No Index, Follow</SelectItem>
                      </SelectContent>
                    </Select>
                  </div>
                </div>
              </CardContent>
            </Card>

            <Card>
              <CardHeader>
                <CardTitle className="flex items-center gap-2 text-lg">
                  <Globe className="h-5 w-5 text-primary" />
                  Social Media (Open Graph & Twitter)
                </CardTitle>
                <CardDescription>Control how your links look when shared on social media like Twitter, Facebook, or LinkedIn.</CardDescription>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div className="space-y-2">
                    <Label htmlFor="openGraphTitle">OG Title</Label>
                    <Input id="openGraphTitle" {...form.register("openGraphTitle")} placeholder="Leave blank to use Meta Title" />
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="twitterCard">Twitter Card Type</Label>
                    <Select
                      value={form.watch("twitterCard")}
                      onValueChange={(v) => form.setValue("twitterCard", v || "", { shouldValidate: true })}
                    >
                      <SelectTrigger>
                        <SelectValue />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value="summary_large_image">Summary with Large Image</SelectItem>
                        <SelectItem value="summary">Summary (Small Image)</SelectItem>
                      </SelectContent>
                    </Select>
                  </div>
                </div>

                <div className="space-y-2">
                  <Label htmlFor="openGraphDescription">OG Description</Label>
                  <Textarea id="openGraphDescription" {...form.register("openGraphDescription")} placeholder="Leave blank to use Meta Description" rows={2} />
                </div>

                <div className="space-y-2">
                  <Label htmlFor="openGraphImage">OG Image URL</Label>
                  <Input id="openGraphImage" {...form.register("openGraphImage")} placeholder="https://example.com/social-cover.jpg" />
                  {form.formState.errors.openGraphImage && (
                    <p className="text-xs text-destructive">{form.formState.errors.openGraphImage.message}</p>
                  )}
                </div>
              </CardContent>
            </Card>
          </form>

          <div className="flex items-center justify-end gap-4 py-4">
            <Button
              type="button"
              variant="ghost"
              onClick={() => {
                if (seo) {
                  form.reset({
                    metaTitle: seo.metaTitle || "",
                    metaDescription: seo.metaDescription || "",
                    keywords: seo.keywords || "",
                    canonicalUrl: seo.canonicalUrl || "",
                    openGraphTitle: seo.openGraphTitle || "",
                    openGraphDescription: seo.openGraphDescription || "",
                    openGraphImage: seo.openGraphImage || "",
                    twitterCard: seo.twitterCard || "summary_large_image",
                    robots: seo.robots || "index, follow",
                  });
                }
              }}
              disabled={!form.formState.isDirty || updateMutation.isPending}
            >
              Reset Changes
            </Button>
            <Button type="submit" form="seo-form" disabled={!form.formState.isDirty || updateMutation.isPending}>
              {updateMutation.isPending ? "Saving..." : "Save SEO Settings"}
            </Button>
          </div>
        </div>

        {/* Live Preview */}
        <div className="xl:col-span-1 space-y-6">
          <div className="sticky top-6 space-y-6">
            <div>
              <h3 className="text-lg font-medium mb-4 flex items-center gap-2">
                <Search className="h-4 w-4" />
                Search Engine Preview
              </h3>
              
              <div className="p-4 border rounded-xl bg-card shadow-sm space-y-1 bg-white dark:bg-zinc-950">
                <div className="flex items-center gap-2 text-sm text-zinc-700 dark:text-zinc-300">
                  <div className="h-6 w-6 rounded-full bg-muted flex items-center justify-center shrink-0">
                    <Globe className="h-3 w-3" />
                  </div>
                  <div>
                    <p className="leading-tight truncate w-full">{watchAllFields.metaTitle || "Your Website Title"}</p>
                    <p className="text-xs text-zinc-500 truncate w-full">https://{primaryDomain}</p>
                  </div>
                </div>
                <h4 className="text-[20px] text-blue-600 dark:text-blue-400 font-medium hover:underline cursor-pointer truncate pt-1">
                  {watchAllFields.metaTitle || "Your Website Title - Best Service"}
                </h4>
                <p className="text-[14px] text-zinc-600 dark:text-zinc-400 line-clamp-2">
                  {watchAllFields.metaDescription || "Provide a brief description of your company and what you do. This will encourage users to click your link in search results."}
                </p>
              </div>
            </div>

            <div>
              <h3 className="text-lg font-medium mb-4 flex items-center gap-2">
                <Globe className="h-4 w-4" />
                Social Card Preview
              </h3>
              
              <div className="border rounded-xl bg-card shadow-sm overflow-hidden flex flex-col bg-white dark:bg-zinc-950">
                <div className="aspect-[1.91/1] w-full bg-muted border-b flex items-center justify-center overflow-hidden">
                  {watchAllFields.openGraphImage ? (
                    /* eslint-disable-next-line @next/next/no-img-element */
                    <img src={watchAllFields.openGraphImage} alt="Social Cover" className="w-full h-full object-cover" />
                  ) : (
                    <ImageIcon className="h-10 w-10 text-muted-foreground/30" />
                  )}
                </div>
                <div className="p-4">
                  <p className="text-[13px] text-zinc-500 uppercase truncate">
                    {primaryDomain}
                  </p>
                  <h4 className="text-[16px] font-semibold text-zinc-900 dark:text-zinc-100 truncate mt-1">
                    {watchAllFields.openGraphTitle || watchAllFields.metaTitle || "Your Website Title"}
                  </h4>
                  <p className="text-[14px] text-zinc-500 dark:text-zinc-400 line-clamp-1 mt-1">
                    {watchAllFields.openGraphDescription || watchAllFields.metaDescription || "A brief description of your content..."}
                  </p>
                </div>
              </div>
            </div>

            <div className="rounded-xl border bg-primary/5 p-4 mt-6 flex items-start gap-3">
              <Sparkles className="h-5 w-5 text-primary shrink-0 mt-0.5" />
              <div>
                <h4 className="font-medium text-sm">AI SEO Recommendations</h4>
                <p className="text-xs text-muted-foreground mt-1">
                  In future updates, our AI agent will analyze your content and automatically suggest optimal keywords, meta tags, and content improvements to boost your rankings.
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
