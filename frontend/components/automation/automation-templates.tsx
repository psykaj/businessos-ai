"use client";

import { useAiTemplates, useInstallAiTemplate } from "@/hooks/useAiAutomation";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle, CardDescription, CardFooter } from "@/components/ui/card";
import { Download, Loader2, Sparkles, LayoutTemplate } from "lucide-react";
import { WorkflowTemplate } from "@/types/automation";

export function AutomationTemplates() {
  const { data: templates = [], isLoading } = useAiTemplates();
  const { mutate: installTemplate, isPending } = useInstallAiTemplate();

  if (isLoading) {
    return (
      <div className="grid md:grid-cols-2 gap-4 animate-pulse">
        {[1,2,3,4].map(i => <Card key={i} className="h-48 bg-muted/20" />)}
      </div>
    );
  }

  if (templates.length === 0) return null;

  return (
    <div className="space-y-4">
      <div className="flex items-center gap-2">
        <LayoutTemplate className="h-5 w-5 text-indigo-500" />
        <h2 className="text-xl font-semibold">Recommended Templates</h2>
      </div>
      
      <div className="grid md:grid-cols-2 gap-4">
        {templates.map((template: WorkflowTemplate) => (
          <Card key={template.id} className="flex flex-col h-full border-indigo-100 dark:border-indigo-900/30">
            <CardHeader className="pb-2">
              <div className="flex justify-between items-start">
                <div>
                  <CardTitle className="text-base">{template.name}</CardTitle>
                  <CardDescription className="mt-1 line-clamp-2 min-h-[40px]">{template.description}</CardDescription>
                </div>
                <div className="p-2 bg-indigo-50 dark:bg-indigo-900/20 text-indigo-500 rounded-full">
                  <Sparkles className="h-4 w-4" />
                </div>
              </div>
            </CardHeader>
            <CardContent className="py-2 flex-grow">
              <div className="text-sm border-l-2 border-indigo-200 dark:border-indigo-800 pl-3 py-1">
                <span className="font-medium">Business Benefit: </span>
                <span className="text-muted-foreground text-xs">
                  {template.name.includes("Overdue") ? "Improve cash flow by automating collections." :
                   template.name.includes("Inactive") ? "Increase retention revenue automatically." :
                   template.name.includes("Stockout") ? "Prevent lost sales from out-of-stock items." :
                   "Increase conversion rate by contacting leads faster."}
                </span>
              </div>
            </CardContent>
            <CardFooter className="pt-2">
              <Button 
                variant="secondary" 
                className="w-full gap-2 bg-indigo-50 hover:bg-indigo-100 text-indigo-700 dark:bg-indigo-900/30 dark:hover:bg-indigo-900/50 dark:text-indigo-300"
                onClick={() => installTemplate(template.id)}
                disabled={isPending}
              >
                {isPending ? <Loader2 className="h-4 w-4 animate-spin" /> : <Download className="h-4 w-4" />}
                Install Template
              </Button>
            </CardFooter>
          </Card>
        ))}
      </div>
    </div>
  );
}
