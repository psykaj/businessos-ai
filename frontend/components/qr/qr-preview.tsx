"use client";

import { useState, useEffect } from "react";
import { Card } from "@/components/ui/card";
import { QrCode } from "lucide-react";
import { QRCodeSVG } from "qrcode.react";

interface QRPreviewProps {
  foregroundColor: string;
  backgroundColor: string;
  size: number;
  qrImageUrl?: string;
  isLoading?: boolean;
  originalValue?: string;
  margin?: number;
  errorCorrectionLevel?: string;
  labelText?: string;
  labelFont?: string;
  logoUrl?: string;
}

export function QRPreview({ 
  foregroundColor, 
  backgroundColor, 
  size, 
  originalValue,
  margin = 1,
  errorCorrectionLevel = "M",
  labelText,
  labelFont,
  logoUrl,
  isLoading 
}: QRPreviewProps) {
  
  const hasValue = originalValue && originalValue.trim().length > 0;
  const [imageError, setImageError] = useState(false);

  useEffect(() => {
    // eslint-disable-next-line react-hooks/set-state-in-effect
    setImageError(false);
  }, [logoUrl]);
  
  return (
    <Card className="flex flex-col items-center justify-center p-8 bg-card border-dashed" id="qr-preview-container">
      <div 
        className="flex flex-col items-center justify-center rounded-md border overflow-hidden p-4"
        style={{ 
          backgroundColor, 
          boxShadow: "0 4px 6px -1px rgb(0 0 0 / 0.1), 0 2px 4px -2px rgb(0 0 0 / 0.1)"
        }}
      >
        {isLoading ? (
          <div className="animate-pulse flex items-center justify-center" style={{ width: 200, height: 200, backgroundColor: `${foregroundColor}20` }}>
            <QrCode className="h-12 w-12" style={{ color: foregroundColor }} />
          </div>
        ) : hasValue ? (
          <>
            {(() => {
              const safeSize = Number.isFinite(size) && size > 0 ? size : 250;
              const logoSize = safeSize * 0.2;
              
              // We use a transparent 1x1 pixel for the SVG excavation to hollow out the center perfectly
              // but we overlay our own HTML element so we can handle errors and styling better.
              const transparentPixel = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";
              
              return (
                <>
                  <div className="relative" style={{ width: "100%", maxWidth: safeSize, height: "auto" }}>
                    <QRCodeSVG 
                      value={originalValue} 
                      size={safeSize} 
                      bgColor={backgroundColor}
                      fgColor={foregroundColor}
                      // eslint-disable-next-line @typescript-eslint/no-explicit-any
                      level={errorCorrectionLevel as any}
                      marginSize={margin}
                      style={{ width: "100%", height: "auto" }}
                      imageSettings={logoUrl ? { src: transparentPixel, height: logoSize, width: logoSize, excavate: true } : undefined}
                    />
                    
                    {/* Absolute HTML Overlay for the Logo */}
                    {logoUrl && (
                      <div className="absolute inset-0 flex items-center justify-center pointer-events-none">
                        {!imageError ? (
                          <img 
                            src={logoUrl} 
                            alt="QR Logo" 
                            style={{ width: logoSize, height: logoSize, objectFit: "contain" }}
                            onError={() => setImageError(true)}
                          />
                        ) : (
                          <QrCode style={{ width: logoSize, height: logoSize, color: foregroundColor }} />
                        )}
                      </div>
                    )}
                  </div>

                  {labelText && (
                    <div 
                      className="mt-4 text-center w-full"
                      style={{ 
                        color: foregroundColor,
                        fontFamily: labelFont || "inherit",
                        fontSize: "16px",
                        fontWeight: 600,
                        maxWidth: safeSize
                      }}
                    >
                      {labelText}
                    </div>
                  )}
                </>
              );
            })()}
          </>
        ) : (
          <div className="flex items-center justify-center" style={{ width: 200, height: 200, backgroundColor }}>
            <QrCode className="h-24 w-24 opacity-30" style={{ color: foregroundColor }} />
          </div>
        )}
      </div>
      <p className="mt-6 text-sm font-medium text-muted-foreground text-center">
        Live Preview
      </p>
      <p className="text-xs text-muted-foreground/70 text-center mt-1">
        Size: {size}px
      </p>
    </Card>
  );
}
