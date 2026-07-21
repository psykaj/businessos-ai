"use client";

import { useCreateCampaign } from "@/hooks/use-campaigns";
import { useRouter } from "next/navigation";
import { toast } from "sonner";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Card, CardContent } from "@/components/ui/card";
import { useState } from "react";

export default function CreateCampaignPage() {
  const router = useRouter();
  const createCampaign = useCreateCampaign();
  
  const [formData, setFormData] = useState({
    name: "",
    campaignType: "Email",
    source: "",
    medium: "",
    budget: 0,
    status: "Draft",
    startDate: new Date().toISOString().split("T")[0],
    endDate: "",
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: name === "budget" ? Number(value) : value }));
  };

  const handleSelect = (name: string, value: string) => {
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    createCampaign.mutate(formData, {
      onSuccess: () => {
        toast.success("Campaign created successfully");
        router.push("/dashboard/campaigns");
      },
      onError: (err: unknown) => {
        toast.error((err as Error)?.message || "Failed to create campaign");
      }
    });
  };

  return (
    <div className="max-w-3xl mx-auto flex flex-col gap-6 p-8">
      <div>
        <h1 className="text-3xl font-bold tracking-tight">Create Campaign</h1>
        <p className="text-muted-foreground mt-1">Set up a new marketing initiative to track performance.</p>
      </div>

      <Card>
        <CardContent className="p-6">
          <form onSubmit={handleSubmit} className="space-y-6">
            <div className="space-y-2">
              <Label htmlFor="name">Campaign Name</Label>
              <Input 
                id="name" 
                name="name" 
                required 
                placeholder="e.g. Q3 Summer Sale" 
                value={formData.name}
                onChange={handleChange}
              />
            </div>
            
            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label>Type</Label>
                <Select value={formData.campaignType} onValueChange={(v) => handleSelect("campaignType", v as string)}>
                  <SelectTrigger>
                    <SelectValue />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="Email">Email</SelectItem>
                    <SelectItem value="Social">Social</SelectItem>
                    <SelectItem value="Search">Search</SelectItem>
                    <SelectItem value="Display">Display</SelectItem>
                    <SelectItem value="Direct">Direct</SelectItem>
                    <SelectItem value="Other">Other</SelectItem>
                  </SelectContent>
                </Select>
              </div>

              <div className="space-y-2">
                <Label htmlFor="budget">Budget ($)</Label>
                <Input 
                  id="budget" 
                  name="budget" 
                  type="number" 
                  min="0"
                  required 
                  value={formData.budget}
                  onChange={handleChange}
                />
              </div>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="source">UTM Source</Label>
                <Input 
                  id="source" 
                  name="source" 
                  placeholder="e.g. google, facebook, newsletter" 
                  value={formData.source}
                  onChange={handleChange}
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="medium">UTM Medium</Label>
                <Input 
                  id="medium" 
                  name="medium" 
                  placeholder="e.g. cpc, banner, email" 
                  value={formData.medium}
                  onChange={handleChange}
                />
              </div>
            </div>
            
            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="startDate">Start Date</Label>
                <Input 
                  id="startDate" 
                  name="startDate" 
                  type="date"
                  required 
                  value={formData.startDate}
                  onChange={handleChange}
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="endDate">End Date (Optional)</Label>
                <Input 
                  id="endDate" 
                  name="endDate" 
                  type="date"
                  value={formData.endDate}
                  onChange={handleChange}
                />
              </div>
            </div>

            <div className="pt-4 flex justify-end gap-2 border-t">
              <Button type="button" variant="outline" onClick={() => router.back()}>Cancel</Button>
              <Button type="submit" disabled={createCampaign.isPending}>
                {createCampaign.isPending ? "Creating..." : "Create Campaign"}
              </Button>
            </div>
          </form>
        </CardContent>
      </Card>
    </div>
  );
}
