export enum QRType {
  Website = "Website",
  PDF = "PDF",
  Image = "Image",
  Video = "Video",
  Text = "Text",
  Email = "Email",
  Phone = "Phone",
  SMS = "SMS",
  WhatsApp = "WhatsApp",
  WiFi = "WiFi",
  GoogleMaps = "GoogleMaps",
  BusinessCard = "BusinessCard",
  Event = "Event",
  SocialMedia = "SocialMedia",
  AppDownload = "AppDownload",
  Menu = "Menu",
  PaymentLink = "PaymentLink",
}

export interface QRCodeDto {
  id: string;
  organizationId: string;
  name: string;
  description?: string;
  qrType: QRType;
  originalValue: string;
  shortCode: string;
  qrImageUrl?: string;
  status: string;
  folder?: string;
  tags: string[];
  foregroundColor: string;
  backgroundColor: string;
  logoUrl?: string;
  labelText?: string;
  labelFont?: string;
  size: number;
  margin: number;
  errorCorrectionLevel: string;
  passwordProtected: boolean;
  expirationDate?: string;
  scanCount: number;
  createdAt: string;
  updatedAt: string;
}

export interface CreateQRCodeDto {
  name: string;
  description?: string;
  qrType: QRType;
  originalValue: string;
  folder?: string;
  tags: string[];
  foregroundColor: string;
  backgroundColor: string;
  logoUrl?: string;
  labelText?: string;
  labelFont?: string;
  size: number;
  margin: number;
  errorCorrectionLevel: string;
  password?: string;
  expirationDate?: string;
}

export interface UpdateQRCodeDto {
  name: string;
  description?: string;
  originalValue: string;
  folder?: string;
  tags: string[];
  status: string;
  foregroundColor: string;
  backgroundColor: string;
  logoUrl?: string;
  labelText?: string;
  labelFont?: string;
  size: number;
  margin: number;
  errorCorrectionLevel: string;
  password?: string;
  expirationDate?: string;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}
