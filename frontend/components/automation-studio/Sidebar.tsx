"use client";

import React from "react";
import { Zap, PlayCircle, Split } from "lucide-react";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Accordion, AccordionContent, AccordionItem, AccordionTrigger } from "@/components/ui/accordion";

export function Sidebar() {
  const onDragStart = (event: React.DragEvent, nodeType: string, label: string) => {
    event.dataTransfer.setData("application/reactflow", nodeType);
    event.dataTransfer.setData("application/reactflow-label", label);
    event.dataTransfer.effectAllowed = "move";
  };

  const categories = [
    {
      name: "CRM",
      items: [
        { type: "triggerNode", label: "Lead Created" },
        { type: "triggerNode", label: "Customer Created" },
        { type: "actionNode", label: "Create Task" },
        { type: "actionNode", label: "Assign Lead" },
      ],
    },
    {
      name: "Finance",
      items: [
        { type: "triggerNode", label: "Invoice Paid" },
        { type: "triggerNode", label: "Invoice Overdue" },
        { type: "actionNode", label: "Generate Invoice" },
      ],
    },
    {
      name: "Marketing",
      items: [
        { type: "triggerNode", label: "Form Submitted" },
        { type: "actionNode", label: "Send Email" },
        { type: "actionNode", label: "Add Loyalty Points" },
      ],
    },
    {
      name: "Logic",
      items: [
        { type: "conditionNode", label: "If/Else" },
        { type: "conditionNode", label: "Delay" },
      ],
    }
  ];

  return (
    <aside className="w-64 bg-card border-r flex flex-col h-full">
      <div className="p-4 border-b">
        <h3 className="font-semibold">Toolbox</h3>
        <p className="text-xs text-muted-foreground mt-1">Drag items to the canvas.</p>
      </div>
      
      <ScrollArea className="flex-1">
        <Accordion defaultValue={["Logic", "CRM", "Marketing"]} className="w-full">
          {categories.map((cat, idx) => (
            <AccordionItem value={cat.name} key={idx} className="border-b-0 px-2">
              <AccordionTrigger className="px-2 py-3 hover:no-underline hover:bg-muted/50 rounded-md text-sm">
                {cat.name}
              </AccordionTrigger>
              <AccordionContent className="pb-3 px-2 space-y-2">
                {cat.items.map((item, itemIdx) => (
                  <div
                    key={itemIdx}
                    className="flex items-center gap-3 p-2 rounded-md border bg-background hover:border-primary cursor-grab active:cursor-grabbing transition-colors"
                    draggable
                    onDragStart={(e) => onDragStart(e, item.type, item.label)}
                  >
                    {item.type === "triggerNode" && <Zap className="h-4 w-4 text-amber-500" />}
                    {item.type === "actionNode" && <PlayCircle className="h-4 w-4 text-blue-500" />}
                    {item.type === "conditionNode" && <Split className="h-4 w-4 text-indigo-500" />}
                    <span className="text-sm font-medium">{item.label}</span>
                  </div>
                ))}
              </AccordionContent>
            </AccordionItem>
          ))}
        </Accordion>
      </ScrollArea>
    </aside>
  );
}
