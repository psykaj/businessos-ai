import React from "react";
import { Users, UserCheck, Gift } from "lucide-react";

interface ReferralFunnelChartProps {
  total: number;
  pending: number;
  converted: number;
  rewarded: number;
}

export function ReferralFunnelChart({ total, pending, converted, rewarded }: ReferralFunnelChartProps) {
  const convertedPct = total > 0 ? Math.round(((converted + rewarded) / total) * 100) : 0;
  const rewardedPct = total > 0 ? Math.round((rewarded / total) * 100) : 0;

  return (
    <div className="space-y-4">
      <div className="relative space-y-2">
        {/* Stage 1: Invited/Total */}
        <div className="p-3 bg-purple-500/10 border border-purple-500/20 rounded-lg flex items-center justify-between">
          <div className="flex items-center gap-3">
            <Users className="h-5 w-5 text-purple-500" />
            <div>
              <p className="text-sm font-semibold text-slate-900 dark:text-white">Referrals Shared</p>
              <p className="text-xs text-slate-500">Unique referral links or invitations sent</p>
            </div>
          </div>
          <span className="text-lg font-bold font-mono text-purple-600 dark:text-purple-400">{total}</span>
        </div>

        {/* Stage 2: Converted */}
        <div
          className="p-3 bg-indigo-500/10 border border-indigo-500/20 rounded-lg flex items-center justify-between ml-4"
          style={{ width: "calc(100% - 1rem)" }}
        >
          <div className="flex items-center gap-3">
            <UserCheck className="h-5 w-5 text-indigo-500" />
            <div>
              <p className="text-sm font-semibold text-slate-900 dark:text-white">Converted Customers</p>
              <p className="text-xs text-slate-500">Signed up & completed first purchase</p>
            </div>
          </div>
          <div className="text-right">
            <span className="text-lg font-bold font-mono text-indigo-600 dark:text-indigo-400">{converted + rewarded}</span>
            <p className="text-xs text-indigo-500 font-medium">{convertedPct}% Conv. Rate</p>
          </div>
        </div>

        {/* Stage 3: Rewarded */}
        <div
          className="p-3 bg-emerald-500/10 border border-emerald-500/20 rounded-lg flex items-center justify-between ml-8"
          style={{ width: "calc(100% - 2rem)" }}
        >
          <div className="flex items-center gap-3">
            <Gift className="h-5 w-5 text-emerald-500" />
            <div>
              <p className="text-sm font-semibold text-slate-900 dark:text-white">Rewards Distributed</p>
              <p className="text-xs text-slate-500">Points & credits issued to referrers</p>
            </div>
          </div>
          <div className="text-right">
            <span className="text-lg font-bold font-mono text-emerald-600 dark:text-emerald-400">{rewarded}</span>
            <p className="text-xs text-emerald-500 font-medium">{rewardedPct}% Rewarded</p>
          </div>
        </div>
      </div>
    </div>
  );
}
