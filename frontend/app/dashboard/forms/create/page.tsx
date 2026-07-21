"use client";

import { FormBuilder } from "@/components/forms/builder/FormBuilder";
import { useCreateForm, FormField } from "@/hooks/use-forms";
import { useRouter } from "next/navigation";
import { toast } from "sonner";

export default function CreateFormPage() {
  const router = useRouter();
  const createForm = useCreateForm();

  const handleSave = (data: { name: string; description: string; fields: Partial<FormField>[] }) => {
    createForm.mutate(data, {
      onSuccess: () => {
        toast.success("Form created successfully!");
        router.push("/dashboard/forms");
      },
      onError: (err: unknown) => {
        toast.error((err as Error)?.message || "Failed to create form.");
      }
    });
  };

  return (
    <div className="flex flex-col gap-6 p-8 h-full">
      <div>
        <h1 className="text-3xl font-bold tracking-tight">Create New Form</h1>
        <p className="text-muted-foreground mt-1">Design your form fields and rules.</p>
      </div>

      <FormBuilder 
        onSave={handleSave} 
        isSaving={createForm.isPending} 
      />
    </div>
  );
}
