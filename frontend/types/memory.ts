export type MemoryType =
  | "BusinessPreference"
  | "CustomerContext"
  | "ProductContext"
  | "Decision"
  | "ImportantEvent"
  | "InteractionSummary"
  | "WorkflowOutcome"
  | "ActionOutcome"
  | "BusinessPattern"
  | "UserPreference";

export type MemoryConfidence = "Low" | "Medium" | "High";
export type MemoryImportance = "Low" | "Medium" | "High" | "Critical";

export interface MemoryDto {
  id: string;
  organizationId: string;
  memoryType: MemoryType;
  title: string;
  content: string;
  structuredData?: string;
  sourceModule: string;
  sourceEntityId: string;
  importance: MemoryImportance;
  confidence: MemoryConfidence;
  expiresAt?: string | null;
  lastUsedAt?: string | null;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface CreateMemoryDto {
  memoryType: MemoryType;
  title: string;
  content: string;
  structuredData?: string;
  sourceModule: string;
  sourceEntityId: string;
  importance?: MemoryImportance;
  confidence?: MemoryConfidence;
  expiresAt?: string | null;
}

export interface UpdateMemoryDto {
  title?: string;
  content?: string;
  structuredData?: string;
  importance?: MemoryImportance;
  confidence?: MemoryConfidence;
  expiresAt?: string | null;
  isActive?: boolean;
}

export interface CopilotContextRequestDto {
  question: string;
  entityType?: string;
  entityId?: string;
}

export interface CopilotContextResponseDto {
  relevantContext: string;
  currentData?: Record<string, unknown>;
  relevantMemories: MemoryDto[];
  suggestedSources: string[];
}
