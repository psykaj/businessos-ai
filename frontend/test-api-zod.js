const { z } = require("zod");
const axios = require("axios");

const API_URL = "http://localhost:5294/api";

const QRType = {
  Website: "Website",
  PDF: "PDF",
  Image: "Image",
  Video: "Video",
  Text: "Text",
  Email: "Email",
  Phone: "Phone",
  SMS: "SMS",
  WhatsApp: "WhatsApp",
  WiFi: "WiFi",
  GoogleMaps: "GoogleMaps",
  BusinessCard: "BusinessCard",
  Event: "Event",
  SocialMedia: "SocialMedia",
  AppDownload: "AppDownload",
  Menu: "Menu",
  PaymentLink: "PaymentLink",
};

const formSchema = z.object({
  name: z.string().min(1, "Name is required").max(150),
  description: z.string().max(500).optional(),
  qrType: z.nativeEnum(QRType),
  originalValue: z.string().min(1, "Destination Value is required"),
  folder: z.string().optional(),
  tags: z.string().optional(),
  foregroundColor: z.string().regex(/^#(?:[0-9a-fA-F]{3}){1,2}$/, "Must be a valid hex color"),
  backgroundColor: z.string().regex(/^#(?:[0-9a-fA-F]{3}){1,2}$/, "Must be a valid hex color"),
  size: z.number().min(100).max(2000),
  margin: z.number().min(0).max(10),
  errorCorrectionLevel: z.enum(["L", "M", "Q", "H"]),
  labelText: z.string().max(100).optional(),
  labelFont: z.string().optional(),
  logoUrl: z.string().url("Must be a valid URL").optional().or(z.literal("")),
  passwordProtected: z.boolean(),
  password: z.string().optional(),
  expirationDate: z.string().optional(),
});

async function run() {
  try {
    // 1. Login to get token
    const loginRes = await axios.post(`${API_URL}/auth/login`, {
      email: "testuser@example.com",
      password: "Password123"
    });
    const token = loginRes.data.data.token;

    // 2. Fetch QR codes
    const qrRes = await axios.get(`${API_URL}/qrcodes`, {
      headers: { Authorization: `Bearer ${token}` }
    });
    
    const items = qrRes.data.data.items;
    
    for (const initialData of items) {
      console.log(`\nTesting QR Code: ${initialData.name}`);
      console.log("Initial Data from API:", JSON.stringify(initialData, null, 2));
      
      const formData = {
        name: initialData?.name || "",
        description: initialData?.description || "",
        qrType: initialData?.qrType || QRType.Website,
        originalValue: initialData?.originalValue || "",
        folder: initialData?.folder || "",
        tags: initialData?.tags?.join(", ") || "",
        foregroundColor: initialData?.foregroundColor || "#000000",
        backgroundColor: initialData?.backgroundColor || "#FFFFFF",
        size: initialData?.size || 300,
        margin: initialData?.margin || 2,
        labelText: initialData?.labelText || "",
        labelFont: initialData?.labelFont || "sans-serif",
        logoUrl: initialData?.logoUrl || "",
        errorCorrectionLevel: initialData?.errorCorrectionLevel || "M",
        passwordProtected: initialData?.passwordProtected || false,
        password: "",
        expirationDate: initialData?.expirationDate ? new Date(initialData.expirationDate).toISOString().split('T')[0] : "",
      };
      
      console.log("Form Data:", JSON.stringify(formData, null, 2));
      const result = formSchema.safeParse(formData);
      if (!result.success) {
        console.error("Zod Validation Failed:", JSON.stringify(result.error.format(), null, 2));
      } else {
        console.log("Zod Validation Passed!");
      }
    }
  } catch (err) {
    console.error("Error:", err.response ? err.response.data : err.message);
  }
}

run();
