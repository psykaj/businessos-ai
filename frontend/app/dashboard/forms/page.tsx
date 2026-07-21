"use client";

import { useForms } from "@/hooks/use-forms";
import { Button } from "@/components/ui/button";
import { Plus, FileText, BarChart3, Activity } from "lucide-react";
import Link from "next/link";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";

export default function FormsPage() {
  const { data: forms, isLoading } = useForms();

  if (isLoading) {
    return <div className="p-8">Loading forms...</div>;
  }

  return (
    <div className="flex flex-col gap-8 p-8">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Forms & Lead Capture</h1>
          <p className="text-muted-foreground mt-1">Manage your dynamic forms and capture new leads.</p>
        </div>
        <Link href="/dashboard/forms/create">
          <Button>
            <Plus className="mr-2 h-4 w-4" />
            Create Form
          </Button>
        </Link>
      </div>

      {forms && forms.length === 0 ? (
        <Card className="flex flex-col items-center justify-center p-12 text-center border-dashed">
          <div className="rounded-full bg-primary/10 p-4 mb-4">
            <FileText className="h-8 w-8 text-primary" />
          </div>
          <h3 className="text-xl font-bold">No forms created yet</h3>
          <p className="text-muted-foreground mt-2 mb-6 max-w-sm">
            Create your first form to start capturing leads from your website or social media.
          </p>
          <Link href="/dashboard/forms/create">
            <Button>
              <Plus className="mr-2 h-4 w-4" />
              Create Your First Form
            </Button>
          </Link>
        </Card>
      ) : (
        <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
          {forms?.map((form: any) => (
            <Card key={form.id} className="overflow-hidden group flex flex-col transition-all hover:shadow-md border-border/50">
              <CardHeader className="pb-3 border-b border-border/10 bg-muted/20">
                <div className="flex items-start justify-between">
                  <div className="space-y-1">
                    <CardTitle className="text-lg line-clamp-1 group-hover:text-primary transition-colors">
                      {form.name}
                    </CardTitle>
                    <CardDescription className="line-clamp-1">{form.description}</CardDescription>
                  </div>
                  <Badge variant={form.status === "Published" ? "default" : "secondary"}>
                    {form.status}
                  </Badge>
                </div>
              </CardHeader>
              <CardContent className="p-4 flex-1 flex flex-col justify-end">
                <div className="flex items-center gap-4 text-sm text-muted-foreground mt-4 mb-6">
                  <div className="flex items-center gap-1.5">
                    <BarChart3 className="h-4 w-4 text-primary/70" />
                    <span className="font-medium text-foreground">{(form as any).views || 0}</span> views
                  </div>
                  <div className="flex items-center gap-1.5">
                    <Activity className="h-4 w-4 text-emerald-500" />
                    <span className="font-medium text-foreground">{(form as any).submissions || 0}</span> leads
                  </div>
                </div>
                <div className="flex items-center gap-2 mt-auto pt-4 border-t border-border/50">
                  <Link href={`/dashboard/forms/edit/${form.id}`} className="flex-1">
                    <Button variant="outline" className="w-full text-xs h-8">Edit Form</Button>
                  </Link>
                  <Link href={`/dashboard/forms/submissions?formId=${form.id}`} className="flex-1">
                    <Button variant="secondary" className="w-full text-xs h-8">View Leads</Button>
                  </Link>
                </div>
              </CardContent>
            </Card>
          ))}
        </div>
      )}
    </div>
  );
}
