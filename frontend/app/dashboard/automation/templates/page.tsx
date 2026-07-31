"use client";

import { useAutomationTemplates, useCloneTemplate } from "@/hooks/useAutomationStudio";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { LayoutTemplate, Copy } from "lucide-react";
import { Badge } from "@/components/ui/badge";

export default function TemplatesPage() {
  const { data: templates = [], isLoading } = useAutomationTemplates();
  const { mutate: clone, isPending } = useCloneTemplate();

  return (
    <div className="space-y-6 max-w-6xl mx-auto py-6">
      <div>
        <h1 className="text-2xl font-bold tracking-tight">Workflow Templates</h1>
        <p className="text-muted-foreground">Start quickly with pre-built automation workflows.</p>
      </div>

      {isLoading ? (
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
          {[1, 2, 3].map(i => <Card key={i} className="animate-pulse h-40 bg-muted/20" />)}
        </div>
      ) : (
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
          {templates.map(tpl => (
            <Card key={tpl.id} className="flex flex-col hover:border-primary/50 transition-colors">
              <CardHeader className="pb-3">
                <div className="flex justify-between items-start mb-2">
                  <Badge variant="secondary">{tpl.category}</Badge>
                  <LayoutTemplate className="h-4 w-4 text-muted-foreground" />
                </div>
                <CardTitle className="text-lg">{tpl.name}</CardTitle>
                <CardDescription className="text-xs mt-1 h-8 line-clamp-2">
                  {tpl.description}
                </CardDescription>
              </CardHeader>
              <CardContent className="mt-auto pt-4 border-t flex justify-end">
                <Button 
                  size="sm" 
                  variant="outline" 
                  className="gap-2" 
                  onClick={() => clone(tpl.id)}
                  disabled={isPending}
                >
                  <Copy className="h-4 w-4" />
                  Use Template
                </Button>
              </CardContent>
            </Card>
          ))}
        </div>
      )}
    </div>
  );
}
