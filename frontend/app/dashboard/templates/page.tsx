"use client";

import React from "react";
import { TemplateManager } from "@/components/communication/TemplateManager";

export default function MessageTemplatesPage() {
  return (
    <div className="w-full flex flex-col min-h-[750px] animate-in fade-in duration-200 pb-12">
      <TemplateManager />
    </div>
  );
}
