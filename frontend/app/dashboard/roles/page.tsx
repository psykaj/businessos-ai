"use client";

import { useState } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Checkbox } from "@/components/ui/checkbox";
import { Shield, ShieldAlert, ShieldCheck, User, Eye } from "lucide-react";

// Mock permissions matrix based on the backend AppPermissions constants
const permissionsMatrix = [
  {
    module: "Dashboard",
    permissions: [
      { id: "Dashboard.View", label: "View Dashboard" },
    ]
  },
  {
    module: "QR Codes",
    permissions: [
      { id: "QRCodes.View", label: "View QR Codes" },
      { id: "QRCodes.Create", label: "Create QR Codes" },
      { id: "QRCodes.Update", label: "Update QR Codes" },
      { id: "QRCodes.Delete", label: "Delete QR Codes" },
    ]
  },
  {
    module: "Analytics",
    permissions: [
      { id: "Analytics.View", label: "View Analytics" },
      { id: "Analytics.Export", label: "Export Analytics" },
    ]
  },
  {
    module: "Team Management",
    permissions: [
      { id: "TeamManagement.View", label: "View Team Members" },
      { id: "TeamManagement.Invite", label: "Invite Members" },
      { id: "TeamManagement.ManageRoles", label: "Manage Roles" },
      { id: "TeamManagement.Remove", label: "Remove Members" },
    ]
  },
  {
    module: "Organization",
    permissions: [
      { id: "OrganizationSettings.View", label: "View Settings" },
      { id: "OrganizationSettings.Update", label: "Update Settings" },
    ]
  },
  {
    module: "Billing",
    permissions: [
      { id: "Billing.View", label: "View Billing" },
      { id: "Billing.Manage", label: "Manage Subscription" },
    ]
  },
  {
    module: "API Keys",
    permissions: [
      { id: "ApiKeys.View", label: "View API Keys" },
      { id: "ApiKeys.Manage", label: "Manage API Keys" },
    ]
  },
];

const systemRoles = [
  {
    id: "Owner",
    name: "Owner",
    icon: ShieldAlert,
    description: "Full access to the organization, including billing and destructive actions.",
    allowedPermissions: permissionsMatrix.flatMap(m => m.permissions.map(p => p.id)), // All
  },
  {
    id: "Admin",
    name: "Admin",
    icon: ShieldCheck,
    description: "Full access to standard features and team management, excluding billing.",
    allowedPermissions: permissionsMatrix.flatMap(m => m.permissions.map(p => p.id)).filter(p => !p.startsWith("Billing")),
  },
  {
    id: "Manager",
    name: "Manager",
    icon: Shield,
    description: "Can manage content, view analytics, and view team.",
    allowedPermissions: [
      "Dashboard.View", "QRCodes.View", "QRCodes.Create", "QRCodes.Update", "QRCodes.Delete",
      "Analytics.View", "Analytics.Export", "TeamManagement.View", "OrganizationSettings.View", "ApiKeys.View"
    ],
  },
  {
    id: "Member",
    name: "Member",
    icon: User,
    description: "Can view and create standard content.",
    allowedPermissions: [
      "Dashboard.View", "QRCodes.View", "QRCodes.Create", "Analytics.View"
    ],
  },
  {
    id: "Viewer",
    name: "Viewer",
    icon: Eye,
    description: "Read-only access to specific modules.",
    allowedPermissions: [
      "Dashboard.View", "QRCodes.View", "Analytics.View"
    ],
  }
];

export default function RolesPermissionsPage() {
  const [selectedRole, setSelectedRole] = useState(systemRoles[0].id);

  const roleData = systemRoles.find(r => r.id === selectedRole);

  return (
    <div className="mx-auto max-w-6xl space-y-6">
      <div>
        <h1 className="text-2xl font-bold tracking-tight text-foreground">Roles & Permissions</h1>
        <p className="mt-1 text-sm text-muted-foreground">View system roles and their assigned permissions.</p>
      </div>

      <Tabs defaultValue="roles" className="space-y-6">
        <TabsList>
          <TabsTrigger value="roles">System Roles</TabsTrigger>
          <TabsTrigger value="permissions">Permission Matrix</TabsTrigger>
        </TabsList>

        <TabsContent value="roles" className="space-y-6">
          <div className="grid gap-6 md:grid-cols-4">
            <div className="md:col-span-1 space-y-2">
              {systemRoles.map((role) => {
                const Icon = role.icon;
                const isSelected = selectedRole === role.id;
                return (
                  <button
                    key={role.id}
                    onClick={() => setSelectedRole(role.id)}
                    className={`w-full flex items-center gap-3 p-3 text-left rounded-xl transition-colors ${
                      isSelected 
                        ? "bg-primary text-primary-foreground shadow-md" 
                        : "bg-card hover:bg-muted text-card-foreground border"
                    }`}
                  >
                    <Icon className="h-5 w-5" />
                    <div>
                      <p className="font-semibold text-sm">{role.name}</p>
                    </div>
                  </button>
                )
              })}
            </div>

            <div className="md:col-span-3">
              <Card>
                <CardHeader>
                  <CardTitle className="flex items-center gap-2">
                    {roleData?.icon && <roleData.icon className="h-5 w-5 text-primary" />}
                    {roleData?.name} Permissions
                  </CardTitle>
                  <CardDescription>{roleData?.description}</CardDescription>
                </CardHeader>
                <CardContent>
                  <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
                    {permissionsMatrix.map((module) => (
                      <div key={module.module} className="space-y-3 rounded-lg border p-4 bg-muted/20">
                        <h4 className="font-semibold text-sm">{module.module}</h4>
                        <div className="space-y-2">
                          {module.permissions.map((perm) => {
                            const isGranted = roleData?.allowedPermissions.includes(perm.id);
                            return (
                              <div key={perm.id} className="flex items-start space-x-2">
                                <Checkbox 
                                  id={`${roleData?.id}-${perm.id}`} 
                                  checked={isGranted} 
                                  disabled 
                                  className={isGranted ? "opacity-100" : "opacity-40"} 
                                />
                                <div className="grid gap-1.5 leading-none">
                                  <label
                                    htmlFor={`${roleData?.id}-${perm.id}`}
                                    className={`text-sm font-medium leading-none cursor-default ${isGranted ? "" : "text-muted-foreground"}`}
                                  >
                                    {perm.label}
                                  </label>
                                </div>
                              </div>
                            )
                          })}
                        </div>
                      </div>
                    ))}
                  </div>
                </CardContent>
              </Card>
            </div>
          </div>
        </TabsContent>

        <TabsContent value="permissions">
          <Card>
            <CardHeader>
              <CardTitle>Permission Matrix Overview</CardTitle>
              <CardDescription>A bird&apos;s-eye view of all permissions across all system roles.</CardDescription>
            </CardHeader>
            <CardContent className="overflow-x-auto">
              <table className="w-full text-sm text-left">
                <thead className="bg-muted text-muted-foreground">
                  <tr>
                    <th className="px-4 py-3 font-semibold rounded-tl-lg">Permission</th>
                    {systemRoles.map(r => (
                      <th key={r.id} className="px-4 py-3 font-semibold text-center">{r.name}</th>
                    ))}
                  </tr>
                </thead>
                <tbody className="divide-y">
                  {permissionsMatrix.flatMap(m => m.permissions).map((perm) => (
                    <tr key={perm.id} className="hover:bg-muted/50">
                      <td className="px-4 py-3 font-medium">{perm.label}</td>
                      {systemRoles.map(r => {
                        const isGranted = r.allowedPermissions.includes(perm.id);
                        return (
                          <td key={`${r.id}-${perm.id}`} className="px-4 py-3 text-center">
                            {isGranted ? (
                              <ShieldCheck className="h-4 w-4 mx-auto text-green-500" />
                            ) : (
                              <span className="text-muted-foreground/30">-</span>
                            )}
                          </td>
                        )
                      })}
                    </tr>
                  ))}
                </tbody>
              </table>
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>
    </div>
  );
}
