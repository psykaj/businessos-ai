"use client";

import { useQuery } from "@tanstack/react-query";
import { crmService } from "@/lib/crm-service";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button, buttonVariants } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { 
  Table, 
  TableBody, 
  TableCell, 
  TableHead, 
  TableHeader, 
  TableRow 
} from "@/components/ui/table";
import { Plus, User, Mail, Phone, Building } from "lucide-react";
import Link from "next/link";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";

export default function ContactsPage() {
  const { data: contacts, isLoading } = useQuery({
    queryKey: ['crm-contacts'],
    queryFn: () => crmService.getContacts()
  });

  return (
    <div className="p-8 space-y-6">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Contacts</h1>
          <p className="text-muted-foreground">Manage your individual contacts and clients.</p>
        </div>
        <Button>
          <Plus className="mr-2 h-4 w-4" /> Add Contact
        </Button>
      </div>

      <Card>
        <CardHeader className="pb-3">
          <CardTitle>All Contacts</CardTitle>
        </CardHeader>
        <CardContent>
          {isLoading ? (
            <div className="space-y-4">
              {[1, 2, 3, 4, 5].map(i => <Skeleton key={i} className="h-12 w-full" />)}
            </div>
          ) : contacts && contacts.length > 0 ? (
            <div className="rounded-md border">
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Contact</TableHead>
                    <TableHead>Company</TableHead>
                    <TableHead>Email</TableHead>
                    <TableHead>Phone</TableHead>
                    <TableHead className="text-right">Actions</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {contacts.map((contact) => (
                    <TableRow key={contact.id}>
                      <TableCell>
                        <div className="flex items-center gap-3">
                          <Avatar className="h-9 w-9">
                            <AvatarFallback className="bg-primary/10 text-primary font-medium">
                              {contact.firstName.charAt(0)}{contact.lastName.charAt(0)}
                            </AvatarFallback>
                          </Avatar>
                          <div className="flex flex-col">
                            <Link href={`/dashboard/contacts/${contact.id}`} className="font-medium hover:underline">
                              {contact.firstName} {contact.lastName}
                            </Link>
                            <span className="text-xs text-muted-foreground">{contact.jobTitle}</span>
                          </div>
                        </div>
                      </TableCell>
                      <TableCell>
                        <div className="flex items-center gap-2 text-sm">
                          <Building className="h-4 w-4 text-muted-foreground" />
                          {contact.companyName || <span className="text-muted-foreground italic">None</span>}
                        </div>
                      </TableCell>
                      <TableCell>
                        <div className="flex items-center gap-2 text-sm">
                          <Mail className="h-4 w-4 text-muted-foreground" />
                          {contact.email}
                        </div>
                      </TableCell>
                      <TableCell>
                        <div className="flex items-center gap-2 text-sm">
                          <Phone className="h-4 w-4 text-muted-foreground" />
                          {contact.phoneNumber || '-'}
                        </div>
                      </TableCell>
                      <TableCell className="text-right">
                        <Link href={`/dashboard/contacts/${contact.id}`} className={buttonVariants({ variant: "ghost", size: "sm" })}>
                          View
                        </Link>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </div>
          ) : (
            <div className="text-center py-10 text-muted-foreground">
              <User className="h-10 w-10 mx-auto text-muted-foreground/50 mb-3" />
              <p>No contacts found.</p>
              <Button variant="link" className="mt-2">Create your first contact</Button>
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
