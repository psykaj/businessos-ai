"use client";

import { useState } from "react";
import { useForm, Controller } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { useRouter } from "next/navigation";
import { QRType, CreateQRCodeDto, UpdateQRCodeDto, QRCodeDto } from "@/types/qr";
import { qrService } from "@/lib/qr-service";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Textarea } from "@/components/ui/textarea";
import { Button } from "@/components/ui/button";
import { Switch } from "@/components/ui/switch";
import { Slider } from "@/components/ui/slider";
import { Tooltip, TooltipContent, TooltipProvider, TooltipTrigger } from "@/components/ui/tooltip";
import { Accordion, AccordionContent, AccordionItem, AccordionTrigger } from "@/components/ui/accordion";
import { Info, Settings } from "lucide-react";
import { toast } from "sonner";
import { QRPreview } from "./qr-preview";
import { ColorPicker } from "./color-picker";

const formSchema = z.object({
  name: z.string().min(1, "Name is required").max(150),
  description: z.string().max(500).optional().nullable(),
  qrType: z.string().min(1),
  originalValue: z.string().min(1, "Destination Value is required"),
  folder: z.string().optional().nullable(),
  tags: z.string().optional().nullable(),
  foregroundColor: z.string(),
  backgroundColor: z.string(),
  size: z.number().min(50).max(2500),
  margin: z.number().min(0).max(10),
  errorCorrectionLevel: z.string(),
  labelText: z.string().max(100).optional().nullable(),
  labelFont: z.string().optional().nullable(),
  logoUrl: z.string().optional().nullable(),
  passwordProtected: z.boolean(),
  password: z.string().optional().nullable(),
  expirationDate: z.string().optional().nullable(),
}).refine(data => {
  if (data.passwordProtected && (!data.password || data.password.trim().length === 0)) {
    return false;
  }
  return true;
}, {
  message: "Password is required",
  path: ["password"]
});

type FormData = z.infer<typeof formSchema>;

interface QRFormProps {
  initialData?: QRCodeDto;
  isEditing?: boolean;
}

export function QRForm({ initialData, isEditing }: QRFormProps) {
  const router = useRouter();
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [qrImageUrl] = useState<string | undefined>(
    isEditing ? `/api/qrcodes/${initialData?.id}/image?format=png` : undefined
  );

  const { register, handleSubmit, control, watch, formState: { errors } } = useForm<FormData>({
    resolver: zodResolver(formSchema),
    defaultValues: {
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
      errorCorrectionLevel: (initialData?.errorCorrectionLevel as "L" | "M" | "Q" | "H") || "M",
      passwordProtected: initialData?.passwordProtected || false,
      password: "",
      expirationDate: initialData?.expirationDate ? new Date(initialData.expirationDate).toISOString().split('T')[0] : "",
    },
  });

  const watchForegroundColor = watch("foregroundColor");
  const watchBackgroundColor = watch("backgroundColor");
  const watchSize = watch("size");
  const watchOriginalValue = watch("originalValue");
  const watchMargin = watch("margin");
  const watchErrorCorrectionLevel = watch("errorCorrectionLevel");
  const watchLabelText = watch("labelText");
  const watchLabelFont = watch("labelFont");
  const watchLogoUrl = watch("logoUrl");
  const watchPasswordProtected = watch("passwordProtected");
  const watchQrType = watch("qrType");

  const getDestinationPlaceholder = (type: QRType) => {
    switch (type) {
      case QRType.Website: return "https://example.com";
      case QRType.PDF: return "https://example.com/document.pdf";
      case QRType.Image: return "https://example.com/image.jpg";
      case QRType.Video: return "https://youtube.com/watch?v=...";
      case QRType.Text: return "Enter any text here...";
      case QRType.Email: return "hello@example.com";
      case QRType.Phone: return "+1234567890";
      case QRType.SMS: return "+1234567890";
      case QRType.WhatsApp: return "+1234567890";
      case QRType.WiFi: return "NetworkSSID";
      case QRType.GoogleMaps: return "https://maps.app.goo.gl/...";
      case QRType.SocialMedia: return "https://instagram.com/username";
      case QRType.AppDownload: return "https://apps.apple.com/app/id123";
      case QRType.Menu: return "https://example.com/menu.pdf";
      case QRType.PaymentLink: return "https://paypal.me/username";
      default: return "Enter destination...";
    }
  };

  const onSubmit = async (data: FormData) => {
    setIsSubmitting(true);
    try {
      const tagsArray = data.tags ? data.tags.split(",").map(t => t.trim()).filter(Boolean) : [];
      
      if (isEditing && initialData) {
        const updateDto: UpdateQRCodeDto = {
          name: data.name,
          description: data.description || undefined,
          originalValue: data.originalValue,
          folder: data.folder || undefined,
          tags: tagsArray,
          status: initialData.status,
          foregroundColor: data.foregroundColor,
          backgroundColor: data.backgroundColor,
          logoUrl: data.logoUrl || undefined,
          labelText: data.labelText || undefined,
          labelFont: data.labelFont || undefined,
          size: data.size,
          margin: data.margin,
          errorCorrectionLevel: data.errorCorrectionLevel,
          password: data.passwordProtected ? (data.password || undefined) : undefined,
          expirationDate: data.expirationDate ? new Date(data.expirationDate).toISOString() : undefined,
        };
        await qrService.updateQRCode(initialData.id, updateDto);
        toast.success("QR Code updated successfully");
      } else {
        const createDto: CreateQRCodeDto = {
          name: data.name,
          qrType: data.qrType as QRType,
          description: data.description || undefined,
          originalValue: data.originalValue,
          folder: data.folder || undefined,
          tags: tagsArray,
          foregroundColor: data.foregroundColor,
          backgroundColor: data.backgroundColor,
          logoUrl: data.logoUrl || undefined,
          labelText: data.labelText || undefined,
          labelFont: data.labelFont || undefined,
          size: data.size,
          margin: data.margin,
          errorCorrectionLevel: data.errorCorrectionLevel,
          password: data.passwordProtected ? (data.password || undefined) : undefined,
          expirationDate: data.expirationDate ? new Date(data.expirationDate).toISOString() : undefined,
        };
        await qrService.createQRCode(createDto);
        toast.success("QR Code created successfully");
      }
      router.push("/dashboard/qr");
      router.refresh();
    } catch (error: unknown) {
      console.error("Submission error:", error);
      toast.error((error as { response?: { data?: { Message?: string } } }).response?.data?.Message || "An error occurred during submission");
    } finally {
      setIsSubmitting(false);
    }
  };

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const onError = (errors: any) => {
    console.error("Form validation errors:", errors);
    toast.error("Please fill in all required fields correctly.");
  };

  return (
    <TooltipProvider>
      <div className="grid grid-cols-1 lg:grid-cols-12 gap-8">
        <div className="lg:col-span-7 xl:col-span-8">
          <form onSubmit={handleSubmit(onSubmit, onError)} className="space-y-8 bg-card p-6 md:p-8 rounded-xl border shadow-sm">
            <div>
              <h2 className="text-2xl font-semibold mb-6">Create QR Code</h2>
              <div className="space-y-6">
                
                {/* 1. Essential Input */}
                <div className="space-y-2">
                  <div className="flex justify-between items-baseline">
                    <Label htmlFor="originalValue" className="text-base">Destination URL or Text *</Label>
                    {errors.originalValue && <span className="text-sm font-medium text-destructive">{errors.originalValue.message}</span>}
                  </div>
                  <Input id="originalValue" className={`h-12 text-base ${errors.originalValue ? 'border-destructive focus-visible:ring-destructive' : ''}`} {...register("originalValue")} placeholder={getDestinationPlaceholder(watchQrType as QRType)} autoFocus />
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                  <div className="space-y-2">
                    <div className="flex justify-between items-baseline">
                      <Label htmlFor="name">Internal Name *</Label>
                      {errors.name && <span className="text-sm font-medium text-destructive">{errors.name.message}</span>}
                    </div>
                    <Input id="name" className={errors.name ? 'border-destructive focus-visible:ring-destructive' : ''} {...register("name")} placeholder="My Website QR" />
                  </div>

                  <div className="space-y-2">
                    <Label htmlFor="qrType">Type of QR Code</Label>
                    <Controller
                      name="qrType"
                      control={control}
                      render={({ field }) => (
                        <Select onValueChange={field.onChange} value={field.value || ""} disabled={isEditing}>
                          <SelectTrigger>
                            <SelectValue placeholder="Select Type" />
                          </SelectTrigger>
                          <SelectContent>
                            {Object.values(QRType).map((type) => (
                              <SelectItem key={type} value={type}>{type}</SelectItem>
                            ))}
                          </SelectContent>
                        </Select>
                      )}
                    />
                  </div>
                </div>

                {/* 3. Advanced Settings Accordion */}
                <Accordion className="w-full mt-4">
                  <AccordionItem value="advanced-settings" className="border-t border-border mt-4">
                    <AccordionTrigger className="text-muted-foreground hover:text-foreground">
                      <div className="flex items-center gap-2">
                        <Settings className="h-4 w-4" />
                        <span>Advanced Options (Colors, Branding, Protection)</span>
                      </div>
                    </AccordionTrigger>
                    <AccordionContent className="space-y-6 pt-4 pb-2 px-1">
{/* 2. Visuals & Layout Settings */}
                <div className="pt-2">
                  <h3 className="text-lg font-medium mb-4">Design & Adjustments</h3>
                  <div className="space-y-6">
                    <div className="space-y-4">
                      <Label className="flex justify-between">
                        <span>Size (Pixels)</span>
                        <span className="text-muted-foreground font-normal">{watchSize}px</span>
                      </Label>
                      <Controller
                        name="size"
                        control={control}
                        render={({ field }) => (
                          <div className="flex items-center gap-4">
                            <Slider 
                              min={100} 
                              max={2000} 
                              step={10} 
                              value={[field.value]} 
                              onValueChange={(val) => field.onChange((val as number[])[0])} 
                              className="flex-1"
                            />
                            <Input 
                              type="number" 
                              className="w-20 text-right" 
                              value={field.value} 
                              onChange={(e) => field.onChange(Number(e.target.value))} 
                              min={100} 
                              max={2000} 
                            />
                          </div>
                        )}
                      />
                    </div>

                    <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
                      <div className="space-y-4">
                        <Label className="flex justify-between">
                          <span>Margin (Quiet Zone)</span>
                          <span className="text-muted-foreground font-normal">{watchMargin} modules</span>
                        </Label>
                        <Controller
                          name="margin"
                          control={control}
                          render={({ field }) => (
                            <Slider 
                              min={0} 
                              max={10} 
                              step={1} 
                              value={[field.value]} 
                              onValueChange={(val) => field.onChange((val as number[])[0])} 
                              className="flex-1 mt-2"
                            />
                          )}
                        />
                      </div>
                      
                      <div className="space-y-2">
                        <Label>Error Correction Level</Label>
                        <Controller
                          name="errorCorrectionLevel"
                          control={control}
                          render={({ field }) => (
                            <Select onValueChange={field.onChange} value={field.value || ""}>
                              <SelectTrigger>
                                <SelectValue placeholder="Select Level" />
                              </SelectTrigger>
                              <SelectContent>
                                <SelectItem value="L">Low (Best for simple designs)</SelectItem>
                                <SelectItem value="M">Medium (Standard)</SelectItem>
                                <SelectItem value="Q">Quartile (More robust)</SelectItem>
                                <SelectItem value="H">High (Best with custom logos)</SelectItem>
                              </SelectContent>
                            </Select>
                          )}
                        />
                      </div>
                    </div>
                  </div>
                </div>

                
<div className="border-t border-border my-6"></div>

                      
                      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                        <Controller
                          name="foregroundColor"
                          control={control}
                          render={({ field }) => (
                            <ColorPicker label="Foreground Color" value={field.value} onChange={field.onChange} />
                          )}
                        />
                        <Controller
                          name="backgroundColor"
                          control={control}
                          render={({ field }) => (
                            <ColorPicker label="Background Color" value={field.value} onChange={field.onChange} />
                          )}
                        />
                      </div>

                      <div className="space-y-2">
                        <Label htmlFor="logoUrl">Custom Logo URL (Center Image)</Label>
                        <Input id="logoUrl" {...register("logoUrl")} placeholder="https://example.com/logo.png" />
                      </div>

                      <div className="grid grid-cols-1 md:grid-cols-2 gap-4 border-t border-border pt-4">
                        <div className="space-y-2">
                          <Label htmlFor="labelText">Text Label (Below QR)</Label>
                          <Input id="labelText" {...register("labelText")} placeholder="Scan Me" />
                        </div>
                        <div className="space-y-2">
                          <Label htmlFor="labelFont">Label Font</Label>
                          <Controller
                            name="labelFont"
                            control={control}
                            render={({ field }) => (
                              <Select onValueChange={field.onChange} value={field.value || ""}>
                                <SelectTrigger><SelectValue placeholder="Select Font" /></SelectTrigger>
                                <SelectContent>
                                  <SelectItem value="sans-serif">Sans Serif</SelectItem>
                                  <SelectItem value="serif">Serif</SelectItem>
                                  <SelectItem value="monospace">Monospace</SelectItem>
                                  <SelectItem value="system-ui">System</SelectItem>
                                </SelectContent>
                              </Select>
                            )}
                          />
                        </div>
                      </div>

                      <div className="grid grid-cols-1 md:grid-cols-2 gap-4 border-t border-border pt-4">
                        <div className="space-y-4">
                          <div className="flex items-center gap-2">
                            <Controller
                              name="passwordProtected"
                              control={control}
                              render={({ field }) => (
                                <Switch checked={field.value} onCheckedChange={field.onChange} id="require-password" />
                              )}
                            />
                            <Label htmlFor="require-password">Require Password to Scan</Label>
                          </div>
                          
                          {watchPasswordProtected && (
                            <div className="space-y-2 mt-2">
                              <div className="flex justify-between items-baseline">
                                <Label htmlFor="password">Password *</Label>
                                {errors.password && <span className="text-sm font-medium text-destructive">{errors.password.message}</span>}
                              </div>
                              <Input id="password" type="password" className={errors.password ? 'border-destructive focus-visible:ring-destructive' : ''} {...register("password")} placeholder="Set a password" />
                            </div>
                          )}
                        </div>
                        
                        <div className="space-y-2">
                          <Label htmlFor="expirationDate">Expiration Date (Optional)</Label>
                          <Input id="expirationDate" type="date" {...register("expirationDate")} min={new Date().toISOString().split('T')[0]} />
                        </div>
                      </div>
                      
                      <div className="space-y-2 border-t border-border pt-4">
                        <Label htmlFor="description">Internal Description</Label>
                        <Textarea id="description" {...register("description")} placeholder="Brief notes for yourself..." />
                      </div>

                      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                        <div className="space-y-2">
                          <Label htmlFor="folder">Folder</Label>
                          <Input id="folder" {...register("folder")} placeholder="Marketing" />
                        </div>
                        <div className="space-y-2">
                          <Label htmlFor="tags">Tags (comma separated)</Label>
                          <Input id="tags" {...register("tags")} placeholder="campaign, social" />
                        </div>
                      </div>
                      
                    </AccordionContent>
                  </AccordionItem>
                </Accordion>
                
              </div>
            </div>

            <div className="flex justify-end gap-4 pt-6 border-t border-border">
              <Button variant="outline" type="button" onClick={() => router.back()} className="px-8">Cancel</Button>
              <Button type="submit" disabled={isSubmitting} className="px-8">
                {isSubmitting ? "Generating..." : isEditing ? "Save Changes" : "Generate QR Code"}
              </Button>
            </div>
          </form>
        </div>
        
        <div className="lg:col-span-5 xl:col-span-4">
          <div className="sticky top-6">
            <QRPreview 
              foregroundColor={watchForegroundColor} 
              backgroundColor={watchBackgroundColor} 
              size={watchSize} 
              originalValue={(isEditing && initialData?.shortCode && typeof window !== 'undefined') ? `${window.location.origin}/r/${initialData.shortCode}` : watchOriginalValue}
              margin={watchMargin}
              errorCorrectionLevel={watchErrorCorrectionLevel}
              labelText={watchLabelText ?? undefined}
              labelFont={watchLabelFont ?? undefined}
              logoUrl={watchLogoUrl ?? undefined}
              qrImageUrl={qrImageUrl}
            />
          </div>
        </div>
      </div>
    </TooltipProvider>
  );
}
