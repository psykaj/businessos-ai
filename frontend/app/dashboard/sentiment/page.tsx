"use client";

import React from "react";
import { useSentimentAnalytics, useImprovementRecommendations } from "@/hooks/use-customer-feedback";
import { AiSentimentVisuals } from "@/components/customer-feedback/ai-sentiment-visuals";
import { Zap, Sparkles, BrainCircuit } from "lucide-react";

export default function SentimentPage() {
  const { data: sentimentData, isLoading: sentimentLoading } = useSentimentAnalytics();
  const { data: recommendations, isLoading: recLoading } = useImprovementRecommendations();

  return (
    <div className="space-y-8 p-6 max-w-7xl mx-auto">
      {/* Header */}
      <div className="border-b border-slate-200 dark:border-slate-800 pb-6">
        <h1 className="text-3xl font-black text-slate-900 dark:text-white tracking-tight flex items-center gap-3">
          <BrainCircuit className="h-8 w-8 text-purple-500" />
          AI Sentiment & Root-Cause Intelligence
        </h1>
        <p className="text-slate-500 dark:text-slate-400 mt-1 text-sm font-medium">
          Provider-Independent AI classifies tone and urgency in real-time without API delays. Pinpoint complaint clusters to eliminate systemic product roadblocks.
        </p>
      </div>

      {/* Main Sentiment Suite */}
      <AiSentimentVisuals
        sentimentData={sentimentData}
        recommendations={recommendations}
        isLoading={sentimentLoading || recLoading}
      />
    </div>
  );
}
