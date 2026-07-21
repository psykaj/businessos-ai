"use client";

import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Switch } from "@/components/ui/switch";
import { Card, CardContent } from "@/components/ui/card";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { GripVertical, Trash2, Settings, PlusCircle, Check, Copy } from "lucide-react";
import { FormField } from "@/hooks/use-forms";
import { cn } from "@/lib/utils";

interface FormBuilderProps {
  initialName?: string;
  initialDescription?: string;
  initialFields?: FormField[];
  onSave: (data: { name: string; description: string; fields: Partial<FormField>[] }) => void;
  isSaving?: boolean;
}

const FIELD_TYPES = [
  { type: "text", label: "Short Text" },
  { type: "textarea", label: "Long Text" },
  { type: "email", label: "Email" },
  { type: "phone", label: "Phone Number" },
  { type: "number", label: "Number" },
  { type: "dropdown", label: "Dropdown" },
  { type: "radio", label: "Radio Buttons" },
  { type: "checkbox", label: "Checkboxes" },
  { type: "date", label: "Date Picker" },
];

export function FormBuilder({ initialName = "", initialDescription = "", initialFields = [], onSave, isSaving = false }: FormBuilderProps) {
  const [name, setName] = useState(initialName);
  const [description, setDescription] = useState(initialDescription);
  const [fields, setFields] = useState<Partial<FormField>[]>(
    initialFields.length > 0 ? initialFields : [{ id: crypto.randomUUID(), name: "firstName", label: "First Name", type: "text", required: true, orderIndex: 0 }]
  );
  const [activeFieldId, setActiveFieldId] = useState<string | null>(fields[0]?.id || null);

  const addField = (type: string) => {
    const newField: Partial<FormField> = {
      id: crypto.randomUUID(),
      name: `field_${Date.now()}`,
      label: "New Field",
      type,
      required: false,
      options: type === "dropdown" || type === "radio" || type === "checkbox" ? "Option 1,Option 2" : "",
      orderIndex: fields.length,
    };
    setFields([...fields, newField]);
    setActiveFieldId(newField.id!);
  };

  const updateField = (id: string, updates: Partial<FormField>) => {
    setFields(fields.map(f => f.id === id ? { ...f, ...updates } : f));
  };

  const removeField = (id: string) => {
    const newFields = fields.filter(f => f.id !== id);
    setFields(newFields);
    if (activeFieldId === id) setActiveFieldId(newFields[0]?.id || null);
  };

  const activeField = fields.find(f => f.id === activeFieldId);

  const handleSave = () => {
    onSave({ name, description, fields });
  };

  return (
    <div className="grid grid-cols-12 gap-6 h-[calc(100vh-10rem)]">
      {/* LEFT: Builder Canvas */}
      <div className="col-span-12 md:col-span-8 flex flex-col h-full bg-muted/30 rounded-xl border border-border/50 overflow-hidden">
        <div className="flex items-center justify-between p-4 border-b bg-background">
          <div className="flex-1 space-y-2">
            <Input 
              value={name} 
              onChange={e => setName(e.target.value)} 
              placeholder="Form Title" 
              className="text-lg font-bold border-transparent focus-visible:ring-0 px-0 shadow-none h-8"
            />
            <Input 
              value={description} 
              onChange={e => setDescription(e.target.value)} 
              placeholder="Form description or instructions" 
              className="text-sm text-muted-foreground border-transparent focus-visible:ring-0 px-0 shadow-none h-6"
            />
          </div>
          <Button onClick={handleSave} disabled={isSaving || !name}>
            {isSaving ? "Saving..." : "Save Form"}
          </Button>
        </div>
        
        <div className="flex-1 overflow-y-auto p-6 space-y-4">
          {fields.map((field, idx) => (
            <Card 
              key={field.id} 
              className={cn(
                "cursor-pointer transition-all border-l-4 group",
                activeFieldId === field.id ? "border-l-primary ring-1 ring-primary/20 shadow-md" : "border-l-transparent hover:border-border"
              )}
              onClick={() => setActiveFieldId(field.id!)}
            >
              <CardContent className="p-4 flex items-start gap-4">
                <div className="mt-2 text-muted-foreground/40 group-hover:text-muted-foreground cursor-grab">
                  <GripVertical className="h-5 w-5" />
                </div>
                <div className="flex-1 space-y-1">
                  <Label className="text-base font-semibold">
                    {field.label} {field.required && <span className="text-destructive">*</span>}
                  </Label>
                  {/* Mock rendering of field based on type */}
                  <div className="mt-2 text-sm text-muted-foreground italic border rounded-md p-2 bg-muted/20">
                    [{FIELD_TYPES.find(t => t.type === field.type)?.label}] Input Simulation
                  </div>
                </div>
                {activeFieldId === field.id && (
                  <Button variant="ghost" size="icon" className="text-destructive hover:text-destructive hover:bg-destructive/10" onClick={(e) => { e.stopPropagation(); removeField(field.id!); }}>
                    <Trash2 className="h-4 w-4" />
                  </Button>
                )}
              </CardContent>
            </Card>
          ))}
          
          <div className="py-8 text-center">
            <p className="text-sm text-muted-foreground mb-4">Add a new field from the sidebar.</p>
          </div>
        </div>
      </div>

      {/* RIGHT: Properties & Palette */}
      <div className="col-span-12 md:col-span-4 flex flex-col h-full bg-background rounded-xl border border-border/50 overflow-hidden shadow-sm">
        <Tabs defaultValue={activeField ? "properties" : "add"} className="w-full flex flex-col h-full">
          <TabsList className="w-full justify-start rounded-none border-b h-14 bg-transparent px-4">
            <TabsTrigger value="add" className="data-[state=active]:bg-transparent data-[state=active]:shadow-none data-[state=active]:border-b-2 data-[state=active]:border-primary rounded-none">
              Add Fields
            </TabsTrigger>
            <TabsTrigger value="properties" disabled={!activeField} className="data-[state=active]:bg-transparent data-[state=active]:shadow-none data-[state=active]:border-b-2 data-[state=active]:border-primary rounded-none">
              Field Properties
            </TabsTrigger>
          </TabsList>

          <TabsContent value="add" className="flex-1 overflow-y-auto p-4 m-0 space-y-2">
            <div className="grid grid-cols-2 gap-2">
              {FIELD_TYPES.map(ft => (
                <Button 
                  key={ft.type} 
                  variant="outline" 
                  className="justify-start h-auto py-3 px-3 hover:border-primary/50 hover:bg-primary/5"
                  onClick={() => addField(ft.type)}
                >
                  <PlusCircle className="h-4 w-4 mr-2 text-muted-foreground" />
                  <span className="text-xs font-medium">{ft.label}</span>
                </Button>
              ))}
            </div>
          </TabsContent>

          <TabsContent value="properties" className="flex-1 overflow-y-auto p-6 m-0">
            {activeField ? (
              <div className="space-y-6">
                <div className="space-y-2">
                  <Label>Field Label</Label>
                  <Input 
                    value={activeField.label} 
                    onChange={e => updateField(activeField.id!, { label: e.target.value })} 
                  />
                </div>
                
                <div className="space-y-2">
                  <Label>Internal Name (API Key)</Label>
                  <Input 
                    value={activeField.name} 
                    onChange={e => updateField(activeField.id!, { name: e.target.value })} 
                    className="font-mono text-xs"
                  />
                  <p className="text-[10px] text-muted-foreground">Used for API payload mapping (e.g. CRM mapping).</p>
                </div>

                <div className="flex items-center justify-between p-3 rounded-lg border bg-card">
                  <div className="space-y-0.5">
                    <Label className="text-sm font-semibold">Required</Label>
                    <p className="text-xs text-muted-foreground">Make this field mandatory.</p>
                  </div>
                  <Switch 
                    checked={activeField.required} 
                    onCheckedChange={c => updateField(activeField.id!, { required: c })} 
                  />
                </div>

                {["dropdown", "radio", "checkbox"].includes(activeField.type || "") && (
                  <div className="space-y-2">
                    <Label>Options (comma separated)</Label>
                    <Textarea 
                      value={activeField.options} 
                      onChange={e => updateField(activeField.id!, { options: e.target.value })} 
                      placeholder="Option 1, Option 2, Option 3"
                    />
                  </div>
                )}
              </div>
            ) : (
              <div className="flex items-center justify-center h-full text-muted-foreground text-sm">
                Select a field to edit its properties.
              </div>
            )}
          </TabsContent>
        </Tabs>
      </div>
    </div>
  );
}
