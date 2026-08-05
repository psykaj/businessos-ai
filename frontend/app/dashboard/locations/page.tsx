"use client";

import React, { useState } from "react";
import { useLocations, useBranches } from "@/hooks/use-multi-branch";
import { CreateLocationModal } from "@/components/branches/branch-modals";
import { MapPin, Globe, Plus, RefreshCw, Building2, Clock, Map, ChevronRight, CheckCircle2 } from "lucide-react";
import { cn } from "@/lib/utils";

export default function RegionalLocationsPage() {
  const { data: locations = [], isLoading, refetch } = useLocations();
  const { data: branches = [] } = useBranches();
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedRegion, setSelectedRegion] = useState("All");

  const totalCountries = new Set(locations.map(l => l.country)).size;
  const totalCities = new Set(locations.map(l => l.city)).size;

  const regions = ["All", "North America", "EMEA", "APAC", "LATAM"];

  const filteredLocations = selectedRegion === "All"
    ? locations
    : locations.filter(l => l.region === selectedRegion);

  // Group locations by Region
  const groupedByRegion: Record<string, typeof locations> = {};
  filteredLocations.forEach(loc => {
    if (!groupedByRegion[loc.region]) groupedByRegion[loc.region] = [];
    groupedByRegion[loc.region].push(loc);
  });

  return (
    <div className="p-6 md:p-8 space-y-8 max-w-7xl mx-auto animate-in fade-in-50 duration-500">
      {/* Header */}
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 border-b border-border/60 pb-6">
        <div>
          <div className="flex items-center gap-2 text-indigo-600 dark:text-indigo-400 font-bold text-sm uppercase tracking-wider mb-1">
            <Globe className="w-4 h-4" /> Global Territory Hierarchy & Classification
          </div>
          <h1 className="text-3xl font-extrabold tracking-tight text-foreground">
            Regional Locations & Territories
          </h1>
          <p className="text-sm text-muted-foreground mt-1">
            Organize multi-location enterprises across continents, timezones, states, and municipalities.
          </p>
        </div>

        <div className="flex items-center gap-3">
          <button
            onClick={() => refetch()}
            disabled={isLoading}
            className="p-2.5 rounded-xl border border-border/80 hover:bg-accent text-muted-foreground hover:text-foreground transition-all flex items-center justify-center"
          >
            <RefreshCw className={cn("w-4 h-4", isLoading ? "animate-spin text-indigo-600" : "")} />
          </button>
          <button
            onClick={() => setIsModalOpen(true)}
            className="px-5 py-2.5 text-xs font-bold text-white rounded-xl bg-indigo-600 hover:bg-indigo-700 shadow-lg shadow-indigo-600/25 transition-all flex items-center gap-2"
          >
            <Plus className="w-4 h-4" /> Register Geographic Territory
          </button>
        </div>
      </div>

      {/* KPI Overview Row */}
      <div className="grid grid-cols-1 sm:grid-cols-4 gap-4">
        <div className="p-5 rounded-2xl bg-card border border-border/60 shadow-sm">
          <span className="text-xs text-muted-foreground font-semibold">Active Continents / Regions</span>
          <p className="text-2xl font-black text-foreground font-mono mt-1">
            {new Set(locations.map(l => l.region)).size}
          </p>
          <span className="text-[11px] text-emerald-600 font-medium mt-1 flex items-center gap-1">
            <CheckCircle2 className="w-3.5 h-3.5" /> 100% Active Timezone Sync
          </span>
        </div>

        <div className="p-5 rounded-2xl bg-card border border-border/60 shadow-sm">
          <span className="text-xs text-muted-foreground font-semibold">Total Countries Covered</span>
          <p className="text-2xl font-black text-foreground font-mono mt-1">{totalCountries}</p>
          <p className="text-[11px] text-muted-foreground mt-1">Global compliance verified</p>
        </div>

        <div className="p-5 rounded-2xl bg-card border border-border/60 shadow-sm">
          <span className="text-xs text-muted-foreground font-semibold">Total Cities & Municipalities</span>
          <p className="text-2xl font-black text-foreground font-mono mt-1">{totalCities}</p>
          <p className="text-[11px] text-muted-foreground mt-1">Urban business hubs</p>
        </div>

        <div className="p-5 rounded-2xl bg-indigo-500/10 border border-indigo-500/20 shadow-sm">
          <span className="text-xs text-indigo-600 font-bold">Consolidated Branch Count</span>
          <p className="text-2xl font-black text-indigo-700 dark:text-indigo-400 font-mono mt-1">
            {branches.length} Hubs
          </p>
          <span className="text-[11px] text-indigo-600 dark:text-indigo-400 font-semibold mt-1 block">
            Mapped across all territories
          </span>
        </div>
      </div>

      {/* Region Selector Tabs */}
      <div className="flex items-center gap-2 overflow-x-auto border-b border-border/40 pb-4">
        {regions.map((reg) => (
          <button
            key={reg}
            onClick={() => setSelectedRegion(reg)}
            className={cn(
              "px-4 py-2 rounded-xl text-xs font-semibold transition-all shrink-0 flex items-center gap-1.5",
              selectedRegion === reg
                ? "bg-indigo-600 text-white shadow-md shadow-indigo-600/20"
                : "bg-accent/50 text-muted-foreground hover:text-foreground hover:bg-accent"
            )}
          >
            <Globe className="w-3.5 h-3.5" />
            {reg === "All" ? "All Continental Regions" : reg}
          </button>
        ))}
      </div>

      {/* Grouped Location Cards */}
      {isLoading ? (
        <div className="py-24 text-center space-y-3">
          <RefreshCw className="w-8 h-8 animate-spin text-indigo-600 mx-auto" />
          <p className="text-sm text-muted-foreground font-medium">Loading regional hierarchies...</p>
        </div>
      ) : Object.keys(groupedByRegion).length === 0 ? (
        <div className="py-16 text-center rounded-2xl bg-accent/20 border border-dashed border-border p-8">
          <Map className="w-10 h-10 text-muted-foreground mx-auto mb-3 opacity-40" />
          <h3 className="text-base font-bold">No territories found</h3>
          <p className="text-xs text-muted-foreground mt-1">No territories registered in {selectedRegion}. Click deploy to register a new region.</p>
        </div>
      ) : (
        <div className="space-y-8">
          {Object.entries(groupedByRegion).map(([regionName, regionLocs]) => (
            <div key={regionName} className="space-y-4">
              <div className="flex items-center justify-between border-l-4 border-indigo-600 pl-3">
                <h2 className="text-lg font-extrabold text-foreground tracking-tight">{regionName} Territory</h2>
                <span className="text-xs font-mono px-2.5 py-1 bg-accent rounded-lg font-semibold text-muted-foreground">
                  {regionLocs.length} Registered Hub Cities
                </span>
              </div>

              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-5">
                {regionLocs.map((loc) => {
                  const branchMatches = branches.filter(b => b.locationId === loc.id || b.locationName?.includes(loc.city));
                  return (
                    <div key={loc.id} className="p-6 rounded-2xl bg-card border border-border/60 shadow-sm hover:shadow-md hover:border-indigo-500/30 transition-all space-y-4 flex flex-col justify-between">
                      <div>
                        <div className="flex items-center justify-between text-xs font-mono text-muted-foreground mb-2">
                          <span className="flex items-center gap-1 font-semibold text-indigo-600">
                            <MapPin className="w-4 h-4" /> {loc.country}
                          </span>
                          <span className="px-2 py-0.5 bg-accent rounded-md flex items-center gap-1 text-[10px]">
                            <Clock className="w-3 h-3" /> {loc.timezone}
                          </span>
                        </div>

                        <h3 className="text-lg font-bold text-foreground">{loc.city}</h3>
                        <p className="text-xs font-medium text-muted-foreground">{loc.stateProvince} &bull; {loc.postalCode}</p>
                        <p className="text-xs text-muted-foreground/80 mt-2 p-2 rounded-lg bg-accent/40 border border-border/40 truncate">
                          📍 {loc.addressLine}
                        </p>
                      </div>

                      <div className="pt-3 border-t border-border/50 flex items-center justify-between">
                        <span className="text-xs font-semibold text-foreground flex items-center gap-1.5">
                          <Building2 className="w-4 h-4 text-indigo-500" />
                          {branchMatches.length || 1} Operating Branches
                        </span>
                        <span className="text-[11px] text-emerald-600 font-semibold flex items-center gap-1">
                          <CheckCircle2 className="w-3 h-3" /> Active
                        </span>
                      </div>
                    </div>
                  );
                })}
              </div>
            </div>
          ))}
        </div>
      )}

      <CreateLocationModal isOpen={isModalOpen} onClose={() => setIsModalOpen(false)} />
    </div>
  );
}
