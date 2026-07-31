"use client";

import React from "react";
import { X } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";

export function NodeConfigPanel({ node, onClose, onUpdate }: { node: Record<string, any>, onClose: () => void, onUpdate: (id: string, data: Record<string, any>) => void }) {
  if (!node) return null;

  const [formData, setFormData] = React.useState(node.data || {});

  React.useEffect(() => {
    setFormData(node.data || {});
  }, [node]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const newData = { ...formData, [e.target.name]: e.target.value };
    setFormData(newData);
    onUpdate(node.id, newData);
  };

  return (
    <aside className="w-80 bg-card border-l flex flex-col h-full absolute right-0 top-0 bottom-0 shadow-xl z-50">
      <div className="p-4 border-b flex justify-between items-center bg-muted/30">
        <div>
          <h3 className="font-semibold text-sm">Configure Node</h3>
          <p className="text-xs text-muted-foreground">{node.data?.label || "Settings"}</p>
        </div>
        <Button variant="ghost" size="icon" onClick={onClose}>
          <X className="h-4 w-4" />
        </Button>
      </div>

      <div className="p-4 space-y-4 flex-1 overflow-y-auto">
        <div className="space-y-2">
          <Label>Node Label</Label>
          <Input 
            name="label" 
            value={formData.label || ""} 
            onChange={handleChange}
            placeholder="E.g. Send Welcome Email"
          />
        </div>
        <div className="space-y-2">
          <Label>Description</Label>
          <Textarea 
            name="description" 
            value={formData.description || ""} 
            onChange={handleChange}
            placeholder="Brief description of this step..."
            rows={2}
          />
        </div>

        {node.type === "actionNode" && (
          <div className="space-y-2 pt-4 border-t">
            <Label>Action Payload (JSON)</Label>
            <Textarea 
              name="payload" 
              value={formData.payload || "{}"} 
              onChange={handleChange}
              placeholder='{"key": "value"}'
              rows={4}
              className="font-mono text-xs"
            />
          </div>
        )}

        {node.type === "conditionNode" && (
          <div className="space-y-2 pt-4 border-t">
            <Label>Condition Expression</Label>
            <Input 
              name="expression" 
              value={formData.expression || ""} 
              onChange={handleChange}
              placeholder="e.g. context.total > 1000"
            />
          </div>
        )}
      </div>
      
      <div className="p-4 border-t bg-muted/20">
        <Button className="w-full" onClick={onClose}>Done</Button>
      </div>
    </aside>
  );
}
