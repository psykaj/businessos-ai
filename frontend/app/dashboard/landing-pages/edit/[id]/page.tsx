"use client";

import { useState, useEffect, use } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useRouter } from "next/navigation";
import Link from "next/link";
import { toast } from "sonner";
import { 
  ArrowLeft, LayoutTemplate, Plus, Trash2, ArrowUp, ArrowDown, 
  Settings, CheckCircle, Save, Globe, Eye
} from "lucide-react";

import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button, buttonVariants } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { Textarea } from "@/components/ui/textarea";
import { Skeleton } from "@/components/ui/skeleton";
import { StatusBadge } from "@/components/ui/status-badge";

import { LandingPagesService, LandingPageDto, LandingPageSectionDto } from "@/lib/landing-pages-service";

const SECTION_TYPES = [
  { id: "Hero", name: "Hero Section", icon: LayoutTemplate },
  { id: "Features", name: "Features Grid", icon: LayoutTemplate },
  { id: "Pricing", name: "Pricing Table", icon: LayoutTemplate },
  { id: "CallToAction", name: "Call To Action", icon: LayoutTemplate },
];

const DEFAULT_CONTENT: Record<string, Record<string, unknown>> = {
  Hero: { title: "Welcome to our Platform", subtitle: "The best tool for your business", buttonText: "Get Started" },
  Features: { title: "Our Features", features: [{ title: "Fast", description: "Lightning fast performance" }] },
  Pricing: { title: "Simple Pricing", price: "$99/mo", plan: "Pro Plan" },
  CallToAction: { title: "Ready to start?", buttonText: "Sign Up Now" },
};

export default function PageBuilder({ params }: { params: Promise<{ id: string }> }) {
  const resolvedParams = use(params);
  const pageId = resolvedParams.id;
  const router = useRouter();
  const queryClient = useQueryClient();

  const [sections, setSections] = useState<Omit<LandingPageSectionDto, "id">[] & { id?: string }[]>([]);
  const [editingIndex, setEditingIndex] = useState<number | null>(null);
  const [isAddSectionOpen, setIsAddSectionOpen] = useState(false);

  const { data: page, isLoading, isError } = useQuery({
    queryKey: ["landing-page", pageId],
    queryFn: () => LandingPagesService.getPage(pageId),
  });

  useEffect(() => {
    if (page && page.sections) {
      // Sort sections by sortOrder
      const sorted = [...page.sections].sort((a, b) => a.sortOrder - b.sortOrder);
      setSections(sorted);
    }
  }, [page]);

  const updateMutation = useMutation({
    mutationFn: ({ status }: { status?: string }) => {
      // Ensure sortOrder is correct before saving
      const updatedSections = sections.map((s, idx) => ({ ...s, sortOrder: idx }));
      return LandingPagesService.updatePage(pageId, {
        status: status || page?.status,
        sections: updatedSections as unknown as LandingPageSectionDto[],
      });
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["landing-page", pageId] });
      queryClient.invalidateQueries({ queryKey: ["landing-pages"] });
      toast.success(data.status === "Published" ? "Page published successfully" : "Draft saved successfully");
    },
    onError: (error: Error | { response?: { data?: { message?: string } } }) => {
      const err = error as { response?: { data?: { message?: string } } };
      toast.error(err.response?.data?.message || "Failed to save page");
    },
  });

  const addSection = (type: string) => {
    setSections((prev) => [
      ...prev,
      {
        sectionType: type,
        sortOrder: prev.length,
        contentJson: JSON.stringify(DEFAULT_CONTENT[type] || {}),
      },
    ]);
    setIsAddSectionOpen(false);
  };

  const removeSection = (index: number) => {
    setSections((prev) => prev.filter((_, i) => i !== index));
    if (editingIndex === index) setEditingIndex(null);
  };

  const moveSection = (index: number, direction: "up" | "down") => {
    if (direction === "up" && index === 0) return;
    if (direction === "down" && index === sections.length - 1) return;

    setSections((prev) => {
      const newSections = [...prev];
      const targetIndex = direction === "up" ? index - 1 : index + 1;
      const temp = newSections[index];
      newSections[index] = newSections[targetIndex];
      newSections[targetIndex] = temp;
      
      // Update sortOrder
      newSections.forEach((s, i) => (s.sortOrder = i));
      
      if (editingIndex === index) setEditingIndex(targetIndex);
      else if (editingIndex === targetIndex) setEditingIndex(index);
      
      return newSections;
    });
  };

  const updateSectionContent = (index: number, content: Record<string, unknown>) => {
    setSections((prev) => {
      const newSections = [...prev];
      newSections[index].contentJson = JSON.stringify(content);
      return newSections;
    });
  };

  if (isLoading) return <div className="p-8"><Skeleton className="h-[600px] w-full" /></div>;
  if (isError || !page) return <div className="p-8 text-destructive">Failed to load page.</div>;

  const currentEditingSection = editingIndex !== null ? sections[editingIndex] : null;
  const parsedContent = currentEditingSection ? JSON.parse(currentEditingSection.contentJson || "{}") : {};

  return (
    <div className="flex flex-col h-[calc(100vh-100px)] overflow-hidden max-w-[1600px] mx-auto gap-4">
      {/* Header */}
      <div className="flex flex-col sm:flex-row items-start sm:items-center justify-between gap-4 shrink-0 px-2">
        <div className="flex items-center gap-4">
          <Link href="/dashboard/landing-pages" className={buttonVariants({ variant: "ghost", size: "icon" })}>
            <ArrowLeft className="h-4 w-4" />
          </Link>
          <div>
            <div className="flex items-center gap-3">
              <h1 className="text-2xl font-bold tracking-tight">{page.title}</h1>
              {page.status === "Published" ? (
                <StatusBadge status="Published" />
              ) : (
                <StatusBadge status="Draft" />
              )}
            </div>
            <p className="text-xs text-muted-foreground font-mono mt-1 flex items-center">
              <Globe className="h-3 w-3 mr-1" /> /{page.slug}
            </p>
          </div>
        </div>
        <div className="flex gap-2">
          <Button 
            variant="outline" 
            onClick={() => updateMutation.mutate({ status: "Draft" })}
            disabled={updateMutation.isPending}
          >
            <Save className="mr-2 h-4 w-4" /> Save Draft
          </Button>
          <Button 
            onClick={() => updateMutation.mutate({ status: "Published" })}
            disabled={updateMutation.isPending}
          >
            <CheckCircle className="mr-2 h-4 w-4" /> Publish
          </Button>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-12 gap-6 flex-1 min-h-0">
        
        {/* Left Sidebar: Structure */}
        <div className="lg:col-span-3 flex flex-col gap-4 overflow-y-auto pr-2 pb-10 custom-scrollbar">
          <Card>
            <CardHeader className="pb-3">
              <CardTitle className="text-base flex justify-between items-center">
                Page Structure
                <Dialog open={isAddSectionOpen} onOpenChange={setIsAddSectionOpen}>
                  <DialogTrigger render={<Button size="sm" variant="outline" className="h-7 px-2"><Plus className="h-3.5 w-3.5 mr-1" /> Add</Button>} />
                  <DialogContent>
                    <DialogHeader>
                      <DialogTitle>Add Section</DialogTitle>
                      <DialogDescription>Choose a pre-built section to add to your page.</DialogDescription>
                    </DialogHeader>
                    <div className="grid grid-cols-2 gap-3 py-4">
                      {SECTION_TYPES.map((type) => (
                        <Button 
                          key={type.id} 
                          variant="outline" 
                          className="h-auto py-4 flex flex-col gap-2 justify-center"
                          onClick={() => addSection(type.id)}
                        >
                          <type.icon className="h-6 w-6 text-primary" />
                          <span>{type.name}</span>
                        </Button>
                      ))}
                    </div>
                  </DialogContent>
                </Dialog>
              </CardTitle>
            </CardHeader>
            <CardContent className="space-y-2">
              {sections.length === 0 ? (
                <div className="text-center p-4 border border-dashed rounded text-xs text-muted-foreground">
                  No sections added yet.
                </div>
              ) : (
                sections.map((section, idx) => (
                  <div 
                    key={idx}
                    className={`flex items-center justify-between p-2 rounded border cursor-pointer transition-colors ${
                      editingIndex === idx ? "border-primary bg-primary/5" : "hover:bg-muted"
                    }`}
                    onClick={() => setEditingIndex(idx)}
                  >
                    <div className="flex items-center gap-2 overflow-hidden">
                      <span className="bg-muted text-muted-foreground w-5 h-5 rounded-full flex items-center justify-center text-[10px] shrink-0">
                        {idx + 1}
                      </span>
                      <span className="font-medium text-sm truncate">{section.sectionType}</span>
                    </div>
                    <div className="flex items-center gap-0.5 shrink-0">
                      <Button variant="ghost" size="icon" className="h-6 w-6" onClick={(e) => { e.stopPropagation(); moveSection(idx, "up"); }} disabled={idx === 0}>
                        <ArrowUp className="h-3 w-3" />
                      </Button>
                      <Button variant="ghost" size="icon" className="h-6 w-6" onClick={(e) => { e.stopPropagation(); moveSection(idx, "down"); }} disabled={idx === sections.length - 1}>
                        <ArrowDown className="h-3 w-3" />
                      </Button>
                      <Button variant="ghost" size="icon" className="h-6 w-6 text-destructive" onClick={(e) => { e.stopPropagation(); removeSection(idx); }}>
                        <Trash2 className="h-3 w-3" />
                      </Button>
                    </div>
                  </div>
                ))
              )}
            </CardContent>
          </Card>

          {/* Section Settings Editor */}
          {currentEditingSection && (
            <Card className="flex-1 min-h-[400px]">
              <CardHeader className="pb-3 border-b">
                <CardTitle className="text-base flex items-center gap-2">
                  <Settings className="h-4 w-4 text-primary" />
                  Edit {currentEditingSection.sectionType}
                </CardTitle>
              </CardHeader>
              <CardContent className="py-4 space-y-4">
                {/* Dynamically render form fields based on the section type's JSON content */}
                {Object.keys(parsedContent).map((key) => {
                  const val = parsedContent[key];
                  if (typeof val === "string") {
                    return (
                      <div key={key} className="space-y-1.5">
                        <Label className="capitalize">{key.replace(/([A-Z])/g, ' $1').trim()}</Label>
                        {val.length > 50 ? (
                          <Textarea 
                            value={val} 
                            onChange={(e) => updateSectionContent(editingIndex!, { ...parsedContent, [key]: e.target.value })}
                            rows={3}
                          />
                        ) : (
                          <Input 
                            value={val} 
                            onChange={(e) => updateSectionContent(editingIndex!, { ...parsedContent, [key]: e.target.value })}
                          />
                        )}
                      </div>
                    );
                  }
                  // Skip complex arrays (like features) in this simplified editor for brevity
                  return null;
                })}
              </CardContent>
            </Card>
          )}
        </div>

        {/* Right Area: Live Preview */}
        <div className="lg:col-span-9 bg-muted/30 rounded-xl border overflow-hidden flex flex-col relative shadow-inner">
          <div className="absolute top-0 inset-x-0 h-10 bg-muted/80 backdrop-blur-md border-b flex items-center px-4 justify-between z-10">
            <div className="flex gap-1.5">
              <div className="w-3 h-3 rounded-full bg-red-400"></div>
              <div className="w-3 h-3 rounded-full bg-amber-400"></div>
              <div className="w-3 h-3 rounded-full bg-emerald-400"></div>
            </div>
            <div className="bg-background px-6 py-1 rounded-full text-xs font-mono text-muted-foreground border">
              yourdomain.com/{page.slug}
            </div>
            <div className="w-10"></div>
          </div>
          
          <div className="flex-1 overflow-y-auto mt-10 custom-scrollbar relative bg-white dark:bg-zinc-950">
            {sections.length === 0 ? (
              <div className="h-full flex items-center justify-center text-muted-foreground flex-col gap-2">
                <Eye className="h-8 w-8 opacity-20" />
                <p>Add sections to preview your page</p>
              </div>
            ) : (
              <div className="flex flex-col">
                {sections.map((section, idx) => {
                  const content = JSON.parse(section.contentJson || "{}");
                  const isActive = editingIndex === idx;
                  
                  return (
                    <div 
                      key={idx} 
                      className={`relative group transition-all ${isActive ? 'ring-2 ring-primary ring-inset z-10' : 'hover:ring-1 hover:ring-primary/50 hover:z-0'}`}
                      onClick={() => setEditingIndex(idx)}
                    >
                      {isActive && (
                        <div className="absolute top-0 left-0 bg-primary text-primary-foreground text-[10px] font-bold px-2 py-0.5 rounded-br z-20">
                          {section.sectionType}
                        </div>
                      )}
                      
                      {/* --- Mock Renderer for Previews --- */}
                      {section.sectionType === "Hero" && (
                        <div className="py-24 px-6 text-center border-b">
                          <h1 className="text-4xl md:text-5xl font-extrabold tracking-tight mb-4">{content.title}</h1>
                          <p className="text-lg text-muted-foreground max-w-2xl mx-auto mb-8">{content.subtitle}</p>
                          <Button size="lg" className="rounded-full">{content.buttonText || "Click Here"}</Button>
                        </div>
                      )}

                      {section.sectionType === "Features" && (
                        <div className="py-20 px-6 bg-muted/20 border-b">
                          <h2 className="text-3xl font-bold text-center mb-12">{content.title}</h2>
                          <div className="grid grid-cols-1 md:grid-cols-3 gap-8 max-w-5xl mx-auto">
                            {[1, 2, 3].map(i => (
                              <div key={i} className="bg-background p-6 rounded-xl border shadow-sm">
                                <div className="h-10 w-10 bg-primary/10 rounded-lg mb-4"></div>
                                <h3 className="font-bold mb-2">Feature {i}</h3>
                                <p className="text-sm text-muted-foreground">Description of this amazing feature.</p>
                              </div>
                            ))}
                          </div>
                        </div>
                      )}

                      {section.sectionType === "Pricing" && (
                        <div className="py-20 px-6 text-center border-b">
                          <h2 className="text-3xl font-bold mb-12">{content.title}</h2>
                          <div className="inline-block border rounded-2xl p-8 bg-background shadow-lg max-w-sm w-full relative overflow-hidden">
                            <div className="absolute top-0 inset-x-0 h-1 bg-primary"></div>
                            <h3 className="font-bold text-xl mb-2">{content.plan}</h3>
                            <p className="text-4xl font-extrabold mb-6">{content.price}</p>
                            <Button className="w-full">Choose Plan</Button>
                          </div>
                        </div>
                      )}

                      {section.sectionType === "CallToAction" && (
                        <div className="py-20 px-6 text-center bg-primary text-primary-foreground">
                          <h2 className="text-3xl font-bold mb-8">{content.title}</h2>
                          <Button variant="secondary" size="lg" className="rounded-full">{content.buttonText}</Button>
                        </div>
                      )}
                    </div>
                  );
                })}
              </div>
            )}
          </div>
        </div>
        
      </div>
    </div>
  );
}
