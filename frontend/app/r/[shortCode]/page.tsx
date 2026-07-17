"use client";

import { useEffect, useState } from "react";
import { useParams } from "next/navigation";
import { qrService } from "@/lib/qr-service";
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";
import { Lock, AlertCircle, Loader2, FileText, Copy } from "lucide-react";
import { toast } from "sonner";

export default function RedirectPage() {
  const params = useParams();
  const shortCode = params.shortCode as string;

  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [isPasswordProtected, setIsPasswordProtected] = useState(false);
  const [password, setPassword] = useState("");
  const [isVerifying, setIsVerifying] = useState(false);
  const [scannedData, setScannedData] = useState<string | null>(null);

  const handleOriginalValue = (value: string) => {
    let urlToRedirect = value;
    
    // Intelligently prepend https:// if it looks like a domain without a protocol
    if (!/^[a-zA-Z][a-zA-Z\d+\-.]*:/.test(value) && value.includes('.') && !value.includes(' ')) {
      urlToRedirect = `https://${value}`;
    }

    if (
      urlToRedirect.startsWith("http://") || 
      urlToRedirect.startsWith("https://") || 
      urlToRedirect.startsWith("mailto:") || 
      urlToRedirect.startsWith("tel:") || 
      urlToRedirect.startsWith("sms:")
    ) {
      window.location.replace(urlToRedirect);
    } else {
      setScannedData(value);
      setIsLoading(false);
    }
  };

  useEffect(() => {
    if (!shortCode) return;

    const fetchQRDetails = async () => {
      try {
        const qrDetails = await qrService.getPublicQRCode(shortCode);
        
        if (qrDetails.passwordProtected) {
          setIsPasswordProtected(true);
          setIsLoading(false);
        } else {
          // If not password protected, verify with empty password to get URL and log scan
          const originalValue = await qrService.verifyPassword(shortCode, "");
          handleOriginalValue(originalValue);
        }
      } catch (err: any) {
        setIsLoading(false);
        setError(err.response?.data?.Message || "QR Code not found or inactive.");
      }
    };

    fetchQRDetails();
  }, [shortCode]);

  const handleVerify = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!password) {
      toast.error("Please enter a password");
      return;
    }

    setIsVerifying(true);
    try {
      const originalValue = await qrService.verifyPassword(shortCode, password);
      // Successful verification, handle value
      handleOriginalValue(originalValue);
    } catch (err: any) {
      toast.error(err.response?.data?.Message || "Invalid password");
      setPassword("");
    } finally {
      setIsVerifying(false);
    }
  };

  if (isLoading) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-muted/30">
        <Loader2 className="h-8 w-8 animate-spin text-primary" />
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-muted/30 p-4">
        <Card className="max-w-md w-full border-destructive/50">
          <CardHeader className="text-center">
            <div className="mx-auto bg-destructive/10 p-3 rounded-full w-12 h-12 flex items-center justify-center mb-4">
              <AlertCircle className="h-6 w-6 text-destructive" />
            </div>
            <CardTitle>Unable to Access</CardTitle>
            <CardDescription>{error}</CardDescription>
          </CardHeader>
        </Card>
      </div>
    );
  }

  if (isPasswordProtected) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-muted/30 p-4">
        <Card className="max-w-md w-full">
          <CardHeader className="text-center">
            <div className="mx-auto bg-primary/10 p-3 rounded-full w-12 h-12 flex items-center justify-center mb-4">
              <Lock className="h-6 w-6 text-primary" />
            </div>
            <CardTitle>Password Required</CardTitle>
            <CardDescription>
              This QR code destination is protected. Please enter the password to continue.
            </CardDescription>
          </CardHeader>
          <form onSubmit={handleVerify}>
            <CardContent className="space-y-4">
              <div className="space-y-2">
                <Label htmlFor="password">Password</Label>
                <Input
                  id="password"
                  type="password"
                  placeholder="Enter password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  autoFocus
                />
              </div>
            </CardContent>
            <CardFooter>
              <Button type="submit" className="w-full" disabled={isVerifying || !password}>
                {isVerifying ? (
                  <>
                    <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                    Verifying...
                  </>
                ) : (
                  "Access Content"
                )}
              </Button>
            </CardFooter>
          </form>
        </Card>
      </div>
    );
  }

  if (scannedData) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-muted/30 p-4">
        <Card className="max-w-md w-full">
          <CardHeader className="text-center pb-2">
            <div className="mx-auto bg-primary/10 p-3 rounded-full w-12 h-12 flex items-center justify-center mb-4">
              <FileText className="h-6 w-6 text-primary" />
            </div>
            <CardTitle>Scanned Content</CardTitle>
            <CardDescription>
              The QR code contained the following information.
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="bg-muted p-4 rounded-md whitespace-pre-wrap break-all font-mono text-sm max-h-64 overflow-y-auto">
              {scannedData}
            </div>
            <Button 
              className="w-full" 
              variant="outline" 
              onClick={() => {
                navigator.clipboard.writeText(scannedData);
                toast.success("Copied to clipboard!");
              }}
            >
              <Copy className="mr-2 h-4 w-4" /> Copy to Clipboard
            </Button>
          </CardContent>
        </Card>
      </div>
    );
  }

  return null;
}
