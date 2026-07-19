'use client';

import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { billingService } from '@/lib/billing-service';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table';
import { Badge } from '@/components/ui/badge';
import { Input } from '@/components/ui/input';
import { format } from 'date-fns';
import { Skeleton } from '@/components/ui/skeleton';
import { Search, Receipt, ArrowUpRight } from 'lucide-react';
import Link from 'next/link';
import { cn } from '@/lib/utils';

export default function PaymentsPage() {
  const [search, setSearch] = useState('');
  
  const { data: payments, isLoading } = useQuery({
    queryKey: ['paymentHistory'],
    queryFn: billingService.getPaymentHistory,
  });

  const filteredPayments = payments?.filter(p => 
    p.transactionId.toLowerCase().includes(search.toLowerCase()) || 
    p.planName.toLowerCase().includes(search.toLowerCase())
  );

  return (
    <div className="relative min-h-full p-6 sm:p-10 max-w-6xl mx-auto space-y-10">
      {/* Background Decor */}
      <div className="absolute top-40 left-10 w-[300px] h-[300px] bg-primary/5 rounded-full blur-[100px] -z-10" />

      <div className="flex flex-col md:flex-row justify-between items-start md:items-end gap-6">
        <div>
          <h1 className="text-4xl font-extrabold tracking-tight bg-clip-text text-transparent bg-gradient-to-r from-foreground to-foreground/70">
            Payment History
          </h1>
          <p className="text-muted-foreground mt-2 text-lg">A complete log of your billing transactions.</p>
        </div>

        <div className="relative w-full md:w-80 group">
          <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
            <Search className="h-4 w-4 text-muted-foreground group-focus-within:text-primary transition-colors" />
          </div>
          <Input
            type="search"
            placeholder="Search by ID or Plan..."
            className="pl-10 h-11 bg-background/60 backdrop-blur-md border-border/50 rounded-xl shadow-sm focus-visible:ring-primary/50 transition-all"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />
        </div>
      </div>

      <div className="rounded-3xl border border-border/50 bg-background/60 backdrop-blur-xl shadow-xl overflow-hidden">
        <div className="overflow-x-auto">
          <Table>
            <TableHeader>
              <TableRow className="bg-muted/30 hover:bg-muted/30 border-b border-border/50">
                <TableHead className="h-12 px-6 font-semibold text-muted-foreground">Date</TableHead>
                <TableHead className="h-12 px-6 font-semibold text-muted-foreground">Plan</TableHead>
                <TableHead className="h-12 px-6 font-semibold text-muted-foreground">Amount</TableHead>
                <TableHead className="h-12 px-6 font-semibold text-muted-foreground">Status</TableHead>
                <TableHead className="h-12 px-6 font-semibold text-muted-foreground">Transaction ID</TableHead>
                <TableHead className="h-12 px-6 font-semibold text-muted-foreground text-right">Invoice</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {isLoading ? (
                [1, 2, 3, 4].map(i => (
                  <TableRow key={i} className="border-b border-border/30">
                    <TableCell className="px-6 py-4"><Skeleton className="h-4 w-24 rounded-full" /></TableCell>
                    <TableCell className="px-6 py-4"><Skeleton className="h-4 w-20 rounded-full" /></TableCell>
                    <TableCell className="px-6 py-4"><Skeleton className="h-4 w-12 rounded-full" /></TableCell>
                    <TableCell className="px-6 py-4"><Skeleton className="h-6 w-16 rounded-full" /></TableCell>
                    <TableCell className="px-6 py-4"><Skeleton className="h-4 w-32 rounded-full" /></TableCell>
                    <TableCell className="px-6 py-4 text-right"><Skeleton className="h-4 w-16 ml-auto rounded-full" /></TableCell>
                  </TableRow>
                ))
              ) : filteredPayments?.length === 0 ? (
                <TableRow>
                  <TableCell colSpan={6} className="h-48 text-center">
                    <div className="flex flex-col items-center justify-center text-muted-foreground space-y-3">
                      <div className="w-12 h-12 rounded-full bg-muted/50 flex items-center justify-center">
                        <Receipt className="w-6 h-6 opacity-50" />
                      </div>
                      <p className="text-sm font-medium">No payments found matching &quot;{search}&quot;.</p>
                    </div>
                  </TableCell>
                </TableRow>
              ) : (
                filteredPayments?.map((payment) => (
                  <TableRow key={payment.id} className="border-b border-border/30 hover:bg-muted/20 transition-colors group">
                    <TableCell className="px-6 py-4 text-muted-foreground whitespace-nowrap">{format(new Date(payment.date), 'MMM dd, yyyy')}</TableCell>
                    <TableCell className="px-6 py-4 font-semibold text-foreground">{payment.planName}</TableCell>
                    <TableCell className="px-6 py-4 font-mono font-medium">${payment.amount.toFixed(2)}</TableCell>
                    <TableCell className="px-6 py-4">
                      <Badge variant="outline" className={cn(
                        "px-2.5 py-0.5 rounded-full text-xs font-bold tracking-wider shadow-sm border",
                        payment.status === 'succeeded' ? 'bg-green-500/10 text-green-600 border-green-500/20' : 
                        payment.status === 'pending' ? 'bg-yellow-500/10 text-yellow-600 border-yellow-500/20' : 
                        'bg-red-500/10 text-red-600 border-red-500/20'
                      )}>
                        {payment.status.toUpperCase()}
                      </Badge>
                    </TableCell>
                    <TableCell className="px-6 py-4 font-mono text-xs text-muted-foreground/80 tracking-tight">{payment.transactionId}</TableCell>
                    <TableCell className="px-6 py-4 text-right">
                      {payment.invoiceId ? (
                        <Link href="/dashboard/invoices" className="inline-flex items-center gap-1 text-primary hover:text-primary/80 font-medium text-sm transition-colors opacity-0 group-hover:opacity-100 focus:opacity-100">
                          View Invoice <ArrowUpRight className="w-3 h-3" />
                        </Link>
                      ) : (
                        <span className="text-muted-foreground/50 text-sm">-</span>
                      )}
                    </TableCell>
                  </TableRow>
                ))
              )}
            </TableBody>
          </Table>
        </div>
      </div>
    </div>
  );
}
