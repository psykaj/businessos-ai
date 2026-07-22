"use client";

import React, { useState } from "react";
import { Sparkles, X, Bot, ArrowRight, Check, RefreshCw } from "lucide-react";
import { Button } from "@/components/ui/button";

interface AiWorkflowAssistantProps {
  isOpen: boolean;
  onClose: () => void;
  onApplyGeneratedWorkflow: (name: string, nodes: any[], edges: any[]) => void;
}

const samplePrompts = [
  "When a new lead is created, assign to sales team and send a welcome WhatsApp message.",
  "If an invoice payment fails, send an urgent email to customer and notify finance team.",
  "When a dynamic QR code is scanned, call AI Assistant to score intent and update CRM stage.",
  "When a form is submitted, generate invoice draft and send notification.",
];

export function AiWorkflowAssistant({ isOpen, onClose, onApplyGeneratedWorkflow }: AiWorkflowAssistantProps) {
  const [prompt, setPrompt] = useState("");
  const [isGenerating, setIsGenerating] = useState(false);
  const [generatedResult, setGeneratedResult] = useState<any | null>(null);

  if (!isOpen) return null;

  const handleGenerate = () => {
    if (!prompt.trim()) return;
    setIsGenerating(true);

    setTimeout(() => {
      // AI workflow generation simulation
      const result = {
        name: "AI Generated: " + prompt.slice(0, 30) + "...",
        nodes: [
          {
            id: "node-ai-1",
            type: "trigger",
            position: { x: 250, y: 80 },
            data: {
              triggerType: "LeadCreated",
              label: "Lead Created",
              description: "Triggered on CRM lead creation",
            },
          },
          {
            id: "node-ai-2",
            type: "action",
            position: { x: 250, y: 240 },
            data: {
              actionType: "AssignSalesperson",
              label: "Assign Sales Agent",
              description: "Round-robin team assignment",
            },
          },
          {
            id: "node-ai-3",
            type: "action",
            position: { x: 250, y: 400 },
            data: {
              actionType: "SendWhatsApp",
              label: "Send Welcome WhatsApp",
              description: "Hi {{CustomerName}}, welcome to BusinessOS AI!",
            },
          },
        ],
        edges: [
          { id: "e1-2", source: "node-ai-1", target: "node-ai-2", animated: true },
          { id: "e2-3", source: "node-ai-2", target: "node-ai-3", animated: true },
        ],
      };

      setGeneratedResult(result);
      setIsGenerating(false);
    }, 1200);
  };

  const handleApply = () => {
    if (generatedResult) {
      onApplyGeneratedWorkflow(generatedResult.name, generatedResult.nodes, generatedResult.edges);
      onClose();
    }
  };

  return (
    <div className="fixed inset-y-0 right-0 z-50 flex w-[440px] flex-col border-l border-border bg-background/95 backdrop-blur-2xl shadow-2xl">
      <div className="flex h-16 items-center justify-between border-b border-border/50 px-6">
        <div className="flex items-center gap-2">
          <div className="flex h-8 w-8 items-center justify-center rounded-xl bg-gradient-to-br from-primary to-primary/80 text-primary-foreground shadow-md">
            <Bot className="h-4 w-4" />
          </div>
          <div>
            <h3 className="text-sm font-bold text-foreground">AI Workflow Assistant</h3>
            <p className="text-[10px] text-muted-foreground">Describe your business automation goal</p>
          </div>
        </div>
        <button
          onClick={onClose}
          className="flex h-8 w-8 items-center justify-center rounded-lg text-muted-foreground hover:bg-muted hover:text-foreground"
        >
          <X className="h-4 w-4" />
        </button>
      </div>

      <div className="flex-1 overflow-y-auto p-6 flex flex-col gap-6">
        <div className="flex flex-col gap-2">
          <label className="text-xs font-semibold text-foreground">Prompt</label>
          <textarea
            rows={4}
            value={prompt}
            onChange={(e) => setPrompt(e.target.value)}
            placeholder="e.g. When a new lead is created, assign to John and send welcome WhatsApp..."
            className="w-full rounded-xl border border-border bg-card p-3 text-sm text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20 shadow-sm"
          />
          <Button onClick={handleGenerate} disabled={isGenerating || !prompt.trim()} className="mt-1 gap-2 shadow-md">
            {isGenerating ? (
              <>
                <RefreshCw className="h-4 w-4 animate-spin" />
                <span>Designing Graph...</span>
              </>
            ) : (
              <>
                <Sparkles className="h-4 w-4" />
                <span>Generate Workflow Graph</span>
              </>
            )}
          </Button>
        </div>

        {/* Sample Prompts */}
        <div className="flex flex-col gap-2">
          <span className="text-xs font-semibold text-muted-foreground">Sample Prompts</span>
          <div className="flex flex-col gap-2">
            {samplePrompts.map((sp, idx) => (
              <button
                key={idx}
                type="button"
                onClick={() => setPrompt(sp)}
                className="text-left rounded-xl border border-border/50 bg-muted/40 p-3 text-xs text-foreground hover:border-primary/40 hover:bg-primary/5 transition-colors"
              >
                "{sp}"
              </button>
            ))}
          </div>
        </div>

        {/* Generated Result Preview */}
        {generatedResult && (
          <div className="rounded-2xl border border-primary/30 bg-primary/5 p-4 flex flex-col gap-3">
            <div className="flex items-center justify-between">
              <span className="text-xs font-bold text-primary flex items-center gap-1.5">
                <Check className="h-4 w-4" />
                Suggested Workflow Ready
              </span>
              <span className="text-[10px] font-mono text-muted-foreground">{generatedResult.nodes.length} Nodes</span>
            </div>
            <p className="text-xs font-semibold text-foreground">{generatedResult.name}</p>
            <div className="flex flex-col gap-1.5">
              {generatedResult.nodes.map((n: any, idx: number) => (
                <div key={n.id} className="flex items-center gap-2 text-xs text-muted-foreground bg-card/80 p-2 rounded-lg border border-border/40">
                  <span className="font-bold text-primary">{idx + 1}.</span>
                  <span className="font-medium text-foreground">{n.data.label}</span>
                </div>
              ))}
            </div>
            <Button size="sm" onClick={handleApply} className="mt-2 gap-1.5 shadow-md">
              <span>Apply to Builder Canvas</span>
              <ArrowRight className="h-4 w-4" />
            </Button>
          </div>
        )}
      </div>
    </div>
  );
}
