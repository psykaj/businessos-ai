import apiClient from "./api-client";
import { CreateQRCodeDto, PagedResult, QRCodeDto, UpdateQRCodeDto, QRType } from "../types/qr";

export const qrService = {
  async getQRCodes(params: {
    search?: string;
    folder?: string;
    status?: string;
    type?: QRType;
    pageNumber?: number;
    pageSize?: number;
  }): Promise<PagedResult<QRCodeDto>> {
    const { data } = await apiClient.get<{ data: PagedResult<QRCodeDto> }>("/api/qrcodes", { params });
    return data.data;
  },

  async getQRCode(id: string): Promise<QRCodeDto> {
    const { data } = await apiClient.get<{ data: QRCodeDto }>(`/api/qrcodes/${id}`);
    return data.data;
  },

  async createQRCode(dto: CreateQRCodeDto): Promise<QRCodeDto> {
    const { data } = await apiClient.post<{ data: QRCodeDto }>("/api/qrcodes", dto);
    return data.data;
  },

  async updateQRCode(id: string, dto: UpdateQRCodeDto): Promise<QRCodeDto> {
    const { data } = await apiClient.put<{ data: QRCodeDto }>(`/api/qrcodes/${id}`, dto);
    return data.data;
  },

  async deleteQRCode(id: string): Promise<void> {
    await apiClient.delete(`/api/qrcodes/${id}`);
  },

  async downloadQRImage(id: string, format: "png" | "svg"): Promise<void> {
    const response = await apiClient.get(`/api/qrcodes/${id}/image?format=${format}`, {
      responseType: "blob",
    });

    const url = window.URL.createObjectURL(new Blob([response.data]));
    const link = document.createElement("a");
    link.href = url;
    link.setAttribute("download", `qrcode-${id}.${format}`);
    document.body.appendChild(link);
    link.click();
    link.remove();
    window.URL.revokeObjectURL(url);
  },

  async getPublicQRCode(shortCode: string): Promise<any> {
    const { data } = await apiClient.get(`/api/public/qrcodes/${shortCode}`);
    return data.data;
  },

  async verifyPassword(shortCode: string, password?: string): Promise<string> {
    const { data } = await apiClient.post(`/api/public/qrcodes/${shortCode}/verify`, { password });
    return data.data; // this returns the originalValue
  }
};
