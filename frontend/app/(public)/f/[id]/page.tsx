"use client";

import { useState } from "react";
import { useParams } from "next/navigation";
import { useForm, usePublicSubmitForm } from "@/hooks/use-forms";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Checkbox } from "@/components/ui/checkbox";
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group";
import { Loader2, CheckCircle2 } from "lucide-react";

export default function PublicFormPage() {
  const { id } = useParams() as { id: string };
  const { data: form, isLoading } = useForm(id);
  const submitForm = usePublicSubmitForm();
  
  const [formData, setFormData] = useState<Record<string, unknown>>({});
  const [isSubmitted, setIsSubmitted] = useState(false);

  const handleChange = (name: string, value: unknown) => {
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    submitForm.mutate({ id, payload: formData }, {
      onSuccess: () => setIsSubmitted(true)
    });
  };

  if (isLoading) {
    return (
      <div className="flex h-screen w-full items-center justify-center bg-muted/20">
        <Loader2 className="h-8 w-8 animate-spin text-primary" />
      </div>
    );
  }

  if (!form || form.status !== "Published") {
    return (
      <div className="flex h-screen w-full items-center justify-center bg-muted/20">
        <div className="text-center space-y-2">
          <h1 className="text-2xl font-bold">Form Not Available</h1>
          <p className="text-muted-foreground">This form has been unpublished or does not exist.</p>
        </div>
      </div>
    );
  }

  if (isSubmitted) {
    return (
      <div className="flex min-h-screen w-full items-center justify-center bg-muted/20 p-4">
        <div className="w-full max-w-md bg-background rounded-2xl shadow-xl border p-8 text-center space-y-4">
          <div className="flex justify-center">
            <CheckCircle2 className="h-16 w-16 text-emerald-500" />
          </div>
          <h1 className="text-2xl font-bold">Thank You!</h1>
          <p className="text-muted-foreground">Your response has been successfully submitted.</p>
        </div>
      </div>
    );
  }

  return (
    <div className="flex min-h-screen w-full justify-center bg-muted/20 py-12 px-4">
      <div className="w-full max-w-2xl bg-background rounded-2xl shadow-xl border overflow-hidden flex flex-col">
        <div className="bg-primary/5 p-8 border-b">
          <h1 className="text-3xl font-bold">{form.name}</h1>
          {form.description && <p className="text-muted-foreground mt-2">{form.description}</p>}
        </div>
        
        <form onSubmit={handleSubmit} className="p-8 space-y-8">
          {form.fields.sort((a: any, b: any) => a.orderIndex - b.orderIndex).map((field: any) => (
            <div key={field.id} className="space-y-3">
              <Label className="text-base">
                {field.label} {field.required && <span className="text-destructive">*</span>}
              </Label>
              
              {(field.type === "text" || field.type === "email" || field.type === "phone" || field.type === "number") && (
                <Input
                  type={field.type === "phone" ? "tel" : field.type}
                  required={field.required}
                  value={(formData[field.name] as string) || ""}
                  onChange={e => handleChange(field.name, e.target.value)}
                  placeholder={`Enter your ${field.label.toLowerCase()}`}
                  className="h-12"
                />
              )}

              {field.type === "textarea" && (
                <Textarea
                  required={field.required}
                  value={(formData[field.name] as string) || ""}
                  onChange={e => handleChange(field.name, e.target.value)}
                  placeholder={`Enter your ${field.label.toLowerCase()}`}
                  className="min-h-[100px]"
                />
              )}

              {field.type === "dropdown" && field.options && (
                <Select
                  required={field.required}
                  value={formData[field.name] || ""}
                  onValueChange={v => handleChange(field.name, v)}
                >
                  <SelectTrigger className="h-12">
                    <SelectValue placeholder="Select an option" />
                  </SelectTrigger>
                  <SelectContent>
                    {field.options.split(",").map((opt: string) => opt.trim()).filter(Boolean).map((opt: string) => (
                      <SelectItem key={opt} value={opt}>{opt}</SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              )}

              {field.type === "radio" && field.options && (
                <RadioGroup 
                  required={field.required}
                  value={(formData[field.name] as string) || ""}
                  onValueChange={(v: string) => handleChange(field.name, v)}
                  className="space-y-2 mt-2"
                >
                  {field.options.split(",").map((opt: string) => opt.trim()).filter(Boolean).map((opt: string) => (
                    <div key={opt} className="flex items-center space-x-2">
                      <RadioGroupItem value={opt} id={`${field.name}-${opt}`} />
                      <Label htmlFor={`${field.name}-${opt}`} className="font-normal">{opt}</Label>
                    </div>
                  ))}
                </RadioGroup>
              )}
              
              {field.type === "checkbox" && field.options && (
                <div className="space-y-3 mt-2">
                  {field.options.split(",").map((opt: string) => opt.trim()).filter(Boolean).map((opt: string) => (
                    <div key={opt} className="flex items-center space-x-2">
                      <Checkbox 
                        id={`${field.name}-${opt}`}
                        checked={((formData[field.name] as string[]) || []).includes(opt)}
                        onCheckedChange={(checked) => {
                          const current = (formData[field.name] as string[]) || [];
                          const updated = checked 
                            ? [...current, opt]
                            : current.filter((x: unknown) => x !== opt);
                          handleChange(field.name, updated);
                        }}
                      />
                      <Label htmlFor={`${field.name}-${opt}`} className="font-normal">{opt}</Label>
                    </div>
                  ))}
                </div>
              )}
            </div>
          ))}

          <Button type="submit" size="lg" className="w-full h-14 text-lg mt-8" disabled={submitForm.isPending}>
            {submitForm.isPending ? "Submitting..." : "Submit Response"}
          </Button>
        </form>
      </div>
    </div>
  );
}
