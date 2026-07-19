"use client";

import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Search, Upload, Trash2, Image as ImageIcon, Link as LinkIcon } from "lucide-react";
import { toast } from "sonner";
import { MediaService } from "@/lib/media-service";

interface MediaAsset {
  id: string;
  url: string;
  name: string;
  size: string;
  uploadedAt: string;
}

export default function MediaLibraryPage() {
  const [isUploading, setIsUploading] = useState(false);
  const [searchQuery, setSearchQuery] = useState("");
  const [assets, setAssets] = useState<MediaAsset[]>([]); // In a real scenario, this would be fetched from the backend

  const handleFileUpload = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (!file) return;

    setIsUploading(true);
    const toastId = toast.loading("Uploading asset...");
    
    try {
      const response = await MediaService.uploadMedia(file, "library");
      toast.success("Asset uploaded successfully", { id: toastId });
      
      const newAsset: MediaAsset = {
        id: Math.random().toString(36).substring(7),
        url: response.url,
        name: file.name,
        size: (file.size / 1024 / 1024).toFixed(2) + " MB",
        uploadedAt: new Date().toLocaleDateString(),
      };
      
      setAssets((prev) => [newAsset, ...prev]);
    } catch (err: unknown) {
      const error = err as { response?: { data?: { message?: string } } };
      toast.error(error.response?.data?.message || "Failed to upload asset", { id: toastId });
    } finally {
      setIsUploading(false);
      // Reset input
      event.target.value = "";
    }
  };

  const handleDelete = (id: string) => {
    setAssets((prev) => prev.filter((a) => a.id !== id));
    toast.success("Asset deleted");
  };

  const filteredAssets = assets.filter((a) => a.name.toLowerCase().includes(searchQuery.toLowerCase()));

  return (
    <div className="flex flex-col gap-6">
      <div className="flex flex-col sm:flex-row items-start sm:items-center justify-between gap-4">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Media Library</h1>
          <p className="text-muted-foreground mt-1">Manage all your uploaded images, logos, and assets.</p>
        </div>
        <div>
          <input 
            type="file" 
            id="media-upload" 
            className="hidden" 
            accept="image/*" 
            onChange={handleFileUpload}
            disabled={isUploading}
          />
          <Button onClick={() => document.getElementById("media-upload")?.click()} disabled={isUploading}>
            <Upload className="mr-2 h-4 w-4" />
            {isUploading ? "Uploading..." : "Upload Asset"}
          </Button>
        </div>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>All Assets</CardTitle>
          <CardDescription>View and manage your files.</CardDescription>
        </CardHeader>
        <CardContent>
          <div className="flex items-center gap-2 mb-6">
            <div className="relative flex-1 max-w-sm">
              <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
              <Input
                type="search"
                placeholder="Search assets..."
                className="pl-8"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
              />
            </div>
          </div>

          {filteredAssets.length === 0 ? (
            <div className="flex flex-col items-center justify-center py-12 text-center border-2 border-dashed rounded-lg border-muted">
              <ImageIcon className="h-12 w-12 text-muted-foreground/50 mb-4" />
              <h3 className="text-lg font-medium">No assets found</h3>
              <p className="text-sm text-muted-foreground mt-1 mb-4">
                {searchQuery ? "Try a different search term" : "Upload your first image to get started"}
              </p>
              {!searchQuery && (
                <Button variant="outline" onClick={() => document.getElementById("media-upload")?.click()}>
                  Upload Now
                </Button>
              )}
            </div>
          ) : (
            <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-4">
              {filteredAssets.map((asset) => (
                <div key={asset.id} className="group relative rounded-xl border bg-card text-card-foreground shadow-sm overflow-hidden flex flex-col">
                  <div className="aspect-square bg-muted/30 relative flex items-center justify-center p-2">
                    {/* eslint-disable-next-line @next/next/no-img-element */}
                    <img src={asset.url} alt={asset.name} className="max-h-full max-w-full object-contain" />
                    
                    <div className="absolute inset-0 bg-black/60 opacity-0 group-hover:opacity-100 transition-opacity flex items-center justify-center gap-2">
                      <Button size="icon" variant="secondary" className="h-8 w-8 rounded-full" onClick={() => {
                        navigator.clipboard.writeText(asset.url);
                        toast.success("URL copied to clipboard");
                      }}>
                        <LinkIcon className="h-4 w-4" />
                      </Button>
                      <Button size="icon" variant="destructive" className="h-8 w-8 rounded-full" onClick={() => handleDelete(asset.id)}>
                        <Trash2 className="h-4 w-4" />
                      </Button>
                    </div>
                  </div>
                  <div className="p-3 text-xs flex flex-col gap-1 border-t">
                    <span className="font-medium truncate" title={asset.name}>{asset.name}</span>
                    <div className="flex justify-between text-muted-foreground">
                      <span>{asset.size}</span>
                      <span>{asset.uploadedAt}</span>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
