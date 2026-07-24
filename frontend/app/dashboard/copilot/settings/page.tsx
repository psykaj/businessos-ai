"use client";

import { useState } from "react";
import { CopilotNav } from "@/components/copilot/CopilotNav";
import { Settings, Shield, Sliders, Bell, Sparkles, Check } from "lucide-react";
import { toast } from "sonner";

export default function CopilotSettingsPage() {
  const [language, setLanguage] = useState("English");
  const [responseStyle, setResponseStyle] = useState("Concise");
  const [safetyLevel, setSafetyLevel] = useState("DestructiveOnly");
  const [enableSuggestions, setEnableSuggestions] = useState(true);
  const [enableNotifications, setEnableNotifications] = useState(true);

  const handleSaveSettings = () => {
    toast.success("AI Copilot preferences saved successfully.");
  };

  return (
    <div className="flex flex-col space-y-6 pb-12">
      <CopilotNav />

      <div className="px-6 space-y-6 max-w-4xl">
        <div>
          <h2 className="text-xl font-bold text-foreground flex items-center gap-2">
            AI Copilot Preferences & Safety Settings
            <Settings className="h-5 w-5 text-indigo-500" />
          </h2>
          <p className="text-xs text-muted-foreground">
            Configure how your AI Business Copilot interprets commands, handles safety confirmation, and issues suggestions.
          </p>
        </div>

        <div className="space-y-4">
          {/* General Preferences */}
          <div className="rounded-2xl border border-border/60 bg-card p-6 shadow-sm space-y-4">
            <div className="flex items-center gap-2 border-b border-border/50 pb-3">
              <Sliders className="h-4 w-4 text-indigo-500" />
              <h3 className="text-sm font-bold text-foreground">General Copilot Behavior</h3>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label className="block text-xs font-semibold text-foreground mb-1">
                  Preferred Business Language
                </label>
                <select
                  value={language}
                  onChange={(e) => setLanguage(e.target.value)}
                  className="w-full rounded-xl border border-border/60 bg-background px-3 py-2 text-xs text-foreground focus:outline-none"
                >
                  <option value="English">English</option>
                  <option value="Spanish">Spanish (Español)</option>
                  <option value="French">French (Français)</option>
                  <option value="German">German (Deutsch)</option>
                  <option value="Hindi">Hindi (हिन्दी)</option>
                </select>
              </div>

              <div>
                <label className="block text-xs font-semibold text-foreground mb-1">
                  Assistant Response Style
                </label>
                <select
                  value={responseStyle}
                  onChange={(e) => setResponseStyle(e.target.value)}
                  className="w-full rounded-xl border border-border/60 bg-background px-3 py-2 text-xs text-foreground focus:outline-none"
                >
                  <option value="Concise">Concise & Direct (Recommended)</option>
                  <option value="Detailed">Comprehensive & Detailed</option>
                  <option value="Analytical">Analytical & Metric-heavy</option>
                </select>
              </div>
            </div>
          </div>

          {/* Safety & Confirmation Layer */}
          <div className="rounded-2xl border border-border/60 bg-card p-6 shadow-sm space-y-4">
            <div className="flex items-center gap-2 border-b border-border/50 pb-3">
              <Shield className="h-4 w-4 text-amber-500" />
              <h3 className="text-sm font-bold text-foreground">Permission & Safety Threshold</h3>
            </div>

            <div className="space-y-3">
              <label className="block text-xs font-semibold text-foreground">
                Action Confirmation Mode
              </label>

              <div className="grid grid-cols-1 sm:grid-cols-3 gap-3">
                {[
                  { id: "DestructiveOnly", label: "Destructive Actions Only", desc: "Require confirmation for deletes & bulk updates" },
                  { id: "AlwaysAsk", label: "Always Ask Confirmation", desc: "Require confirmation for all data modifications" },
                  { id: "AutoApprove", label: "Auto-Approve Low Risk", desc: "Execute safe business operations without prompt" },
                ].map((mode) => (
                  <button
                    key={mode.id}
                    type="button"
                    onClick={() => setSafetyLevel(mode.id)}
                    className={`rounded-xl border p-4 text-left transition-all ${
                      safetyLevel === mode.id
                        ? "border-indigo-500 bg-indigo-500/10 text-indigo-600 dark:text-indigo-400 font-semibold"
                        : "border-border/60 bg-background text-muted-foreground hover:bg-muted"
                    }`}
                  >
                    <div className="text-xs font-bold">{mode.label}</div>
                    <div className="text-[11px] opacity-80 mt-1">{mode.desc}</div>
                  </button>
                ))}
              </div>
            </div>
          </div>

          {/* Proactive Recommendations */}
          <div className="rounded-2xl border border-border/60 bg-card p-6 shadow-sm space-y-4">
            <div className="flex items-center justify-between border-b border-border/50 pb-3">
              <div className="flex items-center gap-2">
                <Sparkles className="h-4 w-4 text-purple-500" />
                <h3 className="text-sm font-bold text-foreground">Proactive Insights & Recommendations</h3>
              </div>
              <input
                type="checkbox"
                checked={enableSuggestions}
                onChange={(e) => setEnableSuggestions(e.target.checked)}
                className="h-4 w-4 rounded border-border text-indigo-600 focus:ring-indigo-500"
              />
            </div>
            <p className="text-xs text-muted-foreground">
              Automatically scan tenant leads, invoices, and campaign metrics to display actionable suggestions on your dashboard.
            </p>
          </div>
        </div>

        <div className="flex justify-end pt-4">
          <button
            type="button"
            onClick={handleSaveSettings}
            className="flex items-center gap-2 rounded-xl bg-indigo-600 px-5 py-2.5 text-xs font-semibold text-white shadow-md hover:bg-indigo-700 transition-colors"
          >
            <Check className="h-4 w-4" />
            Save Preferences
          </button>
        </div>
      </div>
    </div>
  );
}
