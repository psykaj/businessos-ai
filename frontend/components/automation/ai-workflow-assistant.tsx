"use client";

import { useState } from "react";
import { Workflow } from "@/types/automation";
import { WorkflowPreview } from "./workflow-preview";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Textarea } from "@/components/ui/textarea";
import { Loader2, Wand2, ArrowLeft, Check } from "lucide-react";
import { toast } from "sonner";

interface AiWorkflowAssistantProps {
  onCancel: () => void;
  onComplete: () => void;
}

export function AiWorkflowAssistant({ onCancel, onComplete }: AiWorkflowAssistantProps) {
  const [prompt, setPrompt] = useState("");
  const [isGenerating, setIsGenerating] = useState(false);
  const [generatedWorkflow, setGeneratedWorkflow] = useState<Partial<Workflow> | null>(null);

  const handleGenerate = async () => {
    if (!prompt) return;
    
    setIsGenerating(true);
    
    // In a real implementation, we would call an AI endpoint here.
    // For this demonstration, we'll simulate an AI parsing the intent.
    setTimeout(() => {
      let mockTrigger = "InvoiceOverdue";
      let mockAction = "CreateTask";
      let requiresApproval = false;
      let name = "Custom AI Workflow";

      const lowerPrompt = prompt.toLowerCase();
      
      if (lowerPrompt.includes("lead") || lowerPrompt.includes("prospect")) {
        mockTrigger = "NewLeadCreated";
        name = "Lead Follow-up Automation";
      } else if (lowerPrompt.includes("inventory") || lowerPrompt.includes("stock")) {
        mockTrigger = "InventoryLow";
        name = "Inventory Replenishment";
      } else if (lowerPrompt.includes("customer") && lowerPrompt.includes("inactive")) {
        mockTrigger = "CustomerInactive";
        name = "Customer Reactivation";
      }

      if (lowerPrompt.includes("email") || lowerPrompt.includes("message")) {
        mockAction = "SendEmail";
        requiresApproval = true; // Safety UX rule: messages should require approval by default
      }

      setGeneratedWorkflow({
        name,
        triggerType: mockTrigger,
        requiresApproval,
        steps: [
          {
            id: "step-1",
            workflowId: "",
            stepOrder: 1,
            stepType: "Condition",
            name: "AI Evaluated Condition",
            isRequired: true,
            status: "Active",
            createdAt: new Date().toISOString(),
            updatedAt: new Date().toISOString()
          },
          ...(requiresApproval ? [{
            id: "step-appr",
            workflowId: "",
            stepOrder: 2,
            stepType: "Approval" as const,
            name: "Wait for manual approval",
            isRequired: true,
            status: "Active",
            createdAt: new Date().toISOString(),
            updatedAt: new Date().toISOString()
          }] : []),
          {
            id: "step-2",
            workflowId: "",
            stepOrder: requiresApproval ? 3 : 2,
            stepType: "Action",
            name: mockAction === "SendEmail" ? "Send Communication" : "Create Follow-up Task",
            configuration: JSON.stringify({ ActionType: mockAction }),
            isRequired: true,
            status: "Active",
            createdAt: new Date().toISOString(),
            updatedAt: new Date().toISOString()
          }
        ]
      });
      
      setIsGenerating(false);
      toast.success("AI generated a workflow proposal.");
    }, 1500);
  };

  return (
    <div className="flex flex-col lg:flex-row gap-8 max-w-7xl mx-auto py-6">
      <div className="flex-1 space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-bold tracking-tight">AI Assistant</h1>
            <p className="text-muted-foreground mt-1">
              Describe what you want to automate in plain English.
            </p>
          </div>
          <Button variant="ghost" onClick={onCancel}>
            <ArrowLeft className="h-4 w-4 mr-2" /> Back to manual builder
          </Button>
        </div>

        <Card>
          <CardContent className="p-6">
            {!generatedWorkflow ? (
              <div className="space-y-4">
                <Textarea 
                  placeholder="e.g. When an invoice is overdue by 7 days, remind the customer and create a follow-up task."
                  className="min-h-[150px] text-lg p-4 resize-none"
                  value={prompt}
                  onChange={(e) => setPrompt(e.target.value)}
                />
                <Button 
                  className="w-full gap-2 bg-indigo-600 hover:bg-indigo-700 text-white" 
                  size="lg"
                  onClick={handleGenerate}
                  disabled={!prompt || isGenerating}
                >
                  {isGenerating ? <Loader2 className="h-5 w-5 animate-spin" /> : <Wand2 className="h-5 w-5" />}
                  Generate Automation
                </Button>
                
                <div className="mt-8 pt-6 border-t">
                  <p className="text-sm font-medium text-muted-foreground mb-3">Try these examples:</p>
                  <div className="flex flex-wrap gap-2">
                    {["When inventory drops below 10, create a reorder task.", 
                      "If a new lead arrives from the website, assign a task to sales.", 
                      "When a customer is inactive for 30 days, send them a win-back email."].map((ex, i) => (
                      <Badge 
                        key={i} 
                        variant="secondary" 
                        className="cursor-pointer hover:bg-secondary/80 py-1.5 px-3 font-normal"
                        onClick={() => setPrompt(ex)}
                      >
                        {ex}
                      </Badge>
                    ))}
                  </div>
                </div>
              </div>
            ) : (
              <div className="space-y-6 animate-in fade-in">
                <div className="p-4 bg-emerald-50 dark:bg-emerald-900/20 border border-emerald-200 dark:border-emerald-800 rounded-lg flex items-start gap-3">
                  <Check className="h-5 w-5 text-emerald-600 dark:text-emerald-400 mt-0.5" />
                  <div>
                    <h3 className="font-semibold text-emerald-800 dark:text-emerald-300">Workflow Generated Successfully</h3>
                    <p className="text-sm text-emerald-700 dark:text-emerald-400 mt-1">
                      BusinessOS AI has translated your request into a ready-to-use automation. Review the preview on the right.
                    </p>
                  </div>
                </div>
                
                <div className="flex gap-4 pt-4 border-t">
                  <Button variant="outline" className="flex-1" onClick={() => setGeneratedWorkflow(null)}>
                    Try another prompt
                  </Button>
                  <Button className="flex-1" onClick={onComplete}>
                    Looks good, save and activate
                  </Button>
                </div>
              </div>
            )}
          </CardContent>
        </Card>
      </div>

      <div className="w-full lg:w-96">
        <div className="sticky top-6">
          <Card className="bg-slate-50/50 dark:bg-slate-900/20 border-dashed min-h-[400px]">
            <CardHeader className="pb-2">
              <CardTitle className="text-lg">AI Proposal</CardTitle>
              <CardDescription>Visual representation of your prompt</CardDescription>
            </CardHeader>
            <CardContent className="flex justify-center h-full">
              {generatedWorkflow ? (
                <WorkflowPreview workflow={generatedWorkflow} />
              ) : (
                <div className="flex flex-col items-center justify-center text-muted-foreground opacity-50 py-20 text-center">
                  <Wand2 className="h-12 w-12 mb-4" />
                  <p>Type a prompt and let AI build the workflow for you.</p>
                </div>
              )}
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  );
}
