"use client";

import React, { useState, useEffect } from "react";
import { X, Trash2, Tag, Save, Check } from "lucide-react";
import { Button } from "@/components/ui/button";

interface NodeConfigDrawerProps {
  node: any | null;
  onClose: () => void;
  onUpdateNode: (nodeId: string, updatedData: any) => void;
  onDeleteNode: (nodeId: string) => void;
}

const availableVariables = [
  "{{CustomerName}}",
  "{{CustomerEmail}}",
  "{{CustomerPhone}}",
  "{{CompanyName}}",
  "{{InvoiceNumber}}",
  "{{DealValue}}",
  "{{today.date}}",
  "{{AiOutput}}",
];

export function NodeConfigDrawer({ node, onClose, onUpdateNode, onDeleteNode }: NodeConfigDrawerProps) {
  const [config, setConfig] = useState<Record<string, string>>({});
  const [label, setLabel] = useState("");

  useEffect(() => {
    if (node) {
      setLabel(node.data.label || "");
      let parsed = {};
      try {
        parsed = typeof node.data.config === "string" ? JSON.parse(node.data.config) : node.data.config || {};
      } catch {
        parsed = {};
      }
      setConfig(parsed);
    }
  }, [node]);

  if (!node) return null;

  const handleSave = () => {
    onUpdateNode(node.id, {
      ...node.data,
      label,
      config: JSON.stringify(config),
      description: Object.entries(config)
        .map(([k, v]) => `${k}: ${v}`)
        .join(" | ")
        .slice(0, 80),
    });
    onClose();
  };

  const handleInsertVariable = (fieldKey: string, variable: string) => {
    setConfig((prev) => ({
      ...prev,
      [fieldKey]: (prev[fieldKey] || "") + " " + variable,
    }));
  };

  const renderFields = () => {
    const nodeType = node.data.triggerType || node.data.actionType || node.data.logicType;

    switch (nodeType) {
      case "SendEmail":
        return (
          <div className="flex flex-col gap-4">
            <div>
              <label className="text-xs font-semibold text-foreground">To Email</label>
              <input
                type="text"
                value={config.To || ""}
                onChange={(e) => setConfig({ ...config, To: e.target.value })}
                placeholder="e.g. {{CustomerEmail}}"
                className="mt-1 w-full rounded-lg border border-border bg-card px-3 py-2 text-sm text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20"
              />
              <div className="mt-1 flex flex-wrap gap-1">
                {availableVariables.map((v) => (
                  <button
                    key={v}
                    type="button"
                    onClick={() => handleInsertVariable("To", v)}
                    className="rounded bg-muted px-1.5 py-0.5 text-[10px] font-mono text-muted-foreground hover:bg-primary/10 hover:text-primary"
                  >
                    {v}
                  </button>
                ))}
              </div>
            </div>

            <div>
              <label className="text-xs font-semibold text-foreground">Subject</label>
              <input
                type="text"
                value={config.Subject || ""}
                onChange={(e) => setConfig({ ...config, Subject: e.target.value })}
                placeholder="e.g. Welcome {{CustomerName}}!"
                className="mt-1 w-full rounded-lg border border-border bg-card px-3 py-2 text-sm text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20"
              />
            </div>

            <div>
              <label className="text-xs font-semibold text-foreground">Body</label>
              <textarea
                rows={4}
                value={config.Body || ""}
                onChange={(e) => setConfig({ ...config, Body: e.target.value })}
                placeholder="Hello {{CustomerName}}, thanks for choosing {{CompanyName}}!"
                className="mt-1 w-full rounded-lg border border-border bg-card px-3 py-2 text-sm text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20"
              />
            </div>
          </div>
        );

      case "SendWhatsApp":
        return (
          <div className="flex flex-col gap-4">
            <div>
              <label className="text-xs font-semibold text-foreground">Phone Number</label>
              <input
                type="text"
                value={config.Phone || ""}
                onChange={(e) => setConfig({ ...config, Phone: e.target.value })}
                placeholder="e.g. {{CustomerPhone}}"
                className="mt-1 w-full rounded-lg border border-border bg-card px-3 py-2 text-sm text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20"
              />
            </div>
            <div>
              <label className="text-xs font-semibold text-foreground">WhatsApp Message</label>
              <textarea
                rows={4}
                value={config.Message || ""}
                onChange={(e) => setConfig({ ...config, Message: e.target.value })}
                placeholder="Hi {{CustomerName}}, your order is confirmed!"
                className="mt-1 w-full rounded-lg border border-border bg-card px-3 py-2 text-sm text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20"
              />
            </div>
          </div>
        );

      case "CallWebhook":
        return (
          <div className="flex flex-col gap-4">
            <div>
              <label className="text-xs font-semibold text-foreground">Webhook Endpoint URL</label>
              <input
                type="url"
                value={config.Url || ""}
                onChange={(e) => setConfig({ ...config, Url: e.target.value })}
                placeholder="https://api.myapp.com/webhook"
                className="mt-1 w-full rounded-lg border border-border bg-card px-3 py-2 text-sm text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20"
              />
            </div>
          </div>
        );

      case "IfElse":
        return (
          <div className="flex flex-col gap-4">
            <div>
              <label className="text-xs font-semibold text-foreground">Field Name</label>
              <input
                type="text"
                value={config.FieldName || ""}
                onChange={(e) => setConfig({ ...config, FieldName: e.target.value })}
                placeholder="e.g. DealValue"
                className="mt-1 w-full rounded-lg border border-border bg-card px-3 py-2 text-sm text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20"
              />
            </div>

            <div>
              <label className="text-xs font-semibold text-foreground">Operator</label>
              <select
                value={config.Operator || "Equals"}
                onChange={(e) => setConfig({ ...config, Operator: e.target.value })}
                className="mt-1 w-full rounded-lg border border-border bg-card px-3 py-2 text-sm text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20"
              >
                <option value="Equals">Equals</option>
                <option value="NotEquals">Not Equals</option>
                <option value="Contains">Contains</option>
                <option value="GreaterThan">Greater Than</option>
                <option value="LessThan">Less Than</option>
              </select>
            </div>

            <div>
              <label className="text-xs font-semibold text-foreground">Target Value</label>
              <input
                type="text"
                value={config.Value || ""}
                onChange={(e) => setConfig({ ...config, Value: e.target.value })}
                placeholder="e.g. 5000"
                className="mt-1 w-full rounded-lg border border-border bg-card px-3 py-2 text-sm text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20"
              />
            </div>
          </div>
        );

      default:
        return (
          <div className="flex flex-col gap-4">
            <div>
              <label className="text-xs font-semibold text-foreground">Configuration JSON / Parameters</label>
              <textarea
                rows={5}
                value={typeof config === "object" ? JSON.stringify(config, null, 2) : config}
                onChange={(e) => {
                  try {
                    setConfig(JSON.parse(e.target.value));
                  } catch {
                    setConfig({ raw: e.target.value });
                  }
                }}
                className="mt-1 w-full font-mono text-xs rounded-lg border border-border bg-card p-3 text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20"
              />
            </div>
          </div>
        );
    }
  };

  return (
    <div className="fixed inset-y-0 right-0 z-50 flex w-96 flex-col border-l border-border bg-background/95 backdrop-blur-xl shadow-2xl">
      {/* Header */}
      <div className="flex h-16 items-center justify-between border-b border-border/50 px-6">
        <div className="flex items-center gap-2">
          <Tag className="h-4 w-4 text-primary" />
          <h3 className="text-sm font-bold text-foreground">Configure Node</h3>
        </div>
        <button
          onClick={onClose}
          className="flex h-8 w-8 items-center justify-center rounded-lg text-muted-foreground hover:bg-muted hover:text-foreground"
        >
          <X className="h-4 w-4" />
        </button>
      </div>

      {/* Body */}
      <div className="flex-1 overflow-y-auto p-6 flex flex-col gap-6">
        <div>
          <label className="text-xs font-semibold text-foreground">Node Label</label>
          <input
            type="text"
            value={label}
            onChange={(e) => setLabel(e.target.value)}
            className="mt-1 w-full rounded-lg border border-border bg-card px-3 py-2 text-sm text-foreground focus:outline-none focus:ring-2 focus:ring-primary/20 font-semibold"
          />
        </div>

        {renderFields()}
      </div>

      {/* Footer */}
      <div className="flex items-center justify-between border-t border-border/50 p-4 bg-muted/20">
        <Button
          variant="ghost"
          size="sm"
          onClick={() => {
            onDeleteNode(node.id);
            onClose();
          }}
          className="text-destructive hover:bg-destructive/10 hover:text-destructive gap-1.5"
        >
          <Trash2 className="h-4 w-4" />
          <span>Delete Node</span>
        </Button>

        <Button size="sm" onClick={handleSave} className="gap-1.5">
          <Save className="h-4 w-4" />
          <span>Save Changes</span>
        </Button>
      </div>
    </div>
  );
}
