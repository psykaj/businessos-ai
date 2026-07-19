export interface ApiKey {
  id: string;
  name: string;
  keyPrefix: string;
  expiresAt?: string;
  lastUsedAt?: string;
  isActive: boolean;
  createdAt: string;
}

export interface CreateApiKeyRequest {
  name: string;
  expiresInDays?: number;
}

export interface CreateApiKeyResponse {
  apiKey: ApiKey;
  plainTextKey: string; // Only returned once upon creation
}
