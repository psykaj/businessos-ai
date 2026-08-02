"use client";

import React from "react";
import { AnalyticsDashboard } from "@/components/communication/AnalyticsDashboard";

export default function CommunicationAnalyticsPage() {
  return (
    <div className="w-full flex flex-col min-h-[750px] animate-in fade-in duration-200 pb-12">
      <AnalyticsDashboard />
    </div>
  );
}
