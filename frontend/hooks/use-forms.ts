import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import api from "@/lib/api-client";

export interface FormField {
  id: string;
  name: string;
  label: string;
  type: string;
  required: boolean;
  options: string;
  orderIndex: number;
}

export interface Form {
  id: string;
  name: string;
  description: string;
  status: string;
  fields: FormField[];
  createdAt: string;
}

export interface FormSubmission {
  id: string;
  formId: string;
  leadId: string;
  submittedAt: string;
  payload: string;
  source: string;
  browserInfo: string;
}

export function useForms() {
  return useQuery({
    queryKey: ["forms"],
    queryFn: async () => {
      const { data } = await api.get<Form[]>("/forms");
      return data;
    },
  });
}

export function useForm(id: string) {
  return useQuery({
    queryKey: ["forms", id],
    queryFn: async () => {
      const { data } = await api.get<Form>(`/forms/${id}`);
      return data;
    },
    enabled: !!id,
  });
}

export function useCreateForm() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (payload: { name: string; description: string; fields: Partial<FormField>[] }) => {
      const { data } = await api.post<Form>("/forms", payload);
      return data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["forms"] });
    },
  });
}

export function useUpdateForm() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, payload }: { id: string; payload: Record<string, unknown> }) => {
      const { data } = await api.put<Form>(`/forms/${id}`, payload);
      return data;
    },
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: ["forms"] });
      queryClient.invalidateQueries({ queryKey: ["forms", variables.id] });
    },
  });
}

export function usePublishForm() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      const { data } = await api.post<Form>(`/forms/${id}/publish`);
      return data;
    },
    onSuccess: (_, id) => {
      queryClient.invalidateQueries({ queryKey: ["forms"] });
      queryClient.invalidateQueries({ queryKey: ["forms", id] });
    },
  });
}

export function useFormSubmissions(formId?: string) {
  return useQuery({
    queryKey: formId ? ["form-submissions", formId] : ["all-form-submissions"],
    queryFn: async () => {
      const endpoint = formId ? `/forms/${formId}/submissions` : "/forms/submissions/all"; // Adjust based on backend API if exists
      const { data } = await api.get<FormSubmission[]>(endpoint);
      return data;
    },
  });
}

export function usePublicSubmitForm() {
  return useMutation({
    mutationFn: async ({ id, payload }: { id: string; payload: Record<string, unknown> }) => {
      // Assuming public endpoint is mapped under /public/forms in proxy/api setup
      const { data } = await api.post(`/public/forms/${id}/submit`, payload);
      return data;
    },
  });
}
