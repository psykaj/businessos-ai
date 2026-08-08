"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { Workflow, WorkflowStep } from "@/types/automation";
import { useCreateAiWorkflow } from "@/hooks/useAiAutomation";
import { WorkflowPreview } from "./workflow-preview";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group";
import { Switch } from "@/components/ui/switch";
import { Loader2, ArrowRight, ArrowLeft, Wand2, Sparkles, Zap, AlertTriangle } from "lucide-react";
import { toast } from "sonner";
import { AiWorkflowAssistant } from "./ai-workflow-assistant";

const triggers = [
  { id: "InvoiceOverdue", label: "Invoice becomes overdue", icon: "📄" },
  { id: "NewCustomer", label: "New customer is created", icon: "👤" },
  { id: "CustomerInactive", label: "Customer becomes inactive", icon: "💤" },
  { id: "InventoryLow", label: "Inventory is low", icon: "📦" },
  { id: "NewLeadCreated", label: "New lead arrives", icon: "🎯" },
];

const actions = [
  { id: "CreateTask", label: "Create follow-up task", icon: "✅" },
  { id: "SendEmail", label: "Send email", icon: "📧" },
  { id: "CreateCRMActivity", label: "Create CRM activity", icon: "🤝" },
  { id: "CreateNotification", label: "Create notification", icon: "🔔" },
];

export function WorkflowWizard() {
  const router = useRouter();
  const { mutateAsync: createWorkflow, isPending } = useCreateAiWorkflow();
  const [step, setStep] = useState(1);
  const [useAssistant, setUseAssistant] = useState(false);
  
  const [workflowName, setWorkflowName] = useState("");
  const [trigger, setTrigger] = useState("");
  const [action, setAction] = useState("");
  const [hasCondition, setHasCondition] = useState(false);
  const [conditionField, setConditionField] = useState("");
  const [conditionOperator, setConditionOperator] = useState(">");
  const [conditionValue, setConditionValue] = useState("");
  const [requiresApproval, setRequiresApproval] = useState(false);

  // Generate the preview workflow object based on current state
  const buildPreviewWorkflow = (): Partial<Workflow> => {
    const steps: WorkflowStep[] = [];
    let order = 1;

    if (hasCondition && conditionField && conditionValue) {
      steps.push({
        id: "cond-1",
        workflowId: "",
        stepOrder: order++,
        stepType: "Condition",
        name: `Check if ${conditionField} ${conditionOperator} ${conditionValue}`,
        isRequired: true,
        status: "Active",
        createdAt: new Date().toISOString(),
        updatedAt: new Date().toISOString(),
      });
    }

    if (requiresApproval) {
      steps.push({
        id: "appr-1",
        workflowId: "",
        stepOrder: order++,
        stepType: "Approval",
        name: "Wait for manual approval",
        isRequired: true,
        status: "Active",
        createdAt: new Date().toISOString(),
        updatedAt: new Date().toISOString(),
      });
    }

    if (action) {
      const act = actions.find(a => a.id === action);
      steps.push({
        id: "act-1",
        workflowId: "",
        stepOrder: order++,
        stepType: "Action",
        name: act?.label || action,
        configuration: JSON.stringify({ ActionType: action }),
        isRequired: true,
        status: "Active",
        createdAt: new Date().toISOString(),
        updatedAt: new Date().toISOString(),
      });
    }

    return {
      name: workflowName || "New Automation",
      triggerType: trigger,
      requiresApproval,
      steps
    };
  };

  const handleSave = async () => {
    if (!workflowName || !trigger || !action) {
      toast.error("Please provide a name, trigger, and action.");
      return;
    }

    const wf = buildPreviewWorkflow();
    const payload = {
      name: wf.name,
      triggerType: wf.triggerType,
      isActive: true, // Automatically activate on save as it's targeted for non-technical users
      requiresApproval: wf.requiresApproval,
      category: "General",
      steps: wf.steps?.map(s => ({
        stepOrder: s.stepOrder,
        stepType: s.stepType,
        name: s.name,
        configuration: s.configuration,
        condition: s.stepType === "Condition" ? JSON.stringify({
          logic: "AND",
          rules: [{ field: conditionField, operator: conditionOperator, value: conditionValue }]
        }) : undefined,
        isRequired: true
      })) as WorkflowStep[]
    };

    try {
      await createWorkflow(payload);
      router.push("/automation");
    } catch (e) {
      // Error is handled by hook
    }
  };

  if (useAssistant) {
    return <AiWorkflowAssistant onCancel={() => setUseAssistant(false)} onComplete={handleSave} />;
  }

  return (
    <div className="flex flex-col lg:flex-row gap-8 max-w-7xl mx-auto py-6">
      <div className="flex-1 space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-bold tracking-tight">Create Automation</h1>
            <p className="text-muted-foreground mt-1">
              Step {step} of 4
            </p>
          </div>
          <Button variant="outline" className="gap-2 text-indigo-600 border-indigo-200 hover:bg-indigo-50" onClick={() => setUseAssistant(true)}>
            <Sparkles className="h-4 w-4" />
            Use AI Assistant
          </Button>
        </div>

        <Card>
          <CardContent className="p-6">
            {step === 1 && (
              <div className="space-y-4 animate-in fade-in slide-in-from-right-4">
                <h2 className="text-xl font-semibold mb-4">When should this automation run?</h2>
                <div className="space-y-4">
                  <div>
                    <Label>Automation Name</Label>
                    <Input 
                      placeholder="e.g. Follow up on overdue invoices" 
                      value={workflowName}
                      onChange={(e) => setWorkflowName(e.target.value)}
                      className="mt-1 max-w-md"
                    />
                  </div>
                  <div>
                    <Label className="mb-3 block text-muted-foreground">Select a trigger</Label>
                    <RadioGroup value={trigger} onValueChange={setTrigger} className="grid sm:grid-cols-2 gap-4">
                      {triggers.map(t => (
                        <div key={t.id} className="relative">
                          <RadioGroupItem value={t.id} id={t.id} className="peer sr-only" />
                          <Label
                            htmlFor={t.id}
                            className="flex flex-col items-center justify-between rounded-md border-2 border-muted bg-popover p-4 hover:bg-accent hover:text-accent-foreground peer-data-[state=checked]:border-primary [&:has([data-state=checked])]:border-primary cursor-pointer text-center"
                          >
                            <span className="text-2xl mb-2">{t.icon}</span>
                            <span>{t.label}</span>
                          </Label>
                        </div>
                      ))}
                    </RadioGroup>
                  </div>
                </div>
              </div>
            )}

            {step === 2 && (
              <div className="space-y-4 animate-in fade-in slide-in-from-right-4">
                <h2 className="text-xl font-semibold mb-4">What should BusinessOS AI do?</h2>
                <RadioGroup value={action} onValueChange={setAction} className="grid sm:grid-cols-2 gap-4">
                  {actions.map(a => (
                    <div key={a.id} className="relative">
                      <RadioGroupItem value={a.id} id={a.id} className="peer sr-only" />
                      <Label
                        htmlFor={a.id}
                        className="flex flex-col items-center justify-between rounded-md border-2 border-muted bg-popover p-4 hover:bg-accent hover:text-accent-foreground peer-data-[state=checked]:border-primary [&:has([data-state=checked])]:border-primary cursor-pointer text-center"
                      >
                        <span className="text-2xl mb-2">{a.icon}</span>
                        <span>{a.label}</span>
                      </Label>
                    </div>
                  ))}
                </RadioGroup>
              </div>
            )}

            {step === 3 && (
              <div className="space-y-4 animate-in fade-in slide-in-from-right-4">
                <h2 className="text-xl font-semibold mb-4">Should there be a condition?</h2>
                <div className="flex items-center space-x-2 border p-4 rounded-lg bg-slate-50 dark:bg-slate-900/50">
                  <Switch id="condition" checked={hasCondition} onCheckedChange={setHasCondition} />
                  <Label htmlFor="condition">Only run if specific conditions are met</Label>
                </div>

                {hasCondition && (
                  <div className="grid gap-4 sm:grid-cols-3 mt-4 animate-in fade-in">
                    <div>
                      <Label>Field</Label>
                      <Input placeholder="e.g. Amount" value={conditionField} onChange={e => setConditionField(e.target.value)} />
                    </div>
                    <div>
                      <Label>Condition</Label>
                      <select 
                        className="flex h-10 w-full items-center justify-between rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2"
                        value={conditionOperator}
                        onChange={e => setConditionOperator(e.target.value)}
                      >
                        <option value=">">is greater than</option>
                        <option value="<">is less than</option>
                        <option value="==">is exactly</option>
                        <option value="!=">is not</option>
                      </select>
                    </div>
                    <div>
                      <Label>Value</Label>
                      <Input placeholder="e.g. 1000" value={conditionValue} onChange={e => setConditionValue(e.target.value)} />
                    </div>
                  </div>
                )}
              </div>
            )}

            {step === 4 && (
              <div className="space-y-4 animate-in fade-in slide-in-from-right-4">
                <h2 className="text-xl font-semibold mb-4">How should BusinessOS AI handle it?</h2>
                
                <RadioGroup 
                  value={requiresApproval ? "approval" : "automatic"} 
                  onValueChange={(val) => setRequiresApproval(val === "approval")}
                  className="space-y-3"
                >
                  <div className="relative">
                    <RadioGroupItem value="automatic" id="automatic" className="peer sr-only" />
                    <Label
                      htmlFor="automatic"
                      className="flex items-center p-4 rounded-lg border-2 border-muted bg-popover hover:bg-accent cursor-pointer peer-data-[state=checked]:border-primary"
                    >
                      <div className="flex-1">
                        <p className="font-semibold text-base">Automatic</p>
                        <p className="text-sm text-muted-foreground mt-1">Run immediately without asking.</p>
                      </div>
                      <Zap className="h-5 w-5 text-muted-foreground peer-data-[state=checked]:text-primary" />
                    </Label>
                  </div>
                  
                  <div className="relative">
                    <RadioGroupItem value="approval" id="approval" className="peer sr-only" />
                    <Label
                      htmlFor="approval"
                      className="flex items-center p-4 rounded-lg border-2 border-muted bg-popover hover:bg-accent cursor-pointer peer-data-[state=checked]:border-primary"
                    >
                      <div className="flex-1">
                        <p className="font-semibold text-base">Ask me for approval</p>
                        <p className="text-sm text-muted-foreground mt-1">Create an action item for me to review before executing.</p>
                        <p className="text-xs text-rose-500 mt-2 font-medium flex items-center gap-1">
                          <AlertTriangle className="h-3 w-3" />
                          Recommended for actions that communicate with customers.
                        </p>
                      </div>
                      <AlertTriangle className="h-5 w-5 text-muted-foreground peer-data-[state=checked]:text-primary" />
                    </Label>
                  </div>
                </RadioGroup>
              </div>
            )}
          </CardContent>
          
          <div className="border-t p-4 bg-muted/20 flex justify-between rounded-b-xl">
            <Button variant="outline" onClick={() => setStep(s => Math.max(1, s - 1))} disabled={step === 1 || isPending}>
              <ArrowLeft className="h-4 w-4 mr-2" /> Back
            </Button>
            
            {step < 4 ? (
              <Button onClick={() => setStep(s => s + 1)}>
                Next <ArrowRight className="h-4 w-4 ml-2" />
              </Button>
            ) : (
              <Button onClick={handleSave} disabled={isPending}>
                {isPending && <Loader2 className="h-4 w-4 mr-2 animate-spin" />}
                Activate Automation
              </Button>
            )}
          </div>
        </Card>
      </div>

      <div className="w-full lg:w-96">
        <div className="sticky top-6">
          <Card className="bg-slate-50/50 dark:bg-slate-900/20 border-dashed">
            <CardHeader className="pb-2">
              <CardTitle className="text-lg">Live Preview</CardTitle>
              <CardDescription>How your automation will execute</CardDescription>
            </CardHeader>
            <CardContent>
              <WorkflowPreview workflow={buildPreviewWorkflow()} />
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  );
}
