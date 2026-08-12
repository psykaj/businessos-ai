import React from "react";
import { Badge } from "@/components/ui/badge";
import { CheckCircle2, AlertCircle, HelpCircle, User } from "lucide-react";

interface OutcomeBadgeProps {
  confidence: "High" | "Medium" | "Low" | "Unknown" | string;
  attributionLevel?: "Direct" | "Strong" | "Moderate" | "Weak" | "Unknown" | string;
  sourceType?: string;
}

export function OutcomeBadge({ confidence, attributionLevel, sourceType }: OutcomeBadgeProps) {
  if (sourceType === "UserReported") {
    return (
      <Badge variant="outline" className="bg-purple-50 text-purple-700 border-purple-200 gap-1 font-medium">
        <User className="w-3 h-3" />
        User Reported
      </Badge>
    );
  }

  // Determine label based on attribution or confidence
  const isVerified = attributionLevel === "Direct" || confidence === "High";
  const isEstimated = attributionLevel === "Moderate" || confidence === "Medium";
  const isAssociated = attributionLevel === "Weak" || confidence === "Low";

  if (isVerified) {
    return (
      <Badge variant="outline" className="bg-green-50 text-green-700 border-green-200 gap-1 font-medium">
        <CheckCircle2 className="w-3 h-3" />
        Verified
      </Badge>
    );
  }

  if (isEstimated) {
    return (
      <Badge variant="outline" className="bg-blue-50 text-blue-700 border-blue-200 gap-1 font-medium">
        <AlertCircle className="w-3 h-3" />
        Estimated
      </Badge>
    );
  }

  if (isAssociated) {
    return (
      <Badge variant="outline" className="bg-amber-50 text-amber-700 border-amber-200 gap-1 font-medium">
        <HelpCircle className="w-3 h-3" />
        Associated
      </Badge>
    );
  }

  return (
    <Badge variant="outline" className="bg-slate-50 text-slate-700 border-slate-200 gap-1 font-medium">
      <HelpCircle className="w-3 h-3" />
      Unknown
    </Badge>
  );
}
