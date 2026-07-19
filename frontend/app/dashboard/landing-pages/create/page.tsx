"use client";

import { useMutation } from "@tanstack/react-query";
import { useRouter } from "next/navigation";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { toast } from "sonner";
import { LayoutTemplate, ArrowLeft } from "lucide-react";
import Link from "next/link";

import { Card, CardContent, CardDescription, CardHeader, CardTitle, CardFooter } from "@/components/ui/card";
import { Button, buttonVariants } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";

import { LandingPagesService, CreateLandingPageDto } from "@/lib/landing-pages-service";

const pageSchema = z.object({
  title: z.string().min(2, "Title must be at least 2 characters"),
  slug: z.string().regex(/^[a-z0-9-]+$/, "Slug can only contain lowercase letters, numbers, and hyphens"),
});

export default function CreateLandingPage() {
  const router = useRouter();

  const createMutation = useMutation({
    mutationFn: (data: CreateLandingPageDto) => LandingPagesService.createPage(data),
    onSuccess: (newPage) => {
      toast.success("Page created. Redirecting to builder...");
      router.push(`/dashboard/landing-pages/edit/${newPage.id}`);
    },
    onError: (error: Error | { response?: { data?: { message?: string } } }) => {
      const err = error as { response?: { data?: { message?: string } } };
      toast.error(err.response?.data?.message || "Failed to create page");
    },
  });

  const form = useForm<z.infer<typeof pageSchema>>({
    resolver: zodResolver(pageSchema),
    defaultValues: {
      title: "",
      slug: "",
    },
  });

  const onSubmit = (values: z.infer<typeof pageSchema>) => {
    createMutation.mutate(values);
  };

  return (
    <div className="flex flex-col gap-6 max-w-[600px] mx-auto mt-10">
      <Link href="/dashboard/landing-pages" className={`${buttonVariants({ variant: "ghost" })} self-start -ml-4 text-muted-foreground`}>
        <ArrowLeft className="h-4 w-4 mr-2" /> Back to Pages
      </Link>

      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <LayoutTemplate className="h-5 w-5 text-primary" />
            Create Landing Page
          </CardTitle>
          <CardDescription>Give your new page a title and a URL slug.</CardDescription>
        </CardHeader>
        <CardContent>
          <form id="create-page-form" onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
            <div className="space-y-2">
              <Label htmlFor="title">Page Title</Label>
              <Input 
                id="title" 
                placeholder="e.g. Summer Campaign 2026" 
                {...form.register("title")} 
                onChange={(e) => {
                  form.setValue("title", e.target.value);
                  // Auto-generate slug if it's empty
                  if (!form.formState.dirtyFields.slug) {
                    form.setValue(
                      "slug", 
                      e.target.value.toLowerCase().replace(/[^a-z0-9]+/g, "-").replace(/(^-|-$)+/g, "")
                    );
                  }
                }}
              />
              {form.formState.errors.title && (
                <p className="text-xs text-destructive">{form.formState.errors.title.message}</p>
              )}
            </div>

            <div className="space-y-2">
              <Label htmlFor="slug">URL Slug</Label>
              <div className="flex items-center border rounded-md px-3 bg-muted/30 focus-within:ring-1 focus-within:ring-ring">
                <span className="text-muted-foreground text-sm border-r pr-3 mr-3 whitespace-nowrap">
                  yourdomain.com/
                </span>
                <Input 
                  id="slug" 
                  placeholder="summer-campaign" 
                  className="border-0 bg-transparent px-0 focus-visible:ring-0 focus-visible:ring-offset-0"
                  {...form.register("slug")} 
                />
              </div>
              {form.formState.errors.slug && (
                <p className="text-xs text-destructive">{form.formState.errors.slug.message}</p>
              )}
              <p className="text-[11px] text-muted-foreground">Only lowercase letters, numbers, and hyphens allowed.</p>
            </div>
          </form>
        </CardContent>
        <CardFooter className="flex justify-end gap-3 border-t p-6">
          <Link href="/dashboard/landing-pages" className={buttonVariants({ variant: "outline" })}>Cancel</Link>
          <Button type="submit" form="create-page-form" disabled={createMutation.isPending}>
            {createMutation.isPending ? "Creating..." : "Create & Start Building"}
          </Button>
        </CardFooter>
      </Card>
    </div>
  );
}
