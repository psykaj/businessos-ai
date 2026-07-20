"use client";

import { useQuery } from "@tanstack/react-query";
import { crmService } from "@/lib/crm-service";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { 
  Table, 
  TableBody, 
  TableCell, 
  TableHead, 
  TableHeader, 
  TableRow 
} from "@/components/ui/table";
import { Plus, Tags as TagsIcon, Palette } from "lucide-react";
import { Badge } from "@/components/ui/badge";

export default function TagsPage() {
  const { data: tags, isLoading } = useQuery({
    queryKey: ['crm-tags'],
    queryFn: () => crmService.getTags()
  });

  return (
    <div className="p-8 space-y-6 max-w-4xl">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Tags</h1>
          <p className="text-muted-foreground">Manage organization-wide tags for categorization.</p>
        </div>
        <Button>
          <Plus className="mr-2 h-4 w-4" /> Create Tag
        </Button>
      </div>

      <Card>
        <CardHeader className="pb-3">
          <CardTitle>All Tags</CardTitle>
        </CardHeader>
        <CardContent>
          {isLoading ? (
            <div className="space-y-4">
              {[1, 2, 3].map(i => <Skeleton key={i} className="h-12 w-full" />)}
            </div>
          ) : tags && tags.length > 0 ? (
            <div className="rounded-md border">
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Tag Name</TableHead>
                    <TableHead>Color</TableHead>
                    <TableHead>Preview</TableHead>
                    <TableHead className="text-right">Actions</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {tags.map((tag) => (
                    <TableRow key={tag.id}>
                      <TableCell className="font-medium">{tag.name}</TableCell>
                      <TableCell>
                        <div className="flex items-center gap-2 text-sm">
                          <Palette className="h-4 w-4 text-muted-foreground" />
                          <span className="font-mono text-muted-foreground">{tag.color}</span>
                        </div>
                      </TableCell>
                      <TableCell>
                        <Badge 
                          variant="outline" 
                          style={{ backgroundColor: `${tag.color}15`, color: tag.color, borderColor: `${tag.color}30` }}
                        >
                          {tag.name}
                        </Badge>
                      </TableCell>
                      <TableCell className="text-right">
                        <Button variant="ghost" size="sm">Edit</Button>
                        <Button variant="ghost" size="sm" className="text-red-500 hover:text-red-600">Delete</Button>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </div>
          ) : (
            <div className="text-center py-10 text-muted-foreground">
              <TagsIcon className="h-10 w-10 mx-auto text-muted-foreground/50 mb-3" />
              <p>No tags found.</p>
              <Button variant="link" className="mt-2">Create your first tag</Button>
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
