import { z } from "zod";

enum QRType {
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

const data = {
  name: "Netflix",
  description: "",
  qrType: "Website",
  originalValue: "https://www.netflix.com/in/",
  folder: "",
  tags: "",
  foregroundColor: "#000000",
  backgroundColor: "#FFFFFF",
  size: 300,
  margin: 2,
  labelText: "",
  labelFont: "sans-serif",
  logoUrl: "",
  errorCorrectionLevel: "M",
  passwordProtected: false,
  password: "",
  expirationDate: ""
};

const result = formSchema.safeParse(data);
console.log(JSON.stringify(result, null, 2));
