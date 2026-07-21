"use client";

import { FormBuilder } from "@/components/forms/builder/FormBuilder";
import { useForm, useUpdateForm, FormField } from "@/hooks/use-forms";
import { useRouter, useParams } from "next/navigation";
import { toast } from "sonner";
import { Loader2 } from "lucide-react";

export default function EditFormPage() {
  const router = useRouter();
  const { id } = useParams() as { id: string };
  const { data: form, isLoading } = useForm(id);
  const updateForm = useUpdateForm();

  const handleSave = (data: { name: string; description: string; fields: Partial<FormField>[] }) => {
    updateForm.mutate({ id, payload: data }, {
      onSuccess: () => {
        toast.success("Form updated successfully!");
        router.push("/dashboard/forms");
      },
      onError: (err: unknown) => {
        toast.error((err as Error)?.message || "Failed to update form.");
      }
    });
  };

  if (isLoading) {
    return (
      <div className="flex h-full items-center justify-center">
        <Loader2 className="h-8 w-8 animate-spin text-primary" />
      </div>
    );
  }

  if (!form) {
    return <div className="p-8">Form not found.</div>;
  }

  return (
    <div className="flex flex-col gap-6 p-8 h-full">
      <div>
        <h1 className="text-3xl font-bold tracking-tight">Edit Form</h1>
        <p className="text-muted-foreground mt-1">Update your form fields and rules.</p>
      </div>

      <FormBuilder 
        initialName={form.name}
        initialDescription={form.description}
        initialFields={form.fields}
        onSave={handleSave} 
        isSaving={updateForm.isPending} 
      />
    </div>
  );
}
