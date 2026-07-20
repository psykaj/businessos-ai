import apiClient from './api-client';
import {
  Lead,
  Contact,
  Company,
  Deal,
  CrmActivity,
  CrmTask,
  Note,
  CrmTag,
  CrmOverview,
  SalesPerformance,
  GlobalSearchResult,
  PipelineStage
} from '@/types/crm';

export const crmService = {
  // Leads
  getLeads: async () => {
    const res = await apiClient.get<Lead[]>('/api/crm/leads');
    return res.data;
  },
  getLead: async (id: string) => {
    const res = await apiClient.get<Lead>(`/api/crm/leads/${id}`);
    return res.data;
  },
  createLead: async (data: Partial<Lead>) => {
    const res = await apiClient.post<Lead>('/api/crm/leads', data);
    return res.data;
  },
  updateLead: async (id: string, data: Partial<Lead>) => {
    const res = await apiClient.put<Lead>(`/api/crm/leads/${id}`, data);
    return res.data;
  },
  deleteLead: async (id: string) => {
    await apiClient.delete(`/api/crm/leads/${id}`);
  },

  // Contacts
  getContacts: async () => {
    const res = await apiClient.get<Contact[]>('/api/crm/contacts');
    return res.data;
  },
  getContact: async (id: string) => {
    const res = await apiClient.get<Contact>(`/api/crm/contacts/${id}`);
    return res.data;
  },
  createContact: async (data: Partial<Contact>) => {
    const res = await apiClient.post<Contact>('/api/crm/contacts', data);
    return res.data;
  },
  updateContact: async (id: string, data: Partial<Contact>) => {
    const res = await apiClient.put<Contact>(`/api/crm/contacts/${id}`, data);
    return res.data;
  },
  deleteContact: async (id: string) => {
    await apiClient.delete(`/api/crm/contacts/${id}`);
  },

  // Companies
  getCompanies: async () => {
    const res = await apiClient.get<Company[]>('/api/crm/companies');
    return res.data;
  },
  getCompany: async (id: string) => {
    const res = await apiClient.get<Company>(`/api/crm/companies/${id}`);
    return res.data;
  },
  createCompany: async (data: Partial<Company>) => {
    const res = await apiClient.post<Company>('/api/crm/companies', data);
    return res.data;
  },
  updateCompany: async (id: string, data: Partial<Company>) => {
    const res = await apiClient.put<Company>(`/api/crm/companies/${id}`, data);
    return res.data;
  },
  deleteCompany: async (id: string) => {
    await apiClient.delete(`/api/crm/companies/${id}`);
  },

  // Deals
  getDeals: async () => {
    const res = await apiClient.get<Deal[]>('/api/crm/deals');
    return res.data;
  },
  getDeal: async (id: string) => {
    const res = await apiClient.get<Deal>(`/api/crm/deals/${id}`);
    return res.data;
  },
  createDeal: async (data: Partial<Deal>) => {
    const res = await apiClient.post<Deal>('/api/crm/deals', data);
    return res.data;
  },
  updateDeal: async (id: string, data: Partial<Deal>) => {
    const res = await apiClient.put<Deal>(`/api/crm/deals/${id}`, data);
    return res.data;
  },
  updateDealStage: async (id: string, pipelineStage: PipelineStage) => {
    const res = await apiClient.patch<Deal>(`/api/crm/deals/${id}/stage`, { pipelineStage });
    return res.data;
  },
  deleteDeal: async (id: string) => {
    await apiClient.delete(`/api/crm/deals/${id}`);
  },

  // Tasks
  getTasks: async () => {
    const res = await apiClient.get<CrmTask[]>('/api/crm/tasks');
    return res.data;
  },
  createTask: async (data: Partial<CrmTask>) => {
    const res = await apiClient.post<CrmTask>('/api/crm/tasks', data);
    return res.data;
  },
  updateTask: async (id: string, data: Partial<CrmTask>) => {
    const res = await apiClient.put<CrmTask>(`/api/crm/tasks/${id}`, data);
    return res.data;
  },
  deleteTask: async (id: string) => {
    await apiClient.delete(`/api/crm/tasks/${id}`);
  },

  // Activities
  getActivities: async () => {
    const res = await apiClient.get<CrmActivity[]>('/api/crm/activities');
    return res.data;
  },
  createActivity: async (data: Partial<CrmActivity>) => {
    const res = await apiClient.post<CrmActivity>('/api/crm/activities', data);
    return res.data;
  },

  // Tags
  getTags: async () => {
    const res = await apiClient.get<CrmTag[]>('/api/crm/tags');
    return res.data;
  },
  createTag: async (data: Partial<CrmTag>) => {
    const res = await apiClient.post<CrmTag>('/api/crm/tags', data);
    return res.data;
  },

  // Reporting & Search
  getOverview: async () => {
    const res = await apiClient.get<CrmOverview>('/api/crm/reporting/overview');
    return res.data;
  },
  getSalesPerformance: async (startDate?: string, endDate?: string) => {
    let url = '/api/crm/reporting/performance';
    if (startDate || endDate) {
      const params = new URLSearchParams();
      if (startDate) params.append('startDate', startDate);
      if (endDate) params.append('endDate', endDate);
      url += `?${params.toString()}`;
    }
    const res = await apiClient.get<SalesPerformance[]>(url);
    return res.data;
  },
  search: async (query: string) => {
    const res = await apiClient.get<GlobalSearchResult[]>(`/api/crm/search?query=${encodeURIComponent(query)}`);
    return res.data;
  }
};
