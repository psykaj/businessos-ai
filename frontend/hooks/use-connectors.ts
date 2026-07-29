import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import apiClient from "@/lib/api-client";
import { toast } from "sonner";

export interface ConnectorProvider {
  id: string;
  name: string;
  category: string;
  description: string;
  iconUrl?: string;
  isInstalled: boolean;
}

export const useConnectors = () => {
  return useQuery({
    queryKey: ["connectors"],
    queryFn: async () => {
      // For now, mock connectors list as requested by the marketplace design
      // A full implementation would fetch from /api/connectors/providers
      return [
        { id: "GoogleWorkspace", name: "Google Workspace", category: "Productivity", description: "Sync CRM leads to Sheets and meetings to Calendar.", isInstalled: true },
        { id: "Microsoft365", name: "Microsoft 365", category: "Productivity", description: "Connect Outlook and Excel for seamless data flow.", isInstalled: false },
        { id: "Slack", name: "Slack", category: "Communication", description: "Get instant deal alerts and notifications in your channels.", isInstalled: true },
        { id: "Teams", name: "Microsoft Teams", category: "Communication", description: "Post automated workflow activity to Teams.", isInstalled: false },
        { id: "Zoom", name: "Zoom", category: "Meetings", description: "Auto-create Zoom links for qualified sales calls.", isInstalled: false },
        { id: "Stripe", name: "Stripe", category: "Payments", description: "Sync invoices and trigger actions on successful payments.", isInstalled: false },
        { id: "Razorpay", name: "Razorpay", category: "Payments", description: "Receive instant webhook payment confirmations.", isInstalled: false },
        { id: "Shopify", name: "Shopify", category: "E-Commerce", description: "Import orders, products, and customers automatically.", isInstalled: false },
        { id: "WooCommerce", name: "WooCommerce", category: "E-Commerce", description: "Sync your WooCommerce store with BusinessOS CRM.", isInstalled: false },
        { id: "Meta", name: "Meta Ads", category: "Marketing", description: "Capture leads from Facebook and Instagram Ads directly.", isInstalled: false },
        { id: "GoogleAds", name: "Google Ads", category: "Marketing", description: "Track offline conversions and sync audience segments.", isInstalled: false },
      ] as ConnectorProvider[];
    },
  });
};

export const useInstallConnector = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (connectorId: string) => {
      // await apiClient.post(`/api/connectors/${connectorId}/install`);
      return Promise.resolve();
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["connectors"] });
      toast.success("Connector installation started.");
    },
    onError: () => {
      toast.error("Failed to install connector.");
    },
  });
};
