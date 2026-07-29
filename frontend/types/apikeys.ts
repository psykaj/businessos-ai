export interface ApiKey {
  id: string;
  name: string;
  keyPrefix: string;
  expiresAt?: string;
  lastUsedAt?: string;
  isActive: boolean;
  createdAt: string;
  scopes?: string[];
}

export interface CreateApiKeyRequest {
  name: string;
  expiresInDays?: number;
  scopes?: string[];
}

export interface CreateApiKeyResponse {
  apiKey: ApiKey;
  plainTextKey: string; // Only returned once upon creation
}
