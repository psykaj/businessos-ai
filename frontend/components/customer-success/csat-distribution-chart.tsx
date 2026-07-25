import React from "react";
import { Star } from "lucide-react";

interface CSATDistributionChartProps {
  distribution: Record<number, number>;
  totalSubmissions: number;
}

export function CSATDistributionChart({ distribution, totalSubmissions }: CSATDistributionChartProps) {
  const getRatingColor = (star: number) => {
    switch (star) {
      case 5:
        return "bg-emerald-500";
      case 4:
        return "bg-teal-500";
      case 3:
        return "bg-amber-500";
      case 2:
        return "bg-orange-500";
      case 1:
        return "bg-rose-500";
      default:
        return "bg-purple-500";
    }
  };

  return (
    <div className="space-y-3">
      {[5, 4, 3, 2, 1].map((star) => {
        const count = distribution[star] || 0;
        const percentage = totalSubmissions > 0 ? Math.round((count / totalSubmissions) * 100) : 0;

        return (
          <div key={star} className="flex items-center gap-3 text-sm">
            <div className="flex items-center gap-1 w-14 font-medium text-slate-700 dark:text-slate-300">
              <span>{star}</span>
              <Star className="h-3.5 w-3.5 fill-amber-400 text-amber-400" />
            </div>

            <div className="flex-1 h-3 bg-slate-100 dark:bg-slate-800 rounded-full overflow-hidden">
              <div
                className={`h-full ${getRatingColor(star)} transition-all duration-500 rounded-full`}
                style={{ width: `${percentage}%` }}
              />
            </div>

            <div className="w-16 text-right font-mono text-xs text-slate-500 dark:text-slate-400">
              {count} ({percentage}%)
            </div>
          </div>
        );
      })}
    </div>
  );
}
