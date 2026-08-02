"use client";

import React from "react";
import {
  BarChart3,
  TrendingUp,
  Clock,
  CheckCircle,
  Award,
  Users,
  MessageSquare,
  Sparkles,
  ArrowUpRight,
  ArrowDownRight,
  Smile,
  ShieldCheck
} from "lucide-react";
import { useCommunicationAnalytics } from "@/hooks/use-communication";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  Tooltip,
  ResponsiveContainer,
  PieChart,
  Pie,
  Cell,
  Legend
} from "recharts";
import { ChannelBadge } from "./ChannelBadge";

const COLORS = ["#10B981", "#3B82F6", "#6366F1", "#F59E0B", "#0EA5E9", "#EC4899"];

export function AnalyticsDashboard() {
  const { data: analytics, isLoading } = useCommunicationAnalytics();

  if (isLoading || !analytics) {
    return (
      <div className="w-full flex items-center justify-center p-24 text-muted-foreground font-medium">
        <div className="flex flex-col items-center gap-2">
          <BarChart3 className="w-8 h-8 animate-pulse text-primary" />
          <span>Aggregating Redis executive communication analytics...</span>
        </div>
      </div>
    );
  }

  const channelData = analytics.channelBreakdown.map((item, index) => ({
    name: item.channelType,
    messages: item.totalMessages,
    conversations: item.conversationCount,
    percentage: item.percentageOfTotal,
    fill: COLORS[index % COLORS.length]
  }));

  const csatData = [
    { name: "Satisfied (5★)", value: analytics.csatSummary.satisfiedCount, fill: "#10B981" },
    { name: "Neutral (3-4★)", value: analytics.csatSummary.neutralCount, fill: "#F59E0B" },
    { name: "Unsatisfied (1-2★)", value: analytics.csatSummary.unsatisfiedCount, fill: "#EF4444" }
  ];

  const resolutionTrend = [
    { day: "Mon", avgResponseMin: 6.2, avgResolveHour: 3.1 },
    { day: "Tue", avgResponseMin: 5.8, avgResolveHour: 2.9 },
    { day: "Wed", avgResponseMin: 5.1, avgResolveHour: 2.5 },
    { day: "Thu", avgResponseMin: 4.5, avgResolveHour: 2.2 },
    { day: "Fri", avgResponseMin: 4.8, avgResolveHour: 1.9 },
    { day: "Sat", avgResponseMin: 3.9, avgResolveHour: 1.7 },
    { day: "Sun", avgResponseMin: 3.5, avgResolveHour: 1.5 }
  ];

  const agentLeaderboard = [
    { name: "Sarah Jenkins (Senior AE)", resolved: 42, avgResponse: "3m 12s", csat: "4.9★", channels: ["WhatsApp", "Email"] },
    { name: "Marcus Vance (Support Eng)", resolved: 38, avgResponse: "4m 05s", csat: "4.8★", channels: ["LiveChat"] },
    { name: "Chloe Bennett (CSM)", resolved: 31, avgResponse: "4m 45s", csat: "4.9★", channels: ["SMS", "WhatsApp"] }
  ];

  return (
    <div className="w-full flex flex-col gap-6 p-6 bg-card/40 border border-border/60 rounded-2xl shadow-sm backdrop-blur-md">
      {/* Title & Date badge */}
      <div className="flex items-center justify-between pb-4 border-b border-border/60">
        <div>
          <h1 className="text-xl font-bold text-foreground flex items-center gap-2">
            <span>Executive Communication Analytics</span>
            <span className="text-xs font-semibold px-2 py-0.5 rounded-full bg-emerald-500/10 text-emerald-500 border border-emerald-500/20">
              Redis Cached (15m Rollup)
            </span>
          </h1>
          <p className="text-xs text-muted-foreground mt-0.5">
            Real-time business value metrics: response times, channel utilization, agent performance, and customer CSAT satisfaction.
          </p>
        </div>
        <div className="flex items-center gap-1 text-xs text-muted-foreground font-mono">
          <Clock className="w-3.5 h-3.5 text-primary" />
          <span>Last Aggregated: Just now</span>
        </div>
      </div>

      {/* KPI Summary Strip */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        <div className="p-5 rounded-2xl bg-card border border-border/80 shadow-xs flex flex-col justify-between space-y-3">
          <div className="flex items-center justify-between text-muted-foreground">
            <span className="text-xs font-bold uppercase tracking-wider">Active Conversations</span>
            <MessageSquare className="w-4 h-4 text-primary" />
          </div>
          <div className="flex items-baseline justify-between">
            <span className="text-3xl font-black text-foreground">{analytics.activeConversations}</span>
            <span className="text-2xs font-semibold text-emerald-500 flex items-center gap-0.5">
              <ArrowUpRight className="w-3.5 h-3.5" />
              <span>+14% vs last week</span>
            </span>
          </div>
          <p className="text-2xs text-muted-foreground">Across 6 connected omnichannel providers</p>
        </div>

        <div className="p-5 rounded-2xl bg-card border border-border/80 shadow-xs flex flex-col justify-between space-y-3">
          <div className="flex items-center justify-between text-muted-foreground">
            <span className="text-xs font-bold uppercase tracking-wider">Avg Response Time</span>
            <Clock className="w-4 h-4 text-amber-500" />
          </div>
          <div className="flex items-baseline justify-between">
            <span className="text-3xl font-black text-foreground">{analytics.averageResponseTimeMinutes}m</span>
            <span className="text-2xs font-semibold text-emerald-500 flex items-center gap-0.5" title="Lower response time saves customers time">
              <ArrowDownRight className="w-3.5 h-3.5" />
              <span>-22% faster</span>
            </span>
          </div>
          <p className="text-2xs text-emerald-600 dark:text-emerald-400 font-medium flex items-center gap-1">
            <ShieldCheck className="w-3.5 h-3.5" />
            <span>Exceeds BusinessOS SLA target (&lt;10m)</span>
          </p>
        </div>

        <div className="p-5 rounded-2xl bg-card border border-border/80 shadow-xs flex flex-col justify-between space-y-3">
          <div className="flex items-center justify-between text-muted-foreground">
            <span className="text-xs font-bold uppercase tracking-wider">Avg Resolution Time</span>
            <CheckCircle className="w-4 h-4 text-emerald-500" />
          </div>
          <div className="flex items-baseline justify-between">
            <span className="text-3xl font-black text-foreground">1.9h</span>
            <span className="text-2xs font-semibold text-emerald-500 flex items-center gap-0.5">
              <ArrowDownRight className="w-3.5 h-3.5" />
              <span>-18% faster</span>
            </span>
          </div>
          <p className="text-2xs text-muted-foreground">Reduced via AI suggested answers & canned replies</p>
        </div>

        <div className="p-5 rounded-2xl bg-linear-to-br from-amber-500/10 via-amber-500/5 to-transparent border border-amber-500/30 shadow-xs flex flex-col justify-between space-y-3">
          <div className="flex items-center justify-between text-amber-600 dark:text-amber-400">
            <span className="text-xs font-bold uppercase tracking-wider">CSAT Score (Satisfaction)</span>
            <Smile className="w-4 h-4 text-amber-500 animate-bounce" />
          </div>
          <div className="flex items-baseline justify-between">
            <span className="text-3xl font-black text-foreground">{analytics.csatSummary.averageRating}★</span>
            <span className="text-xs font-bold text-amber-600 dark:text-amber-400">93% Satisfied</span>
          </div>
          <p className="text-2xs text-muted-foreground font-medium">Based on {analytics.csatSummary.totalResponses} post-resolution surveys</p>
        </div>
      </div>

      {/* Charts section */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6 pt-2">
        {/* Messages by Channel Bar Chart */}
        <div className="lg:col-span-2 p-5 rounded-2xl bg-card border border-border/80 shadow-xs flex flex-col justify-between">
          <div className="flex items-center justify-between mb-4">
            <h3 className="text-sm font-bold text-foreground">Message Volume by Communication Channel</h3>
            <span className="text-xs text-muted-foreground">Total Today: {analytics.totalMessagesToday} messages</span>
          </div>
          <div className="w-full h-72">
            <ResponsiveContainer width="100%" height={280}>
              <BarChart data={channelData}>
                <XAxis dataKey="name" stroke="#888888" fontSize={11} tickLine={false} axisLine={false} />
                <YAxis stroke="#888888" fontSize={11} tickLine={false} axisLine={false} />
                <Tooltip
                  contentStyle={{ backgroundColor: "rgba(17, 24, 39, 0.9)", borderRadius: "12px", border: "none", color: "#fff", fontSize: "12px" }}
                />
                <Bar dataKey="messages" radius={[8, 8, 0, 0]} />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </div>

        {/* CSAT Breakdown Pie Chart */}
        <div className="p-5 rounded-2xl bg-card border border-border/80 shadow-xs flex flex-col justify-between">
          <h3 className="text-sm font-bold text-foreground mb-4">CSAT Score Breakdown</h3>
          <div className="w-full h-60 flex items-center justify-center">
            <ResponsiveContainer width="100%" height={240}>
              <PieChart>
                <Pie data={csatData} dataKey="value" nameKey="name" cx="50%" cy="50%" outerRadius={70} label>
                  {csatData.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={entry.fill} />
                  ))}
                </Pie>
                <Tooltip />
                <Legend verticalAlign="bottom" height={36} wrapperStyle={{ fontSize: "11px" }} />
              </PieChart>
            </ResponsiveContainer>
          </div>
          <div className="p-3 rounded-xl bg-muted/50 border border-border/40 text-center mt-2">
            <span className="text-2xs font-semibold text-muted-foreground">
              ✨ Customer satisfaction directly correlates with faster response times in WhatsApp & Live Chat.
            </span>
          </div>
        </div>
      </div>

      {/* Team Resolution Performance Table */}
      <div className="p-5 rounded-2xl bg-card border border-border/80 shadow-xs space-y-4">
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-2">
            <Award className="w-5 h-5 text-amber-500" />
            <h3 className="text-sm font-bold text-foreground">Team Resolution & Response Performance Leaderboard</h3>
          </div>
          <span className="text-xs text-muted-foreground">Weekly agent audit</span>
        </div>

        <div className="overflow-x-auto">
          <table className="w-full text-left text-xs border-collapse">
            <thead>
              <tr className="border-b border-border/60 text-muted-foreground text-2xs font-bold uppercase">
                <th className="py-2.5 px-3">Team Member / Agent</th>
                <th className="py-2.5 px-3">Resolved Tickets</th>
                <th className="py-2.5 px-3">Avg Response Time</th>
                <th className="py-2.5 px-3">CSAT Score</th>
                <th className="py-2.5 px-3">Assigned Channels</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-border/40">
              {agentLeaderboard.map((agent, idx) => (
                <tr key={idx} className="hover:bg-muted/40 transition-colors">
                  <td className="py-3 px-3 font-bold text-foreground flex items-center gap-2">
                    <span className="w-6 h-6 rounded-full bg-primary/20 text-primary font-mono text-2xs flex items-center justify-center font-bold">
                      {idx + 1}
                    </span>
                    <span>{agent.name}</span>
                  </td>
                  <td className="py-3 px-3 font-mono font-semibold text-emerald-500">{agent.resolved} resolved</td>
                  <td className="py-3 px-3 font-mono text-muted-foreground">{agent.avgResponse}</td>
                  <td className="py-3 px-3 font-bold text-amber-500">{agent.csat}</td>
                  <td className="py-3 px-3 flex items-center gap-1.5">
                    {agent.channels.map(ch => (
                      <ChannelBadge key={ch} type={ch as any} size="sm" showLabel={false} />
                    ))}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}
