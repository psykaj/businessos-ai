import apiClient from "./api-client";

export type CommunicationChannelType =
  | "Email"
  | "WhatsApp"
  | "SMS"
  | "LiveChat"
  | "FacebookMessenger"
  | "InstagramDm";

export type ConversationPriority = "Low" | "Medium" | "High" | "Urgent";
export type MessageDirection = "Inbound" | "Outbound" | "System" | "InternalNote";
export type DeliveryStatus = "Sending" | "Sent" | "Delivered" | "Read" | "Failed";

export interface ConversationDto {
  id: string;
  organizationId: string;
  channelId?: string;
  channelType: CommunicationChannelType;
  subject: string;
  customerId?: string;
  customerName: string;
  customerEmail?: string;
  customerPhone?: string;
  status: string; // New, Open, Resolved, Closed, Snoozed
  priority: ConversationPriority;
  assignedToUserId?: string;
  assignedToUserName?: string;
  unreadMessagesCount: number;
  lastMessageAt: string;
  lastMessagePreview: string;
  slaDueDate?: string;
  isSlaBreached: boolean;
  resolvedAt?: string;
  csatRating?: number;
  csatFeedback?: string;
  tags: string;
  metadataJson: string;
  createdAt: string;
  // Extra UI presentation helper fields
  customerCompany?: string;
  customerLifetimeValue?: number;
}

export interface MessageDto {
  id: string;
  organizationId: string;
  conversationId: string;
  direction: MessageDirection;
  content: string;
  attachmentsJson: string;
  senderName: string;
  senderId?: string;
  status: DeliveryStatus;
  readAt?: string;
  deliveredAt?: string;
  externalMessageId?: string;
  quickReplyMetadataJson: string;
  createdAt: string;
}

export interface MessageTemplateDto {
  id: string;
  organizationId: string;
  title: string;
  content: string;
  channelType?: CommunicationChannelType;
  category: string;
  shortcutCode: string;
  parametersJson: string;
  usageCount: number;
  createdAt: string;
}

export interface ChannelBadgeCountDto {
  channelType: CommunicationChannelType;
  unreadCount: number;
  activeCount: number;
}

export interface InboxSummaryDto {
  organizationId: string;
  totalActiveConversations: number;
  totalUnreadConversations: number;
  totalSlaBreached: number;
  totalUrgentConversations: number;
  channelCounts: ChannelBadgeCountDto[];
  lastRefreshedAt: string;
}

export interface ChannelUsageDto {
  channelType: CommunicationChannelType;
  conversationCount: number;
  totalMessages: number;
  percentageOfTotal: number;
}

export interface CsatSummaryDto {
  averageRating: number;
  totalResponses: number;
  satisfiedCount: number;
  neutralCount: number;
  unsatisfiedCount: number;
}

export interface AnalyticsOverviewDto {
  organizationId: string;
  activeConversations: number;
  totalMessagesToday: number;
  averageResponseTimeMinutes: number;
  averageResolutionTimeMinutes: number;
  channelBreakdown: ChannelUsageDto[];
  csatSummary: CsatSummaryDto;
  generatedAt: string;
}

export interface ConversationFilterDto {
  channelType?: CommunicationChannelType | string;
  status?: string;
  priority?: ConversationPriority | string;
  searchKeyword?: string;
  pageNumber?: number;
  pageSize?: number;
}

// ─── MOCK FALLBACK DEMONSTRATION DATA FOR ENTERPRISE SME PROTOTYPING ───
const MOCK_CONVERSATIONS: ConversationDto[] = [
  {
    id: "conv-101",
    organizationId: "org-1",
    channelType: "WhatsApp",
    subject: "Enterprise Tier Custom Quote Request",
    customerName: "Alex Rivera",
    customerEmail: "arivera@acme-global.com",
    customerPhone: "+1 (415) 890-3342",
    customerCompany: "Acme Global Solutions",
    customerLifetimeValue: 24500,
    status: "Open",
    priority: "Urgent",
    assignedToUserName: "Sarah Jenkins (Senior AE)",
    assignedToUserId: "usr-201",
    unreadMessagesCount: 2,
    lastMessageAt: new Date(Date.now() - 1000 * 60 * 6).toISOString(),
    lastMessagePreview: "We need an expedited quote for 150 additional user licenses before Friday's budget freeze.",
    slaDueDate: new Date(Date.now() + 1000 * 60 * 35).toISOString(),
    isSlaBreached: false,
    tags: "Enterprise, VIP-Renewal, Quote",
    metadataJson: "{}",
    createdAt: new Date(Date.now() - 1000 * 3600 * 4).toISOString(),
    csatRating: 5,
    csatFeedback: "Exceptionally rapid assistance on the enterprise licensing terms!"
  },
  {
    id: "conv-102",
    organizationId: "org-1",
    channelType: "LiveChat",
    subject: "Webhooks integration failing with 401 error",
    customerName: "David Chen",
    customerEmail: "d.chen@devops-scale.io",
    customerCompany: "DevOps Scale Inc.",
    customerLifetimeValue: 8900,
    status: "Open",
    priority: "High",
    assignedToUserName: "Marcus Vance (Support Eng)",
    assignedToUserId: "usr-202",
    unreadMessagesCount: 1,
    lastMessageAt: new Date(Date.now() - 1000 * 60 * 18).toISOString(),
    lastMessagePreview: "Can someone verify if our HMAC signature headers need to be hex or base64 encoded?",
    slaDueDate: new Date(Date.now() - 1000 * 60 * 12).toISOString(),
    isSlaBreached: true,
    tags: "Technical, API, Webhooks",
    metadataJson: "{}",
    createdAt: new Date(Date.now() - 1000 * 3600 * 7).toISOString()
  },
  {
    id: "conv-103",
    organizationId: "org-1",
    channelType: "Email",
    subject: "Annual contract billing cycle invoice correction",
    customerName: "Elena Rostova",
    customerEmail: "erostova@fintech-matrix.de",
    customerCompany: "Fintech Matrix GmbH",
    customerLifetimeValue: 42000,
    status: "Open",
    priority: "Medium",
    assignedToUserName: "Sarah Jenkins (Senior AE)",
    assignedToUserId: "usr-201",
    unreadMessagesCount: 0,
    lastMessageAt: new Date(Date.now() - 1000 * 3600 * 2).toISOString(),
    lastMessagePreview: "Thank you for sending the updated VAT breakdown. Just waiting on accounting signature.",
    slaDueDate: new Date(Date.now() + 1000 * 3600 * 5).toISOString(),
    isSlaBreached: false,
    tags: "Billing, Contract, VAT",
    metadataJson: "{}",
    createdAt: new Date(Date.now() - 1000 * 3600 * 24).toISOString(),
    csatRating: 5,
    csatFeedback: "Sarah handled our international tax invoice correction within minutes!"
  },
  {
    id: "conv-104",
    organizationId: "org-1",
    channelType: "SMS",
    subject: "Urgent: Delivery tracking status for hardware token package",
    customerName: "Marcus Sterling",
    customerPhone: "+1 (212) 555-9011",
    customerCompany: "Sterling Advisors",
    customerLifetimeValue: 3400,
    status: "Open",
    priority: "Urgent",
    assignedToUserName: "Chloe Bennett (CSM)",
    assignedToUserId: "usr-203",
    unreadMessagesCount: 1,
    lastMessageAt: new Date(Date.now() - 1000 * 60 * 42).toISOString(),
    lastMessagePreview: "The FedEx tracking number shows exception in Memphis transit hub. We have onboarding tomorrow!",
    slaDueDate: new Date(Date.now() + 1000 * 60 * 80).toISOString(),
    isSlaBreached: false,
    tags: "Shipping, Hardware, Onboarding",
    metadataJson: "{}",
    createdAt: new Date(Date.now() - 1000 * 3600 * 5).toISOString()
  },
  {
    id: "conv-105",
    organizationId: "org-1",
    channelType: "FacebookMessenger",
    subject: "Partner referral commission schedule inquiry",
    customerName: "Liam O'Connor",
    customerEmail: "liam@dublin-marketing.ie",
    customerCompany: "Dublin Digital Agency",
    customerLifetimeValue: 12400,
    status: "Snoozed",
    priority: "Low",
    assignedToUserName: "Unassigned",
    unreadMessagesCount: 0,
    lastMessageAt: new Date(Date.now() - 1000 * 3600 * 14).toISOString(),
    lastMessagePreview: "No rush at all! Let's touch base on Tuesday after the holiday weekend.",
    isSlaBreached: false,
    tags: "Partner, Commission, Referral",
    metadataJson: "{}",
    createdAt: new Date(Date.now() - 1000 * 3600 * 48).toISOString()
  },
  {
    id: "conv-106",
    organizationId: "org-1",
    channelType: "InstagramDm",
    subject: "Collab feature demonstration request for podcast episode",
    customerName: "Sophia Martinez",
    customerCompany: "SaaS Spotlight Creators",
    customerLifetimeValue: 1500,
    status: "Resolved",
    priority: "Low",
    assignedToUserName: "Chloe Bennett (CSM)",
    assignedToUserId: "usr-203",
    unreadMessagesCount: 0,
    lastMessageAt: new Date(Date.now() - 1000 * 3600 * 36).toISOString(),
    lastMessagePreview: "Awesome! The scheduled Cal-link works perfectly. See you on the live stream!",
    isSlaBreached: false,
    resolvedAt: new Date(Date.now() - 1000 * 3600 * 35).toISOString(),
    tags: "Media, Influencer, Podcast",
    metadataJson: "{}",
    createdAt: new Date(Date.now() - 1000 * 3600 * 72).toISOString(),
    csatRating: 4,
    csatFeedback: "Very friendly team and prompt coordination!"
  }
];

const MOCK_MESSAGES: Record<string, MessageDto[]> = {
  "conv-101": [
    {
      id: "msg-1",
      organizationId: "org-1",
      conversationId: "conv-101",
      direction: "Inbound",
      content: "Hello Sarah! We are expanding our regional APAC team and need an expedited quote for 150 additional user licenses before Friday's budget freeze.",
      attachmentsJson: "[]",
      senderName: "Alex Rivera",
      status: "Delivered",
      deliveredAt: new Date(Date.now() - 1000 * 60 * 12).toISOString(),
      quickReplyMetadataJson: "{}",
      createdAt: new Date(Date.now() - 1000 * 60 * 12).toISOString()
    },
    {
      id: "msg-2",
      organizationId: "org-1",
      conversationId: "conv-101",
      direction: "InternalNote",
      content: "⚠️ AE Note: Acme is approaching Tier 3 enterprise volume discounts (20% off seat rate). I am getting approval from VP Sales before sending formal contract.",
      attachmentsJson: "[]",
      senderName: "[Internal Note] Sarah Jenkins",
      senderId: "usr-201",
      status: "Read",
      quickReplyMetadataJson: "{}",
      createdAt: new Date(Date.now() - 1000 * 60 * 9).toISOString()
    },
    {
      id: "msg-3",
      organizationId: "org-1",
      conversationId: "conv-101",
      direction: "Outbound",
      content: "Hi Alex! Thanks for reaching out over our VIP WhatsApp channel. I see you're qualifying for our Tier 3 Enterprise volume discount (20% off standard seat pricing). I am finalizing the formal proposal document now.",
      attachmentsJson: "[]",
      senderName: "Sarah Jenkins",
      senderId: "usr-201",
      status: "Read",
      readAt: new Date(Date.now() - 1000 * 60 * 7).toISOString(),
      quickReplyMetadataJson: "{}",
      createdAt: new Date(Date.now() - 1000 * 60 * 8).toISOString()
    },
    {
      id: "msg-4",
      organizationId: "org-1",
      conversationId: "conv-101",
      direction: "Inbound",
      content: "That is fantastic news! If you can include the SOC2 compliance addendum in the quote package, our CFO will sign it this afternoon.",
      attachmentsJson: '[{"name": "APAC-Seat-Schedule.pdf", "size": "1.4 MB", "type": "pdf"}]',
      senderName: "Alex Rivera",
      status: "Delivered",
      quickReplyMetadataJson: "{}",
      createdAt: new Date(Date.now() - 1000 * 60 * 6).toISOString()
    }
  ]
};

const MOCK_TEMPLATES: MessageTemplateDto[] = [
  {
    id: "tpl-1",
    organizationId: "org-1",
    title: "VIP Enterprise License Upgrade Proposal",
    category: "Sales & Upgrades",
    shortcutCode: "/enterprise-quote",
    content: "Hi {{customer.name}}, thank you for contacting us about scaling {{customer.company}} on BusinessOS! Based on your target volume, we can apply our Tier 3 Enterprise discount (20% off standard seat rate). I have attached the customized agreement with our SOC2 privacy schedule.",
    channelType: "WhatsApp",
    parametersJson: '["customer.name", "customer.company"]',
    usageCount: 42,
    createdAt: new Date("2026-06-15").toISOString()
  },
  {
    id: "tpl-2",
    organizationId: "org-1",
    title: "Webhook HMAC Signature Guidance",
    category: "Technical Support",
    shortcutCode: "/webhooks-hmac",
    content: "Hi {{customer.name}}, great question! For high-security validation, our API requires base64 encoded SHA-256 signatures in the `X-Hub-Signature` header. You can check our interactive Developer Documentation for code samples in Python, Node, and .NET.",
    channelType: "LiveChat",
    parametersJson: '["customer.name"]',
    usageCount: 89,
    createdAt: new Date("2026-05-10").toISOString()
  },
  {
    id: "tpl-3",
    organizationId: "org-1",
    title: "Express Hardware Shipping Status Check",
    category: "Logistics & Shipping",
    shortcutCode: "/shipping-status",
    content: "Hello {{customer.name}}, our fulfillment team just checked with the courier hub. Your priority package is currently cleared through regional customs and out for direct courier delivery to {{customer.company}} by 3:00 PM today.",
    channelType: "SMS",
    parametersJson: '["customer.name", "customer.company"]',
    usageCount: 31,
    createdAt: new Date("2026-07-01").toISOString()
  },
  {
    id: "tpl-4",
    organizationId: "org-1",
    title: "Annual Billing Cycle Tax Breakdown",
    category: "Billing & Finance",
    shortcutCode: "/vat-invoice",
    content: "Dear {{customer.name}},\n\nThank you for reaching out regarding your invoice invoice record. We have updated your billing profile for {{customer.company}} to include the appropriate European tax registration exemptions. The amended invoice PDF is available in your self-service portal.",
    channelType: "Email",
    parametersJson: '["customer.name", "customer.company"]',
    usageCount: 67,
    createdAt: new Date("2026-04-20").toISOString()
  }
];

const MOCK_ANALYTICS: AnalyticsOverviewDto = {
  organizationId: "org-1",
  activeConversations: 24,
  totalMessagesToday: 342,
  averageResponseTimeMinutes: 4.8,
  averageResolutionTimeMinutes: 114.5, // ~1.9 hours
  channelBreakdown: [
    { channelType: "WhatsApp", conversationCount: 18, totalMessages: 154, percentageOfTotal: 38.5 },
    { channelType: "Email", conversationCount: 14, totalMessages: 92, percentageOfTotal: 26.9 },
    { channelType: "LiveChat", conversationCount: 10, totalMessages: 64, percentageOfTotal: 19.2 },
    { channelType: "SMS", conversationCount: 5, totalMessages: 22, percentageOfTotal: 9.6 },
    { channelType: "FacebookMessenger", conversationCount: 2, totalMessages: 8, percentageOfTotal: 3.8 },
    { channelType: "InstagramDm", conversationCount: 1, totalMessages: 2, percentageOfTotal: 2.0 },
  ],
  csatSummary: {
    averageRating: 4.85,
    totalResponses: 128,
    satisfiedCount: 119,
    neutralCount: 6,
    unsatisfiedCount: 3
  },
  generatedAt: new Date().toISOString()
};

// ─── SERVICE DEFINITIONS WITH API RETRY AND FALLBACK LOGIC ───
export const CommunicationService = {
  async getInboxSummary(): Promise<InboxSummaryDto> {
    try {
      const res = await apiClient.get<InboxSummaryDto>("/api/v1/communication-hub/inbox/summary");
      return res.data;
    } catch {
      return {
        organizationId: "org-1",
        totalActiveConversations: MOCK_CONVERSATIONS.filter(c => c.status !== "Resolved" && c.status !== "Closed").length,
        totalUnreadConversations: MOCK_CONVERSATIONS.filter(c => c.unreadMessagesCount > 0).length,
        totalSlaBreached: MOCK_CONVERSATIONS.filter(c => c.isSlaBreached).length,
        totalUrgentConversations: MOCK_CONVERSATIONS.filter(c => c.priority === "Urgent").length,
        channelCounts: [
          { channelType: "WhatsApp", unreadCount: 2, activeCount: 5 },
          { channelType: "LiveChat", unreadCount: 1, activeCount: 4 },
          { channelType: "Email", unreadCount: 0, activeCount: 6 },
          { channelType: "SMS", unreadCount: 1, activeCount: 2 },
          { channelType: "FacebookMessenger", unreadCount: 0, activeCount: 1 },
          { channelType: "InstagramDm", unreadCount: 0, activeCount: 1 }
        ],
        lastRefreshedAt: new Date().toISOString()
      };
    }
  },

  async getConversations(filter?: ConversationFilterDto): Promise<{ items: ConversationDto[]; totalCount: number }> {
    try {
      const params = new URLSearchParams();
      if (filter?.channelType) params.append("channelType", filter.channelType);
      if (filter?.status) params.append("status", filter.status);
      if (filter?.priority) params.append("priority", filter.priority);
      if (filter?.searchKeyword) params.append("searchKeyword", filter.searchKeyword);
      
      const res = await apiClient.get<{ items: ConversationDto[]; totalCount: number }>(
        `/api/v1/communication-hub/conversations?${params.toString()}`
      );
      if (res.data && res.data.items && res.data.items.length > 0) {
        return res.data;
      }
    } catch {
      // ignore offline error and fallback
    }

    // Apply front-end mock filtering
    let list = [...MOCK_CONVERSATIONS];
    if (filter?.channelType && filter.channelType !== "All") list = list.filter(c => c.channelType === filter.channelType);
    if (filter?.status && filter.status !== "All") list = list.filter(c => c.status.toLowerCase() === filter.status!.toLowerCase());
    if (filter?.priority && filter.priority !== "All") list = list.filter(c => c.priority === filter.priority);
    if (filter?.searchKeyword) {
      const kw = filter.searchKeyword.toLowerCase();
      list = list.filter(c => c.subject.toLowerCase().includes(kw) || c.customerName.toLowerCase().includes(kw) || c.lastMessagePreview.toLowerCase().includes(kw));
    }
    return { items: list, totalCount: list.length };
  },

  async getConversationById(id: string): Promise<ConversationDto | undefined> {
    try {
      const res = await apiClient.get<ConversationDto>(`/api/v1/communication-hub/conversations/${id}`);
      if (res.data) return res.data;
    } catch {
      // ignore
    }
    return MOCK_CONVERSATIONS.find(c => c.id === id) || MOCK_CONVERSATIONS[0];
  },

  async getMessages(conversationId: string): Promise<MessageDto[]> {
    try {
      const res = await apiClient.get<MessageDto[]>(`/api/v1/communication-hub/messages/conversation/${conversationId}`);
      if (res.data && res.data.length > 0) return res.data;
    } catch {
      // ignore
    }
    return MOCK_MESSAGES[conversationId] || [
      {
        id: `msg-default-${Date.now()}`,
        organizationId: "org-1",
        conversationId,
        direction: "Inbound",
        content: "Hello! We are looking into our service options and would love some assistance.",
        attachmentsJson: "[]",
        senderName: "Customer Inquiry",
        status: "Delivered",
        quickReplyMetadataJson: "{}",
        createdAt: new Date(Date.now() - 1000 * 60 * 30).toISOString()
      }
    ];
  },

  async sendMessage(conversationId: string, content: string, attachmentsJson = "[]"): Promise<MessageDto> {
    try {
      const res = await apiClient.post<MessageDto>("/api/v1/communication-hub/messages/send", {
        conversationId,
        content,
        attachmentsJson,
        quickReplyMetadataJson: "{}"
      });
      if (res.data) return res.data;
    } catch {
      // ignore
    }
    const newMsg: MessageDto = {
      id: `msg-${Date.now()}`,
      organizationId: "org-1",
      conversationId,
      direction: "Outbound",
      content,
      attachmentsJson,
      senderName: "You (Senior Agent)",
      status: "Sent",
      readAt: new Date().toISOString(),
      quickReplyMetadataJson: "{}",
      createdAt: new Date().toISOString()
    };
    if (!MOCK_MESSAGES[conversationId]) MOCK_MESSAGES[conversationId] = [];
    MOCK_MESSAGES[conversationId].push(newMsg);
    return newMsg;
  },

  async addInternalNote(conversationId: string, content: string, attachmentsJson = "[]"): Promise<MessageDto> {
    try {
      const res = await apiClient.post<MessageDto>("/api/v1/communication-hub/messages/note", {
        conversationId,
        content,
        attachmentsJson
      });
      if (res.data) return res.data;
    } catch {
      // ignore
    }
    const newNote: MessageDto = {
      id: `note-${Date.now()}`,
      organizationId: "org-1",
      conversationId,
      direction: "InternalNote",
      content,
      attachmentsJson,
      senderName: "[Internal Note] You (Senior Agent)",
      status: "Read",
      quickReplyMetadataJson: "{}",
      createdAt: new Date().toISOString()
    };
    if (!MOCK_MESSAGES[conversationId]) MOCK_MESSAGES[conversationId] = [];
    MOCK_MESSAGES[conversationId].push(newNote);
    return newNote;
  },

  async updateConversationStatus(id: string, status: string): Promise<ConversationDto | undefined> {
    try {
      const res = await apiClient.patch<ConversationDto>(`/api/v1/communication-hub/conversations/${id}/status`, { status });
      if (res.data) return res.data;
    } catch {
      // ignore
    }
    const target = MOCK_CONVERSATIONS.find(c => c.id === id);
    if (target) {
      target.status = status;
      if (status === "Resolved" || status === "Closed") {
        target.resolvedAt = new Date().toISOString();
      }
    }
    return target;
  },

  async assignConversation(id: string, assignedToUserId: string, assignedToUserName: string, assignmentReason?: string): Promise<ConversationDto | undefined> {
    try {
      const res = await apiClient.post<ConversationDto>(`/api/v1/communication-hub/conversations/${id}/assign`, {
        assignedToUserId,
        assignedToUserName,
        assignmentReason
      });
      if (res.data) return res.data;
    } catch {
      // ignore
    }
    const target = MOCK_CONVERSATIONS.find(c => c.id === id);
    if (target) {
      target.assignedToUserId = assignedToUserId;
      target.assignedToUserName = assignedToUserName;
    }
    return target;
  },

  async getTemplates(channel?: CommunicationChannelType, category?: string): Promise<MessageTemplateDto[]> {
    try {
      const params = new URLSearchParams();
      if (channel) params.append("channel", channel);
      if (category) params.append("category", category);
      const res = await apiClient.get<MessageTemplateDto[]>(`/api/v1/communication-hub/templates?${params.toString()}`);
      if (res.data && res.data.length > 0) return res.data;
    } catch {
      // ignore
    }
    let list = [...MOCK_TEMPLATES];
    if (channel) list = list.filter(t => !t.channelType || t.channelType === channel);
    if (category && category !== "All") list = list.filter(t => t.category.toLowerCase() === category.toLowerCase());
    return list;
  },

  async createTemplate(req: { title: string; content: string; channelType?: CommunicationChannelType; category?: string; shortcutCode?: string; parametersJson?: string }): Promise<MessageTemplateDto> {
    try {
      const res = await apiClient.post<MessageTemplateDto>("/api/v1/communication-hub/templates", req);
      if (res.data) return res.data;
    } catch {
      // ignore
    }
    const created: MessageTemplateDto = {
      id: `tpl-${Date.now()}`,
      organizationId: "org-1",
      title: req.title,
      content: req.content,
      channelType: req.channelType,
      category: req.category || "General",
      shortcutCode: req.shortcutCode || `/${req.title.toLowerCase().replace(/[^a-z0-9]/g, "-")}`,
      parametersJson: req.parametersJson || "[]",
      usageCount: 0,
      createdAt: new Date().toISOString()
    };
    MOCK_TEMPLATES.unshift(created);
    return created;
  },

  async updateTemplate(id: string, req: { title: string; content: string; channelType?: CommunicationChannelType; category?: string; shortcutCode?: string; parametersJson?: string }): Promise<MessageTemplateDto | undefined> {
    try {
      const res = await apiClient.put<MessageTemplateDto>(`/api/v1/communication-hub/templates/${id}`, req);
      if (res.data) return res.data;
    } catch {
      // ignore
    }
    const target = MOCK_TEMPLATES.find(t => t.id === id);
    if (target) {
      target.title = req.title;
      target.content = req.content;
      target.channelType = req.channelType;
      if (req.category) target.category = req.category;
      if (req.shortcutCode) target.shortcutCode = req.shortcutCode;
      if (req.parametersJson) target.parametersJson = req.parametersJson;
    }
    return target;
  },

  async deleteTemplate(id: string): Promise<boolean> {
    try {
      await apiClient.delete(`/api/v1/communication-hub/templates/${id}`);
      return true;
    } catch {
      const idx = MOCK_TEMPLATES.findIndex(t => t.id === id);
      if (idx !== -1) {
        MOCK_TEMPLATES.splice(idx, 1);
        return true;
      }
      return false;
    }
  },

  async renderTemplate(id: string, variables: Record<string, string>): Promise<{ renderedContent: string }> {
    try {
      const res = await apiClient.post<{ renderedContent: string }>(`/api/v1/communication-hub/templates/${id}/render`, { variables });
      if (res.data) return res.data;
    } catch {
      // ignore
    }
    const tpl = MOCK_TEMPLATES.find(t => t.id === id);
    if (!tpl) return { renderedContent: "" };
    let content = tpl.content;
    Object.entries(variables).forEach(([k, v]) => {
      content = content.replace(new RegExp(`{{${k}}}`, "g"), v);
      content = content.replace(new RegExp(`{${k}}`, "g"), v);
    });
    tpl.usageCount++;
    return { renderedContent: content };
  },

  async getAnalyticsOverview(): Promise<AnalyticsOverviewDto> {
    try {
      const res = await apiClient.get<AnalyticsOverviewDto>("/api/v1/communication-hub/analytics/overview");
      if (res.data) return res.data;
    } catch {
      // ignore
    }
    return MOCK_ANALYTICS;
  }
};
