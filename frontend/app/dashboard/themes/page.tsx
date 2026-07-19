"use client";

import { useState, useEffect } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { toast } from "sonner";
import { Paintbrush, Plus, CheckCircle2, Trash2, LayoutTemplate } from "lucide-react";

import { Card, CardContent, CardDescription, CardHeader, CardTitle, CardFooter } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Skeleton } from "@/components/ui/skeleton";

import { ThemesService, ThemeDto, CreateThemeDto, UpdateThemeDto } from "@/lib/themes-service";

const themeConfigSchema = z.object({
  primaryColor: z.string(),
  secondaryColor: z.string(),
  accentColor: z.string(),
  borderRadius: z.string(),
  buttonStyle: z.string(),
  cardStyle: z.string(),
});

const themeSchema = z.object({
  themeName: z.string().min(2, "Name must be at least 2 characters"),
  themeMode: z.enum(["Light", "Dark", "Custom"]),
  config: themeConfigSchema,
});

type ThemeFormValues = z.infer<typeof themeSchema>;

const DEFAULT_CONFIG = {
  primaryColor: "#0ea5e9",
  secondaryColor: "#f1f5f9",
  accentColor: "#f59e0b",
  borderRadius: "0.5rem",
  buttonStyle: "solid",
  cardStyle: "shadow",
};

export default function ThemesPage() {
  const queryClient = useQueryClient();
  const [isAddOpen, setIsAddOpen] = useState(false);
  const [editingThemeId, setEditingThemeId] = useState<string | null>(null);

  const { data: themes, isLoading } = useQuery({
    queryKey: ["themes"],
    queryFn: ThemesService.getThemes,
  });

  const activeTheme = themes?.find((t) => t.id === editingThemeId) || themes?.find((t) => t.isDefault) || themes?.[0];

  const addMutation = useMutation({
    mutationFn: (data: CreateThemeDto) => ThemesService.createTheme(data),
    onSuccess: (newTheme) => {
      queryClient.invalidateQueries({ queryKey: ["themes"] });
      toast.success("Theme created");
      setIsAddOpen(false);
      form.reset();
      setEditingThemeId(newTheme.id);
    },
    onError: (error: Error | { response?: { data?: { message?: string } } }) => {
      const err = error as { response?: { data?: { message?: string } } };
      toast.error(err.response?.data?.message || "Failed to create theme");
    },
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateThemeDto }) => ThemesService.updateTheme(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["themes"] });
      toast.success("Theme updated");
    },
    onError: (error: Error | { response?: { data?: { message?: string } } }) => {
      const err = error as { response?: { data?: { message?: string } } };
      toast.error(err.response?.data?.message || "Failed to update theme");
    },
  });

  const defaultMutation = useMutation({
    mutationFn: (id: string) => ThemesService.setDefault(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["themes"] });
      toast.success("Default theme updated");
    },
    onError: (error: Error | { response?: { data?: { message?: string } } }) => {
      const err = error as { response?: { data?: { message?: string } } };
      toast.error(err.response?.data?.message || "Failed to set default theme");
    },
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => ThemesService.deleteTheme(id),
    onSuccess: (_, id) => {
      queryClient.invalidateQueries({ queryKey: ["themes"] });
      toast.success("Theme deleted");
      if (editingThemeId === id) setEditingThemeId(null);
    },
    onError: (error: Error | { response?: { data?: { message?: string } } }) => {
      const err = error as { response?: { data?: { message?: string } } };
      toast.error(err.response?.data?.message || "Failed to delete theme");
    },
  });

  const form = useForm<ThemeFormValues>({
    resolver: zodResolver(themeSchema),
    defaultValues: {
      themeName: "",
      themeMode: "Light",
      config: DEFAULT_CONFIG,
    },
  });

  const editForm = useForm<ThemeFormValues>({
    resolver: zodResolver(themeSchema),
  });

  // Load active theme into edit form
  useEffect(() => {
    if (activeTheme) {
      try {
        const config = JSON.parse(activeTheme.themeJson);
        editForm.reset({
          themeName: activeTheme.themeName,
          themeMode: activeTheme.themeMode as "Light" | "Dark" | "Custom",
          config: { ...DEFAULT_CONFIG, ...config },
        });
      } catch {
        editForm.reset({
          themeName: activeTheme.themeName,
          themeMode: activeTheme.themeMode as "Light" | "Dark" | "Custom",
          config: DEFAULT_CONFIG,
        });
      }
    }
  }, [activeTheme, editForm]);

  const onAddSubmit = (values: ThemeFormValues) => {
    addMutation.mutate({
      themeName: values.themeName,
      themeMode: values.themeMode,
      themeJson: JSON.stringify(values.config),
    });
  };

  const onEditSubmit = (values: ThemeFormValues) => {
    if (!activeTheme) return;
    updateMutation.mutate({
      id: activeTheme.id,
      data: {
        themeName: values.themeName,
        themeMode: values.themeMode,
        themeJson: JSON.stringify(values.config),
      },
    });
  };

  const watchConfig = editForm.watch("config");

  return (
    <div className="flex flex-col gap-6 max-w-[1400px] mx-auto">
      <div className="flex flex-col sm:flex-row items-start sm:items-center justify-between gap-4">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Theme Engine</h1>
          <p className="text-muted-foreground mt-1">Design and manage multiple UI themes for your platform.</p>
        </div>
        <Dialog open={isAddOpen} onOpenChange={setIsAddOpen}>
          <DialogTrigger render={<Button><Plus className="mr-2 h-4 w-4" />Create Theme</Button>} />
          <DialogContent>
            <DialogHeader>
              <DialogTitle>Create New Theme</DialogTitle>
              <DialogDescription>
                Start with a base theme. You can fully customize it later.
              </DialogDescription>
            </DialogHeader>
            <form onSubmit={form.handleSubmit(onAddSubmit)} className="space-y-4">
              <div className="space-y-2">
                <Label htmlFor="themeName">Theme Name</Label>
                <Input id="themeName" placeholder="e.g. Cyberpunk Dark" {...form.register("themeName")} />
                {form.formState.errors.themeName && (
                  <p className="text-xs text-destructive">{form.formState.errors.themeName.message}</p>
                )}
              </div>
              <div className="space-y-2">
                <Label htmlFor="themeMode">Base Mode</Label>
                <Select
                  value={form.watch("themeMode")}
                  onValueChange={(v) => form.setValue("themeMode", v as "Light" | "Dark" | "Custom", { shouldValidate: true })}
                >
                  <SelectTrigger>
                    <SelectValue placeholder="Select mode" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="Light">Light Mode</SelectItem>
                    <SelectItem value="Dark">Dark Mode</SelectItem>
                    <SelectItem value="Custom">Custom</SelectItem>
                  </SelectContent>
                </Select>
              </div>
              <DialogFooter>
                <Button type="button" variant="outline" onClick={() => setIsAddOpen(false)}>Cancel</Button>
                <Button type="submit" disabled={addMutation.isPending}>
                  {addMutation.isPending ? "Creating..." : "Create Theme"}
                </Button>
              </DialogFooter>
            </form>
          </DialogContent>
        </Dialog>
      </div>

      <div className="grid grid-cols-1 xl:grid-cols-4 gap-6">
        {/* Sidebar: Theme List */}
        <div className="xl:col-span-1 space-y-4">
          <h3 className="font-semibold text-lg flex items-center gap-2">
            <Paintbrush className="h-5 w-5 text-primary" />
            Your Themes
          </h3>
          {isLoading ? (
            <div className="space-y-2">
              <Skeleton className="h-16 w-full" />
              <Skeleton className="h-16 w-full" />
            </div>
          ) : themes && themes.length > 0 ? (
            <div className="space-y-2">
              {themes.map((theme) => (
                <div 
                  key={theme.id}
                  onClick={() => setEditingThemeId(theme.id)}
                  className={`p-3 rounded-lg border cursor-pointer transition-all ${
                    activeTheme?.id === theme.id ? "border-primary bg-primary/5 ring-1 ring-primary/20" : "hover:border-foreground/20 hover:bg-muted/50"
                  }`}
                >
                  <div className="flex justify-between items-start">
                    <div>
                      <h4 className="font-medium text-sm flex items-center gap-2">
                        {theme.themeName}
                        {theme.isDefault && <CheckCircle2 className="h-3.5 w-3.5 text-emerald-500" />}
                      </h4>
                      <p className="text-xs text-muted-foreground capitalize">{theme.themeMode} Mode</p>
                    </div>
                    <div className="flex gap-1">
                      {theme.isDefault ? (
                        <span className="text-[10px] bg-emerald-100 text-emerald-700 dark:bg-emerald-900/30 dark:text-emerald-400 px-2 py-0.5 rounded-full font-medium">Active</span>
                      ) : (
                        <Button 
                          variant="ghost" 
                          size="icon" 
                          className="h-6 w-6 text-muted-foreground hover:text-destructive"
                          onClick={(e) => {
                            e.stopPropagation();
                            if (window.confirm("Delete this theme?")) deleteMutation.mutate(theme.id);
                          }}
                        >
                          <Trash2 className="h-3 w-3" />
                        </Button>
                      )}
                    </div>
                  </div>
                </div>
              ))}
            </div>
          ) : (
            <div className="text-center p-6 border-2 border-dashed rounded-lg border-muted text-muted-foreground text-sm">
              No themes available. Create one to get started.
            </div>
          )}
        </div>

        {/* Main Content: Theme Editor & Preview */}
        <div className="xl:col-span-3">
          {activeTheme ? (
            <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
              {/* Editor */}
              <Card>
                <CardHeader>
                  <CardTitle>Edit {activeTheme.themeName}</CardTitle>
                  <CardDescription>Adjust tokens to instantly see changes in the preview.</CardDescription>
                </CardHeader>
                <CardContent>
                  <form id="theme-edit-form" onSubmit={editForm.handleSubmit(onEditSubmit)} className="space-y-6">
                    <div className="grid grid-cols-2 gap-4">
                      <div className="space-y-2">
                        <Label>Theme Name</Label>
                        <Input {...editForm.register("themeName")} />
                      </div>
                      <div className="space-y-2">
                        <Label>Mode</Label>
                        <Select
                          value={editForm.watch("themeMode")}
                          onValueChange={(v) => editForm.setValue("themeMode", v as "Light" | "Dark" | "Custom", { shouldValidate: true })}
                        >
                          <SelectTrigger>
                            <SelectValue />
                          </SelectTrigger>
                          <SelectContent>
                            <SelectItem value="Light">Light</SelectItem>
                            <SelectItem value="Dark">Dark</SelectItem>
                            <SelectItem value="Custom">Custom</SelectItem>
                          </SelectContent>
                        </Select>
                      </div>
                    </div>

                    <div className="space-y-4">
                      <h4 className="text-sm font-semibold border-b pb-2">Colors</h4>
                      <div className="grid grid-cols-2 gap-4">
                        <div className="space-y-2">
                          <Label>Primary Color</Label>
                          <div className="flex gap-2">
                            <Input type="color" className="w-12 h-10 p-1 cursor-pointer" {...editForm.register("config.primaryColor")} />
                            <Input {...editForm.register("config.primaryColor")} />
                          </div>
                        </div>
                        <div className="space-y-2">
                          <Label>Background (Secondary)</Label>
                          <div className="flex gap-2">
                            <Input type="color" className="w-12 h-10 p-1 cursor-pointer" {...editForm.register("config.secondaryColor")} />
                            <Input {...editForm.register("config.secondaryColor")} />
                          </div>
                        </div>
                        <div className="space-y-2">
                          <Label>Accent Color</Label>
                          <div className="flex gap-2">
                            <Input type="color" className="w-12 h-10 p-1 cursor-pointer" {...editForm.register("config.accentColor")} />
                            <Input {...editForm.register("config.accentColor")} />
                          </div>
                        </div>
                      </div>
                    </div>

                    <div className="space-y-4">
                      <h4 className="text-sm font-semibold border-b pb-2">Layout & Components</h4>
                      <div className="grid grid-cols-2 gap-4">
                        <div className="space-y-2">
                          <Label>Border Radius</Label>
                          <Select
                            value={watchConfig?.borderRadius}
                            onValueChange={(v) => editForm.setValue("config.borderRadius", v || "0.5rem")}
                          >
                            <SelectTrigger>
                              <SelectValue />
                            </SelectTrigger>
                            <SelectContent>
                              <SelectItem value="0">0px (Square)</SelectItem>
                              <SelectItem value="0.3rem">Small</SelectItem>
                              <SelectItem value="0.5rem">Medium</SelectItem>
                              <SelectItem value="1rem">Large</SelectItem>
                              <SelectItem value="9999px">Pill</SelectItem>
                            </SelectContent>
                          </Select>
                        </div>
                        <div className="space-y-2">
                          <Label>Card Style</Label>
                          <Select
                            value={watchConfig?.cardStyle}
                            onValueChange={(v) => editForm.setValue("config.cardStyle", v || "Flat")}
                          >
                            <SelectTrigger>
                              <SelectValue />
                            </SelectTrigger>
                            <SelectContent>
                              <SelectItem value="flat">Flat</SelectItem>
                              <SelectItem value="border">Bordered</SelectItem>
                              <SelectItem value="shadow">Shadow</SelectItem>
                              <SelectItem value="glass">Glassmorphism</SelectItem>
                            </SelectContent>
                          </Select>
                        </div>
                      </div>
                    </div>
                  </form>
                </CardContent>
                <CardFooter className="flex justify-between border-t p-6">
                  {!activeTheme.isDefault ? (
                    <Button 
                      variant="outline" 
                      onClick={() => defaultMutation.mutate(activeTheme.id)}
                      disabled={defaultMutation.isPending}
                    >
                      Set as Default Theme
                    </Button>
                  ) : (
                    <div className="text-sm text-muted-foreground flex items-center gap-1">
                      <CheckCircle2 className="h-4 w-4 text-emerald-500" /> Currently Active Theme
                    </div>
                  )}
                  <Button type="submit" form="theme-edit-form" disabled={!editForm.formState.isDirty || updateMutation.isPending}>
                    {updateMutation.isPending ? "Saving..." : "Save Changes"}
                  </Button>
                </CardFooter>
              </Card>

              {/* Preview */}
              <div>
                <div className="sticky top-6 space-y-4">
                  <h3 className="text-lg font-medium flex items-center gap-2">
                    <LayoutTemplate className="h-5 w-5 text-muted-foreground" />
                    Visual Preview
                  </h3>
                  
                  {/* The Preview UI */}
                  <div 
                    className="p-6 rounded-xl transition-all duration-300 border h-[550px] overflow-hidden"
                    style={{ 
                      backgroundColor: watchConfig?.secondaryColor || "#fff",
                      color: editForm.watch("themeMode") === "Dark" ? "#fff" : "#000"
                    }}
                  >
                    <div className="flex justify-between items-center mb-8">
                      <h4 className="font-bold text-xl tracking-tight" style={{ color: watchConfig?.primaryColor }}>Dashboard</h4>
                      <div className="flex gap-2">
                        <div className="w-8 h-8 rounded-full bg-muted/20 border" />
                      </div>
                    </div>

                    <div className="grid grid-cols-2 gap-4 mb-6">
                      <div 
                        className={`p-4 transition-all duration-300 ${
                          watchConfig?.cardStyle === "shadow" ? "shadow-md bg-white border-transparent" :
                          watchConfig?.cardStyle === "border" ? "border-2 bg-transparent" :
                          watchConfig?.cardStyle === "glass" ? "bg-white/40 backdrop-blur-md border border-white/20 shadow-lg" :
                          "bg-black/5 border-transparent" // flat
                        }`}
                        style={{ borderRadius: watchConfig?.borderRadius }}
                      >
                        <p className="text-xs opacity-70 mb-1">Total Revenue</p>
                        <p className="text-2xl font-bold" style={{ color: watchConfig?.primaryColor }}>$45,231</p>
                      </div>
                      <div 
                        className={`p-4 transition-all duration-300 ${
                          watchConfig?.cardStyle === "shadow" ? "shadow-md bg-white border-transparent" :
                          watchConfig?.cardStyle === "border" ? "border-2 bg-transparent" :
                          watchConfig?.cardStyle === "glass" ? "bg-white/40 backdrop-blur-md border border-white/20 shadow-lg" :
                          "bg-black/5 border-transparent" // flat
                        }`}
                        style={{ borderRadius: watchConfig?.borderRadius }}
                      >
                        <p className="text-xs opacity-70 mb-1">Active Users</p>
                        <p className="text-2xl font-bold" style={{ color: watchConfig?.primaryColor }}>+2,350</p>
                      </div>
                    </div>

                    <div 
                      className={`p-6 mb-6 transition-all duration-300 ${
                        watchConfig?.cardStyle === "shadow" ? "shadow-md bg-white border-transparent" :
                        watchConfig?.cardStyle === "border" ? "border-2 bg-transparent" :
                        watchConfig?.cardStyle === "glass" ? "bg-white/40 backdrop-blur-md border border-white/20 shadow-lg" :
                        "bg-black/5 border-transparent" // flat
                      }`}
                      style={{ borderRadius: watchConfig?.borderRadius }}
                    >
                      <h5 className="font-semibold mb-4">Quick Actions</h5>
                      <div className="flex gap-3 flex-wrap">
                        <button 
                          className="px-4 py-2 font-medium text-sm transition-all"
                          style={{ 
                            backgroundColor: watchConfig?.primaryColor, 
                            color: "#fff",
                            borderRadius: watchConfig?.borderRadius 
                          }}
                        >
                          Primary Action
                        </button>
                        <button 
                          className="px-4 py-2 font-medium text-sm transition-all"
                          style={{ 
                            backgroundColor: watchConfig?.accentColor, 
                            color: "#fff",
                            borderRadius: watchConfig?.borderRadius 
                          }}
                        >
                          Accent Button
                        </button>
                        <button 
                          className="px-4 py-2 font-medium text-sm border-2 transition-all bg-transparent"
                          style={{ 
                            borderColor: watchConfig?.primaryColor,
                            color: watchConfig?.primaryColor,
                            borderRadius: watchConfig?.borderRadius 
                          }}
                        >
                          Outline Button
                        </button>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          ) : (
            <div className="h-full flex items-center justify-center border-2 border-dashed rounded-lg p-12 text-muted-foreground">
              Select a theme from the sidebar or create a new one to start customizing.
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
