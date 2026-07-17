"use client";

import { usePathname, useRouter, useSearchParams } from "next/navigation";
import { useCallback } from "react";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { subDays, startOfMonth, formatISO } from "date-fns";

export function DateRangePicker() {
  const router = useRouter();
  const pathname = usePathname();
  const searchParams = useSearchParams();

  const currentRange = searchParams.get("range") || "30d";

  const handleRangeChange = useCallback(
    (value: string | null) => {
      if (!value) return;
      const params = new URLSearchParams(searchParams.toString());
      params.set("range", value);

      let startDate = "";
      const now = new Date();

      switch (value) {
        case "7d":
          startDate = formatISO(subDays(now, 7));
          break;
        case "30d":
          startDate = formatISO(subDays(now, 30));
          break;
        case "90d":
          startDate = formatISO(subDays(now, 90));
          break;
        case "month":
          startDate = formatISO(startOfMonth(now));
          break;
        case "all":
        default:
          startDate = ""; // Send nothing for all time
          break;
      }

      if (startDate) {
        params.set("startDate", startDate);
      } else {
        params.delete("startDate");
      }
      
      params.delete("endDate"); // For simplicity, end date is always now

      router.push(`${pathname}?${params.toString()}`);
    },
    [pathname, router, searchParams]
  );

  return (
    <div className="flex items-center gap-2">
      <span className="text-sm font-medium text-muted-foreground">Date Range:</span>
      <Select value={currentRange} onValueChange={handleRangeChange}>
        <SelectTrigger className="w-[180px]">
          <SelectValue placeholder="Select date range" />
        </SelectTrigger>
        <SelectContent>
          <SelectItem value="7d">Last 7 Days</SelectItem>
          <SelectItem value="30d">Last 30 Days</SelectItem>
          <SelectItem value="90d">Last 90 Days</SelectItem>
          <SelectItem value="month">This Month</SelectItem>
          <SelectItem value="all">All Time</SelectItem>
        </SelectContent>
      </Select>
    </div>
  );
}
